namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ListThankCardImg")]
    public partial class ListThankCardImg
    {
        public int id { get; set; }

        [StringLength(250)]
        public string NameCardImg { get; set; }
    }
}
