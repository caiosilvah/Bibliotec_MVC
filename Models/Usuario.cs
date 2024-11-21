using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace Bibliotec.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioID {get; set;}

        public string? Nome {get; set;}

        public DateOnly DtNascimento {get; set;}

        public string? Email {get; set;}

        public string ? Senha {get; set;}

        public string? Contato {get; set;}

        public bool Admin {get; set;}

        public bool Status {get; set;}

        //criar um atributo
        //eu falo para este atributo que ele e um FK
        [ForeignKey("Curso")]
        public int CursoID {get; set;}
        public Curso Curso {get; set;}
    }
        // Atributos:
        // int Usuario
        // string Nome
        // DateOnly DtNascimento
        // string Email
        // string Senha
        // string Contato
        // string Admin
        // bool Status
}