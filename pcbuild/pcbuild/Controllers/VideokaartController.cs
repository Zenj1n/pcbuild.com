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
using System.Globalization;
using pcbuild.Models.ProcessorModels;
using System.Text.RegularExpressions;
using pcbuild.Models.VideokaartModels;



namespace pcbuild.Controllers
{
    public class VideokaartController : Controller
    {
        /// <summary>
        /// Deze methode maakt, voegt data en slaat de cookies op
        /// </summary>
        /// <param name="moederbord"> moederbord naam</param>
        /// <param name="prijs">moederbord prijs</param>
        /// <param name="webshop">webshop van de moederbord</param>
        /// <param name="vormfactor">vormfactor van de moederbord</param>
        /// <param name="ddr">ddr van de moederbord</param>
        /// <returns></returns>
        public ActionResult Reload(string moederbord, string prijs, string webshop, string vormfactor, string ddr)
        {
            //Roep cookie arrays
            HttpCookie moederbord_cookie = new HttpCookie("moederbord_cookie");
            HttpCookie moederbordprijs_cookie = new HttpCookie("moederbordprijs_cookie");
            HttpCookie moederbordwebshop_cookie = new HttpCookie("moederbordwebshop_cookie");
            HttpCookie moederbordvormfactor_cookie = new HttpCookie("moederbordvormfactor_cookie");
            HttpCookie moederbordddr_cookie = new HttpCookie("moederbordddr_cookie");
            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");
            HttpCookie videokaartprijs_cookie = new HttpCookie("videokaartprijs_cookie");

            //Roep cookies aan
            totale_prijs_cookie = Request.Cookies["totale_prijs_cookie"];
            videokaartprijs_cookie = Request.Cookies["videokaartprijs_cookie"];

            //Pak alle prijzen tot nu toe en tel ze bij elkaar op en als je terug kwam van vorige stap haal de eerdere prijs eruit.
            decimal prijs_moederbord = Convert.ToDecimal(prijs, new CultureInfo("is-IS"));
            decimal prijs_videokaart = Convert.ToDecimal(videokaartprijs_cookie.Value, new CultureInfo("is-IS"));
            decimal prijs_totaal_vorige = Convert.ToDecimal(totale_prijs_cookie.Value, new CultureInfo("is-IS")) - prijs_videokaart;
            decimal prijs_totaal = prijs_totaal_vorige + prijs_moederbord;
            string prijs_totaal_string = prijs_totaal.ToString();
            totale_prijs_cookie.Value = prijs_totaal_string;
            videokaartprijs_cookie.Value = "0,00";
            Response.Cookies.Add(videokaartprijs_cookie);
            Response.Cookies.Add(totale_prijs_cookie);

            //voeg data toe aan cookies
            moederbord_cookie.Value = moederbord;
            moederbordprijs_cookie.Value = prijs;
            moederbordwebshop_cookie.Value = webshop;
            moederbordvormfactor_cookie.Value = vormfactor;
            moederbordddr_cookie.Value = ddr;

            //save the cookies!!!
            Response.Cookies.Add(moederbord_cookie);
            Response.Cookies.Add(moederbordprijs_cookie);
            Response.Cookies.Add(moederbordwebshop_cookie);
            Response.Cookies.Add(moederbordvormfactor_cookie);
            Response.Cookies.Add(moederbordddr_cookie);

            //Roep methode Index aan
            return RedirectToAction("Index");

        }

        /// <summary>
        /// Roep de cookies aan voor de View, maak een connectie en haal data uit database.
        /// </summary>
        /// <returns>return data van de database in componenten_query naar de view</returns>
        public ActionResult Index()
        {
            //Roep cookies aan voor de View
            HttpCookie moederbord_cookie = new HttpCookie("moederbord_cookie");
            HttpCookie moederbordprijs_cookie = new HttpCookie("moederbordprijs_cookie");
            HttpCookie moederbordwebshop_cookie = new HttpCookie("moederbordwebshop_cookie");

            HttpCookie moederbordvormfactor_cookie = new HttpCookie("moederbordvormfactor_cookie");
            HttpCookie moederbordddr_cookie = new HttpCookie("moederbordddr_cookie");
            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");

            //Connectie met database
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@localhost:8080/db/data"));
            client.Connect();

            // Query om Videokaarten op te halen
            var componenten_query = client
              .Cypher
              .Match("(n:videokaart)-[r:verkrijgbaar]-(p:Webshop)")
              .Return((n, r, p) => new ViewModelVideokaart
              {
                  Videokaart_all = n.As<Videokaart_Model>(),
                  Verkrijgbaar_all = r.As<Verkrijgbaar_Model>(),
                  Webshop_all = p.As<Webshop_Model>(),
              })
              .Results;

            //return naar de View en stuur componenten_query mee
            return View(componenten_query);
        }
    }
}