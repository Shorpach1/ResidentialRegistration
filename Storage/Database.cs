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
            SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM Users WHERE Login='{username}' COLLATE Cyrillic_General_CS_AS AND Password='{password}' COLLATE Cyrillic_General_CS_AS", sqlConnection);
            int result = (int)command.ExecuteScalar();
            Connection();

            return result > 0;
        }
        #endregion
    }
}
