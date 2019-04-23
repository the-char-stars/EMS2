using System;
using System.Data;
using System.Data.SqlClient;

namespace EMS_Backend.Data
{
    public class Database
    {

        #region DATABASE LOGIN
        private string username;
        private string password;

        private string server;
        private string database;

        private SqlConnectionStringBuilder conBldStr;
        #endregion

        public Database(string username, string password)
        {
            this.username = username;
            this.password = password;

            this.server = "thecharstarsems.database.windows.net";
            this.database = "TheCharStars_EMS";

            conBldStr = new SqlConnectionStringBuilder
            {
                DataSource = this.server,
                UserID = this.username,
                Password = this.password,
                InitialCatalog = this.database
            };

        }

        public DataTable GetPatients()
        {
            string query = "SELECT * from tblPatients";
            return QueryDatabase(query);
        }

        private DataTable QueryDatabase(string query)
        {
            // table to store the data queried
            DataTable table = new DataTable();

            try
            {
                using(SqlConnection connection = new SqlConnection(this.conBldStr.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(table);

                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return table;
        }
    }
}
