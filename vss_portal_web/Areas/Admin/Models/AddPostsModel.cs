using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace vss_portal_web.Areas.Admin.Models
{
    public class AddPostsModel
    {
        public int id { get; set; }

        [StringLength(255)]
        public string PostTitle { get; set; }

        public string PostConten { get; set; }

        public int Category { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Author { get; set; }

        public string ThumbNail { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? TImeEdit { get; set; }

        public int Status { get; set; }

        public bool checkBox { get; set; }

        public string CheckValueCategory { get; set; }
    }
}