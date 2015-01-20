using Neo4jClient;
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
    public class MoederbordController : Controller
    {
        // GET: Moederbord
        //public static LijstModel lijstModel = new LijstModel();
        public ActionResult Reload(string processor, string socket, string prijs, string webshop)
        {
            HttpCookie processor_cookie = new HttpCookie("processor_cookie");
            HttpCookie processorprijs_cookie = new HttpCookie("processorprijs_cookie");
            HttpCookie processorwebshop_cookie = new HttpCookie("processorwebshop_cookie");
            HttpCookie processorsocket_cookie = new HttpCookie("processorsocket_cookie");

            //voeg data toe aan cookies
            processor_cookie.Value = processor;
            processorprijs_cookie.Value = prijs;
            processorwebshop_cookie.Value = webshop;
            processorsocket_cookie.Value = socket;

            //save the cookies!!!
            Response.Cookies.Add(processor_cookie);
            Response.Cookies.Add(processorprijs_cookie);
            Response.Cookies.Add(processorwebshop_cookie);
            Response.Cookies.Add(processorsocket_cookie);

            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            HttpCookie processor_cookie = new HttpCookie("processor_cookie");
            HttpCookie processorprijs_cookie = new HttpCookie("processorprijs_cookie");
            HttpCookie processorwebshop_cookie = new HttpCookie("processorwebshop_cookie");
            HttpCookie processorsocket_cookie = new HttpCookie("processorsocket_cookie");

            processor_cookie = Request.Cookies["processor_cookie"];
            processorprijs_cookie = Request.Cookies["processorprijs_cookie"];
            processorwebshop_cookie = Request.Cookies["processorwebshop_cookie"];
            processorsocket_cookie = Request.Cookies["processorsocket_cookie"];

            string processor = processor_cookie.Value;
            string socket = processorsocket_cookie.Value;
            string prijs = processorprijs_cookie.Value;
            string webshop = processorwebshop_cookie.Value;

            string socket_search = "(?i).*" + socket + ".*";
           // int processor_prijs = Convert.ToInt32(prijs);

          //  lijstModel.processor = processor;
          //  ViewBag.lijst_processor = lijstModel.processor;

            

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