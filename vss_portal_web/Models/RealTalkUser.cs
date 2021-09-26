using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace vss_portal_web.Models
{
    public class RealTalkUser
    {
        public int id { get; set; }

        public bool Incognito { get; set; }

        [StringLength(50)]
        public string NameSender { get; set; }

        [StringLength(50)]
        public string MailSender { get; set; }

        [StringLength(50)]
        public string NameReceiver { get; set; }

        [StringLength(50)]
        public string MailReceiver { get; set; }

        public DateTime? TimeSend { get; set; }

        [StringLength(500)]
        public string TitleRealTalk { get; set; }

        public string ContenRealTalk { get; set; }

        public int Department { get; set; }

        public int Field { get; set; }

        public bool sendOnly { get; set; }

        public bool sendMorePersion { get; set; }

        public string MailSendMoreRealTalk { get; set; }

    }
}