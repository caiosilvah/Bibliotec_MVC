using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotec.Models
{
    public class Curso
    {
        [Key]
        public int CursoID;

        public string? Nome;

        public char Periodo;

    }
        // Atributos:
        // int CursoID
        // string Nome
        // char Periodo
        
}