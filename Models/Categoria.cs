using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotec.Models
{
    public class Categoria
    {    
        [Key]
        int CategoriaId {get ; set ;}
        string? Nome {get; set;}
 
    }
        // Atributos:
        // int CategoriaID
        // string Nome
}