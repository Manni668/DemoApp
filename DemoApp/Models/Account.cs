using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models
{

    
    public class Account
    {
        
        public string UserName { get; set; }

        [Key]
        public int UserId { get; set; }

        //public Dictionary<string, decimal> Balance { get; set; }
        
    }

    
}
