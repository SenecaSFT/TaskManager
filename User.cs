using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    internal class User
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        private int _tasksCompleted;

        public User(string firstname, string lastname, string password)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Password = password;
            this._tasksCompleted = 0;
        }
    }
}
