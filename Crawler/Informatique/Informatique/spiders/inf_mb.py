from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import neo4j


class inf_mb(CrawlSpider):
    name = "inf_mb"
    allowed_domains = ["informatique.nl"]
    start_urls = ["http://www.informatique.nl/?m=usl&g=726&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?M=USL&G=670&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?m=usl&g=699&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?m=usl&g=635&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?M=ART&G=192&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?m=usl&g=713&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?M=USL&G=717&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?m=usl&g=655&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?M=USL&G=657&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?M=USL&G=686&view=6&&sort=pop&pl=500",
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
            component = 'moederbord'
            desc = titles.select('div[@id="description"]/ul/li/text()').extract()
            price = titles.select('div[@id="price"]/text()').extract()
            #image_urls = titles.select('div[@id="image"]/a/img/@src').extract()

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                        "MERGE (w:Webshop { naam: {webshop} })")
            inf_mb = query_CreateWebshopNode.execute(webshop=webshop)

            query_CreateComponentNode = neo4j.CypherQuery(graph_db,
                                                      "MERGE (c:moederbord {naam:{namedb}})")
            inf_mb = query_CreateComponentNode.execute(namedb=namedb)

            query_GiveComponentProperties = neo4j.CypherQuery(graph_db,
                                                          "MATCH (c:moederbord) WHERE c.naam = {namedb} SET c.interfaces={interfaces}, c.vormfactor={vormfactor}, c.socket={socket}")
            inf_mb = query_GiveComponentProperties.execute(namedb=namedb, interfaces=interfaces,
                                                         vormfactor=vormfactor, socket=socket)

            query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                                                      "MATCH (c:moederbord)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
            inf_mb = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)

            query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                                                          "MATCH (c:behuizing), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
            inf_mb = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                                                         price=price, url=url)