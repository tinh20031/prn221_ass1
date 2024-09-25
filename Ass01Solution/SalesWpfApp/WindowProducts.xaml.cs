using BusinessObject.Enum;
using DataAccess.DTOs.Order;
using DataAccess.DTOs.OrderDetail;
using DataAccess.DTOs.Product;
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
    /// Interaction logic for WindowProducts.xaml
    /// </summary>
    public partial class WindowProducts : Window
    {
        private readonly IProductRepository _productRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IOrderDetailRepository _orderDetailRepository;


        public decimal? StartPrice
        {
            get
            {
                return (string.IsNullOrEmpty(txtStartPrice.Text)) ? null : Convert.ToDecimal(txtStartPrice.Text);
            }
            set
            {
                txtStartPrice.Text = value.ToString();
            }
        }

        public decimal? EndPrice
        {
            get
            {
                return (string.IsNullOrEmpty(txtEndPrice.Text)) ? null : Convert.ToDecimal(txtEndPrice.Text);
            }
            set
            {
                txtEndPrice.Text = value.ToString();
            }
        }

        public string Keyword
        {
            get { return txtKeyword.Text; }
            set { txtKeyword.Text = value; }
        }

        public int Quantity
        {
            get { return Convert.ToInt32(txtQuantity.Text); }
            set { txtQuantity.Text = value.ToString(); }
        }


        public WindowProducts()
        {
            InitializeComponent();

            if (_productRepository is null)
            {
                _productRepository = new ProductRepository();
            }

            if (_orderRepository is null)
            {
                _orderRepository = new OrderRepository();
            }

            if (_orderDetailRepository is null)
            {
                _orderDetailRepository = new OrderDetailRepository();
            }

            this.Loaded += WindowProducts_Loaded;
            btnUpdate.Click += BtnUpdate_Click;
            btnCreate.Click += BtnCreate_Click;
            txtKeyword.TextChanged += TxtKeyword_TextChanged;
            btnSubmit.Click += BtnSubmit_Click;
            btnReset.Click += BtnReset_Click;
            btnDelete.Click += BtnDelete_Click;
            btnAddToCart.Click += BtnAddToCart_Click;
            btnViewCart.Click += BtnViewCart_Click;
            btnIncrease.Click += BtnIncrease_Click;
            btnDecrease.Click += BtnDecrease_Click;
            btnRefresh.Click += BtnRefresh_Click;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ResetFilter();
            LoadProducts();
        }

        private void BtnDecrease_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Quantity is 1)
                {
                    return;
                }

                --Quantity;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void BtnIncrease_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ++Quantity;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnViewCart_Click(object sender, RoutedEventArgs e)
        {
            new CartWindow().Show();
        }

        private void BtnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var currentProduct = dgProducts.SelectedItem as GetProductDto;

                if (currentProduct is null)
                {
                    MessageBox.Show("Please choose a product");
                    return;
                }

                var result = MessageBox.Show("Add this product to cart?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                var member = MemberSession.CurrentMember;

                if (member is null)
                {
                    return;
                }

                var orderInCart = _orderRepository.GetOrderByStatus(member.MemberId, OrderStatus.InCart);

                if (orderInCart is null)
                {
                    var createOrderDto = new CreateOrderDto
                    {
                        OrderDate = DateTime.Now,
                        MemberId = member.MemberId,
                        Status = OrderStatus.InCart.ToString()
                    };

                    orderInCart = _orderRepository.CreateOrder(createOrderDto);
                }

                _orderDetailRepository.CreateOrderDetails(new CreateOrderDetailDto
                {
                    OrderId = orderInCart.OrderId,
                    ProductId = currentProduct.ProductId,
                    Quantity = Quantity,
                    UnitPrice = currentProduct.UnitPrice
                });

                

                MessageBox.Show("Add product to cart success");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentProduct = dgProducts.SelectedItem as GetProductDto;

                if (currentProduct is null)
                {
                    throw new Exception("Please select a product for deleting");
                }

                var result = MessageBox.Show("Are you sure you want to delete this product", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result is not MessageBoxResult.Yes)
                {
                    return;
                }

                _productRepository.DeleteProduct(currentProduct.ProductId);

                MessageBox.Show("Delete product success!");

                LoadProducts();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Reset filter?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result is not MessageBoxResult.Yes)
            {
                return;
            }

            ResetFilter();
            LoadProducts();
        }

        private void ResetFilter()
        {
            Keyword = string.Empty;
            StartPrice = null;
            EndPrice = null;
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }

        private void TxtKeyword_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadProducts();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            var detailsWindow = new ProductDetailsWindow
            {
                IsUpdate = false
            };

            detailsWindow.Show();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var currentProduct = dgProducts.SelectedItem as GetProductDto;

                var productDetailsWindow = new ProductDetailsWindow
                {
                    ProductId = currentProduct?.ProductId
                };

                productDetailsWindow.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void WindowProducts_Loaded(object sender, RoutedEventArgs e)
        {
            PerformAuthorization();
            LoadProducts();
        }

        private void PerformAuthorization()
        {
            bool isUser = MemberSession.Role == Role.User.ToString();
            btnAddToCart.Visibility = isUser ? Visibility.Visible : Visibility.Hidden;
            btnViewCart.Visibility = isUser ? Visibility.Visible : Visibility.Hidden;
            lbQuantity.Visibility = isUser ? Visibility.Visible : Visibility.Hidden;
            btnIncrease.Visibility = isUser ? Visibility.Visible : Visibility.Hidden;
            btnDecrease.Visibility = isUser ? Visibility.Visible : Visibility.Hidden;
            txtQuantity.Visibility = isUser ? Visibility.Visible : Visibility.Hidden;


            btnCreate.Visibility = isUser ? Visibility.Hidden : Visibility.Visible;
            btnUpdate.Visibility = isUser ? Visibility.Hidden : Visibility.Visible;
            btnDelete.Visibility = isUser ? Visibility.Hidden : Visibility.Visible;


        }

        private void LoadProducts()
        {
            try
            {
                if (StartPrice > EndPrice)
                {
                    throw new Exception("Start price must be smaller than end price");
                }

                var products = _productRepository.GetProducts(Keyword, StartPrice, EndPrice);
                dgProducts.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
