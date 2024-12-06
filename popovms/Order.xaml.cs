using popovms.Entities;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace popovms
{
    /// <summary>
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        Entities.practicaEntities Bd;
        DispatcherTimer timer = new DispatcherTimer();
        int timeLeft = 600; // Установите начальное время в 10 минуты (600 секунд)
        public Order()
        {
            Bd = new Entities.practicaEntities();
            InitializeComponent();
            clientBtn_Click(null, null);
            // Установка интервала таймера в 1 секунду
            timer.Interval = TimeSpan.FromSeconds(1);
            // Добавление обработчиков событий для движения мыши и нажатия клавиш
            this.MouseMove += MainWindow_MouseMove;
            this.KeyDown += MainWindow_KeyDown;
            // Добавление обработчика события для таймера
            timer.Tick += Timer_Tick;
            // Запуск таймера
            timer.Start();
        }
        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            // Сбросьте таймер и время при движении мыши
            timer.Stop();
            timeLeft = 600;
            timer.Start();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Сбросьте таймер и время при нажатии клавиши
            timer.Stop();
            timeLeft = 600;
            timer.Start();
        }
     
        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
           
            // Обновите отображение оставшегося времени
            TimerBlocking.Text = "Таймер: " + TimeSpan.FromSeconds(timeLeft).ToString(@"mm\:ss");
            if (timeLeft == 300)
            {
                timer.Stop();
                timeLeft = 300; timer.Start();
                MessageBox.Show("Таймер", "До закрытия окна осталось пять минут");
                // Покажите предупреждение, когда остается одна минута
           
            }
            else if (timeLeft <= 0)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();          
                this.Close();
                // Ваш код для блокировки входа здесь
                timer.Stop(); // Остановите таймер
            }
        }
        public void hidden()
        {
            clientLV.Visibility = Visibility.Hidden;
            clientGr.Visibility = Visibility.Hidden;
            clientBtn.Background = Brushes.White;
            servicesBtn.Background = Brushes.White;
            servicesGr.Visibility = Visibility.Hidden;
            servicesLV.Visibility = Visibility.Hidden;
            gridOrders.Visibility = Visibility.Hidden;
            ordersBtn.Background = Brushes.White;

        }
        private void addclient_Click(object sender, RoutedEventArgs e)
        {
           addclient addclient  = new addclient();
            addclient.Show();
            this.Close();

        }

        private void clientBtn_Click(object sender, RoutedEventArgs e)
        {
            hidden();
            clientLV.Visibility = Visibility.Visible;
            clientGr.Visibility = Visibility.Visible;
            clientBtn.Background = new SolidColorBrush(Color.FromRgb(118, 227, 131)); 
            clientLV.ItemsSource = Bd.client.ToList();
        }

        private void servicesBtn_Click(object sender, RoutedEventArgs e)
        {
            hidden();
            servicesBtn.Background = new SolidColorBrush(Color.FromRgb(118, 227, 131));
            servicesGr.Visibility = Visibility.Visible;
            servicesLV.Visibility = Visibility.Visible;
            servicesLV.ItemsSource = Bd.services.ToList();
        }

        private void addservices_Click(object sender, RoutedEventArgs e)
        {
            if (nameservices.Text != string.Empty && costservices.Text != string.Empty)
            {
                Random rnd = new Random();
                string codsim = "qwertyuiopasdfghjklzxcvbnm1234567890";
               string cod = "";           
                for (int i = 0; i < 8; i++)
                {
                    char generat = codsim[rnd.Next(codsim.Length)];
                    cod += generat;

                }
                services services = new services
                {
                    name = nameservices.Text,
                    cod = cod,
                    Cost = Convert.ToInt32(costservices.Text),
                };
                Bd.services.Add(services);
                Bd.SaveChanges();
                servicesLV.ItemsSource = Bd.services.ToList();
            }
            else
            {
                MessageBox.Show("Не все поля заполнины");
            }
        }
        // поиск клиентов
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (poiskclient.Text != string.Empty)
            {
                var cliet = poiskclient.Text;
                var _poisk = Bd.client.Where(x => x.surname.ToLower().StartsWith(cliet.ToLower()) |
                x.name.ToLower().StartsWith(cliet.ToLower()) | x.middlename.ToLower().StartsWith(cliet.ToLower()));
                clientLV.ItemsSource = _poisk.ToList();
            }
            else
            {
                clientLV.ItemsSource = Bd.client.ToList();
            }

        }
        // поиск услуг
        private void poiskservices_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (poiskservices.Text != string.Empty)
            {
                var services = poiskservices.Text;
                var _poisk = Bd.services.Where(x => x.name.ToLower().StartsWith(poiskservices.Text.ToLower()) | 
                x.cod.ToLower().StartsWith(services.ToLower()));
                servicesLV.ItemsSource = _poisk.ToList();
            }
            else
            {
                servicesLV.ItemsSource = Bd.services.ToList();
            }
        }

        private void costservices_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            costservices.Text = new string(costservices.Text.Where(char.IsDigit).ToArray());
        }

        private void backbtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ordersBtn_Click(object sender, RoutedEventArgs e)
        {
          hidden();
          gridOrders.Visibility = Visibility.Visible;
          ordersBtn.Background = new SolidColorBrush(Color.FromRgb(118, 227, 131));
            ObservableCollection<popovms.Entities.services> observableServices = new ObservableCollection<popovms.Entities.services>(Bd.services);
            ordersServices.ItemsSource = observableServices;
            ordersClient.ItemsSource = Bd.client.ToList();     
            listServices.Items.Clear();
            surnameNameMiddlename.Text = string.Empty;
            timeOrders.Text = string.Empty;
        }
        // Добавление услуг в список выбраных
        private void ordersServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (services)ordersServices.SelectedItem;

            if (selectedItem != null)
            {
                ObservableCollection<services> items = (ObservableCollection<services>)ordersServices.ItemsSource;
                var id_services = (services)ordersServices.SelectedItem;
                listServices.Items.Add(selectedItem);              
                items.Remove(selectedItem);
               
            }
        }

      
        // Удаданение услуг из списка выбраных
        private void listServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selectedItem = (services)listServices.SelectedItem;
            if (selectedItem != null)
            {
                ObservableCollection<services> items = (ObservableCollection<services>)ordersServices.ItemsSource;
                items.Add(selectedItem); // Добавляем обратно
                listServices.Items.Remove(selectedItem); // Удаляем из listServices
             
            }
        }

        private void arrangeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (surnameNameMiddlename.Text != string.Empty)
            {
                if (timeOrders.Text != string.Empty)
                {
                    if (listServices.Items.Count != null)
                    {
                        DateTime now = DateTime.Now;
                        DateTime date = new DateTime(now.Year, now.Month, now.Day);
                        TimeSpan time = new TimeSpan(now.Hour, now.Minute, now.Second);
                        orders orders = new orders
                        {
                            date_add = date,
                            time_orders = time,
                            id_client = ((client)ordersClient.SelectedItem).id,
                            id_status = 3,
                            date_closing = null,
                            time_rental = Convert.ToInt32(timeOrders.Text),
                        };
                        var id_orders = Bd.orders.Add(orders);
                        Bd.SaveChanges();
                        foreach (var item in listServices.Items)
                        {
                            var id_services = (services)item;
                            services_orders services_Orders = new services_orders
                            {
                                id_orders = id_orders.id,
                                id_services = id_services.id,
                            };
                            Bd.services_orders.Add(services_Orders);
                        }
                        Bd.SaveChanges();
                        MessageBox.Show("Заказ оформлен");
                        ordersBtn_Click(null, null);

                    }
                    else
                    {
                        MessageBox.Show("Выберите услуги");
                    }

                }
                else
                {
                    MessageBox.Show("Введите время проката");
                }
            }
            else
            {
              MessageBox.Show("Выберите клиента");
            }
        }

            private void ordersClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            surnameNameMiddlename.Text = ((client)ordersClient.SelectedItem).surname + " "
              + ((client)ordersClient.SelectedItem).name + " " + ((client)ordersClient.SelectedItem).middlename;
        }
    }
}
