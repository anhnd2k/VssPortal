namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OnboadingPost")]
    public partial class OnboadingPost
    {
        public int id { get; set; }

        [StringLength(255)]
        public string OnboadingTitle { get; set; }

        public string OnboadingConten { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string Thumbnail { get; set; }

        [StringLength(50)]
        public string Author { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(25)]
        public string ButtonTitle { get; set; }
    }
}
