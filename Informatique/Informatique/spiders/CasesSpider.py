import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector


from Informatique.items import  InformatiqueItem

class CasesSpider(CrawlSpider):
    name = "INF_CASES"
    allowed_domains = ["informatique.nl"]
    start_urls = ["http://www.informatique.nl/?m=usl&g=004&view=6&&sort=pop&pl=800"]
    
   #rules = (Rule (SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
   # , callback="parse_start_url", follow= True),
   #)
    
    def parse_start_url(self,response):
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//ul[@id="detailview"]/li')
        items = []
        for titles in titles:
           item = InformatiqueItem()
           item['webshop'] = 'Informatique'
           item['name'] = titles.select('div[@id="title"]/a/text()').extract()
           item['url'] = titles.select('div[@id="title"]/a/@href').extract()
           item['desc'] = titles.select('div[@id="description"]/ul/li/text()').extract()
           item['price'] = titles.select('div[@id="price"]/text()').extract()
           item['image_urls'] = titles.select('div[@id="image"]/a/img/@src').extract()
           items.append(item)
        return (items)