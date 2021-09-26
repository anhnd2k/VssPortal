namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ManageRegisterIdea")]
    public partial class ManageRegisterIdea
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string NameIdea { get; set; }

        public string contenIdea { get; set; }

        public string StatusIdeaBefore { get; set; }

        public string EmailIndividual { get; set; }

        public string LimitApply { get; set; }

        public string Effective { get; set; }

        public int? Field { get; set; }

        [StringLength(150)]
        public string DocumentIdea { get; set; }

        [StringLength(500)]
        public string DetailMore { get; set; }

        public int? IdeaOfDepartment { get; set; }

        public bool? IndividualIdea { get; set; }

        [StringLength(600)]
        public string AuthorFullName { get; set; }

        [StringLength(50)]
        public string AuthorEmail { get; set; }

        public string AuthorDepartment { get; set; }

        [StringLength(600)]
        public string AuthorUserId { get; set; }

        public DateTime? DateSend { get; set; }

        public int? Status { get; set; }

        public DateTime? DateTimeApprove { get; set; }
    }
}
