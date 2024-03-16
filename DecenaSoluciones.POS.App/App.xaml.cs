namespace DecenaSoluciones.POS.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Current.UserAppTheme = AppTheme.Light;
            this.RequestedThemeChanged += (s, e) => { Current.UserAppTheme = AppTheme.Light; };

            MainPage = new MainPage();
        }
    }
}
