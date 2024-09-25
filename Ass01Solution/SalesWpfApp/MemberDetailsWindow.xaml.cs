using BusinessObject;
using DataAccess.DTOs.Member;
using DataAccess.Repositories;
using DataAccess.Validators.Member;
using Mapster;
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
    /// Interaction logic for MemberDetailsWindow.xaml
    /// </summary>
    public partial class MemberDetailsWindow : Window
    {
        public bool IsUpdate { get; set; } = true;

        public int? MemberId { get; set; }

        public string MemberPassword
        {
            get { return pbPassword.Password; }
            set { pbPassword.Password = value; }
        }


        private readonly IMemberRepository _memberRepository;

        public MemberDetailsWindow()
        {
            InitializeComponent();

            if (_memberRepository is null)
            {
                _memberRepository = new MemberRepository();
            }

            this.Loaded += MemberDetailsWindow_Loaded;
            btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string message = IsUpdate ? "Update this member" : "Create this member?";

            var result = MessageBox.Show(message, "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result is not MessageBoxResult.Yes)
            {
                return;
            }

            if (IsUpdate)
            {
                PerformUpdate();
                return;
            }

            PerformCreate();

        }

        private void PerformCreate()
        {
            try
            {

                var createDto = new CreateMemberDto
                {
                    CompanyName = txtCompanyName.Text,
                    City = txtCity.Text,
                    Email = txtEmail.Text,
                    Country = txtCountry.Text,
                    Password = pbPassword.Password
                };

                var createValidator = new CreateMemberValidator();

                var validationResult = createValidator.Validate(createDto);

                if (!validationResult.IsValid)
                {
                    string errorMessage = validationResult.ToString("\n");

                    throw new Exception(errorMessage);
                }

                _memberRepository.AddMember(createDto);

                MessageBox.Show("Create member success!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PerformUpdate()
        {
            try
            {
                var member = this.DataContext as Member;

                if (member is null)
                {
                    return;
                }

                var updateDto = member.Adapt<UpdateMemberDto>();
                updateDto.Password = MemberPassword;

                var updateValidator = new UpdateMemberValidator();

                var validationResult = updateValidator.Validate(updateDto);

                if (!validationResult.IsValid)
                {
                    string errorMessage = validationResult.ToString("\n");

                    throw new Exception(errorMessage);
                }

                _memberRepository.UpdateMember(updateDto);

                MessageBox.Show("Update member success!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MemberDetailsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MemberSession.CurrentMember is not null)
                {
                    tbTitle.Text = "Profile";
                }
                else
                {
                    tbTitle.Text = IsUpdate ? "Member Details" : "Create New Member";
                }
                btnSave.Content = IsUpdate ? "Save" : "Create";
                txtId.IsReadOnly = true;
                txtId.Visibility = IsUpdate ? Visibility.Visible : Visibility.Hidden;

                if (MemberId.HasValue && IsUpdate)
                {
                    var member = _memberRepository.GetMemberById(Convert.ToInt32(MemberId));

                    if (member is null)
                    {
                        return;
                    }

                    this.DataContext = member;

                    MemberPassword = member.Password;
                }



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }
    }
}
