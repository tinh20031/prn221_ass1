using BusinessObject.Enum;
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

namespace SalesWpfApp
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();

            this.Loaded += HomeWindow_Loaded;
            btnMembers.Click += BtnMembers_Click;
            btnProducts.Click += BtnProducts_Click;
            btnOrders.Click += BtnOrders_Click;
            btnLogOut.Click += BtnLogOut_Click;
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Are you sure you want to log out?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result is not MessageBoxResult.Yes)
                {
                    return;
                }

                MemberSession.Role = string.Empty;
                MemberSession.CurrentMember = null;

                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HomeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            bool isAdmin = MemberSession.Role == Role.Admin.ToString();

            if (!isAdmin)
            {
                btnMembers.Content = "Profile";
            }
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
            new WindowOrders().Show();
        }

        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {
            new WindowProducts().Show();
        }

        private void BtnMembers_Click(object sender, RoutedEventArgs e)
        {
            bool isAdmin = MemberSession.Role == Role.Admin.ToString();
            if (isAdmin)
            {
                new WindowMembers().Show();
                return;
            }

            var member = MemberSession.CurrentMember;
            var profileWindow = new MemberDetailsWindow
            {
                IsUpdate = true,
                MemberId = member?.MemberId
            };

            profileWindow.Show();
        }
    }


}
