import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector


from alternate.items import AlternateItem

class alt_cpu(CrawlSpider):
    name = "alt_cpu"
    allowed_domains = ["alternate.nl"]
    start_urls = [
        "http://www.alternate.nl/html/product/listing.html?navId=11572&bgid=10846&tk=7&lk=9487",
        "http://www.alternate.nl/html/product/listing.html?navId=11576&tk=7&lk=9475"
    ]

    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[@class="next"]')), callback='parse_start_url', follow=True),)

    def parse_start_url(self,response):
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//div[@class="listRow"]')
        items = []
        for titles in titles:
           item = AlternateItem()
           item['webshop'] = 'alternate.nl'
           item['name'] = titles.select('a[@class="productLink"]/span[@class="product"]/span[@class="pic"]/@title').extract()
           item['url'] = titles.select('a[@class="productLink"]/@href').extract()
           item['desc'] = titles.select('a[@class="productLink"]/span[@class="info"]/text()').extract()
           item['price'] = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/text()').extract()
           item['image_urls'] = 'placeholder'
           items.append(item)
        return (items)