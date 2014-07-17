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
using System.Windows.Media;
using Microsoft.Phone.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Globalization;

namespace SaveFood
{
    
    public partial class Calender : PhoneApplicationPage
    {
        string msg = "";
        string message = "";
        System.Xml.Linq.XDocument xmlDoc;

        

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

        public static class FoodSaveData
        {
            public static List<SaveFoodStructure> ListsaveFoodData = new List<SaveFoodStructure>();
            public static string saveFoodCalendarId = null;
        }
        public class SaveFoodStructure
        {
            public string name { get; set; }
            public int year { get; set; }
            //public DateTime Begindate { get; set; }
            public DateTime Expairydate { get; set; }
            public string ExpiryTime { get; set; }
            public int month { get; set; }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFoodStructure saveFood = new SaveFoodStructure();
            NumberStyles style;
            CultureInfo culture;

            saveFood.name = message;
            saveFood.Expairydate = Cal.SelectedDate;
            saveFood.Expairydate = (DateTime)currentTime.Value;
            saveFood.year = Cal.SelectedYear;
            saveFood.month = Cal.SelectedMonth;
              
            FoodSaveData.ListsaveFoodData.Add(saveFood);
          
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

                XElement srcTree = new XElement("Appointments", new XAttribute("name", saveFood.name),
                                                                new XAttribute("date", saveFood.Expairydate)
                                                                
                                                                );
                tagRegistry.Add(srcTree);

                using (Stream stream = storage.CreateFile("/AppontmentLists.xml"))
                {
                    document.Save(stream);
                    
                }
            }
            NavigationService.Navigate(new Uri("/MainPage.xaml?msg=" + FoodSaveData.ListsaveFoodData, UriKind.Relative));
            
        }
    }
}