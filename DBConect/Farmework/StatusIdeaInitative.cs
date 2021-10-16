﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConect.Farmework
{
    [Table("StatusIdeaInitative")]
    public partial class StatusIdeaInitative
    {
        public int Id { get; set; }
        public int? IdStatus { get; set; }

        [StringLength(50)]
        public string NameStauts { get; set; }
    }
}
