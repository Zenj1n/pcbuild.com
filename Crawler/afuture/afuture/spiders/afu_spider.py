from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import neo4j


class alt_case(CrawlSpider):
    name = "afu_spider"
    allowed_domains = ["afuture.nl"]
    start_urls = [
        "https://afuture.nl/productlist.php?categoryID=237"
    ]

    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[@id="21"]')), callback='parse_start_url', follow=True),)

    def parse_start_url(self, response):
        #graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//tr')
        print "== Initializing =="

        for titles in titles:
            webshop = 'afuture.nl'
            name = titles.select('td/a/text()').extract()
            url = titles.select('td/a/@href').extract()
            desc = titles.select('td/p/text()').extract()
            euro = titles.select('td/span/text()').extract()
            #cent = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/sup/text()').extract()

            #interfaces = desc[0];
            #vormfactor = desc[1];
            #vormvoeding = desc[2];

            #price = euro + cent



            namesplit = ''.join(name).split(",")
            #namedb = namesplit[0]

            #query_test = 'MATCH (c:behuizing), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:'+price+' ]-w'

            print "== Adding Node to database =="
            print name, url, desc, euro