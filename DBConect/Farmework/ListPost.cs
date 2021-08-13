namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ListPost
    {
        public int id { get; set; }

        [StringLength(255)]
        public string PostTitle { get; set; }

        public string PostConten { get; set; }

        public int? Category { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Author { get; set; }

        public string ThumbNail { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? TImeEdit { get; set; }

        public int? Status { get; set; }
    }
}
