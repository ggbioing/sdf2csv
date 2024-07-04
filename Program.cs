using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.SqlServerCe;

namespace sdf2csv
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                string sdf_file = args[0];
                string sdf_pwd = args[1];
                string sdf_table = args[2];
                string sdf_suffix = args[3];

                string connectionString =
                    //"Provider=Microsoft.SQLSERVER.CE.OLEDB.3.5; " +
                    "Data Source=" + sdf_file + "; " +
                    "SSCE:Database Password=\"" + sdf_pwd + "\"";

                // OleDbConnection conn = new OleDbConnection(connectionString);
                var conn = new SqlCeConnection(connectionString);
                conn.Open();

                string query;
                if (sdf_table == "schema")
                {
                    query = "SELECT TABLE_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS";
                }
                else if (sdf_table == "analisi" || sdf_table == "pazienti")
                {
                    //Read data from table or view to data table
                    query = "SELECT * FROM " + sdf_table;
                }
                else if (sdf_table == "export")
                {
                    query = "SELECT " +
                    "p.id as patient_id, " +
                    "p.cognome as surname, " +
                    "p.nome as name, " +
                    "p.sesso as sex, " +
                    "CAST(p.anno_n AS NVARCHAR) + '-' + CAST(p.mese_n AS NVARCHAR) + '-' + CAST(p.giorno_n AS NVARCHAR) as date_of_birth, " +
                    "CASE WHEN p.etnia = 'Caucasica' THEN 'Caucasian' ELSE p.etnia END AS ethnicity, " +
                    "p.eta_menopausa AS menopause_age, " +
                    "p.patologie as pathologies, " +
                    "p.note as notes, " +
                    "a.id as analysis_id, " +
                    "CAST(a.anno_acq AS NVARCHAR) + '-' + CAST(a.mese_acq AS NVARCHAR) + '-' + CAST(a.giorno_acq AS NVARCHAR) as date_of_analysis, " +
                    "a.eta as patient_age, " +
                    "a.altezza as patient_height_cm, " +
                    "a.peso as patient_weight_kg, " +
                    "ROUND(CAST(a.peso AS float) / CAST(a.altezza AS float) / CAST(a.altezza AS float) * 100 * 100, 2) as BMI, " +
                    "CASE WHEN a.sito = 'Colonna' THEN 'Spine' WHEN a.sito = 'Femore_DX' THEN 'Right_Femur' WHEN a.sito = 'Femore_SX' THEN 'Left_Femur' ELSE a.sito END AS anatomic_site, " +
                    "a.bmd AS bmd_total, " +
                    "a.tscore AS tscore_total, " +
                    "a.zscore AS zscore_total, " +
                    "CASE WHEN a.esito = 'Normale' THEN 'Normal' WHEN a.esito = 'Osteopenia' THEN 'Osteopenia' WHEN a.esito = 'Osteoporosi' THEN 'Osteoporosis' ELSE a.esito END AS diagnosis, " +
                    "CASE WHEN a.bmdl1 = 'N.D.' THEN 'not available' WHEN a.sito = 'Colonna' THEN a.bmdl1 ELSE '---' END AS bmd_L1, " +
                    "CASE WHEN a.bmdl2 = 'N.D.' THEN 'not available' WHEN a.sito = 'Colonna' THEN a.bmdl2 ELSE '---' END AS bmd_L2, " +
                    "CASE WHEN a.bmdl3 = 'N.D.' THEN 'not available' WHEN a.sito = 'Colonna' THEN a.bmdl3 ELSE '---' END AS bmd_L3, " +
                    "CASE WHEN a.bmdl4 = 'N.D.' THEN 'not available' WHEN a.sito = 'Colonna' THEN a.bmdl4 ELSE '---' END AS bmd_L4, " +
                    "CASE WHEN a.sito = 'Colonna' THEN a.tscorel1 ELSE '---' END AS tscore_L1, " +
                    "CASE WHEN a.sito = 'Colonna' THEN a.tscorel2 ELSE '---' END AS tscore_L2, " +
                    "CASE WHEN a.sito = 'Colonna' THEN a.tscorel3 ELSE '---' END AS tscore_L3, " +
                    "CASE WHEN a.sito = 'Colonna' THEN a.tscorel4 ELSE '---' END AS tscore_L4, " +
                    //"CASE WHEN a.sito = 'Colonna' THEN a.yaml1 ELSE '---' END AS yam_L1, " +
                    //"CASE WHEN a.sito = 'Colonna' THEN a.yaml2 ELSE '---' END AS yam_L2, " +
                    //"CASE WHEN a.sito = 'Colonna' THEN a.yaml3 ELSE '---' END AS yam_L3, " +
                    //"CASE WHEN a.sito = 'Colonna' THEN a.yaml4 ELSE '---' END AS yam_L4, " +
                    //"CASE WHEN a.sito = 'Colonna' THEN a.yamSpine ELSE '---' END AS yam_SPINE, " +
                    //"CASE WHEN a.sito LIKE 'Femore%' THEN a.yamNeck ELSE '---' END AS yam_FEMORAL_NECK, " +
                    //"CASE WHEN a.sito LIKE 'Femore%' THEN a.yamFem ELSE '---' END AS yam_FEMUR, " +
                    "CASE WHEN a.sito LIKE 'Femore%' THEN a.bmdl1 ELSE '---' END AS bmd_femoral_neck, " +
                    "CASE WHEN a.sito LIKE 'Femore%' THEN a.bmdl2 ELSE '---' END AS bmd_femoral_trochanter, " +
                    "CASE WHEN a.tscorecollo = 'N.D.' THEN 'not available' WHEN a.sito LIKE 'Femore%' THEN a.tscorecollo ELSE '---' END AS tscore_femoral_neck, " +
                    "CASE WHEN a.tscoretrocantere = 'N.D.' THEN 'not available' WHEN a.sito LIKE 'Femore%' THEN a.tscoretrocantere ELSE '---' END AS tscore_femoral_trochanter, " +
                    "CASE WHEN a.zscorecollo = 'N.D.' THEN 'not available' WHEN a.sito LIKE 'Femore%' THEN a.zscorecollo ELSE '---' END AS zscore_femoral_neck, " +
                    "CASE WHEN a.zscoretrocantere = 'N.D.' THEN 'not available' WHEN a.sito LIKE 'Femore%' THEN a.zscoretrocantere ELSE '---' END AS zscore_femoral_trochanter, " +
                    "CASE WHEN frattura_pregressa = 'si' THEN 'yes' ELSE frattura_pregressa END AS previous_fracture, " +
                    "CASE WHEN genitori_con_femore_fratturato = 'si' THEN 'yes' ELSE genitori_con_femore_fratturato END AS parent_with_fractured_hip, " +
                    "CASE WHEN fumatore_abituale = 'si' THEN 'yes' ELSE fumatore_abituale END AS smoker, " +
                    "CASE WHEN cortisonici = 'si' THEN 'yes' ELSE cortisonici END AS cortisone_based_therapy, " +
                    "CASE WHEN artrite_reumatoide = 'si' THEN 'yes' ELSE artrite_reumatoide END AS rheumatoid_arthritis, " +
                    "CASE WHEN osteoporosi_secondaria = 'si' THEN 'yes' ELSE osteoporosi_secondaria END AS secondary_osteoporosis, " +
                    "CASE WHEN alcol = 'si' THEN 'yes' ELSE alcol END AS alcol, " +
                    "CASE WHEN fraxfemore = 'N.D.' THEN 'not available' ELSE fraxfemore END AS frax_femur, " +
                    "CASE WHEN fraxgenerale = 'N.D.' THEN 'not available' ELSE fraxgenerale END AS frax_general, " +
                    "new_file_vcr AS vcr_name " +
                    "FROM pazienti AS p LEFT OUTER JOIN analisi as A ON p.id = a.id_paziente";
                }
                else
                {
                    query = sdf_table;
                }

                // creo oggetto command e li passo la query
                // OleDbCommand command = new OleDbCommand(query, conn);
                var command = new SqlCeCommand(query, conn);
                // eseguo la query sul database specificato nell'oggetto connection
                //OleDbDataReader reader = command.ExecuteReader();
                var reader = command.ExecuteReader();
                
                if (sdf_suffix == "stdout")
                {
                    System.IO.StreamWriter stm = new System.IO.StreamWriter(Console.OpenStandardOutput());
                    stm.AutoFlush = true;
                    Console.SetOut(stm);
                    WriteQueryResult(stm, reader);
                }
                else
                {
                    string csvout = sdf_file + "." + sdf_suffix + ".csv";
                    System.IO.TextWriter stm = new System.IO.StreamWriter(
                        new System.IO.FileStream(csvout, System.IO.FileMode.Create),
                        Encoding.UTF8
                        );
                    WriteQueryResult(stm, reader);
                }

                conn.Close();

                return 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return 1;
            }
        }

        static void WriteQueryResult(System.IO.TextWriter stm, SqlCeDataReader reader)
        {
            int n_columns = reader.FieldCount;

            List<string> columns = Enumerable.Range(0, n_columns).Select(reader.GetName).ToList();

            #region Write header
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
            #endregion

            #region Write enntries
            string s = null;
            while (reader.Read())
            {
                for (int ic = 0; ic < n_columns; ic++)
                {
                    s = reader[ic].ToString();
                    // Force the use of '.' as decimal separator
                    s = System.Text.RegularExpressions.Regex.Replace(s, "([0-9]{1,}),([0-9]{1,})", "$1.$2");
                    // Replace in-cell newlines for a better visualization when importing the csv in Excel.
                    s = s.Replace(System.Environment.NewLine, " | ");
                    stm.Write('"' + s + '"');
                    // Do not append the column separator at the end
                    if (ic < n_columns - 1)
                    {
                        stm.Write(";");
                    }
                }
                stm.Write(System.Environment.NewLine);
            }
            #endregion

            stm.Close();
        }
    }
}
