using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace popovms
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string captchaText = "";
        int failcaptcha = 0, secondsBlock = 20, id_userblock;
        DispatcherTimer timerBlock = new DispatcherTimer();
        DispatcherTimer time = new DispatcherTimer();
        Entities.practicaEntities Bd;
        public MainWindow()
        {
            InitializeComponent();
            time.Interval = TimeSpan.FromSeconds(1);
            time.Tick += tim_tick;
            Bd = new Entities.practicaEntities();
        }
        int fail = 0;
        //просмотр пароля
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Password.Visibility = Visibility.Hidden;
            password.Visibility = Visibility.Visible;
            password.Text = Password.Password;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            password.Visibility = Visibility.Hidden;
            Password.Visibility = Visibility.Visible;
            Password.Password = password.Text;
        }
        //авторизация
        private void enter_Click(object sender, RoutedEventArgs e)
        {
            if (login.Text != string.Empty && (password.Text != string.Empty || Password.Password != string.Empty))
            {
                if (Bd.employee.Any(x => x.login == login.Text))
                {
               

                if (Bd.employee.Any(x => x.login == login.Text && (password.Text == x.password || Password.Password == x.password)))
                {
                    var id_staff = Bd.employee.Where(x => x.login == login.Text).FirstOrDefault();
                    DateTime now = DateTime.Now;
                    DateTime dateWithoutMilliseconds = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
                    id_staff.Last_entry = dateWithoutMilliseconds;
                    id_staff.type_entrance = 1;
                    Bd.SaveChanges();
                    if (Bd.employee.Any(x => x.id_post == 1 && x.login == login.Text))
                    {
                        admin admin = new admin();
                        admin.Show();
                        this.Close();
                    }
                   
                    else if (Bd.employee.Any(x => (x.id_post == 3 || x.id_post == 2) && x.login == login.Text))
                    {
                        Order order = new Order();
                        order.Show();
                        this.Close();
                    
                        
                    }
                    else
                    { //таймер блокировки                     
                        MessageBox.Show("Пользователь заблокирован. Осталось " + secondsBlock.ToString() + " секунд");
                        timerBlock.Interval = TimeSpan.FromSeconds(1);
                        timerBlock.Tick += timerBlock_tick;
                        timerBlock.Start();
                    }
                }
                else
                {
                    fail++;
                    if (fail == 3)
                    {
                        Avtorizazia.Visibility = Visibility.Hidden;
                        captcha.Visibility = Visibility.Visible;
                        time.Stop();
                        generatorcaptcha();
                        fail = 0;
                        var id_staff = Bd.employee.Where(x => x.login == login.Text).FirstOrDefault();
                            id_staff.Last_entry = null;
                        id_staff.type_entrance = 2;
                        Bd.SaveChanges();
                        }
                    else MessageBox.Show("Неверный пароль");
                
                }
                }
                else
                {
                     MessageBox.Show("Неверный логин");
                }
            }
            else MessageBox.Show("Введите данные");
        }
        //обновление капчи
        private void Obnovit(object sender, RoutedEventArgs e)
        {
            generatorcaptcha();
        }
        //генератор капчи
        public void generatorcaptcha()
        {
            canva.Children.Clear();
            Random rnd = new Random();
            RotateTransform rotateTransform = new RotateTransform();
            TextBlock textBlock = new TextBlock();
            string captchasim = "qwertyuiopasdfghjklzxcvbnm1234567890";
            captchaText = "";
            rotateTransform.Angle = rnd.Next(-20, 20);
            for (int i = 0; i < 4; i++)
            {
                char generat = captchasim[rnd.Next(captchasim.Length)];
                captchaText += generat;

            }
            rotateTransform.Angle = rnd.Next(-20, 20);
            textBlock.Text = captchaText;
            textBlock.FontSize = 32;
            textBlock.RenderTransform = rotateTransform;
            Canvas.SetLeft(textBlock, rnd.Next(20, 100));
            Canvas.SetTop(textBlock, rnd.Next(20, 30));
            canva.Children.Add(textBlock);
            for (int i = 0; i < 600; i++)
            {

                Ellipse ellipse = new Ellipse();
                int r = rnd.Next(3, 5);
                ellipse.Height = r; ellipse.Width = r;
                Brush brus = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(1, 255), (byte)rnd.Next(1, 255), (byte)rnd.Next(1, 233)));
                ellipse.Fill = brus;
                Canvas.SetLeft(ellipse, rnd.Next(250));
                Canvas.SetTop(ellipse, rnd.Next(100));

                canva.Children.Add(ellipse);
            }
            for (int i = 0; i < 2; i++)
            {
                Line line = new Line();
                line.X1 = rnd.Next(0, 50);
                line.Y1 = rnd.Next(0, 50);
                line.X2 = rnd.Next(150, 250);
                line.Y2 = rnd.Next(51, 100);
                Brush brus = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(1, 255), (byte)rnd.Next(1, 255), (byte)rnd.Next(1, 233)));
                line.Stroke = brus;
                line.StrokeThickness = 3;
                canva.Children.Add(line);

            }

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (textcaptcha.Text != String.Empty)
            {
                if (textcaptcha.Text.ToLower() == captchaText)
                {
                    textcaptcha.Text = string.Empty;
                    login.Text = string.Empty;
                    password.Text = string.Empty;
                    Password.Password = string.Empty;
                    Avtorizazia.Visibility = Visibility.Visible;
                    captcha.Visibility = Visibility.Hidden;
                }
                else
                {
                    failcaptcha++;
                    generatorcaptcha();
                    if (failcaptcha > 2)
                    {
                        captcha.Visibility = Visibility.Hidden;
                        Avtorizazia.Visibility = Visibility.Visible;
                        enter.IsEnabled = false;
                        textcaptcha.Text = string.Empty;
                        login.Text = string.Empty;
                        password.Text = string.Empty;
                        Password.Password = string.Empty;
                        failcaptcha = 0;
                        sec = 10;
                        time.Start();
                       

                    }
                    else MessageBox.Show("Неверная каптча");
                }
            }
            else MessageBox.Show("Введите каптчу");

        }
        int sec = 10;
        public void tim_tick(object sender, EventArgs e)
        {
            Time.Text = "Система заблокирована. Осталось " + sec.ToString() + " секунд";
            if (sec == 0)
            {
                Time.Text = string.Empty;
                enter.IsEnabled = true;
                time.Stop();
            }
            sec--;

        }
        public void timerBlock_tick(object sender, EventArgs e)
        {
            if (secondsBlock == 0)
            {                                            
                timerBlock.Stop();
            }
            secondsBlock--;
        }

    }
}

