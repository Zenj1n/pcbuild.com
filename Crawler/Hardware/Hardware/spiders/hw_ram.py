import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import rel, node
from py2neo import neo4j

import time


class hw_ram(CrawlSpider):
    name = "hw_ram"
    allowed_domains = ["hardware.info"]
    start_urls = ["http://nl.hardware.info/productgroep/20/geheugenmodules"]

    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
                  , callback="parse_start_url", follow=True),
    )

    def parse_start_url(self, response):
        graph_db = neo4j.GraphDatabaseService("http://Horayon:Zenjin@localhost:8080/db/data/")
        hxs = HtmlXPathSelector(response)
        row = hxs.select('//tr')
        for titles in row:
            webshop = 'Hardware.info'
            name = titles.select('td[@class="top"]/div[@itemscope]/h3/a/span/text()').extract()
            url = titles.select('td[@class="top"]/div/h3/a/@href').extract()
            component = 'werkgeheugen'
            desc = titles.select('td[@class="top"]/div[@itemscope]/p[@class="specinfo"]/small/text()').extract()
            price = titles.select('td[@class="center"]/a/text()').extract()
            # image_urls = titles.select('td/div[@class="block-center"]/div[@class="thumb_93"]/a/img/@src').extract()

            #filter de data---------------------------------------------------------------------------------------------

            try:
                capaciteit = ','.join(desc).split(",")[0].strip()
            except:
                capaciteit = "onbekend"
            try:
                modules = ','.join(desc).split(",")[4].strip()
            except:
                modules = "onbekend"
            try:
                ddr = ','.join(desc).split(",")[1].strip()
            except:
                ddr = "onbekend"
            try:
                cl = ','.join(desc).split(",")[5].strip()
            except:
                cl = "onbekend"

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            #voeg eventueel missende specificaties toe aan componenten--------------------------------------------------

            query_VoegSpecificatiesToe = neo4j.CypherQuery(graph_db,
                                                           "MATCH (c:werkgeheugen)  WHERE c.naam = {namedb} SET c.capaciteit = {capaciteit}, c.modules = {modules}, c.ddr = {ddr}, c.cl = {cl}")
            hw_ram = query_VoegSpecificatiesToe.execute(namedb=namedb, capaciteit=capaciteit, modules=modules, ddr=ddr,
                                                        cl=cl)
            time.sleep(10)