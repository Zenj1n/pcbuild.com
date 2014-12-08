import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import rel, node
from py2neo import neo4j


from alternate.items import AlternateItem

class alt_tvcard(CrawlSpider):
    name = "alt_tvcard"
    allowed_domains = ["alternate.nl"]
    start_urls = [
        "http://www.alternate.nl/html/product/listing.html?navId=960&tk=7&lk=9525",
        "http://www.alternate.nl/html/product/listing.html?navId=964&tk=7&lk=9522",
        "http://www.alternate.nl/html/product/listing.html?navId=962&tk=7&lk=9524",
        "http://www.alternate.nl/html/product/listing.html?navId=966&tk=7&lk=9523"
    ]

    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[@class="next"]')), callback='parse_start_url', follow=True),)

    def parse_start_url(self,response):
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//div[@class="listRow"]')
        for titles in titles:
           webshop = 'alternate.nl'
           name = titles.select('a[@class="productLink"]/span[@class="product"]/span[@class="pic"]/@title').extract()
           url = titles.select('a[@class="productLink"]/@href').extract()
           desc = titles.select('a[@class="productLink"]/span[@class="info"]/text()').extract()
           euro = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/text()').extract()
           cent = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/sup/text()').extract()
           
           price = euro + cent
           
           price = euro + cent
           
           namestring = ''.join(name)
           namesplit = namestring.split(",")
           namedb = namesplit[0]
        
        
           print "== Adding Node to database =="
        
           query = neo4j.CypherQuery(graph_db, "CREATE (alt_tvcard {webshop:{webshop}, name:{namedb}, url:{url}, desc:{desc}, price:{price}})"
                              "RETURN alt_tvcard")
                              
           alt_tvcard = query.execute(webshop=webshop, namedb=namedb, url=url, desc=desc, price=price) 