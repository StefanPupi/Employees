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

                    //Tast No.2 PNG Pie graph
                    foreach (EmployeeTime et in employeeTimes)
                    {
                        totalHours += (int)et.Ts.TotalHours;
                    }
                    var pathPNG = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\employees.png";

                    int width = 700;
                    int height = 500;
                    Bitmap image = new Bitmap(width, height);

                    Graphics graphics = Graphics.FromImage(image);

                    Color[] colors = { Color.Red, Color.Blue, Color.Black, Color.Violet, Color.Orange, Color.Yellow, Color.Green, Color.Cyan, Color.Brown, Color.Crimson, Color.Gold };

                    int i = 0;
                    float movingAngle = 0;
                    int rectLegendX = 500;
                    int rectLegendY = 100;
                    Font arial = new Font("Arial", 10, FontStyle.Regular);

                    foreach (EmployeeTime et in employeeTimes)
                    {
                        float procent = ((float)et.Ts.TotalHours / totalHours) * 100;
                        float angle = (360 * procent) / 100;

                        Pen pen = new Pen(colors[i]);
                        SolidBrush solidBrush = new SolidBrush(colors[i]);
                        SolidBrush blackBrush = new SolidBrush(Color.Black);
                        Rectangle rectPie = new Rectangle(100, 100, 300, 300);
                        Rectangle rectLegend = new Rectangle(rectLegendX, rectLegendY, 25, 10);
                        graphics.DrawArc(pen, rectPie, movingAngle, angle);
                        graphics.FillPie(solidBrush, rectPie, movingAngle, angle);
                        graphics.DrawRectangle(pen, rectLegend);
                        graphics.FillRectangle(solidBrush, rectLegend);
                        PointF pointF = new PointF(rectLegendX + 35, rectLegendY);
                        graphics.DrawString(et.Name, arial, blackBrush, pointF);
                        i++;
                        movingAngle += angle;
                        rectLegendY += 30;
                    }

                    image.Save(pathPNG);

                    graphics.Dispose();
                    image.Dispose();

                }
                return View();
            }
        }
    }
}