using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConect.model
{
    public partial class GetCommentPostIdea
    {
        public int id { get; set; }

        [StringLength(50)]
        public string NameSender { get; set; }

        [StringLength(50)]
        public string MailSender { get; set; }

        public DateTime? TimeSend { get; set; }

        [StringLength(500)]
        public string TitleRealTalk { get; set; }

        public string Suggestion { get; set; }

        public string Reality { get; set; }

        public DateTime? TimeApproval { get; set; }

        public string NameFieldRealTalk { get; set; }

        public string FullNameUser { get; set; }

        public string CommentValue { get; set; }

        public DateTime? TimeComment { get; set; }
    }
}
