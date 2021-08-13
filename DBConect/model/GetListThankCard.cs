using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConect.model
{
    public partial class GetListThankCard
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

        public int Department { get; set; }

        public string NameDepartment { get; set; }

        [StringLength(250)]
        public string TitleCard { get; set; }

        [StringLength(250)]
        public string CardImg { get; set; }

        public string ContenCard { get; set; }

        public DateTime? SendTime { get; set; }
    }
}
