__author__ = 'Fabian'

import unittest
from Cleaning import Cleaning

class unittest(unittest.TestCase):

    def test_cleaning(self):
        cleaning = Cleaning()
        test1 = "/[CoolerMaster NSE-300-KKN1]"
        correct = "Coolermaster NSE-300-KKN1"
        test1 = cleaning.clean_name(test1)
        print test1
        self.assertEqual(test1, correct)



