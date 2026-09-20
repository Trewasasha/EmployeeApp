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

        
    }
}