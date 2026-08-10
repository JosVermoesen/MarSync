using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;

namespace MarSync
{
    class DataCustomers
    {
        public void CustomersInsertAndUpdate(string marLocatie, string sqlNew, string sqlUpdate, string cnn)
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

                    List<VsoftCustomer> customersNew = new List<VsoftCustomer>();
                    List<VsoftCustomer> customersUpdate = new List<VsoftCustomer>();

                    // NEW customers
                    while (readerNew.Read())
                    {
                        customersNew.Add(new VsoftCustomer
                        {
                            A110 = readerNew["A110"].ToString().Trim(' '),
                            E072 = readerNew["E072"].ToString().Trim(' '),
                            G101 = readerNew["G101"].ToString().Trim(' '),
                            G102 = readerNew["G102"].ToString().Trim(' '),
                            G103 = readerNew["G103"].ToString().Trim(' '),
                            G104 = readerNew["G104"].ToString().Trim(' '),
                            G105 = readerNew["G105"].ToString().Trim(' '),
                            G106 = readerNew["G106"].ToString().Trim(' '),
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
                            V161 = readerNew["v161"].ToString().Trim(' '),
                            A161 = readerNew["A161"].ToString().Trim(' '),
                            V404 = readerNew["v404"].ToString().Trim(' '),
                            V151 = readerNew["v151"].ToString().Trim(' '),
                            V111 = readerNew["v111"].ToString().Trim(' '),
                            V254 = readerNew["v254"].ToString().Trim(' '),
                            V255 = readerNew["v255"].ToString().Trim(' '),
                            V256 = readerNew["v256"].ToString().Trim(' '),
                            A170 = readerNew["A170"].ToString().Trim(' '),
                            Vs04 = readerNew["vs04"].ToString().Trim(' '),
                            Vs05 = readerNew["vs05"].ToString().Trim(' '),
                            Vs06 = readerNew["vs06"].ToString().Trim(' '),
                            Vs07 = readerNew["vs07"].ToString().Trim(' '),
                            V225 = readerNew["v225"].ToString().Trim(' '),
                            V227 = readerNew["v227"].ToString().Trim(' '),
                            V247 = readerNew["v247"].ToString().Trim(' '),
                            A10a = readerNew["A10A"].ToString().Trim(' '),
                            Vs02 = readerNew["vs02"].ToString().Trim(' '),
                            V224 = readerNew["v224"].ToString().Trim(' '),
                            A123 = readerNew["A123"].ToString().Trim(' '),
                            A124 = readerNew["A124"].ToString().Trim(' '),
                            A121 = readerNew["A121"].ToString().Trim(' '),
                            A122 = readerNew["A122"].ToString().Trim(' '),
                            V259 = readerNew["v259"].ToString().Trim(' '),
                            V260 = readerNew["v260"].ToString().Trim(' '),
                            E070 = readerNew["e070"].ToString().Trim(' '),
                            E071 = readerNew["e071"].ToString().Trim(' '),
                            V252 = readerNew["v252"].ToString().Trim(' '),
                            A191 = readerNew["A191"].ToString().Trim(' '),
                            A192 = readerNew["A192"].ToString().Trim(' '),
                            A193 = readerNew["A193"].ToString().Trim(' '),
                            A194 = readerNew["A194"].ToString().Trim(' '),
                            A197 = readerNew["A197"].ToString().Trim(' '),
                            _510z = readerNew["510Z"].ToString().Trim(' '),
                            A130 = readerNew["A130"].ToString().Trim(' '),
                            V301 = readerNew["V301"].ToString().Trim(' '),
                            A102 = readerNew["A102"].ToString().Trim(' '),
                            A100 = readerNew["A100"].ToString().Trim(' '),
                            A101 = readerNew["A101"].ToString().Trim(' '),
                            V226 = readerNew["v226"].ToString().Trim(' '),
                            V243 = readerNew["v243"].ToString().Trim(' '),
                            V302 = readerNew["V302"].ToString().Trim(' '),
                            Vs01 = readerNew["vs01"].ToString().Trim(' '),
                            A125 = readerNew["A125"].ToString().Trim(' '),
                            A127 = readerNew["A127"].ToString().Trim(' '),
                            V002 = readerNew["v002"].ToString().Trim(' '),
                            V257 = readerNew["v257"].ToString().Trim(' '),
                            V258 = readerNew["v258"].ToString().Trim(' '),
                            V244 = readerNew["v244"].ToString().Trim(' '),
                            V251 = readerNew["v251"].ToString().Trim(' '),
                            A120 = readerNew["A120"].ToString().Trim(' '),
                            V201 = readerNew["v201"].ToString().Trim(' '),
                            V202 = readerNew["v202"].ToString().Trim(' '),
                            V203 = readerNew["v203"].ToString().Trim(' '),
                            V204 = readerNew["v204"].ToString().Trim(' '),
                            V205 = readerNew["v205"].ToString().Trim(' '),
                            V206 = readerNew["v206"].ToString().Trim(' '),
                            V207 = readerNew["v207"].ToString().Trim(' '),
                            V208 = readerNew["v208"].ToString().Trim(' '),
                            V209 = readerNew["v209"].ToString().Trim(' '),
                            V210 = readerNew["v210"].ToString().Trim(' '),
                            V211 = readerNew["v211"].ToString().Trim(' '),
                            V245 = readerNew["v245"].ToString().Trim(' '),
                            V246 = readerNew["v246"].ToString().Trim(' '),
                            V253 = readerNew["v253"].ToString().Trim(' '),
                            Uxxx = readerNew["uxxx"].ToString().Trim(' '),
                            V262 = readerNew["v262"].ToString().Trim(' '),
                            V263 = readerNew["v263"].ToString().Trim(' '),
                            V407 = readerNew["v407"].ToString().Trim(' ')
                        });
                    }

