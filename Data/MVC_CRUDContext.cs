using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVC_CRUD.Models;

namespace MVC_CRUD.Data
{
    public class MVC_CRUDContext : DbContext
    {
        public MVC_CRUDContext (DbContextOptions<MVC_CRUDContext> options)
            : base(options)
        {
        }

        public DbSet<MVC_CRUD.Models.CarViewModel> CarViewModel { get; set; } = default!;
    }
}
