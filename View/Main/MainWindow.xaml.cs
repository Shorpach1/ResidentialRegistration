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
using System.Windows.Data;
using System.Windows.Media;
using ResidentialRegistration.Service;

namespace ResidentialRegistration.View.Main
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Database database = new Database();
        private DataGrid SearchGrid;

        public MainWindow()
        {
            InitializeComponent();
            UpdateTheme();
            themeToggle.IsChecked = DarkTheme.isDarkTheme;

            SearchGrid = CitizenGrid;

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
                    DeleteCitizen.IsEnabled = false;
                    EditCitizen.IsEnabled = false;
                    DeleteIssuedDocuments.IsEnabled = false;
                    EditIssuedDocuments.IsEnabled = false;
                    DeleteResidentialUnits.IsEnabled = false;
                    EditResidentialUnits.IsEnabled = false;
                    DeleteAAS.IsEnabled = false;
                    EditAAS.IsEnabled = false;
                    EditADS.IsEnabled = false;
                    DeleteADS.IsEnabled = false;
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
            SearchGrid = CitizenGrid;

            dpFilter.Visibility = Visibility.Visible;
            FilterBtn.Visibility = Visibility.Visible;
        }

        private void btnIssuedDocumentsGrid_Click(object sender, RoutedEventArgs e)
        {
            CitizenGrid.Visibility = Visibility.Collapsed;
            IssuedDocumentsGrid.Visibility = Visibility.Visible;
            ResidentialUnitGrid.Visibility = Visibility.Collapsed;
            AddressArrivalSheetsGrid.Visibility = Visibility.Collapsed;
            AddressedDepartureSheetGrid.Visibility = Visibility.Collapsed;
            TalonToTheASoAGrid.Visibility = Visibility.Collapsed;
            SearchGrid = IssuedDocumentsGrid;

            dpFilter.Visibility = Visibility.Visible;
            FilterBtn.Visibility = Visibility.Visible;
        }

        private void btnResidentialUnitGrid_Click(object sender, RoutedEventArgs e)
        {
            CitizenGrid.Visibility = Visibility.Collapsed;
            IssuedDocumentsGrid.Visibility = Visibility.Collapsed;
            ResidentialUnitGrid.Visibility = Visibility.Visible;
            AddressArrivalSheetsGrid.Visibility = Visibility.Collapsed;
            AddressedDepartureSheetGrid.Visibility = Visibility.Collapsed;
            TalonToTheASoAGrid.Visibility = Visibility.Collapsed;
            SearchGrid = ResidentialUnitGrid;

            dpFilter.Visibility = Visibility.Visible;
            FilterBtn.Visibility = Visibility.Visible;
        }

        private void btnAddressArrivalSheetsGrid_Click(object sender, RoutedEventArgs e)
        {
            CitizenGrid.Visibility = Visibility.Collapsed;
            IssuedDocumentsGrid.Visibility = Visibility.Collapsed;
            ResidentialUnitGrid.Visibility = Visibility.Collapsed;
            AddressArrivalSheetsGrid.Visibility = Visibility.Visible;
            AddressedDepartureSheetGrid.Visibility = Visibility.Collapsed;
            TalonToTheASoAGrid.Visibility = Visibility.Collapsed;
            SearchGrid = AddressArrivalSheetsGrid;

            dpFilter.Visibility = Visibility.Visible;
            FilterBtn.Visibility = Visibility.Visible;
        }

        private void btnAddressedDepartureSheetGrid_Click(object sender, RoutedEventArgs e)
        {
            CitizenGrid.Visibility = Visibility.Collapsed;
            IssuedDocumentsGrid.Visibility = Visibility.Collapsed;
            ResidentialUnitGrid.Visibility = Visibility.Collapsed;
            AddressArrivalSheetsGrid.Visibility = Visibility.Collapsed;
            AddressedDepartureSheetGrid.Visibility = Visibility.Visible;
            TalonToTheASoAGrid.Visibility = Visibility.Collapsed;
            SearchGrid = AddressedDepartureSheetGrid;

            dpFilter.Visibility = Visibility.Visible;
            FilterBtn.Visibility = Visibility.Visible;
        }

        private void btnTalonToTheASoAGrid_Click(object sender, RoutedEventArgs e)
        {
            CitizenGrid.Visibility = Visibility.Collapsed;
            IssuedDocumentsGrid.Visibility = Visibility.Collapsed;
            ResidentialUnitGrid.Visibility = Visibility.Collapsed;
            AddressArrivalSheetsGrid.Visibility = Visibility.Collapsed;
            AddressedDepartureSheetGrid.Visibility = Visibility.Collapsed;
            TalonToTheASoAGrid.Visibility = Visibility.Visible;
            SearchGrid = TalonToTheASoAGrid;

            dpFilter.Visibility = Visibility.Collapsed;
            FilterBtn.Visibility = Visibility.Collapsed;
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

        #region Работа с таблицей "ResidentialUnits"
        private void AddResidentialUnits_Click(object sender, RoutedEventArgs e)
        {
            AddResidentialUnitsForm addResidentialUnitsForm = new AddResidentialUnitsForm(ResidentialUnitGrid);
            addResidentialUnitsForm.ShowDialog();
        }

        private void DeleteResidentialUnits_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedResidentialUnits = ResidentialUnitGrid.SelectedItem as DataRowView;
            if (selectedResidentialUnits != null)
            {
                try
                {
                    database.DeleteResidentialUnits(selectedResidentialUnits);
                }
                catch (SqlException)
                {
                    MessageBox.Show("Удаление невозможно. Удалите связанные данные с этим гражданином!");
                    return;
                }

                database.ReadResidentialUnit(ResidentialUnitGrid);
            }
            else
            {
                MessageBox.Show("Выберите поле для удаления!");
            }
        }

        private void EditResidentialUnits_Click(object sender, RoutedEventArgs e)
        {
            
            var selectedRow = ResidentialUnitGrid.SelectedItem as DataRowView;
            if (selectedRow != null)
            {
                EditResidentialUnitsForm editResidentialUnitsForm = new EditResidentialUnitsForm(ResidentialUnitGrid, Convert.ToInt32(selectedRow.Row.ItemArray[0]), Convert.ToString(selectedRow.Row.ItemArray[1]),
                    Convert.ToString(selectedRow.Row.ItemArray[2]), Convert.ToString(selectedRow.Row.ItemArray[3]), Convert.ToInt32(selectedRow.Row.ItemArray[4]),
                    Convert.ToString(selectedRow.Row.ItemArray[5]), Convert.ToString(selectedRow.Row.ItemArray[6]), Convert.ToString(selectedRow.Row.ItemArray[7]));
                editResidentialUnitsForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Не выбрана строка для редактирования", "Ошибка", MessageBoxButton.OK);
            }
        }


        #endregion

        #region Работа с таблицей AAS
        private void AddAAS_Click(object sender, RoutedEventArgs e)
        {
            AddAASForm addAASForm = new AddAASForm(AddressArrivalSheetsGrid);
            addAASForm.ShowDialog();
        }

        private void DeleteAAS_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedAAS = AddressArrivalSheetsGrid.SelectedItem as DataRowView;
            if (selectedAAS != null)
            {
                try
                {
                    database.DeleteAAS(selectedAAS);
                }
                catch (SqlException)
                {
                    MessageBox.Show("Удаление невозможно. Удалите связанные данные с этим гражданином!");
                    return;
                }

                database.ReadAAS(AddressArrivalSheetsGrid);
            }
            else
            {
                MessageBox.Show("Выберите поле для удаления!");
            }
        }

        private void EditAAS_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = AddressArrivalSheetsGrid.SelectedItem as DataRowView;
            if (selectedRow != null)
            {
                EditAASForm editAASForm = new EditAASForm(AddressArrivalSheetsGrid, Convert.ToInt32(selectedRow.Row.ItemArray[0]), Convert.ToInt32(selectedRow.Row.ItemArray[1]),
                    Convert.ToString(selectedRow.Row.ItemArray[2]), Convert.ToString(selectedRow.Row.ItemArray[3]), Convert.ToString(selectedRow.Row.ItemArray[4]));
                editAASForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Не выбрана строка для редактирования", "Ошибка", MessageBoxButton.OK);
            }
        }
        #endregion

        #region Работа с таблицей ADS
        private void AddADS_Click(object sender, RoutedEventArgs e)
        {
            AddADSForm addADSForm = new AddADSForm(AddressedDepartureSheetGrid);
            addADSForm.ShowDialog();
        }

        private void DeleteADS_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedADS = AddressedDepartureSheetGrid.SelectedItem as DataRowView;
            if (selectedADS != null)
            {
                try
                {
                    database.DeleteADS(selectedADS);
                }
                catch (SqlException)
                {
                    MessageBox.Show("Удаление невозможно. Удалите связанные данные с этим гражданином!");
                    return;
                }

                database.ReadADS(AddressedDepartureSheetGrid);
            }
            else
            {
                MessageBox.Show("Выберите поле для удаления!");
            }
        }

        private void EditADS_Click(object sender, RoutedEventArgs e)
        {
           
            var selectedRow = AddressedDepartureSheetGrid.SelectedItem as DataRowView;
            if (selectedRow != null)
            {
                EditADSForm editADSForm = new EditADSForm(AddressedDepartureSheetGrid, Convert.ToInt32(selectedRow.Row.ItemArray[0]), Convert.ToInt32(selectedRow.Row.ItemArray[1]),
                    Convert.ToString(selectedRow.Row.ItemArray[2]), Convert.ToString(selectedRow.Row.ItemArray[3]), Convert.ToString(selectedRow.Row.ItemArray[4]), Convert.ToString(selectedRow.Row.ItemArray[5]));
                editADSForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Не выбрана строка для редактирования", "Ошибка", MessageBoxButton.OK);
            }
        }
        #endregion

        #region Работа с таблицей "TalonToTheASoA"
        private void AddTalonToTheASoA_Click(object sender, RoutedEventArgs e)
        {
            AddTalonToTheASoAForm addTalonToTheASoAForm = new AddTalonToTheASoAForm(TalonToTheASoAGrid);
            addTalonToTheASoAForm.ShowDialog();
        }

        private void DeleteTalonToTheASoA_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedTalonToTheASoA = TalonToTheASoAGrid.SelectedItem as DataRowView;
            if (selectedTalonToTheASoA != null)
            {
                try
                {
                    database.DeleteTalonToTheASoA(selectedTalonToTheASoA);
                }
                catch (SqlException)
                {
                    MessageBox.Show("Удаление невозможно. Удалите связанные данные с этим гражданином!");
                    return;
                }

                database.ReadTalon(TalonToTheASoAGrid);
            }
            else
            {
                MessageBox.Show("Выберите поле для удаления!");
            }
        }

        private void EditTalonToTheASoA_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = TalonToTheASoAGrid.SelectedItem as DataRowView;
            if (selectedRow != null)
            {
                EditTalonToTheASoAForm editTalonToTheASoAForm = new EditTalonToTheASoAForm(TalonToTheASoAGrid, Convert.ToInt32(selectedRow.Row.ItemArray[0]), Convert.ToString(selectedRow.Row.ItemArray[1]),
                    Convert.ToString(selectedRow.Row.ItemArray[2]), Convert.ToString(selectedRow.Row.ItemArray[3]), Convert.ToString(selectedRow.Row.ItemArray[4]));
                editTalonToTheASoAForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Не выбрана строка для редактирования", "Ошибка", MessageBoxButton.OK);
            }
        }
        #endregion

        #region Поисковик
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            foreach (var item in SearchGrid.Items)
            {
                DataGridRow row = (DataGridRow)SearchGrid.ItemContainerGenerator.ContainerFromItem(item);
                if (row != null)
                {
                    bool rowHighlighted = false;

                    // Если поле поиска пустое, сбрасываем цвет строки на обычный белый
                    if (string.IsNullOrEmpty(searchText))
                    {
                        row.Background = Brushes.White;
                        continue; // Переходим к следующей строке без проверки содержимого ячеек
                    }

                    foreach (DataGridColumn column in SearchGrid.Columns)
                    {
                        if (column is System.Windows.Controls.DataGridTextColumn)
                        {
                            string cellValue = ((Binding)((System.Windows.Controls.DataGridTextColumn)column).Binding).Path.Path;
                            var cellContent = column.GetCellContent(row);
                            if (cellContent != null && cellValue != "")
                            {
                                string cellText = ((TextBlock)cellContent).Text.ToLower();
                                if (cellText.Contains(searchText))
                                {
                                    row.Background = Brushes.LightYellow;
                                    rowHighlighted = true;
                                    break;
                                }
                            }
                        }
                    }

                    // Если строка не была выделена, сбрасываем цвет фона
                    if (!rowHighlighted)
                    {
                        row.Background = Brushes.White;
                    }
                }
            }
        }
        #endregion

        #region Фильтрация
        private void FilterBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in SearchGrid.Items)
            {
                var row = SearchGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                bool isVisible = false;

                for (int i = 0; i < SearchGrid.Columns.Count; i++)
                {
                    var cellContent = SearchGrid.Columns[i].GetCellContent(item);
                    if (cellContent is TextBlock textBlock && textBlock.Text.ToLower().Contains(dpFilter.Text))
                    {
                        isVisible = true;
                        break;
                    }
                }

                if (row != null)
                {
                    row.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
        #endregion

        private void WordCitizen_Click(object sender, RoutedEventArgs e)
        {
            WordRegistrationСard wordRegistrationСard = new WordRegistrationСard();
            var selectedRow = CitizenGrid.SelectedItem as DataRowView;
            if (selectedRow != null)
            {
                wordRegistrationСard.ReplaceFieldsFromDatabase(Convert.ToInt32(selectedRow.Row.ItemArray[0]));
            }
            else
            {
                MessageBox.Show("Не выбрана строка для печати", "Ошибка", MessageBoxButton.OK);
            }
        }
    }

}
