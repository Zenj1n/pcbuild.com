from pyasn1.compat.octets import null
from scrapy.selector import Selector

__author__ = 'Fabian'

import unittest
from Cleaning import Cleaning
from Database import Database

class unittest(unittest.TestCase):

    def test_cleaning(self):
        cleaning = Cleaning()
        test1 = "/[CoolerMaster NSE-300-KKN1]"
        correct = "CoolerMaster NSE-300-KKN1"
        test1 = cleaning.clean_name(test1)
        self.assertEqual(test1, correct)

    def test_cleaningPrice(self):
        cleaning = Cleaning()
        test1 = " /[335,-]"
        correct = "335,00"
        test1 = cleaning.clean_price(test1)
        self.assertEqual(test1, correct)

    def test_crawlNaam(self):
        database = Database()
        databasetest = database.parse_title("http://www.informatique.nl/?m=usl&g=004&view=6&&sort=pop&pl=800")
        self.assertIsNot(databasetest, null)

    def test_crawlUrl(self):
        database = Database()
        databasetest = database.parse_url("http://www.informatique.nl/?m=usl&g=004&view=6&&sort=pop&pl=800")
        self.assertIsNot(databasetest, null)

    def test_crawlDesc(self):
        database = Database()
        databasetest = database.parse_desc("http://www.informatique.nl/?m=usl&g=004&view=6&&sort=pop&pl=800")
        self.assertIsNot(databasetest, null)

    def test_crawlPrice(self):
        database = Database()
        databasetest = database.parse_price("http://www.informatique.nl/?m=usl&g=004&view=6&&sort=pop&pl=800")
        self.assertIsNot(databasetest, null)

    def test_dbCase(self):
        database = Database()
        databasetest = database.check_dbCase()
        self.assertIsNot(databasetest, null)

    def test_dbRam(self):
        database = Database()
        databasetest = database.check_dbRam()
        self.assertIsNot(databasetest, null)

    def test_dbMB(self):
        database = Database()
        databasetest = database.check_dbMB()
        self.assertIsNot(databasetest, null)

    def test_dbOpslag(self):
        database = Database()
        databasetest = database.check_dbOpslag()
        self.assertIsNot(databasetest, null)

    def test_dbCpu(self):
        database = Database()
        databasetest = database.check_dbCpu()
        self.assertIsNot(databasetest, null)

    def test_dbPsu(self):
        database = Database()
        databasetest = database.check_dbPsu()
        self.assertIsNot(databasetest, null)




