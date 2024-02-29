using ResidentialRegistration.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResidentialRegistration.Storage
{
    internal class Database
    {
        static string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Residential_Registration_Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        SqlConnection sqlConnection = new SqlConnection(connection);
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

        public void Update(string query)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                Connection();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            command.ExecuteNonQuery();
            Connection();
        }
    }
}
