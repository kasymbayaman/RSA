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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Numerics;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RSAClass rsa = new RSAClass();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void EncryptBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isValidInputs()) OutputText.Text = "Некорректный ввод";
            else
            {
                AssignKeys();
                string inputText = InputText.Text;
                string encrypted = rsa.Encrypt(inputText);
                OutputText.Text = encrypted;
            }
        }

        private void DecryptBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isValidInputs()) OutputText.Text = "Некорректный ввод";
            else if (!IsDigitsOnly(InputText.Text)) OutputText.Text = "Некорректный шифротекст";
            else
            {
                AssignKeys();

                string inputText = InputText.Text;
                string decrypted = rsa.Decrypt(inputText);
                OutputText.Text = decrypted;
            }
        }


        private void GenerateBtn_Click(object sender, RoutedEventArgs e)
        {
            rsa.GenerateKeys();
            BlockN.Text = rsa.N.ToString();
            Blocke.Text = rsa.e.ToString();
            Blocks.Text = rsa.s.ToString();
        }

        private bool isValidInputs()
        {
            bool isValid = true;
           try
            {
                BigInteger.Parse(BlockN.Text);
                BigInteger.Parse(Blocke.Text);
                BigInteger.Parse(Blocks.Text);
            }
            catch (Exception)
            {
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(InputText.Text)) isValid = false;
            return isValid;
        }

        private void AssignKeys()
        {
            rsa.N = BigInteger.Parse(BlockN.Text);
            rsa.e = BigInteger.Parse(Blocke.Text);
            rsa.s = BigInteger.Parse(Blocks.Text);
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            InputText.Text = "";
            OutputText.Text = "";
            BlockN.Text = "";
            Blocke.Text = "";
            Blocks.Text = "";
        }

        bool IsDigitsOnly(string str)
        {
            str = str.Trim();
            foreach (char c in str)
            {
                if ((c < '0' || c > '9') && c != '|')
                  return false;
            }

            return true;

           
        }


    }
}
