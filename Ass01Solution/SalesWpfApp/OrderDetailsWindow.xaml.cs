using BusinessObject.Enum;
using DataAccess.DTOs.Order;
using DataAccess.Repositories;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SalesWpfApp
{
    public partial class OrderDetailsWindow : Window
    {
        public int OrderId { get; set; }
        public bool IsUpdate { get; set; } = true;

        public DateTime? RequiredDate
        {
            get { return dpRequiredDate.SelectedDate; }
            set { dpRequiredDate.SelectedDate = value; }
        }

        public DateTime? ShippedDate
        {
            get { return dpShippedDate.SelectedDate; }
            set { dpShippedDate.SelectedDate = value; }
        }

        public decimal? Freight
        {
            get { return Convert.ToDecimal(txtFreight.Text); }
            set { txtFreight.Text = value.ToString(); }
        }

        private readonly IOrderRepository _orderRepository;

        public OrderDetailsWindow()
        {
            InitializeComponent();

            _orderRepository = new OrderRepository();

            this.Loaded += OrderDetailsWindow_Loaded;
            btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Update this order?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result is not MessageBoxResult.Yes)
                {
                    return;
                }

                _orderRepository.UpdateOrder(OrderId, new UpdateOrderDto
                {
                    OrderDate = dpOrderDate.SelectedDate!.Value,
                    RequiredDate = RequiredDate,
                    ShippedDate = ShippedDate,
                    Freight = Freight,
                    Status = (string)cbStatus.SelectedItem,
                });

                MessageBox.Show("Update order success!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OrderDetailsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var order = _orderRepository.GetOrderDetailsById(OrderId);
                if (order is null)
                {
                    return;
                }

                var orderStatuses = Enum.GetValues(typeof(OrderStatus))
                                        .Cast<OrderStatus>()
                                        .Select(s => s.ToString())
                                        .ToList();

                cbStatus.ItemsSource = orderStatuses;
                this.DataContext = order;

                cbStatus.SelectedItem = order.Status;
                txtMember.Text = order.Member.Email;
                dpOrderDate.SelectedDate = order.OrderDate;
                dpRequiredDate.SelectedDate = order.RequiredDate;
                dpShippedDate.SelectedDate = order.ShippedDate;
                Freight = order.Freight;

                dgOrderDetails.ItemsSource = order.OrderDetails.Select(od => new
                {
                    ProductId = od.Product.ProductId,
                    ProductName = od.Product.ProductName,
                    SubTotal = od.UnitPrice,
                    Quantity = od.Quantity,
                    Discount = od.Discount
                });

                // Check user role and disable editing if the role is User
                if (MemberSession.Role == Role.User.ToString())
                {
                    DisableEditing();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DisableEditing()
        {
            // Disable all input controls
            dpOrderDate.IsEnabled = false;
            dpRequiredDate.IsEnabled = false;
            dpShippedDate.IsEnabled = false;
            txtFreight.IsReadOnly = true;
            cbStatus.IsEnabled = false;
            txtId.IsEnabled = false;
            txtMember.IsEnabled = false;

            // Hide or disable the save button
            btnSave.Visibility = Visibility.Collapsed; // Alternatively, you can disable it: btnSave.IsEnabled = false;
        }
    }
}
