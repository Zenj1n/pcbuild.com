# -*- coding: utf-8 -*-

# Scrapy settings for Informatique project
#
# For simplicity, this file contains only the most important settings by
# default. All the other settings are documented here:
#
#     http://doc.scrapy.org/en/latest/topics/settings.html
#

BOT_NAME = 'Informatique'

SPIDER_MODULES = ['Informatique.spiders']
NEWSPIDER_MODULE = 'Informatique.spiders'
#ITEM_PIPELINES = {'scrapy.contrib.pipeline.images.ImagesPipeline': 1}
#IMAGES_STORE = 'C:\Users\hoye\Documents\GitHub\Python-Crawler\Informatique\Images'
#IMAGES_EXPIRES = 90
DOWNLOAD_DELAY = 10.0

DOWNLOADER_MIDDLEWARES = {
    'scrapy.contrib.downloadermiddleware.downloadtimeout.DownloadTimeoutMiddleware': 543,
}

EXTENSIONS = {
    'scrapy.contrib.closespider.CloseSpider': 500
}

# Crawl responsibly by identifying yourself (and your website) on the user-agent
#USER_AGENT = 'Informatique (+http://www.yourdomain.com)'

