using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WPControls;

using Microsoft.Phone.Scheduler;

namespace SaveFood
{
    public partial class Calender : PhoneApplicationPage
    {
        string message = "";
        string msg = "";
        public Calender()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (NavigationContext.QueryString.TryGetValue("msg", out msg))
            {
                message = msg;
            }
        }

        private void ApplicationBarSaveButton_Click(object sender, EventArgs e)
        {
            // The code in the following steps goes here.
            // The code in the following steps goes here.
            String name = System.Guid.NewGuid().ToString();
            // Get the begin time for the notification by combining the DatePicker
            // value and the TimePicker value.
            DateTime date = (DateTime)beginDatePicker.SelectedDate;
            DateTime time = (DateTime)beginTimePicker.Value;
            DateTime beginTime = date + time.TimeOfDay;

            // Make sure that the begin time has not already passed.
            if (beginTime < DateTime.Now)
            {
                MessageBox.Show("the begin date must be in the future.");
                return;
            }


            Reminder reminder = new Reminder(name);
            reminder.Title = message;
            reminder.BeginTime = beginTime;
            reminder.ExpirationTime = beginTime;
            reminder.Content = message + " " + "is going to be expired on" + " " + beginTime;
            RecurrenceInterval recurrence = RecurrenceInterval.None;
            Uri navigationUri = new Uri("/MainPage.xaml", UriKind.Relative);
            reminder.RecurrenceType = recurrence;
            reminder.NavigationUri = navigationUri;

            // Register the reminder with the system.
            ScheduledActionService.Add(reminder);
            NavigationService.Navigate(new Uri("/Appointment.xaml", UriKind.RelativeOrAbsolute));
            // Navigate back to the main reminder list page.
            //NavigationService.GoBack();
        }    
    }
}


