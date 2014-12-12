using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pcbuild.Controllers
{
    public class ComponentenController : Controller
    {

        public ActionResult Index()
        {
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
                client.Connect();
            
            
            return View();
        }
    }

}