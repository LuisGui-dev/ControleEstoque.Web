using System.Web.Mvc;

namespace ControleEstoque.Controllers
{
    public class CadastroController : Controller
    {
        [Authorize]
        public ActionResult GrupoProduto()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult MarcaProduto()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult LocalProduto()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult UnidadeMedida()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult Produto()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult Pais()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult Estado()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult Cidade()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult PerfilUsuario()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult Usuario()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult Fornecedor()
        {
            return View();
        }
        
    }
}