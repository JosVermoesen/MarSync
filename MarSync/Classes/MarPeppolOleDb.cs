using System;
using System.Data;
using System.Data.OleDb;

namespace MarSync.Classes
{
    /// <summary>
    /// OleDb-based replacement for the ADODB/Recordset DB access used by FormSyncPeppol.
    /// </summary>
    public class MarPeppolOleDbRepository
    {
        //private readonly DataGridView _notificationsGrid;
        //private readonly ToolStripStatusLabel _statusLabel;

        //public MarPeppolOleDbRepository(
        //    DataGridView notificationsGrid,
        //    ToolStripStatusLabel statusLabel)
        //{
        //    _notificationsGrid = notificationsGrid;
        //    _statusLabel = statusLabel;
        //}

        private string GetConnectionString()
        {
            return SharedGlobals.DbJetProvider +
                   SharedGlobals.MimDataLocation +
                   SharedGlobals.MarntMdvLocation;
        }

        #region Sellers

        public string GetSellersDocumentSentResult(
            string document,
            bool checkAcceptedOrRejected)
        {
            if (string.IsNullOrEmpty(document) || document.Length < 11)
                return string.Empty;

            string docId = document.Substring(0, 11);
            const string sql =
                "SELECT v033, v405, v406, dnnSync " +
                "FROM Dokumenten " +
                "WHERE v033 = ?";

            using (var connection = new OleDbConnection(GetConnectionString()))
            using (var command = new OleDbCommand(sql, connection))
            {
                command.Parameters.Add(new OleDbParameter
                {
                    OleDbType = OleDbType.VarChar,
                    Value = docId
                });

                try
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader == null || !reader.Read())
                            return string.Empty;

                        string fieldToCheck = checkAcceptedOrRejected ? "v406" : "v405";
                        int ordinal = reader.GetOrdinal(fieldToCheck);
                        string fieldValue = reader.IsDBNull(ordinal)
                            ? string.Empty
                            : Convert.ToString(reader.GetValue(ordinal));

                        if (!string.IsNullOrEmpty(fieldValue) &&
                            (fieldValue.Contains("VALIDATION_ERROR") ||
                             fieldValue.Contains("SCHEDULED")))
                        {
                            // Clear transient/invalid states in DB and return empty
                            ClearFieldAndSync(docId, fieldToCheck);
                            fieldValue = string.Empty;
                        }

                        return fieldValue;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in GetSellersDocumentSentResult: " + ex);
                    throw;
                }
            }
        }

        public bool GetSellersPaidResult(string document)
        {
            if (string.IsNullOrEmpty(document) || document.Length < 11)
                return false;

            string docId = document.Substring(0, 11);
            const string sql =
                "SELECT v033, v037, v249 " +
                "FROM Dokumenten " +
                "WHERE v033 = ?";

            using (var connection = new OleDbConnection(GetConnectionString()))
            using (var command = new OleDbCommand(sql, connection))
            {
                command.Parameters.Add(new OleDbParameter
                {
                    OleDbType = OleDbType.VarChar,
                    Value = docId
                });

                try
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader == null || !reader.Read())
                            return false;

                        decimal openAmount = 0m;
                        decimal paidAmount = 0m;

                        int v249Ordinal = reader.GetOrdinal("v249");
                        if (!reader.IsDBNull(v249Ordinal))
                        {
                            openAmount = Convert.ToDecimal(reader.GetValue(v249Ordinal));
                        }

                        int v037Ordinal = reader.GetOrdinal("v037");
                        if (!reader.IsDBNull(v037Ordinal))
                        {
                            paidAmount = Convert.ToDecimal(reader.GetValue(v037Ordinal));
                        }

                        return openAmount - paidAmount == 0m;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in GetSellersPaidResult: " + ex);
                    throw;
                }
            }
        }

        private void ClearFieldAndSync(string docId, string fieldToCheck)
        {
            // simple, same as your existing OleDb helper example
            string sql =
                "UPDATE Dokumenten " +
                $"SET {fieldToCheck} = ?, dnnSync = ? " +
                "WHERE v033 = ?";

            using (var connection = new OleDbConnection(GetConnectionString()))
            using (var command = new OleDbCommand(sql, connection))
            {
                command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.VarChar, Value = "" });
                command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.VarChar, Value = "False" });
                command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.VarChar, Value = docId });

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // match original: ignore update errors, just log
                    Console.WriteLine("Error clearing field in ClearFieldAndSync: " + ex);
                }
            }
        }

        #endregion

        #region Buyers

        public string GetBuyersBookStatus(string sellerDocument)
        {
            if (string.IsNullOrEmpty(sellerDocument))
                return "Nog in te boeken";

            string sql;
            OleDbParameter[] parameters;

            if (sellerDocument.Contains("|"))
            {
                var parts = sellerDocument.Split('|');
                if (parts.Length != 3)
                    return "Nog in te boeken";

                string companyId = parts[0];
                string documentId = parts[1];
                string paymentId = parts[2];

                if (companyId.StartsWith("BE", StringComparison.OrdinalIgnoreCase) &&
                    companyId.Length > 2)
                {
                    companyId = companyId.Substring(2);
                }

                if (string.IsNullOrEmpty(paymentId))
                {
                    paymentId = SanitizePaymentRef(documentId);
                }

                sql =
                    "SELECT " +
                    "Leveranciers.A110, Leveranciers.v404, " +
                    "Dokumenten.v409, Dokumenten.v034, Dokumenten.v035, Dokumenten.v036, " +
                    "Dokumenten.v037, Dokumenten.v039, Dokumenten.v249 " +
                    "FROM Leveranciers, Dokumenten " +
                    "WHERE Leveranciers.v404 = ? " +
                    "AND Dokumenten.v034 = 'L' + Leveranciers.A110 " +
                    "AND Dokumenten.v409 = ? " +
                    "AND Dokumenten.v039 = ?";

                parameters = new[]
                {
                    new OleDbParameter { OleDbType = OleDbType.VarChar, Value = companyId },
                    new OleDbParameter { OleDbType = OleDbType.VarChar, Value = documentId },
                    new OleDbParameter { OleDbType = OleDbType.VarChar, Value = paymentId }
                };
            }
            else
            {
                sql =
                    "SELECT v037, v039, v249, v409 " +
                    "FROM Dokumenten " +
                    "WHERE v409 = ?";

                parameters = new[]
                {
                    new OleDbParameter { OleDbType = OleDbType.VarChar, Value = sellerDocument }
                };
            }

            using (var connection = new OleDbConnection(GetConnectionString()))
            using (var command = new OleDbCommand(sql, connection))
            {
                command.Parameters.AddRange(parameters);

                try
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader == null || !reader.Read())
                            return "Nog in te boeken";

                        decimal openAmount = 0m;
                        decimal paidAmount = 0m;

                        int v249Ordinal = reader.GetOrdinal("v249");
                        if (!reader.IsDBNull(v249Ordinal))
                        {
                            openAmount = Convert.ToDecimal(reader.GetValue(v249Ordinal));
                        }

                        int v037Ordinal = reader.GetOrdinal("v037");
                        if (!reader.IsDBNull(v037Ordinal))
                        {
                            paidAmount = Convert.ToDecimal(reader.GetValue(v037Ordinal));
                        }

                        if (openAmount - paidAmount == 0m)
                            return "Volledig Betaald";

                        if (paidAmount != 0m)
                            return "Deels Betaald";

                        return "Ingeboekt, nog te betalen";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in GetBuyersBookStatus: " + ex);
                    return "Nog in te boeken";
                }
            }
        }

        public bool SetBuyersBookStatus(
            string sellerDocument,
            string jsonToSave,
            int asReceived)
        {
            if (string.IsNullOrEmpty(sellerDocument))
                return false;

            // 1) Resolve v033 for this buyer document
            string selectSql;
            OleDbParameter[] selectParams;

            if (sellerDocument.Contains("|"))
            {
                var parts = sellerDocument.Split('|');
                if (parts.Length != 3)
                    return false;

                string companyId = parts[0];
                string documentId = parts[1];
                string paymentId = parts[2];

                if (companyId.StartsWith("BE", StringComparison.OrdinalIgnoreCase) &&
                    companyId.Length > 2)
                {
                    companyId = companyId.Substring(2);
                }

                if (string.IsNullOrEmpty(paymentId))
                {
                    paymentId = SanitizePaymentRef(documentId);
                }

                selectSql =
                    "SELECT " +
                    "Leveranciers.A110, Leveranciers.v404, " +
                    "Dokumenten.v409, Dokumenten.v033, Dokumenten.v034, Dokumenten.v035, " +
                    "Dokumenten.v036, Dokumenten.v037, Dokumenten.v039, Dokumenten.v249 " +
                    "FROM Leveranciers, Dokumenten " +
                    "WHERE Leveranciers.v404 = ? " +
                    "AND Dokumenten.v034 = 'L' + Leveranciers.A110 " +
                    "AND Dokumenten.v409 = ? " +
                    "AND Dokumenten.v039 = ?";

                selectParams = new[]
                {
                    new OleDbParameter { OleDbType = OleDbType.VarChar, Value = companyId },
                    new OleDbParameter { OleDbType = OleDbType.VarChar, Value = documentId },
                    new OleDbParameter { OleDbType = OleDbType.VarChar, Value = paymentId }
                };
            }
            else
            {
                selectSql =
                    "SELECT v033, v037, v039, v249, v409 " +
                    "FROM Dokumenten " +
                    "WHERE v409 = ?";

                selectParams = new[]
                {
                    new OleDbParameter { OleDbType = OleDbType.VarChar, Value = sellerDocument }
                };
            }

            using (var connection = new OleDbConnection(GetConnectionString()))
            using (var selectCmd = new OleDbCommand(selectSql, connection))
            {
                selectCmd.Parameters.AddRange(selectParams);

                try
                {
                    connection.Open();

                    string marDocumentId;
                    using (var reader = selectCmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader == null || !reader.Read())
                            return false;

                        int v033Ordinal = reader.GetOrdinal("v033");
                        marDocumentId = Convert.ToString(reader.GetValue(v033Ordinal));
                    }

                    if (string.IsNullOrEmpty(marDocumentId))
                        return false;

                    // 2) Update by v033
                    string dbField;
                    if (asReceived == 1)
                    {
                        dbField = "v408"; // responses JSON result
                    }
                    else if (asReceived == 0)
                    {
                        dbField = "v406"; // server received JSON result
                    }
                    else
                    {
                        return false;
                    }

                    string updateSql =
                        "UPDATE Dokumenten " +
                        $"SET {dbField} = ?, dnnSync = ? " +
                        "WHERE v033 = ?";

                    using (var updateCmd = new OleDbCommand(updateSql, connection))
                    {
                        updateCmd.Parameters.Add(new OleDbParameter
                        {
                            OleDbType = OleDbType.LongVarChar,
                            Value = (object)jsonToSave ?? DBNull.Value
                        });
                        updateCmd.Parameters.Add(new OleDbParameter
                        {
                            OleDbType = OleDbType.VarChar,
                            Value = "False"
                        });
                        updateCmd.Parameters.Add(new OleDbParameter
                        {
                            OleDbType = OleDbType.VarChar,
                            Value = marDocumentId
                        });

                        int rows = updateCmd.ExecuteNonQuery();
                        return rows == 1;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        private static string SanitizePaymentRef(string paymentRef)
        {
            // same as existing helper in FormSyncPeppol
            char[] invalidChars = { '+', '/' };
            foreach (char c in invalidChars)
            {
                paymentRef = paymentRef.Replace(c.ToString(), "");
            }
            return paymentRef.Trim();
        }

        #endregion

        #region Document results

        public bool SetDocumentResult(
            string document,
            string jsonResult,
            int toComplete)
        {
            if (string.IsNullOrEmpty(document) || document.Length < 11)
                return false;

            string docId = document.Substring(0, 11);
            const string selectSql =
                "SELECT v033 FROM Dokumenten WHERE v033 = ?";

            using (var connection = new OleDbConnection(GetConnectionString()))
            using (var selectCmd = new OleDbCommand(selectSql, connection))
            {
                selectCmd.Parameters.Add(new OleDbParameter
                {
                    OleDbType = OleDbType.VarChar,
                    Value = docId
                });

                try
                {
                    connection.Open();

                    object exists = selectCmd.ExecuteScalar();
                    if (exists == null || exists == DBNull.Value)
                        return false;

                    string dbField;
                    if (toComplete == 1)
                    {
                        dbField = "v406"; // document sent JSON result
                    }
                    else if (toComplete == 0)
                    {
                        dbField = "v405"; // server/client accepted JSON result
                    }
                    else if (toComplete == 2)
                    {
                        dbField = "v408"; // responses JSON result
                    }
                    else
                    {
                        return false;
                    }

                    string updateSql =
                        "UPDATE Dokumenten " +
                        $"SET {dbField} = ?, dnnSync = ? " +
                        "WHERE v033 = ?";

                    using (var updateCmd = new OleDbCommand(updateSql, connection))
                    {
                        updateCmd.Parameters.Add(new OleDbParameter
                        {
                            OleDbType = OleDbType.LongVarChar,
                            Value = (object)jsonResult ?? DBNull.Value
                        });
                        updateCmd.Parameters.Add(new OleDbParameter
                        {
                            OleDbType = OleDbType.VarChar,
                            Value = "False"
                        });
                        updateCmd.Parameters.Add(new OleDbParameter
                        {
                            OleDbType = OleDbType.VarChar,
                            Value = docId
                        });

                        int rows = updateCmd.ExecuteNonQuery();
                        return rows == 1;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion

        //#region Supported documents for customers

        //public async Task<int> RefreshSupportedDocumentsCustomersAsync()
        //{
        //    int numberUpdated = 0;

        //    const string selectSql =
        //        "SELECT Klanten.A110, Klanten.A100, Klanten.v404, Klanten.v150, Klanten.v407, Klanten.dnnSync " +
        //        "FROM Klanten " +
        //        "WHERE trim(Klanten.v150) = 'BE' AND len(trim(Klanten.v404)) = 10;";

        //    using (var connection = new OleDbConnection(GetConnectionString()))
        //    using (var selectCmd = new OleDbCommand(selectSql, connection))
        //    {
        //        try
        //        {
        //            connection.Open();

        //            using (var adapter = new OleDbDataAdapter(selectCmd))
        //            using (var table = new DataTable())
        //            {
        //                adapter.Fill(table);

        //                if (table.Rows.Count <= 0)
        //                    return 0;

        //                int recordCount = table.Rows.Count;

        //                // Fill the DataGridView – same behaviour as original
        //                _notificationsGrid.Rows.Clear();
        //                _notificationsGrid.Columns.Clear();
        //                _notificationsGrid.Columns.Add("A110", "Id");
        //                _notificationsGrid.Columns.Add("v404", "KBO");
        //                _notificationsGrid.Columns.Add("A100", "Name");
        //                _notificationsGrid.Columns.Add("v407", "Ondersteund");

        //                foreach (DataRow row in table.Rows)
        //                {
        //                    string id = Convert.ToString(row["A110"]).Trim();
        //                    string kbo = Convert.ToString(row["v404"]).Trim();
        //                    string name = Convert.ToString(row["A100"]).Trim();
        //                    string supported = Convert.ToString(row["v407"]).Trim();

        //                    _notificationsGrid.Rows.Add(id, kbo, name, supported);
        //                }

        //                _notificationsGrid.AutoResizeColumns();
        //                _notificationsGrid.Visible = true;

        //                // Now update supported documents in DB
        //                foreach (DataRow row in table.Rows)
        //                {
        //                    string customerId = Convert.ToString(row["A110"]).Trim();
        //                    string customerName = Convert.ToString(row["A100"]).Trim();
        //                    string customerKbo = Convert.ToString(row["v404"]).Trim();
        //                    string customerCountry = Convert.ToString(row["v150"]).Trim();
        //                    string supportedDocuments = Convert.ToString(row["v407"]).Trim();

        //                    if (customerKbo.Length == 10)
        //                    {
        //                        string resultJson = await CheckPeppolSupportedDocuments(customerKbo);
        //                        if (!string.IsNullOrEmpty(resultJson))
        //                        {
        //                            const string updateSql =
        //                                "UPDATE Klanten " +
        //                                "SET v407 = ?, dnnSync = ? " +
        //                                "WHERE A110 = ?";

        //                            using (var updateCmd = new OleDbCommand(updateSql, connection))
        //                            {
        //                                updateCmd.Parameters.Add(new OleDbParameter
        //                                {
        //                                    OleDbType = OleDbType.LongVarChar,
        //                                    Value = (object)resultJson ?? DBNull.Value
        //                                });
        //                                updateCmd.Parameters.Add(new OleDbParameter
        //                                {
        //                                    OleDbType = OleDbType.VarChar,
        //                                    Value = "False"
        //                                });
        //                                updateCmd.Parameters.Add(new OleDbParameter
        //                                {
        //                                    OleDbType = OleDbType.VarChar,
        //                                    Value = customerId
        //                                });

        //                                int rows = updateCmd.ExecuteNonQuery();
        //                                if (rows == 1)
        //                                {
        //                                    numberUpdated++;
        //                                    if (_statusLabel != null)
        //                                    {
        //                                        _statusLabel.Text =
        //                                            "Bezig... " + numberUpdated + " of " + recordCount + " - " + customerName;
        //                                        Application.DoEvents();
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            return numberUpdated;
        //        }
        //        catch
        //        {
        //            return 0;
        //        }
        //    }
        //}

        //// This method must stay in a class that has access to the same HttpClient/UI
        //// You already have it in FormSyncPeppol; if you move it here, adjust accordingly.
        //private Task<string> CheckPeppolSupportedDocuments(string kbo)
        //{
        //    // delegate back to existing helper to avoid duplication
        //    return ((FormSyncPeppol)Application.OpenForms[nameof(FormSyncPeppol)])
        //        .CheckPeppolSupportedDocumentsProxy(kbo);
        //}

        //#endregion
    }
}