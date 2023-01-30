using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace nnnn.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string? Information { get; set; }
        public string? Category { get; set; }
        public string? Status { get; set; }
        public string? Assignee { get; set; }




        public Task()
        {

        }

    }
}
