library("ggplot2")
Prijshistory = read.csv('E:/Repositories Git Hub/pcbuild.com/Crawler/alternate/components/case/prijsgeschiedenis.csv', header = F)
names(Prijshistory) <- c("datum","naam","prijs")

test = read.csv('E:/Repositories Git Hub/pcbuild.com/Crawler/alternate/components/case/test.csv', header = F)
names(test) <- c("datum","naam","prijs")


plot(test$datum, test$prijs, type="b")
ggplot(data=test, aes(x=datum, y=prijs, group=1)) + geom_line()
