namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommentIdeaPost")]
    public partial class CommentIdeaPost
    {
        public int Id { get; set; }

        public int? IdPostIdea { get; set; }

        [StringLength(100)]
        public string FullNameUser { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        public string CommentValue { get; set; }

        public DateTime? TimeComment { get; set; }
    }
}
