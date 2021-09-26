namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LikeIdeaPost")]
    public partial class LikeIdeaPost
    {
        public int Id { get; set; }

        public int? PostId { get; set; }

        [StringLength(50)]
        public string UserNameLike { get; set; }

        public bool? StatusLike { get; set; }

        public DateTime? TimeLike { get; set; }
    }
}
