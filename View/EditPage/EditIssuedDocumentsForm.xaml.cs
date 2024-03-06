using MaterialDesignThemes.Wpf;
using ResidentialRegistration.CB;
using ResidentialRegistration.Service;
using ResidentialRegistration.Storage;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для EditIssuedDocumentsForm.xaml
    /// </summary>
    public partial class EditIssuedDocumentsForm : Window
    {
        DataGrid grid;
        Database database = new Database();

        int id;
        public EditIssuedDocumentsForm(DataGrid grid, int id, string dtId, string docNum, string dateOfIssue, string issuingAuthority, int citId, string other)
        {
            
            InitializeComponent();
            database.ReadDocumentTypeToComboBox(cbType);
            database.ReadCitizenNameToCombobox(cbCitizen);

            this.grid = grid;
            UpdateTheme();
            this.id = id;
            cbType.Text = dtId;
            txtNumberDoc.Text = docNum;
            dpDateOfIssue.Text = dateOfIssue;
            txtIssuingAuthority.Text = issuingAuthority;
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
            txtOther.Text = other;

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
            CBDocumentType cbdt = (CBDocumentType)cbType.SelectedItem;
            string docNum = txtNumberDoc.Text;
            string issuingAuthority = txtIssuingAuthority.Text;
            CBDocumentType citId = (CBDocumentType)cbCitizen.SelectedItem;
            string other = txtOther.Text;

            ValidationFileds validation = new ValidationFileds();

            if (dpDateOfIssue.SelectedDate.HasValue)
            {
                if (validation.ValidationIssuedDocumentsAdd(cbdt, docNum, dpDateOfIssue.SelectedDate.Value, issuingAuthority, citId))
                {
                    database.EditIssuedDocument(id, cbdt, docNum, dpDateOfIssue.SelectedDate.Value, issuingAuthority, citId, other);
                    database.ReadDocuments(grid);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите дату регистрации.");
            }
        }
    }
}
