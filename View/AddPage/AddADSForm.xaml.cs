using MaterialDesignThemes.Wpf;
using ResidentialRegistration.CB;
using ResidentialRegistration.Service;
using ResidentialRegistration.Storage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ResidentialRegistration.View.AddPage
{
    /// <summary>
    /// Логика взаимодействия для AddADSForm.xaml
    /// </summary>
    public partial class AddADSForm : Window
    {
        DataGrid grid;
        Database database = new Database();
        public AddADSForm(DataGrid grid)
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
            string addressFR = txtAddressOfFormerResidence.Text;
            string placeOfArrival = txtPlaceOfArrival.Text;

            ValidationFileds validation = new ValidationFileds();

            if (dpDateOfDeparture.SelectedDate.HasValue)
            {
                if (validation.ValidationADSAdd(citId, address, dpDateOfDeparture.SelectedDate.Value, addressFR, placeOfArrival))
                {
                    database.CreateADS(citId, address, dpDateOfDeparture.SelectedDate.Value, addressFR, placeOfArrival);
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
