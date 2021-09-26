namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UnitApply")]
    public partial class UnitApply
    {
        public int Id { get; set; }

        public int? IdUnitApply { get; set; }

        [StringLength(100)]
        public string NameUnitApply { get; set; }
    }
}
