using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entity
{   
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }

        public bool IsMain { get; set; }
        public int BusinessUserId { get; set; }
    
        public BusinessUser BusinessUser { get; set; }
    }
}