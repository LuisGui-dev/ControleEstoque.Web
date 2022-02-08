using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ControleEstoque.Models;

namespace ControleEstoque.Controllers
{
    public class CadastroController : Controller
    {
        private static readonly List<GrupoProdutoModel> _listaGrupoProduto = new List<GrupoProdutoModel>()
        {
            new GrupoProdutoModel { Id = 1, Nome = "livros", Ativo = true },
            new GrupoProdutoModel { Id = 2, Nome = "Mouses", Ativo = true },
            new GrupoProdutoModel { Id = 3, Nome = "Monitores", Ativo = false },
        };

        [Authorize]
        public ActionResult GrupoProduto()
        {
            return View(_listaGrupoProduto);
        }

        [Authorize]
        public ActionResult RecuperarGrupoProduto(int id)
        {
            return Json(_listaGrupoProduto.Find(x => x.Id == id));
        }

        [HttpPost]
        [Authorize]
        public ActionResult ExcluirGrupoProduto(int id)
        {
            var ret = false;
            var registroDB = _listaGrupoProduto.Find(x => x.Id == id);
            if (registroDB != null)
            {
                _listaGrupoProduto.Remove(registroDB);
                ret = true;
            }

            return Json(ret);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SalvarGrupoProduto(GrupoProdutoModel model)
        {
            var registroDB = _listaGrupoProduto.Find(x => x.Id == model.Id);
            if (registroDB == null)
            {
                registroDB = model;
                registroDB.Id = _listaGrupoProduto.Max(x => x.Id) + 1;
                _listaGrupoProduto.Add(registroDB);
            }
            else
            {
                registroDB.Nome = model.Nome;
                registroDB.Ativo = model.Ativo;
            }

            return Json(registroDB);
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