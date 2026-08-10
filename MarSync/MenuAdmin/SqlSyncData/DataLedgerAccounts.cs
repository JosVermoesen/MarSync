using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;

namespace MarSync
{
    class DataLedgerAccounts
    {
        public void LedgerAccountsInsertAndUpdate(string marLocatie, string sqlNew, string sqlUpdate, string cnn)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(cnn))
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

                    List<VsoftLedgerAccount> ledgerAccountsNew = new List<VsoftLedgerAccount>();
                    List<VsoftLedgerAccount> ledgerAccountsUpdate = new List<VsoftLedgerAccount>();

                    while (readerNew.Read())
                    {
                        // dece022
                        decimal iodece022;
                        if (!Convert.IsDBNull(readerNew["dece022"]))
                        {
                            iodece022 = (decimal)readerNew["dece022"];
                        }
                        else
                        {
                            iodece022 = 0;
                        }

                        // dece023
                        decimal iodece023;
                        if (!Convert.IsDBNull(readerNew["dece023"]))
                        {
                            iodece023 = (decimal)readerNew["dece023"];
                        }
                        else
                        {
                            iodece023 = 0;
                        }

                        // dece024
                        decimal iodece024;
                        if (!Convert.IsDBNull(readerNew["dece024"]))
                        {
                            iodece024 = (decimal)readerNew["dece024"];
                        }
                        else
                        {
                            iodece024 = 0;
                        }

                        // dece025
                        decimal iodece025;
                        if (!Convert.IsDBNull(readerNew["dece025"]))
                        {
                            iodece025 = (decimal)readerNew["dece025"];
                        }
                        else
                        {
                            iodece025 = 0;
                        }

                        // dece026
                        decimal iodece026;
                        if (!Convert.IsDBNull(readerNew["dece026"]))
                        {
                            iodece026 = (decimal)readerNew["dece026"];
                        }
                        else
                        {
                            iodece026 = 0;
                        }

                        // dece027
                        decimal iodece027;
                        if (!Convert.IsDBNull(readerNew["dece027"]))
                        {
                            iodece027 = (decimal)readerNew["dece027"];
                        }
                        else
                        {
                            iodece027 = 0;
                        }

                        // dece028
                        decimal iodece028;
                        if (!Convert.IsDBNull(readerNew["dece028"]))
                        {
                            iodece028 = (decimal)readerNew["dece028"];
                        }
                        else
                        {
                            iodece028 = 0;
                        }

                        // dece029
                        decimal iodece029;
                        if (!Convert.IsDBNull(readerNew["dece029"]))
                        {
                            iodece029 = (decimal)readerNew["dece029"];
                        }
                        else
                        {
                            iodece029 = 0;
                        }

                        // dece030
                        decimal iodece030;
                        if (!Convert.IsDBNull(readerNew["dece030"]))
                        {
                            iodece030 = (decimal)readerNew["dece030"];
                        }
                        else
                        {
                            iodece030 = 0;
                        }

                        // dece031
                        decimal iodece031;
                        if (!Convert.IsDBNull(readerNew["dece031"]))
                        {
                            iodece031 = (decimal)readerNew["dece031"];
                        }
                        else
                        {
                            iodece031 = 0;
                        }

                        ledgerAccountsNew.Add(new VsoftLedgerAccount
                        {
                            V019 = readerNew["v019"].ToString().Trim(' '),
                            V020 = readerNew["v020"].ToString().Trim(' '),
                            V021 = readerNew["v021"].ToString().Trim(' '),
                            V032 = readerNew["v032"].ToString().Trim(' '),
                            V216 = readerNew["v216"].ToString().Trim(' '),

                            Dece022 = iodece022,
                            Dece023 = iodece023,
                            Dece024 = iodece024,
                            Dece025 = iodece025,
                            Dece026 = iodece026,
                            Dece027 = iodece027,
                            Dece028 = iodece028,
                            Dece029 = iodece029,
                            Dece030 = iodece030,
                            Dece031 = iodece031
                        });
                    }

