from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import neo4j

import csv
import datetime


class alt_koel(CrawlSpider):
    name = "alt_koel_case"
    allowed_domains = ["alternate.nl"]
    start_urls = [
        "http://www.alternate.nl/html/product/listing.html?navId=11568&bgid=8215&tk=7&lk=9346",
        "http://www.alternate.nl/html/product/listing.html?navId=17557&bgid=8459&tk=7&lk=9601",
        "http://www.alternate.nl/html/product/listing.html?navId=11898&bgid=8215&tk=7&lk=9344",
        "http://www.alternate.nl/html/product/listing.html?navId=749&tk=7&lk=9345",
        "http://www.alternate.nl/html/product/listing.html?navId=756&tk=7&lk=9359",
        "http://www.alternate.nl/html/product/listing.html?navId=805&navId=806&tk=7&lk=13094",
        "http://www.alternate.nl/html/product/listing.html?navId=758&tk=7&lk=9350"
    ]

    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[@class="next"]')), callback='parse_start_url', follow=True),)

    def parse_start_url(self, response):
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//div[@class="listRow"]')
        for titles in titles:
            webshop = 'alternate.nl'
            name = titles.select('a[@class="productLink"]/span[@class="product"]/span[@class="pic"]/@title').extract()
            url = titles.select('a[@class="productLink"]/@href').extract()
            component = 'fan behuizing'
            desc = titles.select('a[@class="productLink"]/span[@class="info"]/text()').extract()
            euro = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/text()').extract()
            cent = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/sup/text()').extract()

            price = euro + cent

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query = neo4j.CypherQuery(graph_db,
                                      "CREATE (alt_koel {webshop:{webshop}, name:{namedb}, url:{url}, desc:{desc}, price:{price}, component:{component}})"
                                      "RETURN alt_koel")

            alt_koel = query.execute(webshop=webshop, namedb=namedb, url=url, desc=desc, price=price,
                                     component=component)