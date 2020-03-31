using Amigos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amigos.DataAccessLayer
{
    public class AmigoDBContext : DbContext
    {
        public DbSet<Amigo> Amigos { get; set; }

        public AmigoDBContext(DbContextOptions<AmigoDBContext> options)
           : base(options)
        {
        }
    }
}
