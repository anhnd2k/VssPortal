namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ThankCard")]
    public partial class ThankCard
    {
        public int id { get; set; }

        public bool? Incognito { get; set; }

        [StringLength(50)]
        public string Sender { get; set; }

        [StringLength(50)]
        public string MailSender { get; set; }

        [StringLength(50)]
        public string Receiver { get; set; }

        [StringLength(50)]
        public string MailReceiver { get; set; }

        public int? Department { get; set; }

        [StringLength(250)]
        public string TitleCard { get; set; }

        [StringLength(250)]
        public string CardImg { get; set; }

        public string ContenCard { get; set; }

        public DateTime? SendTime { get; set; }
    }
}
