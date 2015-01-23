using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient.Cypher;
using Neo4jClient;
using System.Text.RegularExpressions;
using System.Diagnostics;
namespace r_script_c
{
    /*Script doet het wel op m'n desktop maar niet op de server. 
     * Op m'n desktop wacht hij nu tot dat de vorige cmd.exe 
     * is afgesloten voordat hij de volgende opent. Op de server 
     * loopt hij nogsteeds van en duurt ongeveer 20 min voordat 
     * je weer wat kan doen. Verder heb ik dit script nog op m'n desktop
     * getest door te syncen met git, de database naar die van git te wijzigen, 
     * en de prijshistory naar m'n pc gekopieerd maardan werkt het nogsteeds
     * niet doordat de namen van de databse 
     * niet gelijk zijn met de namen in het csv bestand. 
     * Misschien dat het nog een oude csv 
     * is op misschien nog nooit geupdate is maar bij mij laat hij ook wat 
     * specificaties bij de naam zien om 1 of andere reden. Daardoor staan de namen van de database 
     * niet gelijk aan die in het csv bestand en kan hij niet de grafieken maken. In de crawlers worden er 
     * namelijk 2 verschillende string weggeschreven. 
     * "Namedb" naar de databse en "name" naar het csv bestand. Volgens mij moeten beide "namedb" zijn.
     * Het zou fijn zijn als iemand dat morgen kan testen aangezien ik niet op m'n desktop kan crawlen.
     */
     
    class Program
    {
        static void Main(string[] args)
        {
            CPU_grafiek();
            GPU_grafiek();
            PSU_grafiek();
            Moederbord_grafiek();
            Case_grafiek();
            RAM_grafiek();
            Opslag_grafiek();
        }
        public static void Run_Rscript(string component)
        {
            string strCmdText;
            strCmdText = "/C Rscript prijshistory.r " + component;
            ProcessStartInfo start = new ProcessStartInfo("cmd.exe");
            start.Arguments = strCmdText;
            Process cmd = Process.Start(start);
            Console.WriteLine(strCmdText);
            Console.WriteLine("Prijshistory van " + component + " aan get maken");
            cmd.WaitForExit();
        }

        public static void Moederbord_grafiek()
        {
            string component;
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@localhost:8080/db/data"));
            client.Connect();
            var query_componenten = client
           .Cypher
           .Match("(n:moederbord)")
           .Return(n => n.As<moederbord>())
           .Results
           .ToArray();
            for (int i = 0; i < query_componenten.Length; i++)
            {
                component = query_componenten[i].naam;
                component = Regex.Replace(component, " ", "_");
                Run_Rscript(component);
            }
        }

        public static void CPU_grafiek()
        {
            string component;
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@localhost:8080/db/data"));
            client.Connect();
            var query_componenten = client
           .Cypher
           .Match("(n:processor)")
           .Return(n => n.As<processor>())
           .Results
           .ToArray();
            for (int i = 0; i < query_componenten.Length; i++)
            {
                try
                {
                    component = query_componenten[i].naam;
                    component = Regex.Replace(component, " ", "_");
                    Run_Rscript(component);
                }
                catch
                {
                    break;
                }

            }
        }

        public static void GPU_grafiek()
        {
            string component;
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@localhost:8080/db/data"));
            client.Connect();
            var query_componenten = client
           .Cypher
           .Match("(n:videokaart)")
           .Return(n => n.As<videokaart>())
           .Results
           .ToArray();
            for (int i = 0; i < query_componenten.Length; i++)
            {
                component = query_componenten[i].naam;
                component = Regex.Replace(component, " ", "_");
                Run_Rscript(component);
            }
        }

        public static void RAM_grafiek()
        {
            string component;
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@localhost:8080/db/data"));
            client.Connect();
            var query_componenten = client
           .Cypher
           .Match("(n:werkgeheugen)")
           .Return(n => n.As<werkgeheugen>())
           .Results
           .ToArray();
            for (int i = 0; i < query_componenten.Length; i++)
            {
                component = query_componenten[i].naam;
                component = Regex.Replace(component, " ", "_");
                Run_Rscript(component);
            }
        }
        public static void Case_grafiek()
        {
            string component;
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@localhost:8080/db/data"));
            client.Connect();
            var query_componenten = client
           .Cypher
           .Match("(n:behuizing)")
           .Return(n => n.As<behuizing>())
           .Results
           .ToArray();
            for (int i = 0; i < query_componenten.Length; i++)
            {
                component = query_componenten[i].naam;
                component = Regex.Replace(component, " ", "_");
                Run_Rscript(component);
            }
        }
        public static void Opslag_grafiek()
        {
            string component;
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@localhost:8080/db/data"));
            client.Connect();
            var query_componenten = client
           .Cypher
           .Match("(n:opslag)")
           .Return(n => n.As<opslag>())
           .Results
           .ToArray();
            for (int i = 0; i < query_componenten.Length; i++)
            {
                component = query_componenten[i].naam;
                component = Regex.Replace(component, " ", "_");
                Run_Rscript(component);
            }
        }

        public static void PSU_grafiek()
        {
            string component;
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@localhost:8080/db/data"));
            client.Connect();
            var query_componenten = client
           .Cypher
           .Match("(n:voeding)")
           .Return(n => n.As<voeding>())
           .Results
           .ToArray();
            for (int i = 0; i < query_componenten.Length; i++)
            {
                component = query_componenten[i].naam;
                component = Regex.Replace(component, " ", "_");
                Run_Rscript(component);
            }
        }

    }
}
