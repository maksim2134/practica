using popovms.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace popovms
{
    /// <summary>
    /// Логика взаимодействия для admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        Entities.practicaEntities Bd;
        DispatcherTimer timer = new DispatcherTimer();
        int timeLeft = 600; // Установите начальное время в 10 минуты (600 секунд)
        public admin()
        {
            Bd = new Entities.practicaEntities();
            InitializeComponent();
            employeeBtn_Click(null, null);
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
                timer.Stop(); // Остановите таймер
            }
        }

        private void filtrac_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            employeeLV.ItemsSource = Bd.employee.Where(x => x.login == filtrac.Text).ToList();
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            employeeLV.ItemsSource = Bd.employee.ToList();
            filtrac.SelectedIndex = -1;
        }

        private void ordersBtn_Click(object sender, RoutedEventArgs e)
        {
            ordersBtn.Background = new SolidColorBrush(Color.FromRgb(118, 227, 131));
            employeeBtn.Background = Brushes.White;
            employeeLV.Visibility = Visibility.Hidden;
            filterGr.Visibility = Visibility.Hidden;
            ordersLV.Visibility = Visibility.Visible;
            ordersLV.ItemsSource = Bd.services_orders.AsEnumerable().GroupBy(z => z.orders).Select(z => new
            {
                Itemsone = $"Клиент {Bd.client.Where(x => x.id == z.Key.id_client).FirstOrDefault().surname} " +
                $"{Bd.client.Where(x => x.id == z.Key.id_client).FirstOrDefault().name} {Bd.client.Where(x => x.id == z.Key.id_client).FirstOrDefault().middlename}",

                Itemstwo = $"Статус: {Bd.status.Where(x => x.id == z.Key.id_status).FirstOrDefault().name} " +
                $"Дата и время создание заказа: {z.Key.date_add}  {z.Key.time_orders} Дата закрытии:{z.Key.date_closing}",

                Itemsthree = $"Время проката {z.Key.time_rental} мин " +
                $"Услуги: {string.Join(", ", z.Select(c => Bd.services.FirstOrDefault(v => v.id == c.id_services).name))}",
            }).ToList();

        }

        private void employeeBtn_Click(object sender, RoutedEventArgs e)
        {
            employeeBtn.Background = new SolidColorBrush(Color.FromRgb(118, 227, 131));
            ordersBtn.Background = Brushes.White;
            employeeLV.ItemsSource = Bd.employee.ToList();
            filtrac.ItemsSource = Bd.employee.ToList();
            employeeLV.Visibility = Visibility.Visible;
            filterGr.Visibility = Visibility.Visible;
            ordersLV.Visibility = Visibility.Hidden;

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
