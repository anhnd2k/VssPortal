namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FieldRealTalk")]
    public partial class FieldRealTalk
    {
        public int Id { get; set; }

        public int? IdFieldRealTalk { get; set; }

        [StringLength(50)]
        public string NameFieldRealTalk { get; set; }
    }
}
