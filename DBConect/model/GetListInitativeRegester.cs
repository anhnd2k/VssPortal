using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConect.model
{
    public partial class GetListInitativeRegester
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string UserSend { get; set; }

        [StringLength(100)]
        public string UserSendFullName { get; set; }

        public string UserSenDepartment { get; set; }

        [StringLength(500)]
        public string EmailUserSend { get; set; }

        public bool PersonalInitative { get; set; }

        public string MailOfEach { get; set; }

        public int? InitativeOfDepartment { get; set; }

        public string NameDepartmentSendInitative { get; set; }

        [StringLength(500)]
        public string NameInitative { get; set; }

        public int? IdUnitApply { get; set; }

        public int? IdField { get; set; }

        public string TechnicalCondition { get; set; }

        public string EffectiveApply { get; set; }

        public string Applicability { get; set; }

        public string NewPoint { get; set; }

        public string ContentInitative { get; set; }

        [StringLength(500)]
        public string NameDocument { get; set; }

        public string DetailMore { get; set; }

        public DateTime? TimeSendInitative { get; set; }

        public DateTime? TimeApprove { get; set; }

        public int? Status { get; set; }

        public string NameField { get; set; }

        public string NameStauts { get; set; }

        public string NameUnitApply { get; set; }
    }
}
