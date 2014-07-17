using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SaveFood.Resources;
using Microsoft.Phone.Tasks;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;



namespace SaveFood
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        string msg = "";
        string message = "";
        //public static List<SaveFoodStructure> ListsaveFoodData = new List<SaveFoodStructure>();
        //List<SaveAppointmentTask>
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (NavigationContext.QueryString.TryGetValue("msg", out msg))
            {
                message = msg;
            }
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                XDocument document;
                XElement tagRegistry = null;

                if (storage.FileExists("/AppontmentListNew.xml"))
                {
                    using (var stream = storage.OpenFile("/AppontmentListNew.xml", FileMode.Open))
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
                            date = (DateTime)c.Attribute("date")
                        };
                foreach (var obj in q)
                {
                    string remainderedString=obj.Name + " "+obj.date.Day + "-"+obj.date.Month + "-"+obj.date.Year;
                    if (message == remainderedString)
                    {
                        document.Descendants("Appointments").Where(xe => xe.Attribute("name") != null
                                                        && xe.Attribute("name").Value == obj.Name).Remove();
                    }

                    using (Stream stream = storage.CreateFile("/AppontmentListNew.xml"))
                    {
                        document.Save(stream);

                    }
                }
            }
        }

        private void calender_Click(object sender, RoutedEventArgs e)
        {
            string FoodName="";
            if (AddFood.Text == "Food Name")
            {
                MessageBox.Show("Please Enter the food name");
                // NavigationService.Navigate(new Uri("/MainPage.xaml?" + FoodName, UriKind.Relative));
            }
            else
            {
                FoodName = AddFood.Text;
                NavigationService.Navigate(new Uri("/Calender.xaml?msg=" + FoodName, UriKind.Relative));
            }

            
        }

        /*private void AppoinmentList_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Appointment.xaml?", UriKind.Relative));
        }*/

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Appointment.xaml?", UriKind.Relative));
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}