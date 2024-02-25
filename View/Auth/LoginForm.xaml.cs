using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using ResidentialRegistration.Storage;

namespace ResidentialRegistration.View.Auth
{
    /// <summary>
    /// Логика взаимодействия для LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        #region Смена темы
        public bool isDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
                
        private void toggleTheme(object sender, RoutedEventArgs e)  
        {
            ITheme theme = paletteHelper.GetTheme();

            if (isDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                isDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                isDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }
        #endregion

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            Database database = new Database();
            bool isAuth = database.Check(username, password);

            if (isAuth)
            {
                AuthManager.CurrentUsername = username;

                this.Hide();

            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}
