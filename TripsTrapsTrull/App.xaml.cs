namespace TripsTrapsTrull
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Määrame algse teema (vaikimisi hele)
            UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new TrullPage()));
        }
    }
}