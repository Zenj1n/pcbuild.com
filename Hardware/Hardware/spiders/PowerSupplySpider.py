import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import rel, node
from py2neo import neo4j


from Hardware.items import  HWItem

class PowerSupplySpider(CrawlSpider):
    name = "HW_PSU"
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
           url2 = titles.select('td[@class="top"]/div/h3/a/@href').extract()
           url1 = str(url2)
           url = 'www.nl.hardware.info' + url1
           desc = titles.select('td[@class="top"]/div[@itemscope]/p[@class="specinfo"]/small/text()').extract()
           price = titles.select('td[@class="center"]/a/text()').extract()
           
           print "== Adding Node to database =="
        
           query = neo4j.CypherQuery(graph_db, "CREATE (hw_psu {webshop:{webshop}, name:{name}, url:{url}, desc:{desc}, price:{price}})"
                              "RETURN hw_psu")
                              
           hw_psu = query.execute(webshop=webshop, name=name, url=url, desc=desc, price=price)