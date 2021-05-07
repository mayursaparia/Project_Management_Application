using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ProjectManagementApp.Models;

namespace ProjectManagementApp.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public List<User> userlist { get; set; }
    }

    public class UserMetadata
    {
        [Display(Name = "Employee Id")]
        public int Id { get; set; }


        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name Required")]
        public string firstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name Required")]
        public string lastName { get; set; }


    }
}