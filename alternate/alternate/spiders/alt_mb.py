import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import rel, node
from py2neo import neo4j


from alternate.items import AlternateItem

class alt_mb(CrawlSpider):
    name = "alt_mb"
    allowed_domains = ["alternate.nl"]
    start_urls = [
        "http://www.alternate.nl/html/product/listing.html?navId=11622&tk=7&lk=9419",
        "http://www.alternate.nl/html/product/listing.html?navId=11626&tk=7&lk=9435"
    ]

    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[@class="next"]')), callback='parse_start_url', follow=True),)

    def parse_start_url(self,response):
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//div[@class="listRow"]')
        items = []
        for titles in titles:
           webshop = 'alternate.nl'
           name = titles.select('a[@class="productLink"]/span[@class="product"]/span[@class="pic"]/@title').extract()
           url = titles.select('a[@class="productLink"]/@href').extract()
           desc = titles.select('a[@class="productLink"]/span[@class="info"]/text()').extract()
           price = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/text()').extract()
        
        print "== Adding Node to database =="
        
        query = neo4j.CypherQuery(graph_db, "CREATE (alt_mb {webshop:{webshop}, name:{name}, url:{url}, desc:{desc}, price:{price}})"
                              "RETURN n")
                              
        alt_mb = query.execute(webshop=webshop, name=name, url=url, desc=desc, price=price)