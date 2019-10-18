using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_2.Models
{
    public class BlogPost
    {
        public int ID { get; set; } //auto generated Primary key 
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        
        public string Title { get; set;}
        public string Slug { get; set; }// created behind the scene for custom routing
        
        public string Abstract { get; set; }

        [Display(Name = "Post Body")]

        [AllowHtml]
        public string BlogPostBody { get; set; }
        
        public string ImagePath { get; set; }
        public bool Published { get; set; }
        // Navigation
        public virtual ICollection<Comment>Comments { get; set; }// collection of multiple objects
        public BlogPost() //constructor must be same name as class
        {

            this.Comments = new HashSet<Comment>();

        }
    }
}