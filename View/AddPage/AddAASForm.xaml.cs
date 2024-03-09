using MaterialDesignThemes.Wpf;
using ResidentialRegistration.CB;
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
    /// Логика взаимодействия для AddAAS.xaml
    /// </summary>
    public partial class AddAASForm : Window
    {
        DataGrid grid;
        Database database = new Database();
        public AddAASForm(DataGrid grid)
        {
            InitializeComponent();
            this.grid = grid;
            UpdateTheme();

            database.ReadCitizenNameToCombobox(cbCitizen);
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
            CBDocumentType citId = (CBDocumentType)cbCitizen.SelectedItem;
            string address = txtAddress.Text;
            string regAuth = txtRegistrationAuthority.Text;

            ValidationFileds validation = new ValidationFileds();

            if (dpDateOfDeparture.SelectedDate.HasValue)
            {
                if (validation.ValidationAASAdd(citId,address, dpDateOfDeparture.SelectedDate.Value, regAuth))
                {
                    database.CreateAAS(citId, address, dpDateOfDeparture.SelectedDate.Value, regAuth);
                    database.ReadAAS(grid);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите дату прибытия.");
            }
        }
    }
}
