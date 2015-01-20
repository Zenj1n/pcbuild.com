library(ggplot2)
args<-commandArgs(TRUE)
options(echo=TRUE)
Prijshistory = read.csv('E:/Repositories Git Hub/pcbuild.com/Crawler/alternate/components/case/prijsgeschiedenis.csv', header = F)
names(Prijshistory) <- c("datum","naam","prijs")

test = read.csv('E:/Repositories Git Hub/pcbuild.com/Crawler/alternate/components/case/test.csv', header = F)
names(test) <- c("datum","naam","prijs")

var = args[1]
pngpath = paste("E:/Repositories Git Hub/pcbuild.com/Crawler/afbeeldingen",var,".png", sep="")
png(var=pngpath)
ggplot(data=test[test$naam == "a", ], aes(x=datum, y=prijs, group=1)) + geom_line()
dev.off()