                    try
                    {
                        connection.Execute(
                            "spVsoftLedgerAccounts_InsertFull @V019, @V020, @Dece022, @Dece023, @Dece024, @Dece025, @Dece026, @Dece027, @Dece028, @Dece029, @Dece030, @Dece031, @V021, @V032, @V216",
                            ledgerAccountsNew);
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    readerNew.Close();

                    while (readerUpdate.Read())
                    {
                        // dece022
                        decimal iodece022;
                        if (!Convert.IsDBNull(readerUpdate["dece022"]))
                        {
                            iodece022 = (decimal)readerUpdate["dece022"];
                        }
                        else
                        {
                            iodece022 = 0;
                        }

                        // dece023
                        decimal iodece023;
                        if (!Convert.IsDBNull(readerUpdate["dece023"]))
                        {
                            iodece023 = (decimal)readerUpdate["dece023"];
                        }
                        else
                        {
                            iodece023 = 0;
                        }

                        // dece024
                        decimal iodece024;
                        if (!Convert.IsDBNull(readerUpdate["dece024"]))
                        {
                            iodece024 = (decimal)readerUpdate["dece024"];
                        }
                        else
                        {
                            iodece024 = 0;
                        }

                        // dece025
                        decimal iodece025;
                        if (!Convert.IsDBNull(readerUpdate["dece025"]))
                        {
                            iodece025 = (decimal)readerUpdate["dece025"];
                        }
                        else
                        {
                            iodece025 = 0;
                        }

                        // dece026
                        decimal iodece026;
                        if (!Convert.IsDBNull(readerUpdate["dece026"]))
                        {
                            iodece026 = (decimal)readerUpdate["dece026"];
                        }
                        else
                        {
                            iodece026 = 0;
                        }

                        // dece027
                        decimal iodece027;
                        if (!Convert.IsDBNull(readerUpdate["dece027"]))
                        {
                            iodece027 = (decimal)readerUpdate["dece027"];
                        }
                        else
                        {
                            iodece027 = 0;
                        }

                        // dece028
                        decimal iodece028;
                        if (!Convert.IsDBNull(readerUpdate["dece028"]))
                        {
                            iodece028 = (decimal)readerUpdate["dece028"];
                        }
                        else
                        {
                            iodece028 = 0;
                        }

                        // dece029
                        decimal iodece029;
                        if (!Convert.IsDBNull(readerUpdate["dece029"]))
                        {
                            iodece029 = (decimal)readerUpdate["dece029"];
                        }
                        else
                        {
                            iodece029 = 0;
                        }

                        // dece030
                        decimal iodece030;
                        if (!Convert.IsDBNull(readerUpdate["dece030"]))
                        {
                            iodece030 = (decimal)readerUpdate["dece030"];
                        }
                        else
                        {
                            iodece030 = 0;
                        }

                        // dece031
                        decimal iodece031;
                        if (!Convert.IsDBNull(readerUpdate["dece031"]))
                        {
                            iodece031 = (decimal)readerUpdate["dece031"];
                        }
                        else
                        {
                            iodece031 = 0;
                        }

                        ledgerAccountsUpdate.Add(new VsoftLedgerAccount
                        {
                            V019 = readerUpdate["v019"].ToString().Trim(' '),
                            V020 = readerUpdate["v020"].ToString().Trim(' '),
                            V021 = readerUpdate["v021"].ToString().Trim(' '),
                            V032 = readerUpdate["v032"].ToString().Trim(' '),
                            V216 = readerUpdate["v216"].ToString().Trim(' '),

                            Dece022 = iodece022,
                            Dece023 = iodece023,
                            Dece024 = iodece024,
                            Dece025 = iodece025,
                            Dece026 = iodece026,
                            Dece027 = iodece027,
                            Dece028 = iodece028,
                            Dece029 = iodece029,
                            Dece030 = iodece030,
                            Dece031 = iodece031
                        });
                    }

                    try
                    {
                        connection.Execute(
                            "spVsoftLedgerAccounts_UpdateFull @V019, @V020, @Dece022, @Dece023, @Dece024, @Dece025, @Dece026, @Dece027, @Dece028, @Dece029, @Dece030, @Dece031, @V021, @V032, @V216",
                            ledgerAccountsUpdate);
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    readerUpdate.Close();

                    // Per-batch resync for just these ledger accounts
                    if (ledgerAccountsNew.Count > 0 || ledgerAccountsUpdate.Count > 0)
                    {
                        Console.WriteLine(
                            "Aantal nieuwe rekeningen: " + ledgerAccountsNew.Count +
                            ", aantal updates: " + ledgerAccountsUpdate.Count);
                        SyncAccessForLedgerAccounts(marLocatie, cnn, ledgerAccountsNew, ledgerAccountsUpdate);
                    }

                    cnnMarJet.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void LedgerAccountsAllSync(string marLocatie, string cnn)
        {
            // Load records from SQL Server into a DataTable
            DataTable sqlTable = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(cnn))
            {
                sqlConn.Open();
                string sqlQuery = "SELECT [Id], V019 FROM VsoftLedgerAccounts"; // adjust column names accordingly
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
                    string V019 = (string)row["V019"]; // adjust as needed

                    // Check if this record already exists in the Access table
                    bool recordExists = false;
                    using (OleDbCommand checkCmd = new OleDbCommand("SELECT COUNT(*) FROM Rekeningen WHERE [V019] = ?", accessConn))
                    {
                        checkCmd.Parameters.AddWithValue("?", V019);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        recordExists = (count > 0);
                    }

                    if (!recordExists)
                    {
                        Console.WriteLine("Opzoeking vanuit SQLSERVER VsoftLedgerAccounts:\nSleutel in tabel Rekeningen niet gevonden in marIntegraal mdv: " + V019);
                    }
                    else
                    {
                        // Update the record if other fields have changed
                        using (OleDbCommand updateCmd = new OleDbCommand("UPDATE Rekeningen SET sqlID = ? WHERE [V019] = ?", accessConn))
                        {
                            updateCmd.Parameters.AddWithValue("?", sqlID);
                            updateCmd.Parameters.AddWithValue("?", V019);
                            updateCmd.ExecuteNonQuery();
                        }
                        // TODO: sync VsoftSupplierId to Dokumenten table
                        bool docsExists = false;
                        // TODO: sync VsoftCustomerId to Dokumenten table
                        using (OleDbCommand checkCmd = new OleDbCommand("SELECT COUNT(*) FROM Journalen WHERE [V019] = ?", accessConn))
                        {
                            checkCmd.Parameters.AddWithValue("?", V019);
                            int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                            docsExists = (count > 0);
                        }
                        if (!docsExists)
                        {
                            Console.WriteLine("Geen journalen gevonden in marIntegraal mdv Journalen tabel voor " + V019);
                        }
                        else
                        {
                            // Update the record if other fields have changed
                            using (OleDbCommand updateCmd = new OleDbCommand("UPDATE Journalen SET vsoftRID = ? WHERE [V019] = ?", accessConn))
                            {
                                updateCmd.Parameters.AddWithValue("?", sqlID);
                                updateCmd.Parameters.AddWithValue("?", V019);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Database sync completed successfully.");
        }

        private void SyncAccessForLedgerAccounts(
            string marLocatie,
            string cnn,
            IEnumerable<VsoftLedgerAccount> ledgerAccountsNew,
            IEnumerable<VsoftLedgerAccount> ledgerAccountsUpdate)
        {
            var keys = new HashSet<string>(
                ledgerAccountsNew.Select(a => a.V019)
                                 .Concat(ledgerAccountsUpdate.Select(a => a.V019))
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

                foreach (string v019 in keys)
                {
                    int? sqlId = null;

                    // 1) Get SQL Id for this ledger account
                    using (var cmd = new SqlCommand(
                        "SELECT Id FROM VsoftLedgerAccounts WHERE V019 = @V019", sqlConn))
                    {
                        cmd.Parameters.AddWithValue("@V019", v019);
                        object obj = cmd.ExecuteScalar();
                        if (obj != null && obj != DBNull.Value)
                        {
                            sqlId = Convert.ToInt32(obj);
                        }
                    }

                    if (!sqlId.HasValue)
                    {
                        Console.WriteLine("Geen VsoftLedgerAccounts.Id gevonden voor V019=" + v019);
                        continue;
                    }

                    // 2) Update Rekeningen for this ledger account
                    using (var updateRec = new OleDbCommand(
                        "UPDATE Rekeningen SET sqlID = ?, dnnSyncAlready = True, dnnSync = True WHERE V019 = ?",
                        accessConn))
                    {
                        updateRec.Parameters.AddWithValue("?", sqlId.Value);
                        updateRec.Parameters.AddWithValue("?", v019);
                        updateRec.ExecuteNonQuery();
                    }

                    // 3) Update Journalen for this ledger account
                    using (var updateJ = new OleDbCommand(
                        "UPDATE Journalen SET vsoftRID = ? WHERE V019 = ?",
                        accessConn))
                    {
                        updateJ.Parameters.AddWithValue("?", sqlId.Value);
                        updateJ.Parameters.AddWithValue("?", v019);
                        updateJ.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
