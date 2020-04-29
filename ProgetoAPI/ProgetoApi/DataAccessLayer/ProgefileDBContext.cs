using Microsoft.EntityFrameworkCore;
using ProgetoApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgetoApi.DataAccessLayer
{
    public class ProgefileDBContext : DbContext
    {
        public DbSet<Progefile> Files { get; set; }

        public ProgefileDBContext(DbContextOptions<ProgefileDBContext> options)
            : base(options)
        { }

    }
}
