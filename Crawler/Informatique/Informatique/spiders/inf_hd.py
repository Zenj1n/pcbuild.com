from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import neo4j

class inf_hd(CrawlSpider):
    name = "inf_hd"
    allowed_domains = ["informatique.nl"]
    start_urls = ["http://www.informatique.nl/?m=usl&g=026&view=6&&sort=pop&pl=500",

    ]

    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
                  , callback="parse_start_url", follow=True),
    )

    def parse_start_url(self, response):
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//ul[@id="detailview"]/li')
        for titles in titles:
            webshop = 'Informatique'
            name = titles.select('div[@id="title"]/a/text()').extract()
            url = titles.select('div[@id="title"]/a/@href').extract()
            component = 'opslag'
            desc = titles.select('div[@id="description"]/ul/li/text()').extract()
            price = titles.select('div[@id="price"]/text()').extract()
            #image_urls = titles.select('div[@id="image"]/a/img/@src').extract()

            try:
                capaciteit = desc[0]
            except:
                capiciteit = "onbekend"
            try:
                snelheid = desc[2]
            except:
                snelheid = "onbekend"

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                        "MERGE (w:Webshop { naam: {webshop} })")
            inf_hd = query_CreateWebshopNode.execute(webshop=webshop)

            query_CheckOnExistingComponent = neo4j.CypherQuery(graph_db,
                                                      "match (c:hd) where c.naam = {namedb} with COUNT(c) as Count_C RETURN Count_C")
            matchCount = query_CheckOnExistingComponent.execute(namedb=namedb)
            for record in query_CheckOnExistingComponent.stream(namedb=namedb):
                matchCountNumber = record[0]

            if matchCountNumber != 0:
                query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                "MATCH (c:hd)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
                inf_hd = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                "MATCH (c:hd), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                inf_hd = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop, price=price, url=url)
            else:
                query_CreateComponentNode = neo4j.CypherQuery(graph_db,
                "Create (c:hd {naam:{namedb}, capaciteit:{capaciteit}, snelheid:{snelheid}})")
                inf_hd = query_CreateComponentNode.execute(namedb=namedb, capaciteit=capaciteit,
                snelheid=snelheid)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                "MATCH (c:hd), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                inf_hd = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                price=price, url=url)

            query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                                                      "MATCH (c:hd)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
            inf_hd = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)

            query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                                                          "MATCH (c:behuizing), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
            inf_hd = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                                                         price=price, url=url)