using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace nnnn.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string? TaskInfo { get; set; }
        public string? TaskCategory { get; set; }


        public Task()
        {

        }

    }
}
