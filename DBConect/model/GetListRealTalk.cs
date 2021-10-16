using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConect.model
{
    public partial class GetListRealTalk
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

        public int Field { get; set; }

        public DateTime? TimeApproval { get; set; }

        public int? TruthStatus { get; set; }

        public bool ShowInNewFeed { get; set; }

        public int? Status { get; set; }

        public string NameFieldRealTalk { get; set; }

        public string TruthStatusName { get; set; }

        public string StatusConfirm { get; set; }

        public string UserNameLike { get; set; }

        public string UserNameComment { get; set; }

        public DateTime TimeUpdateStatus { get; set; }
    }
}
