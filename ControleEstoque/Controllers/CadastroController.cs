﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ControleEstoque.Models;

namespace ControleEstoque.Controllers
{
    public class CadastroController : Controller
    {
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
        public ActionResult Fornecedor()
        {
            return View();
        }
    }
}