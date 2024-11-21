using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotec.Models
{
    public class Livro
    {
        [Key]
         public int LivroId {get ; set ;}
        public string? Nome {get; set;}
        public string? Escritor {get; set;}
        public string? Editora {get; set;}
        public string? Descricao {get; set;}
        public string? Idioma {get; set;}
        public string? Imagen {get; set;}
        public bool? Reservado {get; set;}
        public bool Ativo {get; set;}
    }
         // Atributos:
        // int CategoriaID
        // string Nome
        // string Escritor
        // string Editora
        // string Descricao
        // string Idioma
        // string Imagem
        // string Reservado
        // string Ativo
}