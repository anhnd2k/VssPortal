using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConect.Farmework
{
    [Table("CommentDetailTruth")]
    public partial class CommentDetailTruth
    {
        public int Id { get; set; }
        public int IdTruth { get; set; }
        public int StatusTruth { get; set; }
        public string CommentTruth { get; set; }
        public string NameManagerCmt { get; set; }
        public string EmailManagerCmt { get; set; }
        public DateTime TimeCmt { get; set; }

    }
}
