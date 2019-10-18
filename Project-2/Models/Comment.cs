using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_2.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public int BlogPostId { get; set; }
        public string AuthorID { get; set; }
        public DateTime Created { get; set; }
        public string CommentBody { get; set; }
        [Display(Name = "Comments")]
        public DateTime? Updated { get; set; }
        
        public string UpdateReason { get; set; }

        //Virtual Navigation System
        public virtual BlogPost BlogPost { get; set; } 
        public virtual ApplicationUser Author { get; set; }



    }
}