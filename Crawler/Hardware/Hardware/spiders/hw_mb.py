import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import rel, node
from py2neo import neo4j


from Hardware.items import  HWItem

class hw_mb(CrawlSpider):
    name = "hw_mb"
    allowed_domains = ["hardware.info"]
    start_urls = ["http://nl.hardware.info/productgroep/1/moederborden"]
    
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
            component = 'moederbord'
            desc = titles.select('td[@class="top"]/div[@itemscope]/p[@class="specinfo"]/small/text()').extract()
            price = titles.select('td[@class="center"]/a/text()').extract()
            #image_urls = titles.select('td/div[@class="block-center"]/div[@class="thumb_93"]/a/img/@src').extract()

            try:
                ddr = ','.join(desc).split(",")[2].strip().lower()
            except:
                ddr = "onbekend"
            try:
                vormfactor = ','.join(desc).split(",")[0].strip()
            except:
                vormfactor = "onbekend"
            interfaces = "onbekend"
            try:
                socket = ','.join(desc).split(",")[1].strip()
            except:
                socket = "onbekend"

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query_VoegSpecificatiesToe = neo4j.CypherQuery(graph_db,
            "MATCH (c:moederbord)  WHERE c.naam = {namedb} SET c.ddr = {ddr}")
            hw_mb = query_VoegSpecificatiesToe.execute(namedb=namedb, ddr=ddr)