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

namespace ResidentialRegistration.View.EditPage
{
    /// <summary>
    /// Логика взаимодействия для EditTalonToTheASoAForm.xaml
    /// </summary>
    public partial class EditTalonToTheASoAForm : Window
    {
        DataGrid grid;
        Database database = new Database();

        int id;
        public EditTalonToTheASoAForm(DataGrid grid, int id, string aasID, string puroise, string anotherReason, string arrivalDate)
        {
            InitializeComponent();
            this.grid = grid;
            UpdateTheme();

            database.ReadTalonIDToCombobox(cbAAS);

            this.id = id;
            foreach (var item in cbAAS.Items)
            {
                int idFromComboBox = (int)item; // Предполагается, что элементы ComboBox содержат идентификаторы типа int
                if (idFromComboBox == Convert.ToInt32(aasID))
                {
                    cbAAS.SelectedItem = item;
                    break;
                }
            }
            cbPurpose.Text = puroise;
            txtAnotherReason.Text = anotherReason;
            txtArrivalDate.Text = arrivalDate;
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
                database.EditTalonToTheASoA(id, aasID, purpose, anotherReason, arrivalDate);
                database.ReadTalon(grid);
                this.Close();
            }
        }
    }
}
