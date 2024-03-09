using MaterialDesignThemes.Wpf;
using ResidentialRegistration.CB;
using ResidentialRegistration.Service;
using ResidentialRegistration.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace ResidentialRegistration.View.EditPage
{
    /// <summary>
    /// Логика взаимодействия для EditADSForm.xaml
    /// </summary>
    public partial class EditADSForm : Window
    {
        DataGrid grid;
        Database database = new Database();

        int id;
        public EditADSForm(DataGrid grid, int id, int citId, string address, string date, string addressFR, string placeOfArrival)
        {
            InitializeComponent();
            this.grid = grid;
            UpdateTheme();

            database.ReadCitizenNameToCombobox(cbCitizen);

            this.id = id;
            foreach (var item in cbCitizen.Items)
            {
                var cbDocumentType = (CBDocumentType)item;
                var idFromCBDocumentType = cbDocumentType.id; // Предположим, что это свойство называется citId
                if (idFromCBDocumentType == citId)
                {
                    cbCitizen.SelectedItem = item;
                    break;
                }
            }
            txtAddress.Text = address;
            dpDateOfDeparture.Text = date;
            txtAddressOfFormerResidence.Text = addressFR;
            txtPlaceOfArrival.Text = placeOfArrival;
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

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            CBDocumentType citId = (CBDocumentType)cbCitizen.SelectedItem;
            string address = txtAddress.Text;
            string addressFR = txtAddressOfFormerResidence.Text;
            string placeOfArrival = txtPlaceOfArrival.Text;

            ValidationFileds validation = new ValidationFileds();

            if (dpDateOfDeparture.SelectedDate.HasValue)
            {
                if (validation.ValidationADSAdd(citId, address, dpDateOfDeparture.SelectedDate.Value, addressFR, placeOfArrival))
                {
                    database.EditADS(id, citId, address, dpDateOfDeparture.SelectedDate.Value, addressFR, placeOfArrival);
                    database.ReadADS(grid);
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
