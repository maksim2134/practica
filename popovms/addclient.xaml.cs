using popovms.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Xml.Linq;

namespace popovms
{
    /// <summary>
    /// Логика взаимодействия для addclient.xaml
    /// </summary>
    public partial class addclient : Window
    {
        Entities.practicaEntities Bd;
        public addclient()
        {
            Bd = new Entities.practicaEntities();
            InitializeComponent();
        }  
        // Только цифры в тексте
        private void SeriesClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            SeriesClient.Text = new string(SeriesClient.Text.Where(char.IsDigit).ToArray());
        }
     
        private void CodeClient_TextChanged(object sender, TextChangedEventArgs e)
        {
           
            CodeClient.Text = new string(CodeClient.Text.Where(char.IsDigit).ToArray());
        }
        
        private void NumberClient_TextChanged(object sender, TextChangedEventArgs e)
        {
      
            NumberClient.Text = new string(NumberClient.Text.Where(char.IsDigit).ToArray());
        }
      
        private void IndexClient_TextChanged(object sender, TextChangedEventArgs e)
        {
         
            IndexClient.Text = new string(IndexClient.Text.Where(char.IsDigit).ToArray());
        }

        private void SaveAddClient_Click(object sender, RoutedEventArgs e)
        {
            if(SurnameClient.Text != string.Empty &&
                NameClient.Text != string.Empty && MiddlenameClient.Text!= string.Empty 
                && SeriesClient.Text != string.Empty &&
                NumberClient.Text != string.Empty 
                && IndexClient.Text != string.Empty && DateOfBirthClient.Text != string.Empty
                && AddressClient.Text != string.Empty && EmailClient.Text != string.Empty )
            {
                 client client = new client
                {
                    id = Convert.ToInt32(CodeClient.Text),
                    surname = SurnameClient.Text,
                    name = NameClient.Text,
                    middlename = MiddlenameClient.Text,
                    seria = Convert.ToInt32(SeriesClient.Text),
                    number = Convert.ToInt32(NumberClient.Text),
                    indexs = Convert.ToInt32(IndexClient.Text),
                    date_birth = Convert.ToDateTime(DateOfBirthClient.Text),
                    address = AddressClient.Text,
                    email = EmailClient.Text,
                };
                Bd.client.Add(client);
                Bd.SaveChanges();
                backBtn_Click(null, null);
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
           
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            Order order = new Order();
            order.Show();
            this.Close();
        }
    }
}
