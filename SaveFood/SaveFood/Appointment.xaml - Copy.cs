using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Scheduler;

namespace SaveFood
{
    public partial class Appointment : PhoneApplicationPage
    {
        XDocument xmlDoc;
        public Appointment()
        {
            InitializeComponent();
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                XDocument document;
                XElement tagRegistry = null;

                if (storage.FileExists("/AppontmentLists.xml"))
                {
                    using (var stream = storage.OpenFile("/AppontmentLists.xml", FileMode.Open))
                    {
                        document = XDocument.Load(stream);
                    }

                    tagRegistry = document.Descendants("AppointmentList").FirstOrDefault();
                }
                else
                {
                    document = new XDocument();
                }

                if (tagRegistry == null)
                {
                    tagRegistry = new XElement("AppointmentList");
                    document.Add(tagRegistry);
                }
                
                var q = from c in document.Descendants("Appointments")

                        select new
                        {
                            Name = c.Attribute("name").Value,
                            date =(DateTime)c.Attribute("date"),
                        };
                
                int i = 0;

                /*foreach (var obj in q)
                {
                    if (obj.date < DateTime.Now.Date)
                    {
                        document.Descendants("Appointments").Where(xe => xe.Attribute("name") != null
                                                        && xe.Attribute("name").Value == obj.Name).Remove();
                        
                    }
                }*/

                foreach (var obj in q)
                {
                    Grid subGrid = new Grid();
                    TextBlock txtRun = new TextBlock();
                    txtRun.Name = obj.Name;
                    string queryString = "";
                    if (!ContentPanel.Children.Contains(txtRun))
                    {
                        txtRun.Text = obj.Name + " "+obj.date.Day + "-"+obj.date.Month + "-"+obj.date.Year;
                        HorizontalAlignment = HorizontalAlignment.Left;
                        HorizontalContentAlignment = HorizontalAlignment.Left;
                        txtRun.Margin = new Thickness(0, i * 50 + 30, 0, 0);
                        txtRun.FontSize = 32;
                        txtRun.VerticalAlignment = VerticalAlignment.Top;
                        ContentPanel.Children.Add(txtRun);
                        i++;

                        
                        String name = System.Guid.NewGuid().ToString();
            
                        DateTime expairyDate =obj.date;
                        string message = obj.Name + " " + obj.date.Day + "-" + obj.date.Month + "-" + obj.date.Year;
                        Uri navigationUri = new Uri("/MainPage.xaml" + message, UriKind.Relative);
                        RecurrenceInterval recurrence = RecurrenceInterval.None;
                        if (expairyDate < DateTime.Now)
                        {
                            ScheduledAction _OldReminder = ScheduledActionService.Find("TodoReminder"); if (_OldReminder != null)
                                ScheduledActionService.Remove(_OldReminder.Name);
                        }
                        else
                        {
                            Reminder _Reminder = new Reminder("TodoReminder")
                            {
                                BeginTime = DateTime.Now,
                                ExpirationTime = expairyDate,
                                Title = "Save Food",
                                Content = obj.Name + " " + "is goint to be expaied today",
                                RecurrenceType = recurrence,
                                NavigationUri = navigationUri,
                            };
                            ScheduledActionService.Add(_Reminder);
                        }
                        //beginTime=beginTime.AddHours(-3);
                        //beginTime=beginTime.AddMinutes(05);

                        //DateTime expirationTime = beginTime.AddHours(1);

                        /*Reminder reminder = new Reminder(name);
                        reminder.Title = "Save Food";
                        reminder.Content = obj.Name + " " +"is goint to be expaied today";
                        reminder.BeginTime = beginTime;
                        //reminder.ExpirationTime = ;
                        //reminder.RecurrenceType = recurrence;
                        string message = obj.Name + " " + obj.date.Day + "-" + obj.date.Month + "-" + obj.date.Year;
                        Uri navigationUri = new Uri("/MainPage.xaml" + message, UriKind.Relative);
                        reminder.NavigationUri = navigationUri;

                        // Register the reminder with the system.
                        ScheduledActionService.Add(reminder);*/
                                    

                    }
                    
                }


            }
            
            

            
           
        }
        
    }
}