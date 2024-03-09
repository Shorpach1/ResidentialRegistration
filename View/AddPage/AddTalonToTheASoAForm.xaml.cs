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
    /// Логика взаимодействия для AddTalonToTheASoAForm.xaml
    /// </summary>
    public partial class AddTalonToTheASoAForm : Window
    {
        DataGrid grid;
        Database database = new Database();
        public AddTalonToTheASoAForm(DataGrid grid)
        {
            InitializeComponent();
            this.grid = grid;
            UpdateTheme();

            database.ReadTalonIDToCombobox(cbAAS);
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
            int aasID;
            if (cbAAS.SelectedItem != null)
            {
                aasID = (int)cbAAS.SelectedItem;
            }
            else
            {
                MessageBox.Show("Выберите ад. лист прибытия!");
                return; // Возможно, вам нужно вернуться из метода, чтобы не выполнять остальную логику
            }
            string purpose = cbPurpose.Text;
            string anotherReason = txtAnotherReason.Text;
            string arrivalDate = txtArrivalDate.Text;

            ValidationFileds validation = new ValidationFileds();

            if (validation.ValidationTalonAASAdd(purpose, arrivalDate))
            {
                database.CreateTalonToTheASoA(aasID, purpose, anotherReason, arrivalDate);
                database.ReadTalon(grid);
                this.Close();
            }
        }
    }
}
