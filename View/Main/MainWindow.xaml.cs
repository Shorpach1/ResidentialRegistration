using MaterialDesignThemes.Wpf;
using ResidentialRegistration.Storage;
using ResidentialRegistration.View.Auth;
using System.Windows;
using System.Windows.Input;

namespace ResidentialRegistration.View.Main
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateTheme();
            themeToggle.IsChecked = DarkTheme.isDarkTheme;


        }

        #region Перетаскивание окна
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
        #endregion

        #region Смена темы
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();

            if (DarkTheme.isDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                DarkTheme.isDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                DarkTheme.isDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }
        private void UpdateTheme()
        {
            ITheme theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(DarkTheme.isDarkTheme ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);
        }
        #endregion

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RegFormBtn_Click(object sender, RoutedEventArgs e)
        {
            RegForm regForm = new RegForm();
            regForm.ShowDialog();
        }
    }
}