                    // INSERT batch
                    try
                    {
                        connection.Execute(
                            "spVsoftCustomers_InsertFull @A110, @E072, @G101, @G102, @G103, @G104, @G105, @G106, @A10c, @A104, @A105, @A106, @A107, @A108, @V149, @A109, @V150, @Vs03, @V161, @A161, @V404, @V151, @V111, @V254, @V255, @V256, @A170, @Vs04, @Vs05, @Vs06, @Vs07, @V225, @V227, @V247, @A10a, @Vs02, @V224, @A123, @A124, @A121, @A122, @V259, @V260, @E070, @E071, @V252, @A191, @A192, @A193, @A194, @A197, @_510z, @A130, @V301, @A102, @A100, @A101, @V226, @V243, @V302, @Vs01, @A125, @A127, @V002, @V257, @V258, @V244, @V251, @A120, @V201, @V202, @V203, @V204, @V205, @V206, @V207, @V208, @V209, @V210, @V211, @V245, @V246, @V253, @Uxxx, @V262, @V263, @V407",
                            customersNew);
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    // UPDATED customers
                    while (readerUpdate.Read())
                    {
                        customersUpdate.Add(new VsoftCustomer
                        {
                            A110 = readerUpdate["A110"].ToString().Trim(' '),
                            E072 = readerUpdate["E072"].ToString().Trim(' '),
                            G101 = readerUpdate["G101"].ToString().Trim(' '),
                            G102 = readerUpdate["G102"].ToString().Trim(' '),
                            G103 = readerUpdate["G103"].ToString().Trim(' '),
                            G104 = readerUpdate["G104"].ToString().Trim(' '),
                            G105 = readerUpdate["G105"].ToString().Trim(' '),
                            G106 = readerUpdate["G106"].ToString().Trim(' '),
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
                            V161 = readerUpdate["v161"].ToString().Trim(' '),
                            A161 = readerUpdate["A161"].ToString().Trim(' '),
                            V404 = readerUpdate["v404"].ToString().Trim(' '),
                            V151 = readerUpdate["v151"].ToString().Trim(' '),
                            V111 = readerUpdate["v111"].ToString().Trim(' '),
                            V254 = readerUpdate["v254"].ToString().Trim(' '),
                            V255 = readerUpdate["v255"].ToString().Trim(' '),
                            V256 = readerUpdate["v256"].ToString().Trim(' '),
                            A170 = readerUpdate["A170"].ToString().Trim(' '),
                            Vs04 = readerUpdate["vs04"].ToString().Trim(' '),
                            Vs05 = readerUpdate["vs05"].ToString().Trim(' '),
                            Vs06 = readerUpdate["vs06"].ToString().Trim(' '),
                            Vs07 = readerUpdate["vs07"].ToString().Trim(' '),
                            V225 = readerUpdate["v225"].ToString().Trim(' '),
                            V227 = readerUpdate["v227"].ToString().Trim(' '),
                            V247 = readerUpdate["v247"].ToString().Trim(' '),
                            A10a = readerUpdate["A10A"].ToString().Trim(' '),
                            Vs02 = readerUpdate["vs02"].ToString().Trim(' '),
                            V224 = readerUpdate["v224"].ToString().Trim(' '),
                            A123 = readerUpdate["A123"].ToString().Trim(' '),
                            A124 = readerUpdate["A124"].ToString().Trim(' '),
                            A121 = readerUpdate["A121"].ToString().Trim(' '),
                            A122 = readerUpdate["A122"].ToString().Trim(' '),
                            V259 = readerUpdate["v259"].ToString().Trim(' '),
                            V260 = readerUpdate["v260"].ToString().Trim(' '),
                            E070 = readerUpdate["e070"].ToString().Trim(' '),
                            E071 = readerUpdate["e071"].ToString().Trim(' '),
                            V252 = readerUpdate["v252"].ToString().Trim(' '),
                            A191 = readerUpdate["A191"].ToString().Trim(' '),
                            A192 = readerUpdate["A192"].ToString().Trim(' '),
                            A193 = readerUpdate["A193"].ToString().Trim(' '),
                            A194 = readerUpdate["A194"].ToString().Trim(' '),
                            A197 = readerUpdate["A197"].ToString().Trim(' '),
                            _510z = readerUpdate["510Z"].ToString().Trim(' '),
                            A130 = readerUpdate["A130"].ToString().Trim(' '),
                            V301 = readerUpdate["V301"].ToString().Trim(' '),
                            A102 = readerUpdate["A102"].ToString().Trim(' '),
                            A100 = readerUpdate["A100"].ToString().Trim(' '),
                            A101 = readerUpdate["A101"].ToString().Trim(' '),
                            V226 = readerUpdate["v226"].ToString().Trim(' '),
                            V243 = readerUpdate["v243"].ToString().Trim(' '),
                            V302 = readerUpdate["V302"].ToString().Trim(' '),
                            Vs01 = readerUpdate["vs01"].ToString().Trim(' '),
                            A125 = readerUpdate["A125"].ToString().Trim(' '),
                            A127 = readerUpdate["A127"].ToString().Trim(' '),
                            V002 = readerUpdate["v002"].ToString().Trim(' '),
                            V257 = readerUpdate["v257"].ToString().Trim(' '),
                            V258 = readerUpdate["v258"].ToString().Trim(' '),
                            V244 = readerUpdate["v244"].ToString().Trim(' '),
                            V251 = readerUpdate["v251"].ToString().Trim(' '),
                            A120 = readerUpdate["A120"].ToString().Trim(' '),
                            V201 = readerUpdate["v201"].ToString().Trim(' '),
                            V202 = readerUpdate["v202"].ToString().Trim(' '),
                            V203 = readerUpdate["v203"].ToString().Trim(' '),
                            V204 = readerUpdate["v204"].ToString().Trim(' '),
                            V205 = readerUpdate["v205"].ToString().Trim(' '),
                            V206 = readerUpdate["v206"].ToString().Trim(' '),
                            V207 = readerUpdate["v207"].ToString().Trim(' '),
                            V208 = readerUpdate["v208"].ToString().Trim(' '),
                            V209 = readerUpdate["v209"].ToString().Trim(' '),
                            V210 = readerUpdate["v210"].ToString().Trim(' '),
                            V211 = readerUpdate["v211"].ToString().Trim(' '),
                            V245 = readerUpdate["v245"].ToString().Trim(' '),
                            V246 = readerUpdate["v246"].ToString().Trim(' '),
                            V253 = readerUpdate["v253"].ToString().Trim(' '),
                            Uxxx = readerUpdate["uxxx"].ToString().Trim(' '),
                            V262 = readerUpdate["v262"].ToString().Trim(' '),
                            V263 = readerUpdate["v263"].ToString().Trim(' '),
                            V407 = readerUpdate["v407"].ToString().Trim(' ')
                        });
                    }

                    // UPDATE batch
                    try
                    {
                        connection.Execute(
                            "spVsoftCustomers_UpdateFull @A110, @E072, @G101, @G102, @G103, @G104, @G105, @G106, @A10c, @A104, @A105, @A106, @A107, @A108, @V149, @A109, @V150, @Vs03, @V161, @A161, @V404, @V151, @V111, @V254, @V255, @V256, @A170, @Vs04, @Vs05, @Vs06, @Vs07, @V225, @V227, @V247, @A10a, @Vs02, @V224, @A123, @A124, @A121, @A122, @V259, @V260, @E070, @E071, @V252, @A191, @A192, @A193, @A194, @A197, @_510z, @A130, @V301, @A102, @A100, @A101, @V226, @V243, @V302, @Vs01, @A125, @A127, @V002, @V257, @V258, @V244, @V251, @A120, @V201, @V202, @V203, @V204, @V205, @V206, @V207, @V208, @V209, @V210, @V211, @V245, @V246, @V253, @Uxxx, @V262, @V263, @V407",
                            customersUpdate);
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    // Now sync Access only for the customers we just handled
                    if (customersNew.Count > 0 || customersUpdate.Count > 0)
                    {
                        Console.WriteLine("Aantal nieuwe klanten: " + customersNew.Count +
                                          ", aantal updates: " + customersUpdate.Count);
                        SyncAccessForCustomers(marLocatie, cnn, customersNew, customersUpdate);
                    }

                    readerNew.Close();
                    readerUpdate.Close();
                    cnnMarJet.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void SyncAccessForCustomers(
            string marLocatie,
            string cnn,
            System.Collections.Generic.IEnumerable<VsoftCustomer> customersNew,
            System.Collections.Generic.IEnumerable<VsoftCustomer> customersUpdate)
        {
            // Collect distinct A110 keys for this batch
            var keys = new System.Collections.Generic.HashSet<string>(
                customersNew.Select(c => c.A110)
                            .Concat(customersUpdate.Select(c => c.A110))
                            .Where(k => !string.IsNullOrWhiteSpace(k)));

            if (keys.Count == 0)
            {
                return;
            }

            using (var sqlConn = new System.Data.SqlClient.SqlConnection(cnn))
            using (var accessConn = new OleDbConnection(marLocatie))
            {
                sqlConn.Open();
                accessConn.Open();

                foreach (string a110 in keys)
                {
                    int? sqlId = null;

                    // 1) Get SQL Id for this customer (from VsoftCustomers)
                    using (var cmd = new System.Data.SqlClient.SqlCommand(
                        "SELECT Id FROM VsoftCustomers WHERE A110 = @A110", sqlConn))
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
                        Console.WriteLine("Geen VsoftCustomers.Id gevonden voor A110=" + a110);
                        continue;
                    }

                    // 2) Update Klanten row for this A110
                    using (var updateKl = new OleDbCommand(
                        "UPDATE Klanten SET sqlID = ?, dnnSyncAlready = True, dnnSync = True WHERE A110 = ?",
                        accessConn))
                    {
                        updateKl.Parameters.AddWithValue("?", sqlId.Value);
                        updateKl.Parameters.AddWithValue("?", a110);
                        updateKl.ExecuteNonQuery();
                    }

                    // 3) Update Dokumenten (customer documents: V034 = 'K' + A110)
                    using (var updateDocs = new OleDbCommand(
                        "UPDATE Dokumenten SET vsoftKLID = ? WHERE V034 = ?",
                        accessConn))
                    {
                        updateDocs.Parameters.AddWithValue("?", sqlId.Value);
                        updateDocs.Parameters.AddWithValue("?", "K" + a110);
                        updateDocs.ExecuteNonQuery();
                    }

                    // 4) Update Polissen for this A110
                    using (var updatePol = new OleDbCommand(
                        "UPDATE Polissen SET vsoftKID = ? WHERE A110 = ?",
                        accessConn))
                    {
                        updatePol.Parameters.AddWithValue("?", sqlId.Value);
                        updatePol.Parameters.AddWithValue("?", a110);
                        updatePol.ExecuteNonQuery();
                    }
                }
            }
        }

        public void CustomersAllSync(string marLocatie, string cnn)
        {
            // Load records from SQL Server into a DataTable
            DataTable sqlTable = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(cnn))
            {
                sqlConn.Open();
                string sqlQuery = "SELECT [Id], [A110] FROM VsoftCustomers"; // adjust column names accordingly
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
                    using (OleDbCommand checkCmd = new OleDbCommand("SELECT COUNT(*) FROM Klanten WHERE [A110] = ?", accessConn))
                    {
                        checkCmd.Parameters.AddWithValue("?", A110);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        recordExists = (count > 0);
                    }

                    if (!recordExists)
                    {
                        Console.WriteLine("Opzoeking vanuit SQLSERVER VsoftCustomers:\nSleutel in tabel Klanten niet gevonden in marIntegraal mdv:  " + A110);
                    }
                    else
                    {
                        // Update the record if other fields have changed
                        using (OleDbCommand updateCmd = new OleDbCommand("UPDATE Klanten SET sqlID = ? WHERE [A110] = ?", accessConn))
                        {
                            updateCmd.Parameters.AddWithValue("?", sqlID);
                            updateCmd.Parameters.AddWithValue("?", A110);
                            updateCmd.ExecuteNonQuery();
                        }

                        bool docsExists = false;
                        // TODO: sync VsoftCustomerId to Dokumenten table
                        using (OleDbCommand checkCmd = new OleDbCommand("SELECT COUNT(*) FROM Dokumenten WHERE [V034] = ?", accessConn))
                        {
                            checkCmd.Parameters.AddWithValue("?", "K" + A110);
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
                                updateCmd.Parameters.AddWithValue("?", "K" + A110);
                                updateCmd.ExecuteNonQuery();
                            }

                            bool contractsExists = false;
                            // TODO: sync VsoftCustomerId to Dokumenten table
                            using (OleDbCommand checkCmd = new OleDbCommand("SELECT COUNT(*) FROM Polissen WHERE [A110] = ?", accessConn))
                            {
                                checkCmd.Parameters.AddWithValue("?", A110);
                                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                                contractsExists = (count > 0);
                            }
                            if (!contractsExists)
                            {
                                Console.WriteLine("Geen Polissen gevonden in marIntegraal mdv Polissen tabel voor " + A110);
                            }
                            else
                            {
                                // Update the record if other fields have changed
                                using (OleDbCommand updateCmd = new OleDbCommand("UPDATE Polissen SET vsoftKID = ? WHERE [A110] = ?", accessConn))
                                {
                                    updateCmd.Parameters.AddWithValue("?", sqlID);
                                    updateCmd.Parameters.AddWithValue("?", A110);
                                    updateCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Database sync completed successfully.");
        }
    }
}

