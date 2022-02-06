using System.Web.Mvc;

namespace ControleEstoque.Controllers
{
    public class GraficoController : Controller
    {
        public ActionResult PerdaMes()
        {
            return View();
        }
        
        public ActionResult EntradaSaida()
        {
            return View();
        }
    }
}