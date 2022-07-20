using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace sdf2csv
{
    class Program
    {
        static void Main(string[] args)
        {
            string sdf_file = args[0];
            string sdf_pwd = args[1];
            string sdf_table = args[2];

            string connectionString =
                "Provider=Microsoft.SQLSERVER.CE.OLEDB.3.5; Data Source=" + sdf_file + "; SSCE:Database Password='" + sdf_pwd + "'";

            OleDbConnection conn = new OleDbConnection(connectionString);
            
            try
            {
                conn.Open();
                //Read data from table or view to data table
                string query = "SELECT * FROM " + sdf_table;                
                // creo oggetto command e li passo la query
                OleDbCommand command = new OleDbCommand(query, conn);
                // eseguo la query sul database specificato nell'oggetto connection
                OleDbDataReader reader = command.ExecuteReader();
                int n_columns = reader.FieldCount;

                System.IO.TextWriter stm = new System.IO.StreamWriter(
                    new System.IO.FileStream(sdf_file + "." + sdf_table + ".csv", System.IO.FileMode.Create),
                    Encoding.UTF8
                    );

                List<string> columns = Enumerable.Range(0, n_columns).Select(reader.GetName).ToList();

                {
                    int ic = 0;
                    foreach (string c in columns)
                    {
                        stm.Write(c);
                        if (ic < n_columns - 1)
                        {
                            stm.Write(";");
                        }
                        ic++;
                    }
                    stm.Write(System.Environment.NewLine);
                }

                string s = null;
                while (reader.Read())
                {
                    // Write all fields except the last one...
                    for (int ic = 0; ic < n_columns; ic++)
                    {
                        s = reader[ic].ToString();
                        // Force the use of '.' as decimal separator
                        s = System.Text.RegularExpressions.Regex.Replace(s, "([0-9]{1,}),([0-9]{1,})", "$1.$2");
                        // Replace newlines for a better visualization when importing the csv in Excel.
                        s = s.Replace(System.Environment.NewLine, " | ");
                        stm.Write('"' + s + '"');
                        if (ic < n_columns - 1)
                        {
                            stm.Write(";");
                        }
                    }
                    stm.Write(System.Environment.NewLine);
                }
                stm.Close();
                Console.WriteLine("Done");
                conn.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
