import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import rel, node
from py2neo import neo4j


from Hardware.items import  HWItem

class PowerSupplySpider(CrawlSpider):
    name = "hw_psu"
    allowed_domains = ["hardware.info"]
    start_urls = ["http://nl.hardware.info/productgroep/21/voedingen"]
    
    rules = (Rule (SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
    , callback="parse_start_url", follow= True),
    )
    
    def parse_start_url(self,response):
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = HtmlXPathSelector(response)
        row = hxs.select('//tr')
        for titles in row:
            webshop = 'Hardware.info'
            name = titles.select('td[@class="top"]/div[@itemscope]/h3/a/span/text()').extract()
            url = titles.select('td[@class="top"]/div/h3/a/@href').extract()
            component = 'voeding'
            desc = titles.select('td[@class="top"]/div[@itemscope]/p[@class="specinfo"]/small/text()').extract()
            price = titles.select('td[@class="center"]/a/text()').extract()
            #image_urls = titles.select('td/div[@class="block-center"]/div[@class="thumb_93"]/a/img/@src').extract()
            print "== Adding Node to database =="

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]
        
            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                    "MERGE (w:Webshop { naam: {webshop} })")
            hw_psu = query_CreateWebshopNode.execute(webshop=webshop)

            query_CreateComponentNode = neo4j.CypherQuery(graph_db,
                                                      "MERGE (c:voeding {naam:{namedb}})")
            hw_psu = query_CreateComponentNode.execute(namedb=namedb)

            query_GiveComponentProperties = neo4j.CypherQuery(graph_db,
                                                          "MATCH (c:voeding) WHERE c.naam = {namedb} SET c.geluid={geluid}, c.vermogen={vermogen}, c.zuinigheid={zuinigheid}")
            hw_psu = query_GiveComponentProperties.execute(namedb=namedb, vermogen=vermogen,
                                                         geluid=geluid, zuinigheid=zuinigheid)

            query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                                                      "MATCH (c:voeding)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
            hw_psu = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)

            query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                                                          "MATCH (c:behuizing), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
            hw_psu = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                                                         price=price, url=url)