namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FieldInIdeal")]
    public partial class FieldInIdeal
    {
        public int Id { get; set; }

        public int? IdField { get; set; }

        [StringLength(100)]
        public string NameField { get; set; }
    }
}
