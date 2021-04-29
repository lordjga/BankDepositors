using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BankDepositors
{
    public partial class Connection
    {
        string connectionString;

        public Connection()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        public DataTable Select(string selestSQL)
        {
            DataTable dataTable = new DataTable("dataBase"); // создаём таблицу в приложении
            // подключаемся к базе данных
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();                                           // открываем базу данных
            SqlCommand sqlCommand = sqlConnection.CreateCommand();          // создаём команду
            sqlCommand.CommandText = selestSQL;                             // присваиваем команде текст
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand); // создаём обработчик
            sqlDataAdapter.Fill(dataTable);                                 // возращаем таблицу с результатом
            return dataTable;
        }

    }
}
