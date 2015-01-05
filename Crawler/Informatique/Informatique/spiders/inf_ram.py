from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import neo4j


class inf_ram(CrawlSpider):
    name = "inf_ram"
    allowed_domains = ["informatique.nl"]
    start_urls = ["http://www.informatique.nl/?m=usl&g=522&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?m=usl&g=725&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?M=USL&G=194&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?M=ART&G=077&view=6&&sort=pop&pl=500"
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
            component = 'werkgeheugen'
            desc = titles.select('div[@id="description"]/ul/li/text()').extract()
            price = titles.select('div[@id="price"]/text()').extract()
            #image_urls = titles.select('div[@id="image"]/a/img/@src').extract()

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                        "MERGE (w:Webshop { naam: {webshop} })")
            inf_ram = query_CreateWebshopNode.execute(webshop=webshop)

            query_CreateComponentNode = neo4j.CypherQuery(graph_db,
                                                      "MERGE (c:werkgeheugen {naam:{namedb}})")
            inf_ram = query_CreateComponentNode.execute(namedb=namedb)

            query_GiveComponentProperties = neo4j.CypherQuery(graph_db,
                                                          "MATCH (c:werkgeheugen) WHERE c.naam = {namedb} SET c.modules={modules}, c.capaciteit={capaciteit}, c.timings={timings}")
            inf_ram = query_GiveComponentProperties.execute(namedb=namedb, modules=modules,
                                                         capaciteit=capaciteit, timings=timings)

            query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                                                      "MATCH (c:werkgeheugen)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
            inf_ram = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)

            query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                                                          "MATCH (c:behuizing), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
            inf_ram = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                                                         price=price, url=url)