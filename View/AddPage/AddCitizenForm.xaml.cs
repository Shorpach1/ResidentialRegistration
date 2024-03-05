using MaterialDesignThemes.Wpf;
using ResidentialRegistration.Service;
using ResidentialRegistration.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ResidentialRegistration.View.AddPage
{
    /// <summary>
    /// Логика взаимодействия для AddCitizenForm.xaml
    /// </summary>
    public partial class AddCitizenForm : Window
    {
        DataGrid grid;
        Database database = new Database();
        public AddCitizenForm(DataGrid grid)
        {
            InitializeComponent();
            this.grid = grid;
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

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            string surname = txtSurname.Text;
            string name = txtName.Text;
            string middleName = txtMiddlename.Text;
            string gender = cbGender.Text;
            string placeOfBirth = txtPlaceOfbirth.Text;
            string other = txtOther.Text;

            ValidationFileds validation = new ValidationFileds();

            if (dpDateOfBirth.SelectedDate.HasValue)
            {
                if (validation.ValidationCitizenAdd(surname, name, middleName, dpDateOfBirth.SelectedDate.Value, gender, placeOfBirth))
                {
                    database.CreateCitizen(surname, name, middleName, dpDateOfBirth.SelectedDate.Value, gender, placeOfBirth, other);
                    database.ReadCitizen(grid);
                    this.Close();
                }  
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите дату рождения.");
            }
        }
    }
}
