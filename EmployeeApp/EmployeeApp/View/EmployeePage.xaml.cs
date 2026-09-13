using System;
using EmployeeApp.Models;
using EmployeeApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmployeeApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeePage : ContentPage
    {
        private EmployeeViewModel viewModel;

        public EmployeePage(Employee employee = null)
        {
            try
            {
                InitializeComponent();

                viewModel = new EmployeeViewModel();
                BindingContext = viewModel;

                if (employee != null)
                {
                    viewModel.LoadEmployee(employee);
                }

                SubscribeEvents();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка в EmployeePage: {ex.Message}");
            }
        }

        private void SubscribeEvents()
        {
            if (viewModel != null)
            {
                viewModel.SaveCompleted += OnSaveCompleted;
                viewModel.CancelCompleted += OnCancelCompleted;
                viewModel.DeleteCompleted += OnDeleteCompleted;
            }
        }

        private void UnsubscribeEvents()
        {
            if (viewModel != null)
            {
                viewModel.SaveCompleted -= OnSaveCompleted;
                viewModel.CancelCompleted -= OnCancelCompleted;
                viewModel.DeleteCompleted -= OnDeleteCompleted;
            }
        }

        private void OnSaveCompleted(object sender, Employee employee)
        {
            MessagingCenter.Send(this, "EmployeeSaved", employee);
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Navigation.PopModalAsync();
            });
        }

        private void OnCancelCompleted(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Navigation.PopModalAsync();
            });
        }

        private void OnDeleteCompleted(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "EmployeeDeleted", sender);
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Navigation.PopModalAsync();
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            UnsubscribeEvents();
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Navigation.PopModalAsync();
            });
            return true;
        }
    }
}