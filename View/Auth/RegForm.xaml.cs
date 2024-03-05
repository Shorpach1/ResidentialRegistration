using MaterialDesignThemes.Wpf;
using ResidentialRegistration.Service;
using ResidentialRegistration.Storage;
using System.Windows;
using System.Windows.Input;

namespace ResidentialRegistration.View.Auth
{
    public partial class RegForm : Window
    {
        ValidationFileds validationFileds = new ValidationFileds();

        public RegForm()
        {
            InitializeComponent();
            UpdateTheme();
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

        private void UpdateTheme()
        {
            ITheme theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(DarkTheme.isDarkTheme ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);
        }
        #endregion

        #region Кнопка регистрации
        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string firstPassword = txtPassword.Password;
            string secondPassword = txtPasswordRepeat.Password;
            string surname = txtSurname.Text;
            string firstName = txtName.Text;
            string middleName = txtMiddlename.Text;
            bool isAdmin = (bool)AdminBox.IsChecked;
            string role = validationFileds.ReturnRole(isAdmin);

            if (validationFileds.ValidateFields(username, firstPassword, secondPassword, surname, firstName, middleName))
            {
                Database database = new Database();
                if (database.CountUsersWithLogin(username) == 0)
                {
                    database.RegisterAccount(username, firstPassword, role, surname, firstName, middleName);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!");
                    return;
                }
            }
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
