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
    /// Логика взаимодействия для AddResidentialUnitsForm.xaml
    /// </summary>
    public partial class AddResidentialUnitsForm : Window
    {
        DataGrid grid;
        Database database = new Database();
        public AddResidentialUnitsForm(DataGrid grid)
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
            string address = txtAddress.Text;
            string area = txtArea.Text;
            string numOfRooms = txtCountOfRooms.Text;
            CBDocumentType citId = (CBDocumentType)cbCitizen.SelectedItem;
            string ifOwner = cbIfOwner.Text;
            string other = txtOther.Text;

            ValidationFileds validation = new ValidationFileds();

            if (dpDateOfConstruction.SelectedDate.HasValue)
            {
                if (validation.ValidationResidentialUnitsAdd(address, area, numOfRooms, citId, dpDateOfConstruction.SelectedDate.Value, ifOwner))
                {
                    database.CreateResidentialUnit(address, area, numOfRooms, citId, dpDateOfConstruction.SelectedDate.Value, ifOwner, other);
                    database.ReadResidentialUnit(grid);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите дату постройки.");
            }
        }
    }
}
