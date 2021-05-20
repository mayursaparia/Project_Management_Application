using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagementApp.Models
{
    [MetadataType(typeof(SP_View_Task_ResultMetadata))]
    public partial class SP_View_Task_Result
    {
    }

    public class SP_View_Task_ResultMetadata
    {
        public int Id { get; set; }

        [Display(Name = "Project Name")]
        public string projectName { get; set; }

        [Display(Name = "Task Name")]
        public string taskName { get; set; }

        [Display(Name = "Priority")]
        public Nullable<int> priority { get; set; }

        [Display(Name = "Parent Task")]
        public string parentTask { get; set; }

        [Display(Name = "Start Date")]
        public Nullable<System.DateTime> startDate { get; set; }

        [Display(Name = "End Date")]
        public Nullable<System.DateTime> endDate { get; set; }

        [Display(Name = "User Name")]
        public string userName { get; set; }


    }
}