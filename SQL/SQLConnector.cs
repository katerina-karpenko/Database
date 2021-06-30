using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security;
using System.Data;

namespace SQL
{
    public class SQLConnector
    {
        private readonly SqlCredential _credential;
        string _connectionString;

        public SQLConnector(string login, string password)
        {
            var credential = new SecureString();
            for (var i = 0; i < password.Length; i++)
                credential.InsertAt(i, password[i]);

            credential.MakeReadOnly();

            _credential = new SqlCredential(login, credential);
        }

        public void ConnectToCatalog(string catalogName)
        {
            _connectionString = "Data Source=DESKTOP-THEKFEB; " + $"Initial Catalog={catalogName}; ";
        }

        public DataTable Execute(string sqlRequest)
        {
            var dataSet = new DataSet();

            using (var sqlConnection = new SqlConnection(_connectionString, _credential))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(sqlRequest, sqlConnection))
                {
                    sqlCommand.CommandText = sqlRequest;
                    var adapter = new SqlDataAdapter(sqlCommand);
                    adapter.Fill(dataSet);
                }
            sqlConnection.Close();
            }
            return dataSet.Tables[0];
            }
    }
    }