using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConect.model
{
    public partial class GetListIdeaRegester
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string NameIdea { get; set; }
        public string contenIdea { get; set; }

        public string StatusIdeaBefore { get; set; }

        public string EmailIndividual { get; set; }

        public string LimitApply { get; set; }

        public string Effective { get; set; }

        [StringLength(150)]
        public string DocumentIdea { get; set; }

        [StringLength(500)]
        public string DetailMore { get; set; }

        public int? IdeaOfDepartment { get; set; }

        public int Status { get; set; }

        public bool IndividualIdea { get; set; }

        public string IdeaOfDepartmentName { get; set; }

        [StringLength(600)]
        public string AuthorFullName { get; set; }

        public string AuthorEmail { get; set; }

        [StringLength(600)]
        public string AuthorUserId { get; set; }

        public string AuthorDepartment { get; set; }

        public DateTime? DateSend { get; set; }

        public DateTime? DateTimeApprove { get; set; }

        public string NameField { get; set; }

        public string NameStauts { get; set; }
    }
}
