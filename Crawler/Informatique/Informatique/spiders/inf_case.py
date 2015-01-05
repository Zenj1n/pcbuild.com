import scrapy
from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import Selector
from py2neo import rel, node
from py2neo import neo4j

from Informatique.items import InformatiqueItem

class inf_case(CrawlSpider):
    name = "inf_case"
    allowed_domains = ["informatique.nl"]
    start_urls = ["http://www.informatique.nl/?m=usl&g=004&view=6&&sort=pop&pl=800"]

    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
                  , callback="parse_start_url", follow=True),
    )

    def parse_start_url(self, response):
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = Selector(response)
        titles = hxs.xpath('//ul[@id="detailview"]/li')
        for titles in titles:
            webshop = 'Informatique'
            name = titles.xpath('div[@id="title"]/a/text()').extract()
            url = titles.xpath('div[@id="title"]/a/@href').extract()
            component = 'behuizing'
            desc = titles.xpath('div[@id="description"]/ul/li/text()').extract()
            price = titles.xpath('div[@id="price"]/text()').extract()
            #image_urls = titles.xpath('div[@id="image"]/a/img/@src').extract()

            interfaces = desc[3];
            vormfactor = desc[0];
            vormvoeding = desc[2];

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                        "MERGE (w:Webshop { naam: {webshop} })")
            inf_case = query_CreateWebshopNode.execute(webshop=webshop)

            query_CreateComponentNode = neo4j.CypherQuery(graph_db,
                                                      "MERGE (c:behuizing {naam:{namedb}})")
            inf_case = query_CreateComponentNode.execute(namedb=namedb)

            query_GiveComponentProperties = neo4j.CypherQuery(graph_db,
                                                          "MATCH (c:behuizing) WHERE c.naam = {namedb} SET c.interfaces={interfaces}, c.vormfactor={vormfactor}, c.vormvoeding={vormvoeding}")
            inf_case = query_GiveComponentProperties.execute(namedb=namedb, interfaces=interfaces,
                                                         vormfactor=vormfactor, vormvoeding=vormvoeding)

            query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                                                      "MATCH (c:behuizing)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
            inf_case = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)

            query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                                                          "MATCH (c:behuizing), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
            inf_case = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                                                         price=price, url=url)