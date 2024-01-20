using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WebApplication1.Models;
using System.Text.Json;
using System.Drawing;

namespace WebApplication1.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            using (var client = new HttpClient())
            {
                //Tast No.1 - GET Employees and show them on html page, mark those with less than 100 hours 
                var endpoint = new Uri("https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==");
                var result = client.GetAsync(endpoint).Result;
                string json = result.Content.ReadAsStringAsync().Result;
                var employeesJSON = JsonSerializer.Deserialize<List<Employee>>(json);
                List<Employee> employees = new List<Employee>();
                List<string> employeeNames = new List<string>();
                List<EmployeeTime> employeeTimes = new List<EmployeeTime>();
                var totalHours = 0;

                if (employeesJSON != null)
                {
                    foreach (var e in employeesJSON)
                    {
                        if (employeeNames.Count == 0)
                        {
                            employeeNames.Add(e.EmployeeName);
                            continue;
                        }
                        if (!employeeNames.Contains(e.EmployeeName))
                        {
                            employeeNames.Add(e.EmployeeName);
                        }
                    }
                    foreach (string name in employeeNames)
                    {
                        employeeTimes.Add(new EmployeeTime(name, TimeSpan.Parse("00:00:00")));
                    }
                    foreach (var e in employeesJSON)
                    {
                        TimeSpan ts = e.EndTimeUtc - e.StarTimeUtc;
                        foreach (EmployeeTime etm in employeeTimes)
                        {
                            if (etm.Name == e.EmployeeName)
                            {
                                etm.Ts += ts;
                                continue;
                            }
                        }
                    }
                    employeeTimes.Sort((x, y) => y.Ts.CompareTo(x.Ts));
                    ViewData["EmployeesTimes"] = employeeTimes;

                    

                }
                return View();
            }
        }
    }
}