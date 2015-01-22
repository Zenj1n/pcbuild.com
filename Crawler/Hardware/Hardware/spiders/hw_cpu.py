import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import rel, node
from py2neo import neo4j

import time



class hw_cpu(CrawlSpider):
    name = "hw_cpu"
    allowed_domains = ["hardware.info"]
    start_urls = ["http://nl.hardware.info/productgroep/3/processors"]
    
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
            component = 'processor'
            desc = titles.select('td[@class="top"]/div[@itemscope]/p[@class="specinfo"]/small/text()').extract()
            price = titles.select('td[@class="center"]/a/text()').extract()
            #image_urls = titles.select('td/div[@class="block-center"]/div[@class="thumb_93"]/a/img/@src').extract()


            #filter de data---------------------------------------------------------------------------------------------
            try:
                socket = ','.join(desc).split(",")[0].strip();
            except:
                socket = "onbekend"
            try:
                kloksnelheid = ','.join(desc).split(",")[1].strip()
            except:
                kloksnelheid = "onbekend"

            try:
                kernen = ','.join(desc).split(",")[2].strip()
            except:
                kernen  = "onbekend"


            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            #voeg eventueel missende specificaties toe aan componenten--------------------------------------------------

            query_VoegSpecificatiesToe = neo4j.CypherQuery(graph_db,
            "MATCH (c:processor)  WHERE c.naam = {namedb} SET c.kloksnelheid = {kloksnelheid}, c.kernen = {kernen}")
            hw_cpu = query_VoegSpecificatiesToe.execute(namedb=namedb, kloksnelheid=kloksnelheid, kernen = kernen)
            time.sleep(10)