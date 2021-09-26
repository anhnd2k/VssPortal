using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConect.model
{
    public partial class GetListPosts
    {
        public int id { get; set; }

        [StringLength(255)]
        public string PostTitle { get; set; }

        public string PostConten { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Author { get; set; }

        public int? Category { get; set; }

        public string ThumbNail { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? TImeEdit { get; set; }

        public string ShowPostOnboading { get; set; }

        public string CategoryName { get; set; }

        public int? Status { get; set; }
    }
}
