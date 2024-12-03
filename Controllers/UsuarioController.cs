using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bibliotec.Contexts;
using Bibliotec.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bibliotec_mvc.Controllers
{
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        //Criando um obj da classe Context:
        Context context = new Context();

        //O metodo esta retornando a view Usuario/Index.cshtml
        public IActionResult Index()
        {
            //pegar as informacoes da session para que apareca os detalhes do meu usuario:
            int id = int.Parse (HttpContext.Session.GetString("UsuarioID")!);
            ViewBag.Admin = HttpContext.Session.GetString("Admin")!;

            //busquei o usuario que esta logado (beatriz)
            Usuario usuarioEncontrado = context.Usuario.FirstOrDefault(usuario => usuario.UsuarioID == id)!;
            //se mao for encontrado ninguem
            if(usuarioEncontrado == null){
                return NotFound();
            }

            //procurar o curso que meu usuarioEncontrado esta cadastrado
            Curso cursoEncontrado = context.Curso.FirstOrDefault(curso => curso.CursoID == usuarioEncontrado.CursoID)!;

            //Verificar se o usuario possui ou nao o curso
            if(cursoEncontrado ==  null){
                //O usuario nao possui curso cadastrado
                ViewBag.Curso = "O usuario nao possui curso cadastrado";
            }else{
                //o usuario possui o curso XXX
                ViewBag.Curso = cursoEncontrado.Nome;
            }

            ViewBag.Nome = usuarioEncontrado.Nome;
            ViewBag.Email = usuarioEncontrado.Email;
            ViewBag.Contato = usuarioEncontrado.Contato;
            ViewBag.DtNascimento = usuarioEncontrado.DtNascimento.ToString("dd/MM/yyyy");



            return View();
        }


        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}