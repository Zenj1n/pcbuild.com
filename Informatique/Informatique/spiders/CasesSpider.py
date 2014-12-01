import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from scrapy.selector import Selector
from scrapy.http import HtmlResponse
from py2neo import rel, node
from py2neo import neo4j


from Informatique.items import  InformatiqueItem

class CasesSpider(CrawlSpider):
    name = "INF_CASES"
    allowed_domains = ["informatique.nl"]
    start_urls = ["http://www.informatique.nl/?m=usl&g=004&view=6&&sort=pop&pl=800"]
    
    rules = (Rule (SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
     , callback="parse_start_url", follow= True),
    )
    
    def parse_start_url(self,response):
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = Selector(response)
        titles = hxs.xpath('//ul[@id="detailview"]/li')
        for titles in titles:
           webshop = 'Informatique'
           name = titles.xpath('div[@id="title"]/a/text()').extract()
           url = titles.xpath('div[@id="title"]/a/@href').extract()
           desc = titles.xpath('div[@id="description"]/ul/li/text()').extract()
           price = titles.xpath('div[@id="price"]/text()').extract()
           image_urls = titles.xpath('div[@id="image"]/a/img/@src').extract()
        
           print "== Adding Node to database =="
        
           query = neo4j.CypherQuery(graph_db, "CREATE (inf_case {webshop:{webshop}, name:{name}, url:{url}, desc:{desc}, price:{price}})"
                              "RETURN inf_case")
                              
           inf_case = query.execute(webshop=webshop, name=name, url=url, desc=desc, price=price)