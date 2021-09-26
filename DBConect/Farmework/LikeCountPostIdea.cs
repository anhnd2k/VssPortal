namespace DBConect.Farmework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LikeCountPostIdea")]
    public partial class LikeCountPostIdea
    {
        public int Id { get; set; }

        public int? IdPostIdea { get; set; }

        [StringLength(50)]
        public string UserNameLike { get; set; }

        [StringLength(100)]
        public string EmailUserLike { get; set; }
    }
}
