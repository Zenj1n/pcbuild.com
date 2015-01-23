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
                component = Regex.Replace(component," ","_");
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
                component = query_componenten[i].naam;
                component = Regex.Replace(component, " ", "_");
                Run_Rscript(component);
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
