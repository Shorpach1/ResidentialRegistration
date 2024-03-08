using ResidentialRegistration.CB;
using ResidentialRegistration.Service;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace ResidentialRegistration.Storage
{
    internal class Database
    {
        static string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Residential_Registration_Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        SqlConnection sqlConnection = new SqlConnection(connection);

        private string selectCitizen = $"select * from Citizens";
        
        private string selectDocuments = $"select IssuedDocuments.DocumentID, DocumentTypes.DocumentType, IssuedDocuments.DocumentNumber, IssuedDocuments.DateOfIssue," +
            $"IssuedDocuments.IssuingAuthority, Citizens.CitizenID, IssuedDocuments.AdditionalInformation from IssuedDocuments, Citizens, DocumentTypes where IssuedDocuments.CitizenID = Citizens.CitizenID AND IssuedDocuments.DocumentTypeID = DocumentTypes.DocumentTypeID";
        
        private string selectResidentialUnit = $"select ResidentialUnits.UnitID, ResidentialUnits.Address, ResidentialUnits.Area, ResidentialUnits.NumberOfRooms, Citizens.LastName, ResidentialUnits.isOwner ,ResidentialUnits.DateOfConstruction, " +
            $"ResidentialUnits.OtherCharacteristics from ResidentialUnits, Citizens where ResidentialUnits.CitizenID = Citizens.CitizenID";
        
        private string selectAAS = $"select AddressArrivalSheets.AddressArrivalSheetID, Citizens.LastName, AddressArrivalSheets.DepartureAddress," +
            $"AddressArrivalSheets.DateOfDeparture, AddressArrivalSheets.RegistrationAuthority from AddressArrivalSheets, Citizens where AddressArrivalSheets.CitizenID = Citizens.CitizenID";
       
        private string selectTalon = $"select * from TalonToTheASoA";
        
        private string selectADS = $"select AddressedDepartureSheet.AddressedDepartureSheetID, Citizens.LastName, AddressedDepartureSheet.DepartureAddress," +
            $"AddressedDepartureSheet.DateOfDeparture, AddressedDepartureSheet.AddressOfFormerResidence, AddressedDepartureSheet.PlaceOfArrival from AddressedDepartureSheet, Citizens " +
            $"where AddressedDepartureSheet.CitizenID = Citizens.CitizenID";

        private string selectCitizenName = $"select CitizenID, LastName  + ' ' +  FirstName + ' ' +  MiddleName from Citizens";

        private string selectDocumentType = $"select * from DocumentTypes";

        public void Connection()
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
            }
            else if (sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        #region Проверка при входе (регистр учитывается)
        public bool Check(string username, string password)
        {
            Connection();
            SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM Users WHERE Login='{username}' COLLATE Cyrillic_General_CS_AS AND Password='{PasswordEncoder.GetSHA256Hash(password)}' COLLATE Cyrillic_General_CS_AS", sqlConnection);
            int result = (int)command.ExecuteScalar();
            Connection();

            return result > 0;
        }
        #endregion

        #region Проверка логина
        public int CountUsersWithLogin(string username)
        {
            Connection();
            SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM Users WHERE Login='{username}'", sqlConnection);
            int count = (int)command.ExecuteScalar();
            Connection();
            return count;
        }
        #endregion

        #region Регистрация аккаунта
        public void RegisterAccount(string username, string password, string role, string surname, string firstName, string middleName)
        {
            string query = $"INSERT INTO Users (Login, Password, Role, Surname, Name, SecondName) VALUES ('{username}', '{PasswordEncoder.GetSHA256Hash(password)}', '{role}', N'{surname}', N'{firstName}', N'{middleName}')";
            Update(query);
        }
        #endregion

        #region Проверка на админа
        public bool CheckAdmin(string username)
        {
            Connection();
            SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM Users WHERE Login='{username}' AND Role='ROLE_ADMIN'", sqlConnection);
            int result = (int)command.ExecuteScalar();
            Connection();

            return result > 0;
        }
        #endregion

        public DataTable Select(string query)
        {
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

            DataTable data = new DataTable();
            adapter.Fill(data);
            return data;
        }

        public void Select(string query, DataGrid dataGrid)
        {
            Connection();
            SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGrid.ItemsSource = dataTable.DefaultView;
            Connection();
        }

        public void Update(string query)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                Connection();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            command.ExecuteNonQuery();
            Connection();
        }

        #region Вывод в таблицу
        public void ReadCitizen(DataGrid grid)
        {
            Select(selectCitizen, grid);
        }

        public void ReadResidentialUnit(DataGrid grid)
        {
            Select(selectResidentialUnit, grid);
        }

        public void ReadDocuments(DataGrid grid)
        {
            Select(selectDocuments, grid);
        }

        public void ReadAAS(DataGrid grid)
        {
            Select(selectAAS, grid);
        }

        public void ReadADS(DataGrid grid)
        {
            Select(selectADS, grid);
        }

        public void ReadTalon(DataGrid grid)
        {
            Select(selectTalon, grid);
        }
        #endregion

        public void ReadDocumentTypeToComboBox(ComboBox box)
        {
            ComboBoxToTableDT(selectDocumentType, box);
        }

        public void ReadCitizenNameToCombobox(ComboBox box)
        {
            ComboBoxToTableDT(selectCitizenName, box);
        }

        public void ComboBoxToTableDT(string query, ComboBox box)
        {
            Connection();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            box.Items.Clear();
            while (reader.Read())
            {
                CBDocumentType cbdt = new CBDocumentType();
                cbdt.id = reader.GetInt32(0);
                cbdt.name = reader.GetString(1).ToString();
                box.Items.Add(cbdt);
            }
            reader.Close();
            Connection();
        }

        #region Работа с таблицой "Citizens"

        #region Добавление гражданина
        public void CreateCitizen(string surname, string name, string middlename, DateTime dateOfBirth, string gender, string placeOfBirth, string other)
        {
            string query = $"INSERT INTO Citizens (LastName, FirstName, MiddleName, DateOfBirth, Gender, PlaceOfBirth, OtherPersonalData) VALUES (N'{surname}', N'{name}', N'{middlename}', N'{dateOfBirth.ToString("yyyy-MM-dd")}', N'{gender}'," +
                $" N'{placeOfBirth}', N'{other}' )";
            Update(query);
        }
        #endregion

        #region Удаление гражданина
        public void DeleteCitizen(DataRowView selectedRow)
        {
            Update($"DELETE FROM Citizens Where CitizenID = {selectedRow.Row.ItemArray[0]}");
        }
        #endregion

        #region Изменение гражданина
        public void EditCitizen(long id,string surname, string name, string middlename, DateTime dateOfBirth, string gender, string placeOfBirth, string other)
        {
            Update($"UPDATE Citizens SET LastName = N'{surname}', FirstName = N'{name}', MiddleName = N'{middlename}', DateOfBirth = '{dateOfBirth.ToString("yyyy-MM-dd")}', Gender = N'{gender}', PlaceOfBirth = N'{placeOfBirth}', OtherPersonalData = N'{other}' " +
                $"WHERE Citizens.CitizenID = {id}");
        }
        #endregion

        #endregion

        #region Работа с таблицей "IssuedDocuments"
        public void CreateIssuedDocuments(CBDocumentType dtId, string docNum, DateTime dateOfIssue, string issuingAuthority, CBDocumentType citId, string other)
        {
            string query = $"INSERT INTO IssuedDocuments (DocumentTypeID, DocumentNumber, DateOfIssue, IssuingAuthority, CitizenID, AdditionalInformation) VALUES " +
                $"('{dtId.id}','{docNum}', '{dateOfIssue.ToString("yyyy-MM-dd")}', N'{issuingAuthority}', '{citId.id}', N'{other}')";
            Update(query);
        }

        public void DeleteissuedDocument(DataRowView selectedRow)
        {
            Update($"DELETE FROM IssuedDocuments Where DocumentID = {selectedRow.Row.ItemArray[0]}");
        }

        public void EditIssuedDocument(int id, CBDocumentType dtId, string docNum, DateTime dateOfIssue, string issuingAuthority, CBDocumentType citId, string other)
        {
            Update($"UPDATE IssuedDocuments SET DocumentTypeID = '{dtId.id}', DocumentNumber = '{docNum}', DateOfIssue = '{dateOfIssue.ToString("yyyy-MM-dd")}',IssuingAuthority = N'{issuingAuthority}', " +
                $"CitizenID = '{citId.id}', AdditionalInformation = N'{other}' WHERE DocumentID = {id}");
        }
        #endregion

        #region Работа с таблицей "ResidentialUnits"
        public void CreateResidentialUnit(string address, string area, string numOfRooms, CBDocumentType citId, DateTime date, string ifOwner, string other)
        {
            string query = $"INSERT INTO ResidentialUnits (Address, Area, NumberOfRooms, CitizenID, isOwner, DateOfConstruction, OtherCharacteristics) VALUES " +
                $"( N'{address}', '{Convert.ToInt32(area)}', '{Convert.ToInt32(numOfRooms)}', '{citId.id}', N'{ifOwner}', '{date.ToString("yyyy-MM-dd")}', N'{other}')";
            Update(query);
        }
        #endregion
    }
}
