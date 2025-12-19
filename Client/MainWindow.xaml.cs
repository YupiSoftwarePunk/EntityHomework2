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
            try
            {
                phones = await httpService.GetPhones();
                Dispatcher.Invoke(() =>
                {
                    mainListBox.ItemsSource = null;
                    mainListBox.ItemsSource = phones;
                });

                Dispatcher.Invoke(() =>
                {
                    ModelInput.Clear();
                    BrandInput.Clear();
                    PriceInput.Clear();
                });

                HideErrorMessage();
            }
            catch(Exception ex) 
            {
                ShowErrorMessage($"Ошибка при загрузке данных: {ex.Message}");
            }
        }




        public async void AddClick(object sender, RoutedEventArgs e)
        {
            HideErrorMessage();

            if (!ValidateInput(out string errorMessage))
            {
                //Dispatcher.Invoke(() =>
                //{
                //    ModelInput.Clear();
                //    BrandInput.Clear();
                //    PriceInput.Clear();
                //});

                ShowErrorMessage(errorMessage);

                return;
            }

            var phone = new Phone
            {
                Company = ModelInput.Text,
                Title = BrandInput.Text,
                Price = Convert.ToDecimal(PriceInput.Text)
            };

            if (phone == null)
            {
                mainListBox.ItemsSource = null;
            }


            //mainListBox.ItemsSource = null;
            //mainListBox.ItemsSource = phones;

            await httpService.AddPhones(phone);
            await LoadPhones();

            Dispatcher.Invoke(() =>
            {
                mainListBox.ItemsSource = null;
                mainListBox.ItemsSource = phones;
            });


            Dispatcher.Invoke(() =>
            {
                ModelInput.Clear();
                BrandInput.Clear();
                PriceInput.Clear();
            });
        }


        public async void RemoveClick(object sender, RoutedEventArgs e)
        {
            HideErrorMessage();
            //if (!ValidateInput())
            //{
            //    Dispatcher.Invoke(() =>
            //    {
            //        ModelInput.Clear();
            //        BrandInput.Clear();
            //        PriceInput.Clear();
            //    });
            //}


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

            Dispatcher.Invoke(() =>
            {
                mainListBox.ItemsSource = null;
                mainListBox.ItemsSource = phones;
            });


            Dispatcher.Invoke(() =>
            {
                ModelInput.Clear();
                BrandInput.Clear();
                PriceInput.Clear();
            });
        }


        private bool ValidateInput(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(BrandInput.Text))
            {
                errorMessage = "Поле 'Производитель' не может быть пустым";
                BrandInput.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(ModelInput.Text))
            {
                errorMessage = "Поле 'Модель' не может быть пустым";
                ModelInput.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(PriceInput.Text))
            {
                errorMessage = "Поле 'Цена' не может быть пустым";
                PriceInput.Focus();
                return false;
            }

            if (!decimal.TryParse(PriceInput.Text, out decimal price))
            {
                errorMessage = "Введите корректное числовое значение для цены";
                PriceInput.Focus();
                PriceInput.SelectAll();
                return false;
            }

            if (price <= 0)
            {
                errorMessage = "Цена должна быть больше нуля";
                PriceInput.Focus();
                PriceInput.SelectAll();
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }




        private void ShowErrorMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                ErrorMessage.Text = message;
                ErrorBorder.Visibility = Visibility.Visible;
            });
        }


        private void HideErrorMessage()
        {
            Dispatcher.Invoke(() =>
            {
                ErrorMessage.Text = string.Empty;
                ErrorBorder.Visibility = Visibility.Collapsed;
                //errorTimer.Stop();
            });
        }
    }
}