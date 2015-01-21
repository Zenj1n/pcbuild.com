import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import rel, node
from py2neo import neo4j


import time

class hw_psu(CrawlSpider):
    name = "hw_psu"
    allowed_domains = ["hardware.info"]
    start_urls = ["http://nl.hardware.info/productgroep/21/voedingen"]
    
    rules = (Rule (SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
    , callback="parse_start_url", follow= True),
    )
    
    def parse_start_url(self,response):
        graph_db = neo4j.GraphDatabaseService("http://Horayon:Zenjin@localhost:8080/db/data/")
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

            try:
                vermogen = ','.join(desc).split(",")[0].strip();
            except:
                vermogen = "onbekend"
            geluid = "onbekend"
            try:
                zuinigheid = ','.join(desc).split(",")[2].strip()
            except:
                zuinigheid = "onbekend"
            try:
                type = ','.join(desc).split(",")[3].strip()
                if type != "modulair":
                    type = "niet modulair"
            except:
                type = "onbekend"


            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query_VoegSpecificatiesToe = neo4j.CypherQuery(graph_db,
            "MATCH (c:voeding)  WHERE c.naam = {namedb} SET c.vermogen = {vermogen}, c.zuinigheid = {zuinigheid}, c.type = {type}")
            hw_psu = query_VoegSpecificatiesToe.execute(namedb=namedb, vermogen=vermogen, zuinigheid = zuinigheid, type=type)
            time.sleep(10)