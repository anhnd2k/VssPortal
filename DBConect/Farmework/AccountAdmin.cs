namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccountAdmin")]
    public partial class AccountAdmin
    {
        [Key]
        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string PassWord { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }

        [StringLength(250)]
        public string EmailVt { get; set; }

        [StringLength(50)]
        public string Role { get; set; }
    }
}
