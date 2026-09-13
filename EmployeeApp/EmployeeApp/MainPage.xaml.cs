using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmployeeApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            try
            {
                InitializeComponent();

                BindingContext = new MainPageViewModel();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка в MainPage: {ex.Message}");
            }
        }
    }
}