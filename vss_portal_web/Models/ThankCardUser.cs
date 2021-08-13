using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace vss_portal_web.Models
{
    public class ThankCardUser
    {
        public int id { get; set; }

        public bool Incognito { get; set; }

        [StringLength(50)]
        public string Sender { get; set; }

        [StringLength(50)]
        public string MailSender { get; set; }

        [StringLength(50)]
        public string Receiver { get; set; }

        [StringLength(50)]
        public string MailReceiver { get; set; }

        public int Department { get; set; }

        [StringLength(250)]
        public string TitleCard { get; set; }

        [StringLength(250)]
        public string CardImg { get; set; }

        public string ContenCard { get; set; }

        public string TextDepartment { get; set; }

        //gui nhieu nguoi
        public bool sendOnly { get; set; }

        public bool sendDepartment { get; set; }

        public bool sendMorePersion { get; set; }

        public string MailSendMore { get; set; }

    }
}