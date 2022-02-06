using System.Web.Mvc;

namespace ControleEstoque.Controllers
{
    public class GraficoController : Controller
    {
        [Authorize]
        public ActionResult PerdaMes()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult EntradaSaida()
        {
            return View();
        }
    }
}