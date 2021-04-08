using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectManagementApp.Models;

namespace ProjectManagementApp.Models
{
    [MetadataType(typeof(ProjectMetadata))]
    public partial class Project
    {
        public IEnumerable<SelectListItem> managerlist { get; set; }
    }

    public class ProjectMetadata
    {
        public int Id { get; set; }

        [Display(Name = "Project")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Project Name Required")]
        public string name { get; set; }

        [Display(Name = "Start Date")]
        public Nullable<System.DateTime> startDate { get; set; }

        [Display(Name = "End Date")]
        public Nullable<System.DateTime> endDate { get; set; }

        [Display(Name = "Priority")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Priority Required")]
        public int priority { get; set; }

        [Display(Name = "No of Task")]
        public Nullable<int> taskNo { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }

        [Display(Name = "Manager")]

        [Required(AllowEmptyStrings = false, ErrorMessage = "Manager Required")]
        public string manager { get; set; }

        
    }
}