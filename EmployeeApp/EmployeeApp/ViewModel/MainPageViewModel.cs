using System.Windows.Input;
using EmployeeApp.View;
using Xamarin.Forms;

namespace EmployeeApp
{
    public class MainPageViewModel
    {
        public ICommand GoToEmployeeListCommand { get; set; }
        public ICommand GoToAddEmployeeCommand { get; set; }
        public ICommand ShowAboutCommand { get; set; }

        public MainPageViewModel()
        {
            GoToEmployeeListCommand = new Command(async () => await GoToEmployeeList());
            GoToAddEmployeeCommand = new Command(async () => await GoToAddEmployee());
            ShowAboutCommand = new Command(async () => await ShowAbout());
        }

        private async System.Threading.Tasks.Task GoToEmployeeList()
        {
            try
            {
                var page = new EmployeeListPage();
                await Application.Current.MainPage.Navigation.PushAsync(page);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка навигации: {ex.Message}");
            }
        }

        private async System.Threading.Tasks.Task GoToAddEmployee()
        {
            try
            {
                var employeePage = new EmployeePage();
                await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(employeePage));
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка навигации: {ex.Message}");
            }
        }

        private async System.Threading.Tasks.Task ShowAbout()
        {
            await Application.Current.MainPage.DisplayAlert(
                "О программе",
                "EmployeeApp\nВерсия 1.0.0\n\n" +
                "Приложение для управления сотрудниками\n" +
                "Разработано с использованием Xamarin.Forms\n" +
                "© 2026 Все права защищены",
                "OK");
        }
    }
}