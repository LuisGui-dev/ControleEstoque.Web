using System.Web.Mvc;

namespace ControleEstoque.Controllers
{
    public class OperacaoController : Controller
    {
        public ActionResult EntradaProdutos()
        {
            return View();
        }
        
        public ActionResult SaidaProduto()
        {
            return View();
        }
        
        public ActionResult LancamentoPerdaProduto()
        {
            return View();
        }
        
        public ActionResult Inventario()
        {
            return View();
        }
    }
}