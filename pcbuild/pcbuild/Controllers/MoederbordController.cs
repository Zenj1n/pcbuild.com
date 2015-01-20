﻿using Neo4jClient;
using pcbuild.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections;
using Neo4jClient.Cypher;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using pcbuild.Models.MoederbordModels;

namespace pcbuild.Controllers
{
    public class MoederbordController : ProcessorController
    {
        // GET: Moederbord
        public static LijstModel lijstModel = new LijstModel();

        public ActionResult Index(string processor, string socket, string prijs, string webshop)
        {
            //Maak cookie arrays
            HttpCookie processor_cookie = new HttpCookie("processor_cookie");
            HttpCookie processorprijs_cookie = new HttpCookie("processorprijs_cookie");
            HttpCookie processorwebshop_cookie = new HttpCookie("processorwebshop_cookie");

            Debug.WriteLine(socket);

            string socket_search = "(?i).*" + socket + ".*";
            int processor_prijs = Convert.ToInt32(prijs);

            lijstModel.processor = processor;

            //voeg data toe aan cookies
            processor_cookie.Value = processor;
            processorprijs_cookie.Value = prijs;
            processorwebshop_cookie.Value = webshop;

            //save the cookies!!!
            Response.Cookies.Add(processor_cookie);
            Response.Cookies.Add(processorprijs_cookie);
            Response.Cookies.Add(processorwebshop_cookie);

            //Connectie met database
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            var componenten_query = client
           .Cypher
           .Match("(n:moederbord)-[r:verkrijgbaar]-(p:Webshop)")
           .Where("n.naam =~ {socket_c}")
           .WithParam("socket_c", socket_search)
           .Return((n, r, p) => new ViewModelMoederbord
           {
               Moederbord_all = n.As<Moederbord_Model>(),
               Verkrijgbaar_all = r.As<Verkrijgbaar_Model>(),
               Webshop_all = p.As<Webshop_Model>(),
           })
          .Results;


            return View(componenten_query);
        }

    }
}