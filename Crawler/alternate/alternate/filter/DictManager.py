import re as regex

class FilterDict():
    '''
    This class is used to filter the dictionaries that are crawled.
    '''
    timesFiltered = 0
    def __init__(self):
        pass

    def filterDictionary(self, dictionary):
        '''
        :rtype : Dict
        :param dictionary: A Dictionary that is just crawled.
        :return:Returns a dictionary that is filtered.
        '''
        filteredDict = {}
        filteredDict = self.filterEmpty(dictionary)
        filteredDict = self.filterUnicode(filteredDict)

        return filteredDict

    def filterEmpty(self, dictionary):
        '''

        :param dictionary: A dictionary that needs to be filtered.
        :return: Returns a dictionary that is filtered from [\xa0], [] and [ ]
        '''
        newDict = {}
        y = 0
        for x in dictionary:
            key =  dictionary.keys()[y]
            value =  dictionary.values()[y]
            y +=1
            pattern = r"\[u'\\xa0'\]"
            pattern2 = r"(\[\])"
            pattern3 = r"(\[\s\])"
            if regex.search(pattern, key) or regex.search(pattern, value):
                continue
            elif regex.search(pattern2, key) or regex.search(pattern2, value):
                continue
            elif regex.search(pattern3, key) or regex.search(pattern3, value):
                continue
            else:
                newDict.update({key.encode('ascii', errors='ignore'): str(value)})
        self.timesFiltered += 1
        return newDict

    def printDict(self, dict):
        '''
        Prints the dictionary that is send in.
        :param dict: A dictionary that needs to be printed
        :return: isPrinted, a Boolean
        '''
        isPrinted = False
        print " ++++++++++++++++++++"
        print " PRINTING NEWEST DICT"
        print " Dict has been %s times filtered!" % self.timesFiltered
        print " Dict has %s lines!" % len(dict)
        print " ++++++++++++++++++++"
        y = 0
        for x in dict:
            key =  dict.keys()[y]
            value =  dict.values()[y]
            print key, value
            y +=1

        isPrinted = True
        return isPrinted

    def filterUnicode(self, dict):
        filteredDict = {}
        y = 0
        for x in dict:
            key =  dict.keys()[y]
            value =  dict.values()[y]
            tempKey = str(key).replace("[u'", "")
            tempValue = str(value).replace("[u'", "")
            tempKey2 = str(tempKey).replace("']", "")
            tempValue2 = str(tempValue).replace("']", "")

            filteredDict.update({tempKey2: tempValue2})
            y+=1
        return filteredDict