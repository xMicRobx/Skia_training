using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
[assembly: ExportFont("SF-Pro-Display-Bold.otf", Alias = "SF700")]
[assembly: ExportFont("SF-Pro-Display-Regular.otf", Alias = "SF800")]

namespace Skia_training_cross
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Device.SetFlags(new []{"Shapes_Experimental"});
            MainPage = new NavigationPage(new MainPage());
            
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}