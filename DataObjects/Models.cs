using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DataObjects.Models
{
    [DataContract]
    public class ComposeMessage
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Contact { get; set; }
        [DataType(DataType.Time)]
        [DataMember]
        public string Time { get; set; }
    }

    [DataContract]
    public class Contacts
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [RegularExpression(@"^[0-9]*$")]
        [MinLength(10)]
        [MaxLength(10)]
        [DataMember]
        public string Contact { get; set; }
    }

    [DataContract]
    public class SearchFilter
    {
        [DataMember]
        public int StartIndex { get; set; }
        [DataMember]
        public int EndIndex { get; set; }
    }

    [DataContract]
    public class Result
    {
        [DataMember(Order = 1, IsRequired = true)]
        public int Status { get; set; }

        [DataMember(Order = 2, EmitDefaultValue = false)]
        public string Message { get; set; }

        [DataMember(Order = 2, EmitDefaultValue = false)]
        public string Err_Message { get; set; }

        [DataMember(Order = 3, EmitDefaultValue = false)]
        public int Err_Code { get; set; }

        [DataMember(Order = 3, EmitDefaultValue = false)]
        public List<Contacts> ListOfContacts { get; set; }

        [DataMember(Order = 3, EmitDefaultValue = false)]
        public List<ComposeMessage> ListOfMessages { get; set; }

        [DataMember(Order = 3, EmitDefaultValue = false)]
        public int Data { get; set; }
    }

    [DataContract]
    public class User
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}