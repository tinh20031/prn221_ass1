using BusinessObject;
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
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        private readonly IOrderRepository _orderRepository;

        private readonly IOrderDetailRepository _orderDetailRepository;

        private int orderId;



        public CartWindow()
        {
            InitializeComponent();

            if (_orderRepository is null)
            {
                _orderRepository = new OrderRepository();
            }

            if (_orderDetailRepository is null)
            {
                _orderDetailRepository = new OrderDetailRepository();
            }

            this.Loaded += CartWindow_Loaded;
            btnOrder.Click += BtnOrder_Click;
        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Confirm order", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes)
                {
                    return;
                }


                //Call update order info here
              
                _orderRepository.UpdateOrderFreight(orderId);

                MessageBox.Show("Confirm order success!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CartWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                var member = MemberSession.CurrentMember;

                if (member is null)
                {
                    return;
                }

                var orderInCart = _orderRepository.GetOrderByStatus(member.MemberId, OrderStatus.InCart);

                if (orderInCart is null)
                {
                    return;
                }

                var orderDetails = _orderDetailRepository.GetOrderDetailWithProducts(orderInCart.OrderId);

                dgCarts.ItemsSource = orderDetails.Select(od => new
                {
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    ProductName = od.Product.ProductName,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                    Discount = od.Discount
                });

                decimal total = orderDetails.Sum(od => od.UnitPrice * od.Quantity);

                tbTotal.Text = $"Total: {total}";

                orderId = orderInCart.OrderId;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
