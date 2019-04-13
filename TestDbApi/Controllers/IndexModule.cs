using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDbApi.Controllers
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = _  => Response.AsRedirect("/Index");
            Get["/Index"] = _ => {
                ViewBag.Title = "Пользователи";
                return View["Index.cshtml", null];
            };
        }
    }
}
