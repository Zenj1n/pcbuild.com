import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import Selector
from scrapy.http import HtmlResponse
from py2neo import rel, node
from py2neo import neo4j
from scrapy.selector import HtmlXPathSelector

import time


class hw_case(CrawlSpider):
    name = "hw_case"
    allowed_domains = ["hardware.info"]
    start_urls = [
        "http://nl.hardware.info/productgroep/7/behuizingen"
    ]

    rules = (
    Rule(SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]')), callback='parse_start_url', follow=True),)

    def parse_start_url(self, response):
        graph_db = neo4j.GraphDatabaseService("http://Horayon:Zenjin@localhost:8080/db/data/")
        hxs = HtmlXPathSelector(response)
        row = hxs.select('//tr')
        for titles in row:
            webshop = 'Hardware.info'
            name = titles.select('td[@class="top"]/div[@itemscope]/h3/a/span/text()').extract()
            url = titles.select('td[@class="top"]/div/h3/a/@href').extract()
            component = 'behuizing'
            desc = titles.select('td[@class="top"]/div[@itemscope]/p[@class="specinfo"]/small/text()').extract()
            price = titles.select('td[@class="center"]/a/text()').extract()
            # image_urls = titles.select('td/div[@class="block-center"]/div[@class="thumb_93"]/a/img/@src').extract()

            #filter de data---------------------------------------------------------------------------------------------
            try:
                vormfactor = ','.join(desc).split(",")[1].strip();
            except:
                vormfactor = "onbekend"

            try:
                type = ','.join(desc).split(",")[0].strip();
            except:
                type = "onbekend"
            interfaces = "onbekend"
            try:
                vormvoeding = ','.join(desc).split(",")[4].strip()
            except:
                vormvoeding = "onbekend"

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            #voeg eventueel missende specificaties toe aan componenten--------------------------------------------------

            query_VoegSpecificatiesToe = neo4j.CypherQuery(graph_db,
                                                           "MATCH (c:behuizing)  WHERE c.naam = {namedb} SET c.type = {type}")
            hw_case = query_VoegSpecificatiesToe.execute(namedb=namedb, type=type)
            time.sleep(10)