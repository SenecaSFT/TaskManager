using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    internal class Task
    {
        private int _taskNumber;
        public string Name { get; set; }
        public string Description { get; set; }

        public Task(string name, string desc)
        {
            this.Name = name; 
            this.Description = desc;
        }
    }
}
