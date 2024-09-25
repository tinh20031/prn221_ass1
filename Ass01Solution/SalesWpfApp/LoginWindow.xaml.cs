using BusinessObject.Enum;
using DataAccess.DAO;
using DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace SalesWpfApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IMemberRepository _memberRepository;

        string Email
        {
            get => txtEmail.Text;
            set => txtEmail.Text = value;
        }

        string Password
        {
            get => txtPassword.Password;
            set => txtPassword.Password = value;
        }


        public LoginWindow()
        {
            InitializeComponent();

            if (_memberRepository is null)
            {
                _memberRepository = new MemberRepository();
            }

            //Events
            btnSubmit.Click += BtnSubmit_Click;

            var window = Window.GetWindow(this);
            window.KeyDown += Window_KeyDown;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            PerformSubmit();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            PerformSubmit();
        }

        private void PerformSubmit()
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                                                    .SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json", true, true).Build();

                string adminEmail = configuration["AdminAccount:Email"]!;
                string adminPassword = configuration["AdminAccount:Password"]!;


                var member = _memberRepository.Login(Email, Password);

                if (member is not null)
                {
                    MemberSession.CurrentMember = member;
                    MemberSession.Role = Role.User.ToString();

                    new HomeWindow().Show();
                }
                else if (Email == adminEmail && Password == adminPassword)
                {
                    MemberSession.Role = Role.Admin.ToString();
                    new HomeWindow().Show();
                }
                else
                {
                    MessageBox.Show("Wrong email or password", "Login error", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
