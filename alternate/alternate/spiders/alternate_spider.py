import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector


from alternate.items import AlternateItem

class alternate_spider(CrawlSpider):
    name = "alternate"
    allowed_domains = ["alternate.nl"]
    start_urls = [
        "http://www.alternate.nl/html/product/listing.html?navId=11626&tk=7&lk=9435"
    ]

    #rules = [Rule(SgmlLinkExtractor(allow=[r'product/\d+']), callback='parse_alternate', follow=True)]


    def parse_start_url(self, response):
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//div[@class="listRow"]')
        items = []
        for titles in titles:
            item = AlternateItem()
            item['name'] = titles.select('a[@class = "productLink"]/span[@class = "product"]/span[@class = "pic"]/@title').extract()
            item['desc'] = titles.select('a[@class = "productLink"]/span[@class = "info"]/text()').extract()
            item['price'] = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/text()').extract()
            item['url'] = titles.select('a[@class="productLink"]/@href').extract()
        items.append(item)
        return (items)

        
