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

        [Route("Editar/{id}")]
        public IActionResult Editar(int id ){

            ViewBag.Admin = HttpContext.Session.GetString("Admin")!;

            ViewBag.CategoriasDoSistema = context.Categoria.ToList();

            

            // LivroID == 3

            // Buscar quem e o tal do id numero 3:
            Livro livroAtualizado = context.Livro.FirstOrDefault(livro => livro.LivroID == id)!;

            //Buscar as categorias que o livroAtualizado possui 
            var categoriasDoLivroEncontrado = context.LivroCategoria.Where(identificadorLivro => identificadorLivro.LivroID == id).Select(livro => livro.Categoria).ToList();

            //Quero pegar as informacoes do meu livro selecionado e mandar para a minha View
            ViewBag.Livro = livroAtualizado;
            ViewBag.Categoria = categoriasDoLivroEncontrado;


            return View();
        }

        //Metodo que atualiza as informacoes do livro
        [Route ("Atualizar/{id}")]
        public IActionResult Atualizar(IFormCollection form, int id, IFormFile imagem){
            //Buscar o livro especifico peloID
            Livro livroAtualizado = context.Livro.FirstOrDefault(livro => livro.LivroID == id)!;

            livroAtualizado.Nome = form ["Nome"].ToString();
            livroAtualizado.Escritor = form["Escritor"];
            livroAtualizado.Editora = form["Editora"];
            livroAtualizado.Idioma = form["Idioma"];
            livroAtualizado.Descricao = form["Descricao"];

            //Upload de imagem
            if(imagem.Length > 0){
                //Definir o caminho da minha imagem do livro atual que eu quero alterar:
                var caminhoImagem = Path.Combine("wwwroot/images/Livros", imagem.FileName);

                //Verificar se o usuario colocou uma imagem para atualizar o livro
                if(string.IsNullOrEmpty (livroAtualizado.Imagem)){
                    //Caso exista, ela ira ser apagada 
                    var caminhoImagemAntiga = Path.Combine("wwwroot/images/Livros", livroAtualizado.Imagem);
                    //Ver ser existe uma imagem no caminho antigo
                    if(System.IO.File.Exists(caminhoImagemAntiga)){
                        System.IO.File.Delete(caminhoImagemAntiga);
                    }
                }

                //Salvar a imagem nova
                using(var stream = new FileStream(caminhoImagem, FileMode.Create)){
                    imagem.CopyTo(stream);
                }

                //Subir essa mudanca para o meu banco de deaos
                livroAtualizado.Imagem = imagem.FileName;
                
            }
        }


        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}