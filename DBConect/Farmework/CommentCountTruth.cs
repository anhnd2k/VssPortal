using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConect.Farmework
{
    [Table("CommentCountTruth")]
    public partial class CommentCountTruth
    {
        public int Id { get; set; }

        public int? IdPostTruth { get; set; }

        public string UserNameComment { get; set; }
    }
}
