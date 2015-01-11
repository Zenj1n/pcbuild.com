import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import rel, node
from py2neo import neo4j


from Hardware.items import  HWItem

class hw_gfx(CrawlSpider):
    name = "hw_gfx"
    allowed_domains = ["hardware.info"]
    start_urls = ["http://nl.hardware.info/productgroep/5/videokaarten"]
    
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
            component = 'videokaart'
            desc = titles.select('td[@class="top"]/div[@itemscope]/p[@class="specinfo"]/small/text()').extract()
            price = titles.select('td[@class="center"]/a/text()').extract()
            #image_urls = titles.select('td/div[@class="block-center"]/div[@class="thumb_93"]/a/img/@src').extract()

            try:
                gfx = ','.join(desc).split(",")[0]
            except:
                gfx = "onbekend"
            try:
                geheugen = ','.join(desc).split(",")[4]
            except:
                geheugen = "onbeklend"
            try:
                slots = ','.join(desc).split(",")[2]
            except:
                slots = "onbekend"

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]
        
            print "== Adding Node to database =="

            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                        "MERGE (w:Webshop { naam: {webshop} })")
            hw_gfx = query_CreateWebshopNode.execute(webshop=webshop)

            query_CheckOnExistingComponent = neo4j.CypherQuery(graph_db,
                                                      "match (c:videokaart) where c.naam = {namedb} with COUNT(c) as Count_C RETURN Count_C")
            matchCount = query_CheckOnExistingComponent.execute(namedb=namedb)
            for record in query_CheckOnExistingComponent.stream(namedb=namedb):
                matchCountNumber = record[0]

            if matchCountNumber != 0:
                query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                "MATCH (c:videokaart)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
                hw_gfx = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                "MATCH (c:videokaart), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                hw_gfx = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop, price=price, url=url)
            else:
                query_CreateComponentNode = neo4j.CypherQuery(graph_db,
                "Create (c:videokaart {naam:{namedb}, gfx:{gfx}, geheugen:{geheugen}, slots:{slots}})")
                hw_gfx = query_CreateComponentNode.execute(namedb=namedb, gfx=gfx,
                geheugen=geheugen, slots=slots)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                "MATCH (c:videokaart), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                hw_gfx = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                price=price, url=url)