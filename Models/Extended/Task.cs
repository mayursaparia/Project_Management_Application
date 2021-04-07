using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectManagementApp.Models;

namespace ProjectManagementApp.Models
{
    [MetadataType(typeof(TaskMetadata))]
    public partial class Task
    {
        public IEnumerable<SelectListItem> projectlist { get; set; }

        public IEnumerable<SelectListItem> parenttasklist { get; set; }

        public IEnumerable<SelectListItem> userlist { get; set; }
    }

    public class TaskMetadata
    {
        public int Id { get; set; }

        [Display(Name = "Project Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Project Name Required")]
        public string projectName { get; set; }

        [Display(Name = "Task Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Task Name Required")]
        public string taskName { get; set; }

        [Display(Name = "Priority")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Priority Required")]
        public Nullable<int> priority { get; set; }

        [Display(Name = "Parent Task")]
        public string parentTask { get; set; }

        [Display(Name = "Start Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Start Date Required")]
        public Nullable<System.DateTime> startDate { get; set; }

        [Display(Name = "End Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "End Date Required")]
        public Nullable<System.DateTime> endDate { get; set; }

        [Display(Name = "User Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "User Name Required")]
        public string userName { get; set; }


    }
}