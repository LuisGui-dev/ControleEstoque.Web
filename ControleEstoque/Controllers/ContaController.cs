using System.Web.Mvc;
using System.Web.Security;
using ControleEstoque.Models;

namespace ControleEstoque.Controllers
{
    public class ContaController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel login)
        {
            var achou = UsuarioModel.ValidarUsuario(login.Usuario, login.Senha);

            if (ModelState.IsValid && achou)
            {
                FormsAuthentication.SetAuthCookie(login.Usuario, login.LembrarMe);
                return RedirectToAction("Index", "Home");
            }
            
            ModelState.AddModelError("", "Login inválido.");
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        
    }
}