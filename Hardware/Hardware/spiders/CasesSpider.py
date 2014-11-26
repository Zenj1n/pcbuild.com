import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import Selector
from scrapy.http import HtmlResponse
from py2neo import rel, node
from py2neo import neo4j

from Hardware.items import HWItem

class CasesSpider(CrawlSpider):
    name = "HW_Cases"
    allowed_domains = ["hardware.info"]
    start_urls = ["http://nl.hardware.info/productgroep/7/behuizingen"]
    
    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]')), callback='parse_start_url', follow=True),)
    
    def parse_start_url(self,response):
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = Selector(response)
        titles = hxs.xpath('//tr')
        items = []
        for titles in titles:
           webshop = 'Hardware.info'
           name = titles.xpath('td[@class="top"]/div[@itemscope]/h3/a/span/text()').extract()
           url = titles.xpath('td[@class="top"]/div/h3/a/@href').extract()
           desc = titles.xpath('td[@class="top"]/div[@itemscope]/p[@class="specinfo"]/small/text()').extract()
           price = titles.xpath('td[@class="center"]/a/text()').extract()
           image_urls = titles.xpath('td/div[@class="block-center"]/div[@class="thumb_93"]/a/img/@src').extract()
        
           
           
           print "== Adding Node to database =="
        
        query = neo4j.CypherQuery(graph_db, "CREATE (hw_case {webshop:{webshop}, name:{name}, url:{url}, desc:{desc}, price:{price}})"
                             "RETURN hw_case")
                              
        hw_case = query.execute(webshop=webshop, name=name, url=url, desc=desc, price=price)
