using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bibliotec.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Bibliotec.contexts
{
    
        //Classe que tera as informacoes do banco de dados 
    public class Context : DbContext
    {
        //Criar o metodo construtor
        public Context(){

        }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        //OnConfiguring -> possui a configuracap da conexao com o banco de dados          
       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
                //colocar as informacoes do banco

                //as configuracoes existem?
             if (optionsBuilder.IsConfigured){
                // A string de conexao do banco de dados:
                // Data Source => Nome do servidor do banco de dados
                // Initial Catalog => Nome do banco de dados
                // User id e Password => informacoes de acesso ao servidor do banco de dados
                optionsBuilder.UseSqlServer
                (@"Data Source=NOTE32-S28\\SQLEXPRESS; 
                Initial Catalog = Bibliotec_mvc; 
                User Id=sa;
                Password=123;
                Integrated Security=true;
                TrustServerCertificate = true");
             }
               
        }
        protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder){
            //
         public DbSet<Categoria> Categoria {get; set;}
            //curso
        public DbSet<Curso> Curso {get; set;}
            //livro
         public DbSet<Livro> Livro {get; set;}
            //usuario
        public DbSet<Usuario> Usuario {get; set;}
            //LivroCategoria
        public DbSet<LivroCategoria> LivroCategoria {get; set;}
            //LivroRerseva
        public DbSet<LivroReserva> LivroReserva {get; set;}
        }


    }
    
}