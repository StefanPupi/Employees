using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class EmployeeTime
    {
        string name;
        TimeSpan ts;

        public EmployeeTime(string name, TimeSpan ts)
        {
            this.Name = name;
            this.Ts = ts;
        }

        public string Name { get => name; set => name = value; }
        public TimeSpan Ts { get => ts; set => ts = value; }
    }
}