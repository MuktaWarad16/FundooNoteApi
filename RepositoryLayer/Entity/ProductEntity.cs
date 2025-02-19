using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class ProductEntity
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int Cost { get; set; }
        
    }
}
