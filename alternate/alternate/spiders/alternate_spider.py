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


    def parse(self, response):
        item = AlternateItem()
        item['name'] = response.xpath('body/div[@id = "pageBig"]/div[@id = "content"]/div[@id = "pageContent"]/div[@id = "listingResult"]/div[@class = "listRow"]/a[@class = "productLink"]/span[@class = "product"]/span[@class = "pic"]/@title').extract()
        item['desc'] = response.xpath('body/div[@id = "pageBig"]/div[@id = "content"]/div[@id ="pageContent"]/div[@id = "listingResult"]/div[@class = "listRow"]/a[@class = "productLink"]/span[@class = "info"]').extract()
        item['price'] = response.xpath('body/div[@id = "pageBig"]/div[@id = "content"]/div[@id ="pageContent"]/div[@id = "listingResult"]/div[@class = "listRow"]/div[@class= "waresSum"]/p/span[@class = "price right right10"]').extract()
        return item

        
