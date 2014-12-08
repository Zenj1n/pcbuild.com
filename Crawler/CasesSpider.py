import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector


from alternate.items import  AlternateItem

class CasesSpider(CrawlSpider):
    name = "ALT_CASES"
    allowed_domains = ["alternate.nl"]
    start_urls = [
        "https://www.alternate.nl/html/product/listing.html?navId=2436"
    ]
    
    #rules = (Rule (SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
    #, callback="parse_start_url", follow= True),
    #)
    def parse_start_url(self,response):
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('/div[@class="listRow"]')
        items = AlternateItem()
           item['name'] = titles.select('a[@class="productLink"]/span[@class="product"]/span[@class="pic"]/@title').extract()
           item['url'] = titles.select('a[@class="productLink"]/@href').extract()
           item['desc'] = titles.select('a[@class="productLink"]/span[@class="info"]/text()').extract()
           items.append(item)
        return (items)
