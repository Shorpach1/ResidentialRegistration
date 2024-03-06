using MaterialDesignThemes.Wpf;
using ResidentialRegistration.Storage;
using ResidentialRegistration.View.AddPage;
using ResidentialRegistration.View.Auth;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using ResidentialRegistration.View.EditPage;

namespace ResidentialRegistration.View.Main
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Database database = new Database();
        private bool _isContextMenuOpen = false;

        public MainWindow()
        {
            InitializeComponent();
            UpdateTheme();
            themeToggle.IsChecked = DarkTheme.isDarkTheme;

            database.ReadCitizen(CitizenGrid);
            database.ReadDocuments(IssuedDocumentsGrid);
            database.ReadResidentialUnit(ResidentialUnitGrid);
            database.ReadAAS(AddressArrivalSheetsGrid);
            database.ReadADS(AddressedDepartureSheetGrid);
            database.ReadTalon(TalonToTheASoAGrid);

            if (AuthManager.CurrentUsername != null)
            {
                bool isAdmin = database.CheckAdmin(AuthManager.CurrentUsername);
                if (!isAdmin)
                {
                    RegFormBtn.Visibility = Visibility.Collapsed;
                }
                else
                {
                    RegFormBtn.Visibility = Visibility.Visible;
                }
            }
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
        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();

            if (DarkTheme.isDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                DarkTheme.isDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                DarkTheme.isDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }
        private void UpdateTheme()
        {
            ITheme theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(DarkTheme.isDarkTheme ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);
        }
        #endregion

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RegFormBtn_Click(object sender, RoutedEventArgs e)
        {
            RegForm regForm = new RegForm();
            regForm.ShowDialog();
        }

        #region Работа с таблицей "Citizens"

        #region Добавление пользователя
        private void AddCitizen_Click(object sender, RoutedEventArgs e)
        {
            AddCitizenForm addCitizenForm = new AddCitizenForm(CitizenGrid);
            addCitizenForm.ShowDialog();
        }
        #endregion

        #region Удаление гражданина
        private void DeleteCitizen_Click(object sender, RoutedEventArgs e)
        {

            DataRowView selectedCitizen = CitizenGrid.SelectedItem as DataRowView;
            if (selectedCitizen != null )
            {
                try
                {
                    database.DeleteCitizen(selectedCitizen);
                }
                catch (SqlException)
                {
                    MessageBox.Show("Удаление невозможно. Удалите связанные данные с этим гражданином!");
                    return;
                }

                database.ReadCitizen(CitizenGrid);
            }
            else
            {
                MessageBox.Show("Выберите поле для удаления!");
            }


        }
        #endregion

        #region Редактирование
        private void EditCitizen_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = CitizenGrid.SelectedItem as DataRowView;
            if (selectedRow != null)
            {
                EditCititzenForm productTypeEdit = new EditCititzenForm(Convert.ToInt32(selectedRow.Row.ItemArray[0]), Convert.ToString(selectedRow.Row.ItemArray[1]),
                    Convert.ToString(selectedRow.Row.ItemArray[2]), Convert.ToString(selectedRow.Row.ItemArray[3]), Convert.ToString(selectedRow.Row.ItemArray[4]),
                    Convert.ToString(selectedRow.Row.ItemArray[5]), Convert.ToString(selectedRow.Row.ItemArray[6]), Convert.ToString(selectedRow.Row.ItemArray[7]), CitizenGrid);
                productTypeEdit.ShowDialog();
            }
            else
            {
                MessageBox.Show("Не выбрана строка для редактирования", "Ошибка", MessageBoxButton.OK);
            }
        }
        #endregion
        #endregion

        #region Переключение таблиц
        private void btnCitizenGrid_Click(object sender, RoutedEventArgs e)
        {
            CitizenGrid.Visibility = Visibility.Visible;
            IssuedDocumentsGrid.Visibility = Visibility.Collapsed;
            ResidentialUnitGrid.Visibility = Visibility.Collapsed;
            AddressArrivalSheetsGrid.Visibility = Visibility.Collapsed;
            AddressedDepartureSheetGrid.Visibility = Visibility.Collapsed;
            TalonToTheASoAGrid.Visibility = Visibility.Collapsed;
        }

        private void btnIssuedDocumentsGrid_Click(object sender, RoutedEventArgs e)
        {
            CitizenGrid.Visibility = Visibility.Collapsed;
            IssuedDocumentsGrid.Visibility = Visibility.Visible;
            ResidentialUnitGrid.Visibility = Visibility.Collapsed;
            AddressArrivalSheetsGrid.Visibility = Visibility.Collapsed;
            AddressedDepartureSheetGrid.Visibility = Visibility.Collapsed;
            TalonToTheASoAGrid.Visibility = Visibility.Collapsed;
        }

        private void btnResidentialUnitGrid_Click(object sender, RoutedEventArgs e)
        {
            CitizenGrid.Visibility = Visibility.Collapsed;
            IssuedDocumentsGrid.Visibility = Visibility.Collapsed;
            ResidentialUnitGrid.Visibility = Visibility.Visible;
            AddressArrivalSheetsGrid.Visibility = Visibility.Collapsed;
            AddressedDepartureSheetGrid.Visibility = Visibility.Collapsed;
            TalonToTheASoAGrid.Visibility = Visibility.Collapsed;
        }

        private void btnAddressArrivalSheetsGrid_Click(object sender, RoutedEventArgs e)
        {
            CitizenGrid.Visibility = Visibility.Collapsed;
            IssuedDocumentsGrid.Visibility = Visibility.Collapsed;
            ResidentialUnitGrid.Visibility = Visibility.Collapsed;
            AddressArrivalSheetsGrid.Visibility = Visibility.Visible;
            AddressedDepartureSheetGrid.Visibility = Visibility.Collapsed;
            TalonToTheASoAGrid.Visibility = Visibility.Collapsed;
        }

        private void btnAddressedDepartureSheetGrid_Click(object sender, RoutedEventArgs e)
        {
            CitizenGrid.Visibility = Visibility.Collapsed;
            IssuedDocumentsGrid.Visibility = Visibility.Collapsed;
            ResidentialUnitGrid.Visibility = Visibility.Collapsed;
            AddressArrivalSheetsGrid.Visibility = Visibility.Collapsed;
            AddressedDepartureSheetGrid.Visibility = Visibility.Visible;
            TalonToTheASoAGrid.Visibility = Visibility.Collapsed;
        }

        private void btnTalonToTheASoAGrid_Click(object sender, RoutedEventArgs e)
        {
            CitizenGrid.Visibility = Visibility.Collapsed;
            IssuedDocumentsGrid.Visibility = Visibility.Collapsed;
            ResidentialUnitGrid.Visibility = Visibility.Collapsed;
            AddressArrivalSheetsGrid.Visibility = Visibility.Collapsed;
            AddressedDepartureSheetGrid.Visibility = Visibility.Collapsed;
            TalonToTheASoAGrid.Visibility = Visibility.Visible;
        }


        #endregion

        #region Работа с таблицей "IssuedDocuments"
        private void AddIssuedDocuments_Click(object sender, RoutedEventArgs e)
        {
            AddIssuedDocumentsForm addIssuedDocumentsForm = new AddIssuedDocumentsForm(IssuedDocumentsGrid);
            addIssuedDocumentsForm.ShowDialog();
        }

        private void DeleteIssuedDocuments_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedUssuedDocument = IssuedDocumentsGrid.SelectedItem as DataRowView;
            if (selectedUssuedDocument != null)
            {
                try
                {
                    database.DeleteissuedDocument(selectedUssuedDocument);
                }
                catch (SqlException)
                {
                    MessageBox.Show("Удаление невозможно. Удалите связанные данные с этим гражданином!");
                    return;
                }

                database.ReadDocuments(IssuedDocumentsGrid);
            }
            else
            {
                MessageBox.Show("Выберите поле для удаления!");
            }
        }

        private void EditIssuedDocuments_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = IssuedDocumentsGrid.SelectedItem as DataRowView;
            if (selectedRow != null)
            {
                EditIssuedDocumentsForm issuedDocumentEdit = new EditIssuedDocumentsForm(IssuedDocumentsGrid, Convert.ToInt32(selectedRow.Row.ItemArray[0]), Convert.ToString(selectedRow.Row.ItemArray[1]),
                    Convert.ToString(selectedRow.Row.ItemArray[2]), Convert.ToString(selectedRow.Row.ItemArray[3]), Convert.ToString(selectedRow.Row.ItemArray[4]),
                    Convert.ToInt32(selectedRow.Row.ItemArray[5]), Convert.ToString(selectedRow.Row.ItemArray[6]));
                issuedDocumentEdit.ShowDialog();
            }
            else
            {
                MessageBox.Show("Не выбрана строка для редактирования", "Ошибка", MessageBoxButton.OK);
            }
        }
        #endregion
    }

}
