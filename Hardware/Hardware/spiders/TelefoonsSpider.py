import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector


from Hardware.items import  HWPhoneItem

class Storagepider(CrawlSpider):
    name = "HW_PHONE"
    allowed_domains = ["hardware.info"]
    start_urls = ["http://nl.hardware.info/productgroep/86/smartphones#filter:q1Yqzi8qUbJSKsgvKM1JLMosqVTSUSpOTSxKzijPL0opBkoBBQoS01OBLEMgM6koMS9FySpaydhMKbYWAA"]
    
    rules = (Rule (SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
    , callback="parse_start_url", follow= True),
    )
    
    def parse_start_url(self,response):
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//tr')
        items = []
        for titles in titles:
           item = HWPhoneItem()
           item['name'] = titles.select('td[@class="top"]/div[@itemscope]/h3/a/span/text()').extract()
           item['brand'] = 'Apple'
           item['desc'] = titles.select('td[@class="top"]/div[@itemscope]/p[@class="specinfo"]/small/text()').extract()
           item['price'] = titles.select('td[@class="center"]/a/text()').extract()
           items.append(item)
        return (items)