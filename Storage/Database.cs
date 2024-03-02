using ResidentialRegistration.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ResidentialRegistration.Storage
{
    internal class Database
    {
        static string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Residential_Registration_Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        SqlConnection sqlConnection = new SqlConnection(connection);

        private string selectCitizen = $"select * from Citizens";
        
        private string selectDocuments = $"select IssuedDocuments.DocumentID, IssuedDocuments.DocumentTypeID, IssuedDocuments.DocumentNumber, IssuedDocuments.DateOfIssue," +
            $"IssuedDocuments.IssuingAuthority, Citizens.LastName, IssuedDocuments.AdditionalInformation from IssuedDocuments, Citizens where IssuedDocuments.CitizenID = Citizens.CitizenID";
        
        private string selectResidentialUnit = $"select ResidentialUnits.UnitID, ResidentialUnits.Address, ResidentialUnits.Area, ResidentialUnits.NumberOfRooms, Citizens.LastName, ResidentialUnits.ifOwner ,ResidentialUnits.DateOfConstruction, " +
            $"ResidentialUnits.OtherCharacteristics from ResidentialUnits, Citizens where ResidentialUnits.CitizenID = Citizens.CitizenID";
        
        private string selectAAS = $"select AddressArrivalSheets.AddressArrivalSheetID, Citizens.LastName, AddressArrivalSheets.DepartureAddress," +
            $"AddressArrivalSheets.DateOfDeparture, AddressArrivalSheets.RegistrationAuthority from AddressArrivalSheets, Citizens where AddressArrivalSheets.CitizenID = Citizens.CitizenID";
       
        private string selectTalon = $"select * from TalonToTheASoA";
        
        private string selectADS = $"select AddressedDepartureSheet.AddressedDepartureSheetID, Citizens.LastName, AddressedDepartureSheet.DepartureAddress," +
            $"AddressedDepartureSheet.DateOfDeparture, AddressedDepartureSheet.AddressOfFormerResidence, AddressedDepartureSheet.PlaceOfArrival from AddressedDepartureSheet, Citizens " +
            $"where AddressedDepartureSheet.CitizenID = Citizens.CitizenID";

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

    }
}
