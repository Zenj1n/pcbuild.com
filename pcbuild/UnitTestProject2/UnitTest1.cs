using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4jClient;
using pcbuild.Controllers;
using pcbuild.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Neo4jClient.Cypher;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace UnitTestProject2
{

    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Test of er een connectie is met de database
        /// </summary>
        [TestMethod]
        public void TestConnectionDatabase()
        {
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@145.24.222.155:8080/db/data"));
            client.Connect();
        }

        /// <summary>
        /// Test of hij data uit de database kan lezen
        /// </summary>
        [TestMethod]
        public void TestQueryRead()
        {
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@145.24.222.155:8080/db/data"));
            client.Connect();

            client
            .Cypher
            .Match("(n)");
        }

        /// <summary>
        /// Test de query van processors om data op te halen met een Where statement
        /// </summary>
        [TestMethod]
        public void TestQueryProcessor()
        {
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@145.24.222.155:8080/db/data"));
            client.Connect();

            client
              .Cypher
              .Match("(n:processor)-[r:verkrijgbaar]-(p:Webshop)")
              .Where((Webshop_Model p) => p.naam == "alternate.nl" || p.naam == "Informatique");
        }

        /// <summary>
        /// Test de query van moederborden om data op te halen met parameters
        /// </summary>
        [TestMethod]
        public void TestQueryMoederbord()
        {
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@145.24.222.155:8080/db/data"));
            client.Connect();

            string ddr_mb = ".*.*";
            string socket = "AM3";

            client
           .Cypher
           .Match("(n:moederbord)-[r:verkrijgbaar]-(p:Webshop)")
           .Where("n.socket = {socket_c}")
           .AndWhere("n.ddr =~ {ddr_mb}")
           .WithParam("socket_c", socket)
           .WithParam("ddr_mb", ddr_mb);
        }

        /// <summary>
        /// Test de query van videokaart om data te halen
        /// </summary>
        [TestMethod]
        public void TestQueryVideokaart()
        {
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@145.24.222.155:8080/db/data"));
            client.Connect();

            client
              .Cypher
              .Match("(n:videokaart)-[r:verkrijgbaar]-(p:Webshop)");
        }

        /// <summary>
        /// Test query van werkgeheugen om data op te halen met een ddr parameter
        /// </summary>
        [TestMethod]
        public void TestQueryWerkgeheugen()
        {
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@145.24.222.155:8080/db/data"));
            client.Connect();

            string ddr = "ddr3";

            client
              .Cypher
              .Match("(n:werkgeheugen)-[r:verkrijgbaar]-(p:Webshop)")
              .Where("n.ddr = {ddr_query}")
              .WithParam("ddr_query", ddr);
        }

        /// <summary>
        /// Test query van behuzing om data op te halen met een vormfactor parameter
        /// </summary>
        [TestMethod]
        public void TestQueryBehuizing()
        {
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@145.24.222.155:8080/db/data"));
            client.Connect();

            string vormfactor = "ATX";
            string vormfactor_search = "(?i)" + vormfactor[0] + vormfactor[1] + vormfactor[2] + ".*";

            client
              .Cypher
              .Match("(n:behuizing)-[r:verkrijgbaar]-(p:Webshop)")
              .Where("n.vormfactor =~ {vormfactor_c}")
              .WithParam("vormfactor_c", vormfactor_search);
        }
        /// <summary>
        /// Test query van opslag op om data op te halen
        /// </summary>
        [TestMethod]
        public void TestQueryOpslag()
        {
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@145.24.222.155:8080/db/data"));
            client.Connect();

            client
              .Cypher
              .Match("(n:opslag)-[r:verkrijgbaar]-(p:Webshop)");
        }

        /// <summary>
        /// Test query van voedingen op om data op te halen
        /// </summary>
        [TestMethod]
        public void TestQueryVoeding()
        {
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@145.24.222.155:8080/db/data"));
            client.Connect();

            client
               .Cypher
               .Match("(n:voeding)-[r:verkrijgbaar]-(p:Webshop)");
        }
    }
}
