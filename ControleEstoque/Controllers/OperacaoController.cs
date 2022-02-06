using System.Web.Mvc;

namespace ControleEstoque.Controllers
{
    public class OperacaoController : Controller
    {
        [Authorize]
        public ActionResult EntradaProdutos()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult SaidaProduto()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult LancamentoPerdaProduto()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult Inventario()
        {
            return View();
        }
    }
}