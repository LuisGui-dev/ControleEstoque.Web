﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ControleEstoque.Models;

namespace ControleEstoque.Controllers.Cadastro
{
    public class CadUsuarioController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;
        private const string _senhaPadrao = "{##%!!3***}";

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Senhapadrao = _senhaPadrao;
            ViewBag.ListaTamPagina =
                new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = UsuarioModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            var quant = UsuarioModel.RecuperarQuantidade();

            var difQuantPaginas = quant % ViewBag.QuantMaxLinhasPorPagina > 0 ? 1 : 0;
            ViewBag.QuantPaginas = quant / ViewBag.QuantMaxLinhasPorPagina + difQuantPaginas;

            return View(lista);
        }
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult UsuarioPagina(int pagina, int tamPag)
        {
            var lista = UsuarioModel.RecuperarLista(pagina, tamPag);

            return Json(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarUsuario(int id)
        {
            return Json(UsuarioModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirUsuario(int id)
        {
            return Json(UsuarioModel.ExcluirPeloId(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarUsuario(UsuarioModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    if (model.Senha == _senhaPadrao)
                    {
                        model.Senha = "";
                    }

                    var id = model.Salvar();
                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}