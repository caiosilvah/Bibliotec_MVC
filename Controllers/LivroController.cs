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
    public class LivroController : Controller
    {
        private readonly ILogger<LivroController> _logger;

        public LivroController(ILogger<LivroController> logger)
        {
            _logger = logger;
        }

        Context context = new Context();

        public IActionResult Index()
        {
            ViewBag.Admin = HttpContext.Session.GetString("Admin")!;

            //Criar uma lista de livros
            List<Livro> listaLivros = context.Livro.ToList();

            //Verificar se o livro tem reserva ou nao
            var livrosreservados = context.LivroReserva.ToDictionary(livro => livro.LivroID, livror => livror.DtReserva);

            ViewBag.livros = listaLivros;
            ViewBag.LivroComReserva = livrosreservados;


            return View();
        }

        [Route("Cadastro")]
        //Metodo que aparece/retorna a tela de cadastro
        public IActionResult Cadastro()
        {

            ViewBag.Admin = HttpContext.Session.GetString("Admin")!;

            ViewBag.Categorias = context.Categoria.ToList();
            //Retorna a View de cadastro
            return View();
        }

        //Metodo para cadastrar um livro
        [Route("Cadastrar")]
        public IActionResult Cadastrar(IFormCollection form)
        {

            Livro novoLivro = new Livro();
            //O que meu usuario escrever no formulario sera atribuido ao novoLivro

            novoLivro.Nome = form["Nome"].ToString();
            novoLivro.Descricao = form["Descricao"].ToString();
            novoLivro.Editora = form["Editora"].ToString();
            novoLivro.Escritor = form["Escritor"].ToString();
            novoLivro.Idioma = form["Idioma"].ToString();
            //Trabalha com imagens
            if (form.Files.Count > 0)
            {
                //Primeiro passo:
                //Armazenarmos o arquivo enviado pelo usuario
                var arquivo = form.Files[0];

                //Segundo passo:
                //Criar variavel do caminho da minha pasta para colocar as fotos dos livros 
                var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Livros");
                //Validarmos se a pasta que sera armazenada as imagens, existe. Caso nao exista, criaremos uma nova pasta.
                if (!Directory.Exists(pasta))
                {
                    //criar a pasta:
                    Directory.CreateDirectory(pasta);
                }
                //Terceiro passo:
                //Criar a variavel para armazenar o caminho em que meu arquivo estara, alem do nome dele
                var caminho = Path.Combine(pasta, arquivo.FileName);

                using (var stream = new FileStream(caminho, FileMode.Create))
                {
                    //Copiou o arquivo para o meu diretorio
                    arquivo.CopyTo(stream);
                }

                novoLivro.Imagem = arquivo.FileName;
            }
            else
            {
                novoLivro.Imagem = "padrao.png";
            }

            context.Livro.Add(novoLivro);
            context.SaveChanges();

            //SEGUNDA PARTE: E adicionar dentro de LivroCategoria que pertence ao novoLivro
            //Listar as categorias

            List<LivroCategoria> listaLivroCategorias = new List<LivroCategoria>();

            //Array que possui as categorias selecionadas pelo usuarrio
            string[] categoriasSelecionadas = form["Categoria"].ToString().Split(',');
            //Acao, terror,suspense

            foreach (string categoria in categoriasSelecionadas)
            {
                //string categoria possui a informacao do ID da c ategoreia ATUAL selecionada
                LivroCategoria livroCategoria = new LivroCategoria();

                livroCategoria.CategoriaID = int.Parse(categoria);
                livroCategoria.LivroID = novoLivro.LivroID;
                //Adicionamos o obj livroCategoria dentro da lista listaLivrosCategorias
                listaLivroCategorias.Add(livroCategoria);
            }
            //Peguei a colecao da listaLivrosCategorias
            context.LivroCategoria.AddRange(listaLivroCategorias);

            context.SaveChanges();

            return LocalRedirect("/Cadastro");
        }


        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}