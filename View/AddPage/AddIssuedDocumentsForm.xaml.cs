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
    /// Логика взаимодействия для AddIssuedDocumentsForm.xaml
    /// </summary>
    public partial class AddIssuedDocumentsForm : Window
    {
        DataGrid grid;
        Database database = new Database();
        public AddIssuedDocumentsForm(DataGrid grid)
        {
            InitializeComponent();
            this.grid = grid;
            UpdateTheme();

            database.ReadDocumentTypeToComboBox(cbType);
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
            CBDocumentType cbdt = (CBDocumentType) cbType.SelectedItem;
            string docNum = txtNumberDoc.Text;
            string issuingAuthority = txtIssuingAuthority.Text;
            CBDocumentType citId = (CBDocumentType) cbCitizen.SelectedItem;
            string other = txtOther.Text;

            ValidationFileds validation = new ValidationFileds();

            if (dpDateOfIssue.SelectedDate.HasValue)
            {
                if (validation.ValidationIssuedDocumentsAdd(cbdt, docNum, dpDateOfIssue.SelectedDate.Value, issuingAuthority, citId))
                {
                    database.CreateIssuedDocuments(cbdt, docNum, dpDateOfIssue.SelectedDate.Value, issuingAuthority, citId, other);
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
