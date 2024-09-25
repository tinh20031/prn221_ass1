using BusinessObject.Enum;
using DataAccess.DTOs.Order;
using DataAccess.Repositories;
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
    /// Interaction logic for WindowOrders.xaml
    /// </summary>
    public partial class WindowOrders : Window
    {
        private readonly IOrderRepository _orderRepository;

        private int? memberId = null;

        public DateTime? StartDate
        {
            get => dpStartDate.SelectedDate;
            set => dpStartDate.SelectedDate = value;
        }

        public DateTime? EndDate
        {
            get => dpEndDate.SelectedDate;
            set => dpEndDate.SelectedDate = value;
        }

        public WindowOrders()
        {
            InitializeComponent();

            if (_orderRepository is null)
            {
                _orderRepository = new OrderRepository();
            }

            this.Loaded += WindowOrders_Loaded;
            btnApply.Click += BtnApply_Click;
            btnReset.Click += BtnReset_Click;
            btnDetails.Click += BtnDetails_Click;
            btnDelete.Click += BtnDelete_Click;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var order = dgOrders.SelectedItem as GetOrderDto;
                if (order is null)
                {
                    throw new Exception("Please select an order");
                }

                var result = MessageBox.Show("Are you sure you want to delete this order?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result is not MessageBoxResult.Yes)
                {
                    return;
                }

                _orderRepository.DeleteOrder(order.OrderId);

                MessageBox.Show("Delete order success");
                LoadOrders();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var order = dgOrders.SelectedItem as GetOrderDto;
                if (order is null)
                {
                    throw new Exception("Please select an order");
                }

                var detailsWindow = new OrderDetailsWindow
                {
                    IsUpdate = true,
                    OrderId = order.OrderId
                };

                detailsWindow.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Reset filter?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) is not MessageBoxResult.Yes)
            {
                return;
            }

            StartDate = null;
            EndDate = null;
            LoadOrders();
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void WindowOrders_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var member = MemberSession.CurrentMember;
                bool isAdmin = MemberSession.Role == Role.Admin.ToString();

                btnDelete.Visibility = isAdmin ? Visibility.Visible : Visibility.Hidden;
                

                //if user, only allow to view his/her orders
                memberId = member is not null ? member.MemberId : null;

                LoadOrders();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void LoadOrders()
        {
            var orders = _orderRepository.GetOrders(StartDate, EndDate, memberId);
            dgOrders.ItemsSource = orders;
        }
    }
}
