using Client.Models;
using Client.Services;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Phone> phones;
        private readonly HttpClientService httpService;

        public MainWindow()
        {
            InitializeComponent();


            //phones = new List<Phone>
            //{
            //    new Phone { Company = "Apple", Title = "iPhone 10", Price = 58000},
            //    new Phone { Company = "Samsung", Title = "Galaxy J20", Price = 120000},
            //    new Phone { Company = "Leica", Title = "Cam 10", Price = 200000}
            //};

            //mainListBox.ItemsSource = phones;

            httpService = new HttpClientService("http://localhost:5050/api");
            _ = LoadPhones();
        }


        private async Task LoadPhones() 
        { 
            phones = await httpService.GetPhones(); 
            Dispatcher.Invoke(() => 
            { 
                mainListBox.ItemsSource = null; 
                mainListBox.ItemsSource = phones; 
            }); 
        }




        public async void AddClick(object sender, RoutedEventArgs e)
        {

            var phone = new Phone
            {
                Company = ModelInput.Text,
                Title = BrandInput.Text,
                Price = Convert.ToDecimal(PriceInput.Text)
            };

            //mainListBox.ItemsSource = null;
            //mainListBox.ItemsSource = phones;

            await httpService.AddPhones(phone);
            await LoadPhones();
        }


        public async void RemoveClick(object sender, RoutedEventArgs e)
        {
            //Dispatcher.Invoke(() =>
            //{
            //    phones.Remove(mainListBox.SelectedItem as Phone);

            //    mainListBox.ItemsSource = null;
            //    mainListBox.ItemsSource = phones;
            //});

            if (mainListBox.SelectedItem is Phone selected)
            {
                await httpService.DeletePhones(selected.Id);
                await LoadPhones();
            }
        }
    }
}