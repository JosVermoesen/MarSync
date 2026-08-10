using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;

namespace MarSync
{
    class DataSuppliers
    {
        public void SuppliersInsertAndUpdate(string marLocatie, string sqlNew, string sqlUpdate, string cnn)
        {
            using (IDbConnection connection = new SqlConnection(cnn))
            {
                try
                {
                    OleDbConnection cnnMarJet = new OleDbConnection
                    {
                        ConnectionString = marLocatie
                    };

                    cnnMarJet.Open();

                    OleDbCommand cmdNew = new OleDbCommand
                    {
                        Connection = cnnMarJet,
                        CommandText = sqlNew
                    };

                    OleDbCommand cmdUpdate = new OleDbCommand
                    {
                        Connection = cnnMarJet,
                        CommandText = sqlUpdate
                    };

                    OleDbDataReader readerNew = cmdNew.ExecuteReader();
                    OleDbDataReader readerUpdate = cmdUpdate.ExecuteReader();

                    List<VsoftSupplier> suppliersNew = new List<VsoftSupplier>();
                    List<VsoftSupplier> suppliersUpdate = new List<VsoftSupplier>();

                    while (readerNew.Read())
                    {
                        suppliersNew.Add(new VsoftSupplier
                        {
                            A110 = readerNew["A110"].ToString().Trim(' '),
                            A102 = readerNew["A102"].ToString().Trim(' '),
                            A100 = readerNew["A100"].ToString().Trim(' '),
                            Vs01 = readerNew["vs01"].ToString().Trim(' '),
                            A125 = readerNew["A125"].ToString().Trim(' '),
                            A10c = readerNew["A10C"].ToString().Trim(' '),
                            A104 = readerNew["A104"].ToString().Trim(' '),
                            A105 = readerNew["A105"].ToString().Trim(' '),
                            A106 = readerNew["A106"].ToString().Trim(' '),
                            A107 = readerNew["A107"].ToString().Trim(' '),

                            A108 = readerNew["A108"].ToString().Trim(' '),
                            V149 = readerNew["v149"].ToString().Trim(' '),
                            A109 = readerNew["A109"].ToString().Trim(' '),
                            V150 = readerNew["v150"].ToString().Trim(' '),
                            Vs03 = readerNew["vs03"].ToString().Trim(' '),
                            A10a = readerNew["A10A"].ToString().Trim(' '),
                            Vs02 = readerNew["vs02"].ToString().Trim(' '),
                            V224 = readerNew["v224"].ToString().Trim(' '),
                            V163 = readerNew["v163"].ToString().Trim(' '),
                            V016 = readerNew["v016"].ToString().Trim(' '),

                            V161 = readerNew["v161"].ToString().Trim(' '),
                            A161 = readerNew["A161"].ToString().Trim(' '),
                            V404 = readerNew["v404"].ToString().Trim(' '),
                            A170 = readerNew["A170"].ToString().Trim(' '),

                            V259 = readerNew["v259"].ToString().Trim(' '),
                            V260 = readerNew["v260"].ToString().Trim(' '),

                            A400 = readerNew["A400"].ToString().Trim(' '),
                            V015 = readerNew["v015"].ToString().Trim(' '),
                            V151 = readerNew["v151"].ToString().Trim(' '),
                            V111 = readerNew["v111"].ToString().Trim(' '),
                            Vs04 = readerNew["vs04"].ToString().Trim(' '),
                            V017 = readerNew["v017"].ToString().Trim(' '),
                            V018 = readerNew["v018"].ToString().Trim(' '),

                            V001 = readerNew["v001"].ToString().Trim(' '),
                            V002 = readerNew["v002"].ToString().Trim(' '),
                            V226 = readerNew["v226"].ToString().Trim(' '),
                            V227 = readerNew["v227"].ToString().Trim(' '),
                            V247 = readerNew["v247"].ToString().Trim(' '),
                            V254 = readerNew["v254"].ToString().Trim(' '),

                            V255 = readerNew["v255"].ToString().Trim(' '),
                            V256 = readerNew["v256"].ToString().Trim(' '),
                            V262 = readerNew["v262"].ToString().Trim(' ')
                        });
                    }

                    try
                    {
                        connection.Execute(
                            "spVsoftSuppliers_InsertFull  @A110, @A102, @A100, @Vs01, @A125, @A10c, @A104, @A105, @A106, @A107, @A108, @V149, @A109, @V150, @Vs03, @A10a, @Vs02, @V224, @V163, @V016, @V161, @A161, @V404, @A170, @V259, @V260, @A400, @V015, @V151, @V111, @Vs04, @V017, @V018, @V001, @V002, @V226, @V227, @V247, @V254, @V255, @V256, @V262",
                            suppliersNew);
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    while (readerUpdate.Read())
                    {
                        suppliersUpdate.Add(new VsoftSupplier
                        {
                            A110 = readerUpdate["A110"].ToString().Trim(' '),
                            A102 = readerUpdate["A102"].ToString().Trim(' '),
                            A100 = readerUpdate["A100"].ToString().Trim(' '),
                            Vs01 = readerUpdate["vs01"].ToString().Trim(' '),
                            A125 = readerUpdate["A125"].ToString().Trim(' '),
                            A10c = readerUpdate["A10C"].ToString().Trim(' '),
                            A104 = readerUpdate["A104"].ToString().Trim(' '),
                            A105 = readerUpdate["A105"].ToString().Trim(' '),
                            A106 = readerUpdate["A106"].ToString().Trim(' '),
                            A107 = readerUpdate["A107"].ToString().Trim(' '),

                            A108 = readerUpdate["A108"].ToString().Trim(' '),
                            V149 = readerUpdate["v149"].ToString().Trim(' '),
                            A109 = readerUpdate["A109"].ToString().Trim(' '),
                            V150 = readerUpdate["v150"].ToString().Trim(' '),
                            Vs03 = readerUpdate["vs03"].ToString().Trim(' '),
                            A10a = readerUpdate["A10A"].ToString().Trim(' '),
                            Vs02 = readerUpdate["vs02"].ToString().Trim(' '),
                            V224 = readerUpdate["v224"].ToString().Trim(' '),
                            V163 = readerUpdate["v163"].ToString().Trim(' '),
                            V016 = readerUpdate["v016"].ToString().Trim(' '),

                            V161 = readerUpdate["v161"].ToString().Trim(' '),
                            A161 = readerUpdate["A161"].ToString().Trim(' '),
                            V404 = readerUpdate["v404"].ToString().Trim(' '),
                            A170 = readerUpdate["A170"].ToString().Trim(' '),

                            V259 = readerUpdate["v259"].ToString().Trim(' '),
                            V260 = readerUpdate["v260"].ToString().Trim(' '),

                            A400 = readerUpdate["A400"].ToString().Trim(' '),
                            V015 = readerUpdate["v015"].ToString().Trim(' '),
                            V151 = readerUpdate["v151"].ToString().Trim(' '),
                            V111 = readerUpdate["v111"].ToString().Trim(' '),
                            Vs04 = readerUpdate["vs04"].ToString().Trim(' '),
                            V017 = readerUpdate["v017"].ToString().Trim(' '),
                            V018 = readerUpdate["v018"].ToString().Trim(' '),

                            V001 = readerUpdate["v001"].ToString().Trim(' '),
                            V002 = readerUpdate["v002"].ToString().Trim(' '),
                            V226 = readerUpdate["v226"].ToString().Trim(' '),
                            V227 = readerUpdate["v227"].ToString().Trim(' '),
                            V247 = readerUpdate["v247"].ToString().Trim(' '),
                            V254 = readerUpdate["v254"].ToString().Trim(' '),

                            V255 = readerUpdate["v255"].ToString().Trim(' '),
                            V256 = readerUpdate["v256"].ToString().Trim(' '),
                            V262 = readerUpdate["v262"].ToString().Trim(' ')
                        });
                    }

                    try
                    {
                        connection.Execute(
                            "spVsoftSuppliers_UpdateFull @A110, @A102, @A100, @Vs01, @A125, @A10c, @A104, @A105, @A106, @A107, @A108, @V149, @A109, @V150, @Vs03, @A10a, @Vs02, @V224, @V163, @V016, @V161, @A161, @V404, @A170, @V259, @V260, @A400, @V015, @V151, @V111, @Vs04, @V017, @V018, @V001, @V002, @V226, @V227, @V247, @V254, @V255, @V256, @V262",
                            suppliersUpdate);
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    readerNew.Close();
                    readerUpdate.Close();

                    // Per-batch resync for just these suppliers
                    if (suppliersNew.Count > 0 || suppliersUpdate.Count > 0)
                    {
                        Console.WriteLine(
                            "Aantal nieuwe leveranciers: " + suppliersNew.Count +
                            ", aantal updates: " + suppliersUpdate.Count);
                        SyncAccessForSuppliers(marLocatie, cnn, suppliersNew, suppliersUpdate);
                    }

                    cnnMarJet.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void SyncAccessForSuppliers(
            string marLocatie,
            string cnn,
            IEnumerable<VsoftSupplier> suppliersNew,
            IEnumerable<VsoftSupplier> suppliersUpdate)
        {
            var keys = new HashSet<string>(
                suppliersNew.Select(s => s.A110)
                            .Concat(suppliersUpdate.Select(s => s.A110))
                            .Where(k => !string.IsNullOrWhiteSpace(k)));

            if (keys.Count == 0)
            {
                return;
            }

            using (var sqlConn = new SqlConnection(cnn))
            using (var accessConn = new OleDbConnection(marLocatie))
            {
                sqlConn.Open();
                accessConn.Open();

                foreach (string a110 in keys)
                {
                    int? sqlId = null;

                    // 1) Get SQL Id for this supplier
                    using (var cmd = new SqlCommand(
                        "SELECT Id FROM VsoftSuppliers WHERE A110 = @A110", sqlConn))
                    {
                        cmd.Parameters.AddWithValue("@A110", a110);
                        object obj = cmd.ExecuteScalar();
                        if (obj != null && obj != DBNull.Value)
                        {
                            sqlId = Convert.ToInt32(obj);
                        }
                    }

                    if (!sqlId.HasValue)
                    {
                        Console.WriteLine("Geen VsoftSuppliers.Id gevonden voor A110=" + a110);
                        continue;
                    }

                    // 2) Update Leveranciers row
                    using (var updateLev = new OleDbCommand(
                        "UPDATE Leveranciers SET sqlID = ?, dnnSyncAlready = True, dnnSync = True WHERE A110 = ?",
                        accessConn))
                    {
                        updateLev.Parameters.AddWithValue("?", sqlId.Value);
                        updateLev.Parameters.AddWithValue("?", a110);
                        updateLev.ExecuteNonQuery();
                    }

                    // 3) Update Dokumenten for this supplier (V034 = 'L' + A110)
                    using (var updateDocs = new OleDbCommand(
                        "UPDATE Dokumenten SET vsoftKLID = ? WHERE V034 = ?",
                        accessConn))
                    {
                        updateDocs.Parameters.AddWithValue("?", sqlId.Value);
                        updateDocs.Parameters.AddWithValue("?", "L" + a110);
                        updateDocs.ExecuteNonQuery();
                    }
                }
            }
        }

        public void SuppliersAllSync(string marLocatie, string cnn)
        {
            // Load records from SQL Server into a DataTable
            DataTable sqlTable = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(cnn))
            {
                sqlConn.Open();
                string sqlQuery = "SELECT [Id], A110 FROM VsoftSuppliers"; // adjust column names accordingly
                using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd);
                    sqlAdapter.Fill(sqlTable);
                }
            }

            // Connect to Access and sync each record
            using (OleDbConnection accessConn = new OleDbConnection(marLocatie))
            {
                accessConn.Open();

                foreach (DataRow row in sqlTable.Rows)
                {
                    int sqlID = Convert.ToInt32(row["Id"]);
                    string A110 = (string)row["A110"]; // adjust as needed

                    // Check if this record already exists in the Access table
                    bool recordExists = false;
                    using (OleDbCommand checkCmd = new OleDbCommand("SELECT COUNT(*) FROM Leveranciers WHERE [A110] = ?", accessConn))
                    {
                        checkCmd.Parameters.AddWithValue("?", A110);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        recordExists = (count > 0);
                    }

                    if (!recordExists)
                    {
                        Console.WriteLine("Opzoeking vanuit SQLSERVER VsoftSuppliers:\nSleutel in tabel Leveranciers niet gevonden in marIntegraal mdv: " + A110);
                    }
                    else
                    {
                        // Update the record if other fields have changed
                        using (OleDbCommand updateCmd = new OleDbCommand("UPDATE Leveranciers SET sqlID = ? WHERE [A110] = ?", accessConn))
                        {
                            updateCmd.Parameters.AddWithValue("?", sqlID);
                            updateCmd.Parameters.AddWithValue("?", A110);
                            updateCmd.ExecuteNonQuery();
                        }
                        // TODO: sync VsoftSupplierId to Dokumenten table
                        bool docsExists = false;
                        // TODO: sync VsoftCustomerId to Dokumenten table
                        using (OleDbCommand checkCmd = new OleDbCommand("SELECT COUNT(*) FROM Dokumenten WHERE [V034] = ?", accessConn))
                        {
                            checkCmd.Parameters.AddWithValue("?", "L" + A110);
                            int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                            docsExists = (count > 0);
                        }
                        if (!docsExists)
                        {
                            Console.WriteLine("Geen documenten gevonden in marIntegraal mdv Dokumenten tabel voor " + A110);
                        }
                        else
                        {
                            // Update the record if other fields have changed
                            using (OleDbCommand updateCmd = new OleDbCommand("UPDATE Dokumenten SET vsoftKLID = ? WHERE [V034] = ?", accessConn))
                            {
                                updateCmd.Parameters.AddWithValue("?", sqlID);
                                updateCmd.Parameters.AddWithValue("?", "L" + A110);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Database sync completed successfully.");
        }
    }
}
