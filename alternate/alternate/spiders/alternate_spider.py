import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from alternate.items import AlternateItem

class alternate_spider(CrawlSpider):
    name = "alternate"
    allowed_domains = ["alternate.nl"]
    start_urls = [
        "http://www.alternate.nl/Club-3D/Radeon-R9-290-royalKing-grafische-kaart/html/product/1126000"
    ]

    rules = [Rule(SgmlLinkExtractor(allow=[r'product/\d+']), callback='parse_alternate', follow=True)]


    def parse_alternate(self, response):
        item = AlternateItem()
        item['url'] = response.xpath('head/link[@rel = "canonical"]/@href').extract()
        item['name'] = response.xpath('body/div[@id = "page"]/div[@id = "content"]/div[@id = "contentWrapper"]/div[@id = "pageContent"]/form[@id = "buyProduct"]/div[@class = "productNameContainer"]/meta[@itemprop = "name"]/@content').extract()
        item['description'] = response.xpath('body/div[@id = "page"]/div[@id = "content"]/div[@id = "contentWrapper"]/div[@id = "pageContent"]/form[@id = "buyProduct"]/div[@class = "tabberBox"]/div[@class = "content"]/div[@id = "details"]/div[@class = "description"]/meta[@itemprop = "description"]/@content').extract()
        item['price'] = response.xpath('//span[@itemprop = "price"]/@content').extract()
        return item

        
