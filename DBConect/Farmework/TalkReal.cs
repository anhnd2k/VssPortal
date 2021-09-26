namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TalkReal")]
    public partial class TalkReal
    {
        public int id { get; set; }

        [StringLength(50)]
        public string NameSender { get; set; }

        [StringLength(50)]
        public string MailSender { get; set; }

        public string DepartmentSender { get; set; }

        public DateTime? TimeSend { get; set; }

        [StringLength(500)]
        public string TitleRealTalk { get; set; }

        public string Suggestion { get; set; }

        public string Reality { get; set; }

        public int? Field { get; set; }

        public DateTime? TimeApproval { get; set; }

        public int? TruthStatus { get; set; }

        public int? Status { get; set; }
    }
}
