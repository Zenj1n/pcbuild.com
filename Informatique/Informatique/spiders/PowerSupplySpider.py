import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import rel, node
from py2neo import neo4j


from Informatique.items import  InformatiqueItem

class CasesSpider(CrawlSpider):
    name = "INF_PS"
    allowed_domains = ["informatique.nl"]
    start_urls = ["http://www.informatique.nl/?m=usl&g=171&view=6&&sort=pop&pl=500"]
    
   #rules = (Rule (SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
   # , callback="parse_start_url", follow= True),
   #)
    
    def parse_start_url(self,response):
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//ul[@id="detailview"]/li')
        for titles in titles:
           webshop = 'Informatique'
           name = titles.select('div[@id="title"]/a/text()').extract()
           url = titles.select('div[@id="title"]/a/@href').extract()
           desc = titles.select('div[@id="description"]/ul/li/text()').extract()
           price = titles.select('div[@id="price"]/text()').extract()
           image_urls = titles.select('div[@id="image"]/a/img/@src').extract()
        
        print "== Adding Node to database =="
        
        query = neo4j.CypherQuery(graph_db, "CREATE (inf_psu {webshop:{webshop}, name:{name}, url:{url}, desc:{desc}, price:{price}})"
                              "RETURN n")
                              
        inf_psu = query.execute(webshop=webshop, name=name, url=url, desc=desc, price=price)