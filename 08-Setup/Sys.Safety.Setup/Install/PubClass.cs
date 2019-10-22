using System;
using MySql.Data.MySqlClient;
using System.IO;

namespace Sys.Safety.Setup.Install
{
    public class PubClass
    {
        public static void ExecSqlFile(string strFilePath, string strConnectString)
        {
            MySqlConnection conn = new MySqlConnection(strConnectString);
           
            try
            {
                conn.Open();
                string path = strFilePath;
                FileInfo file = new FileInfo(path);
                string strsql = file.OpenText().ReadToEnd();
                MySqlScript script = new MySqlScript(conn);
                script.Query = strsql;
                script.Delimiter = "??";
                int count = script.Execute();
                script.Delimiter = ";";
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool BlnConnection(string strConnection)
        {
            MySqlConnection conn = new MySqlConnection(strConnection);
            try
            {
                conn.Open();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
          
        }

    }
}
