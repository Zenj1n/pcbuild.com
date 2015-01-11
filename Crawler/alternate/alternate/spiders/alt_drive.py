import scrapy
from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import rel, node
from py2neo import neo4j

from alternate.items import AlternateItem


class alt_drive(CrawlSpider):
    name = "alt_drive"
    allowed_domains = ["alternate.nl"]
    start_urls = [
        "http://www.alternate.nl/html/product/listing.html?navId=11610&tk=7&lk=9388",
        "http://www.alternate.nl/html/product/listing.html?navId=11614&tk=7&lk=9384"
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
            component = 'optische drives'
            desc = titles.select('a[@class="productLink"]/span[@class="info"]/text()').extract()
            euro = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/text()').extract()
            cent = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/sup/text()').extract()

            lezen = desc[0]
            schrijven = desc[1]
            aansluiting = desc[2]

            price = euro + cent

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                        "MERGE (w:Webshop { naam: {webshop} })")
            alt_drive = query_CreateWebshopNode.execute(webshop=webshop)

            query_CheckOnExistingComponent = neo4j.CypherQuery(graph_db,
                                                      "match (c:optischedrive) where c.naam = {namedb} with COUNT(c) as Count_C RETURN Count_C")
            matchCount = query_CheckOnExistingComponent.execute(namedb=namedb)
            for record in query_CheckOnExistingComponent.stream(namedb=namedb):
                matchCountNumber = record[0]

            if matchCountNumber != 0:
                query_SetSpecifications = neo4j.CypherQuery(graph_db,
                "MATCH (c:optischedrive) WHERE c.naam = {namedb} SET c.lezen = {lezen}, c.schrijven = {schrijven}, c.aansluiting = {aansluiting}")
                alt_drive = query_DeleteRelationships.execute(namedb=namedb, lezen=lezen, schrijven=schrijven, aansluiting=aansluiting)
                query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                "MATCH (c:optischedrive)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
                alt_drive = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                "MATCH (c:optischedrive), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                alt_drive = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop, price=price, url=url)
            else:
                query_CreateComponentNode = neo4j.CypherQuery(graph_db,
                "Create (c:optischedrive {naam:{namedb}, lezen:{lezen}, schrijven:{schrijven}, aansluiting:{aansluiting}})")
                alt_drive = query_CreateComponentNode.execute(namedb=namedb, lezen=lezen,
                schrijven=schrijven, aansluiting=aansluiting)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                "MATCH (c:optischedrive), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                alt_drive = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                price=price, url=url)