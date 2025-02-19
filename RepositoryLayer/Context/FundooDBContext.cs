using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class FundooDBContext : DbContext
    {
        public FundooDBContext(DbContextOptions options) : base(options) { }
        public DbSet<Users> Users { get; set; }//table creation

        public DbSet<NotesEntity> NotesEntity { get; set; }

        public DbSet<LabelEntity> LabelEntity { get; set; }

        public DbSet<LabelDataEntity> LabelData { get; set; }

        public DbSet<CollaboratorEntity> CollaboratorEntity { get; set; }
       // ------------------------------------------------------------------

        public DbSet<ProductEntity> Products { get; set; } 

    }
}
