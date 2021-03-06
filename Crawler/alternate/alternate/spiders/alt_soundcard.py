from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import neo4j

import csv
import datetime


class alt_soundcard(CrawlSpider):
    name = "alt_soundcard"
    allowed_domains = ["alternate.nl"]
    start_urls = [
        "http://www.alternate.nl/html/product/listing.html?navId=17364&navId=17363&navId=17362&tk=7&lk=9517"
    ]

    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[@class="next"]')), callback='parse_start_url', follow=True),)

    def parse_start_url(self, response):
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//div[@class="listRow"]')
        for titles in titles:
            webshop = 'alternate.nl'
            name = titles.select('a[@class="productLink"]/span[@class="product"]/span[@class="pic"]/@title').extract()
            url = titles.select('a[@class="productLink"]/@href').extract()
            component = 'geluidskaart'
            desc = titles.select('a[@class="productLink"]/span[@class="info"]/text()').extract()
            euro = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/text()').extract()
            cent = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/sup/text()').extract()

            interface = desc[0].strip()
            geluidschip = desc[1].strip()
            aansluitingen = desc[2].strip()

            price = euro + cent

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                        "MERGE (w:Webshop { naam: {webshop} })")
            alt_soundcard = query_CreateWebshopNode.execute(webshop=webshop)

            query_CreateComponentNode = neo4j.CypherQuery(graph_db,
                                                          "MERGE (c:geluidskaart {naam:{namedb}, interfaces:{interfaces}, geluidschip:{geluidschip}, aansluitingen:{aansluitingen}})")
            alt_soundcard = query_CreateComponentNode.execute(namedb=namedb, interface=interface,
                                                              geluidschip=geluidschip, aansluitingen=aansluitingen)

            query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                                                          "MATCH (c:geluidskaart)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
            alt_soundcard = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)

            query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                                                              "MATCH (c:geluidskaart), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
            alt_soundcard = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                                                                  price=price, url=url)