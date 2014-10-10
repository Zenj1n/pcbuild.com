import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector


from Hardware.items import  HWItem

class CasesSpider(CrawlSpider):
    name = "HW_Cases"
    allowed_domains = ["hardware.info"]
    start_urls = ["http://nl.hardware.info/productgroep/7/behuizingen"]
    
    rules = (Rule (SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
    , callback="parse_start_url", follow= True),
    )
    
    def parse_start_url(self,response):
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//tr')
        items = []
        for titles in titles:
           item = HWItem()
           item['webshop'] = 'Hardware.info'
           item['name'] = titles.select('td[@class="top"]/div[@itemscope]/h3/a/span/text()').extract()
           item['url'] = titles.select('td[@class="top"]/div/h3/a/@href').extract()
           item['desc'] = titles.select('td[@class="top"]/div[@itemscope]/p[@class="specinfo"]/small/text()').extract()
           item['price'] = titles.select('td[@class="center"]/a/text()').extract()
           item['image_urls'] = titles.select('td/div[@class="block-center"]/div[@class="thumb_93"]/a/img/@src').extract()
           items.append(item)
        return (items)