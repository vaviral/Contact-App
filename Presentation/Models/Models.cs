using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class ComposeMessage
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Contact { get; set; }
        [DataType(DataType.Time)]
        public string Time { get; set; }
    }

    public class Contacts
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [RegularExpression(@"^[0-9]*$")]
        [MinLength(10)]
        [MaxLength(10)]
        [Required]
        public string Contact { get; set; }
    }

    public class SearchFilter
    {
        [Required]
        public int StartIndex { get; set; }

        [Required]
        public int EndIndex { get; set; }
    }

    public class Result
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public string Err_Message { get; set; }

        public int Err_Code { get; set; }

        public List<Contacts> ListOfContacts { get; set; }

        public List<ComposeMessage> ListOfMessages { get; set; }

        public int Data { get; set; }
    }

    public class User
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}