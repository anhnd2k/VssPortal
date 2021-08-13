namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Employee")]
    public partial class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [StringLength(255)]
        public string EmloyeeId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [Key]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string PhoneNumber { get; set; }

        [StringLength(255)]
        public string Department { get; set; }

        [StringLength(255)]
        public string GroupDepartment { get; set; }

        [StringLength(255)]
        public string Birthday { get; set; }

        [StringLength(255)]
        public string Sex { get; set; }

        [StringLength(255)]
        public string DateJoin { get; set; }

        [StringLength(255)]
        public string DateRecruitment { get; set; }
    }
}
