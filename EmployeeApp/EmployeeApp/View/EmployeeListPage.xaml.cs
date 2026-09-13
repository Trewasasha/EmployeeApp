using System;
using EmployeeApp.Models;
using EmployeeApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmployeeApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeeListPage : ContentPage
    {
        private EmployeeListViewModel viewModel;

        public EmployeeListPage()
        {
            try
            {
                InitializeComponent();

                viewModel = new EmployeeListViewModel();
                BindingContext = viewModel;

                SubscribeEvents();
                SubscribeMessaging();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка в EmployeeListPage: {ex.Message}");
            }
        }

        private void SubscribeEvents()
        {
            if (viewModel != null)
            {
                viewModel.AddEmployeeRequested += OnAddEmployeeRequested;
                viewModel.EditEmployeeRequested += OnEditEmployeeRequested;
            }
        }

        private void SubscribeMessaging()
        {
            MessagingCenter.Subscribe<EmployeePage, Employee>(this, "EmployeeSaved", OnEmployeeSaved);
            MessagingCenter.Subscribe<EmployeePage>(this, "EmployeeDeleted", OnEmployeeDeleted);
        }

        private void UnsubscribeEvents()
        {
            if (viewModel != null)
            {
                viewModel.AddEmployeeRequested -= OnAddEmployeeRequested;
                viewModel.EditEmployeeRequested -= OnEditEmployeeRequested;
            }
            MessagingCenter.Unsubscribe<EmployeePage, Employee>(this, "EmployeeSaved");
            MessagingCenter.Unsubscribe<EmployeePage>(this, "EmployeeDeleted");
        }

        private async void OnAddEmployeeRequested(object sender, EventArgs e)
        {
            try
            {
                var employeePage = new EmployeePage();
                await Navigation.PushModalAsync(new NavigationPage(employeePage));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при добавлении: {ex.Message}");
            }
        }

        private async void OnEditEmployeeRequested(object sender, Employee employee)
        {
            try
            {
                var employeePage = new EmployeePage(employee);
                await Navigation.PushModalAsync(new NavigationPage(employeePage));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при редактировании: {ex.Message}");
            }
        }

        private void OnEmployeeSaved(EmployeePage sender, Employee employee)
        {
            if (viewModel != null)
            {
                viewModel.RefreshList();
            }
        }

        private void OnEmployeeDeleted(EmployeePage sender)
        {
            if (viewModel != null)
            {
                viewModel.RefreshList();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel?.RefreshList();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Navigation.PopAsync();
            });
            return true;
        }
    }
}