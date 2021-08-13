namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CategoryPost")]
    public partial class CategoryPost
    {
        public int id { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(50)]
        public string CategoryName { get; set; }
    }
}
