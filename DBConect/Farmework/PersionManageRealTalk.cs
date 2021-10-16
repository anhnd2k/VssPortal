namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PersionManageRealTalk")]
    public partial class PersionManageRealTalk
    {
        public int id { get; set; }

        public int? IdField { get; set; }

        [StringLength(100)]
        public string FullNameManage { get; set; }

        [StringLength(100)]
        public string EmailManage { get; set; }

        [StringLength(100)]
        public string Department { get; set; }
    }
}
