using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
//using System.Data.SqlClient;

namespace FindDatabaseApp
{
    public class DatabaseManagement
    {
        public string serverName { get; set; }
        public string userName { get; set; }
        public string pass { get; set; }
        public string databaseNameDefault { get; set; }
        public string databaseName { get; set; }
        public List<string> dataBaseList;
        public string valueLookedAfter { get; set; }
        //public List <string> serverList { get; set; }
        
        public DatabaseManagement()
        {
            databaseNameDefault = "master";
        }


    }

    public static class DatabaseManagementMethods
    {
        public static DataSet ds = new DataSet();
        public static void ConnectToSelectedDatabase(string DataSource, string InitialCatalog, string UserID, string Password, string valueLookedAfter)
        {
       
            string connectionString = "Data Source =" + DataSource +
                                "; Initial Catalog =" + InitialCatalog +
                                "; User ID=" + UserID +
                                "; Password=" + Password;
            

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("The database has been opened!");
                Console.WriteLine("Connection State: " + connection.State.ToString());
                string queryStatement = "SELECT OBJECT_NAME(id) FROM SYSCOMMENTS WHERE [text] LIKE '%" + valueLookedAfter + "%' AND OBJECTPROPERTY(id, 'IsProcedure') = 1 GROUP BY OBJECT_NAME(id)";
                using (SqlDataAdapter _cmd = new SqlDataAdapter(queryStatement, connection))
                {
                   
                    var dataAdapter = new SqlDataAdapter(queryStatement, connection);
                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                    dataAdapter.Fill(ds);
                }
                List<FileData> filedt = new List<FileData>();
                FileRW objflr = new FileRW();
                FileData fileInfo = new FileData();
                IDictionary<string, string> d = new Dictionary<string, string>();
                string encryptedPassword = StringUtil.Crypt(Password);
                d.Add(UserID, encryptedPassword);
                FileData fileInfoObj = new FileData(fileInfo.serverName = DataSource, fileInfo.UserPassDict = d);
                filedt.Add(fileInfoObj);
                if (FileOperations.readFromFile(objflr) == null || !FileOperations.readFromFile(objflr).Contains(fileInfoObj))
                {
                    
                    FileOperations.writeToFile(objflr, filedt);
                    
                }
                else { Console.WriteLine("server name already exists"); }

                connection.Close();
                Console.WriteLine("The database has been closed!");

                
            }
            catch (Exception ex)
            {
                Console.WriteLine("There's an error connecting to the database!\n" + ex.Message);
            }
        }




        public static List<string> GetDatabaseList(string serverName, string databaseNameDefault, string userName, string pass)
        {
            
            List<string> list = new List<string>();
            string connectionString = BuildConnectionString(serverName, userName, pass, databaseNameDefault);

            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand("SELECT name FROM sys.databases", sqlConn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string dbName = reader["name"].ToString();
                    list.Add(dbName);
                }
                sqlConn.Close();
            }


            return list;
        }

        public static string BuildConnectionString(string serverName, string userName, string pass, string databaseNameDefault)
        {
            return $"server={serverName};uid ={userName};pwd={pass};database= {databaseNameDefault}";
        }

       
    }
}
