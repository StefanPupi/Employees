using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Lists
    {
        public static List<Employee> employees = new List<Employee>();
        public static List<string> employeeNames = new List<string>();
        public static List<EmployeeTime> employeeTimes = new List<EmployeeTime>();
    }
}