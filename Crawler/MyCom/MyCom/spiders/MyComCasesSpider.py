import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector


from MyCom.items import  MyComItem

class CasesSpider(CrawlSpider):
    name = "lol"
    allowed_domains = ["alternate.nl"]
    start_urls = ["https://www.alternate.nl/html/product/listing.html?navId=2436"]
    
    #rules = (Rule (SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
    #, callback="parse_start_url", follow= True),
    #)
    
    def parse_start_url(self,response):
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//div[@id="listingResult"]')
        items = []
        for titles in titles:
           item = MyComItem()
           #item['webshop'] = 'MyCom'
           item['name'] = titles.select('div[@class="listRow"]/a/span[@class="product"]/span[@class="description"]/h2/span[@class="name"]/span/text()').extract()
           #item['url'] = titles.select('td[@class="top"]/div/h3/a/@href').extract()
           #item['desc'] = titles.select('td[@class="top"]/div[@itemscope]/p[@class="specinfo"]/small/text()').extract()
           #item['price'] = titles.select('td[@class="center"]/a/text()').extract()
           #item['image_urls'] = titles.select('td/div[@class="block-center"]/div[@class="thumb_93"]/a/img/@src').extract()
           #items.append(item)
        return (items)
