using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace MarSync
{
    class DataCustomerInvoices
    {
        public void CustomerInvoicesInsertAndUpdate(string marLocatie, string sqlNew, string sqlUpdate, string cnn)
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

                    List<VsoftCustomerInvoice> customerInvoicesNew = new List<VsoftCustomerInvoice>();
                    List<VsoftCustomerInvoice> customerInvoicesUpdate = new List<VsoftCustomerInvoice>();


                    while (readerNew.Read())
                    {
                        decimal iodecv249  = GetDecimalSafe(readerNew, "decv249");
                        decimal iodecv040  = GetDecimalSafe(readerNew, "decv040");
                        decimal iodecv055  = GetDecimalSafe(readerNew, "decv055");
                        decimal iodecv056  = GetDecimalSafe(readerNew, "decv056");
                        decimal iodecv057  = GetDecimalSafe(readerNew, "decv057");
                        decimal iodecv058  = GetDecimalSafe(readerNew, "decv058");
                        decimal iodecv059  = GetDecimalSafe(readerNew, "decv059");
                        decimal iodecv060  = GetDecimalSafe(readerNew, "decv060");
                        decimal iodecv061  = GetDecimalSafe(readerNew, "decv061");
                        decimal iodecv062  = GetDecimalSafe(readerNew, "decv062");
                        decimal iodecv063  = GetDecimalSafe(readerNew, "decv063");
                        decimal iodecv064  = GetDecimalSafe(readerNew, "decv064");
                        decimal iodecB010  = GetDecimalSafe(readerNew, "decB010");
                        decimal iodecB014  = GetDecimalSafe(readerNew, "decB014");
                        decimal iodecB090  = GetDecimalSafe(readerNew, "decB090");
                        decimal iodecB094  = GetDecimalSafe(readerNew, "decB094");
                        decimal iodecv065  = GetDecimalSafe(readerNew, "decv065");

                        customerInvoicesNew.Add(new VsoftCustomerInvoice
                        {
                            V033 = readerNew["v033"].ToString().Trim(' '),
                            A110 = readerNew["v034"].ToString().Substring(1).Trim(' '), // A110 Unique code number (Customer ID)
                            V035 = readerNew["v035"].ToString().Trim(' '),
                            V036 = readerNew["v036"].ToString().Trim(' '),
                            V037 = readerNew["v037"].ToString().Trim(' '),
                            V038 = readerNew["v038"].ToString().Trim(' '),
                            V039 = readerNew["v039"].ToString().Trim(' '),
                            Vs03 = readerNew["vs03"].ToString().Trim(' '),
                            V040 = readerNew["v040"].ToString().Trim(' '),

                            // Opgelet v041 is veld van slechts één teken, LinqToSql generator 2008 geeft CHAR weer welke geen string aanvaardt. 
                            V041 = readerNew["v041"].ToString().Trim(' '),
                            V055 = readerNew["v055"].ToString().Trim(' '),
                            V056 = readerNew["v056"].ToString().Trim(' '),
                            V057 = readerNew["v057"].ToString().Trim(' '),
                            V058 = readerNew["v058"].ToString().Trim(' '),
                            V059 = readerNew["v059"].ToString().Trim(' '),
                            V060 = readerNew["v060"].ToString().Trim(' '),
                            V061 = readerNew["v061"].ToString().Trim(' '),
                            V062 = readerNew["v062"].ToString().Trim(' '),
                            V063 = readerNew["v063"].ToString().Trim(' '),
                            V064 = readerNew["v064"].ToString().Trim(' '),
                            A000 = readerNew["A000"].ToString().Trim(' '),
                            B010 = readerNew["B010"].ToString().Trim(' '),
                            B014 = readerNew["B014"].ToString().Trim(' '),
                            B090 = readerNew["B090"].ToString().Trim(' '),
                            B094 = readerNew["B094"].ToString().Trim(' '),
                            V065 = readerNew["v065"].ToString().Trim(' '),
                            V245 = readerNew["v245"].ToString().Trim(' '),
                            V246 = readerNew["v246"].ToString().Trim(' '),
                            V066 = readerNew["v066"].ToString().Trim(' '),
                            V249 = readerNew["v249"].ToString().Trim(' '),

                            Decv249 = iodecv249,
                            Decv040 = iodecv040,
                            Decv055 = iodecv055,
                            Decv056 = iodecv056,
                            Decv057 = iodecv057,
                            Decv058 = iodecv058,
                            Decv059 = iodecv059,
                            Decv060 = iodecv060,
                            Decv061 = iodecv061,
                            Decv062 = iodecv062,
                            Decv063 = iodecv063,
                            Decv064 = iodecv064,
                            DecB010 = iodecB010,
                            DecB014 = iodecB014,
                            DecB090 = iodecB090,
                            DecB094 = iodecB094,
                            Decv065 = iodecv065,

                            // opgelet MEMO Field
                            Dece069 = readerNew["dece069"].ToString().Trim(' '),

                            E069 = readerNew["e069"].ToString().Trim(' '),
                            RvDm = readerNew["rvDM"].ToString().Trim(' '),
                            V405 = readerNew["v405"].ToString().Trim(' '),
                            V406 = readerNew["v406"].ToString().Trim(' '),
                            // BstndNaam37 = readerNew["bstndNaam37"].ToString().Trim(' '),
                            // TypeZending37 = readerNew["typeZending37"].ToString().Trim(' '),
                            // If IsDBNull(adoRS.Fields("bstBLOB37").Value) Then
                            // Else
                            // 'Stop
                            // '.bstBLOB37 = System.Convert.ToString(adoRS.Fields("bstBLOB37").Value)
                            // End If

                            RvXmltb2 = readerNew["rvXMLTB2"].ToString().Trim(' '),

                            // RvID = (int)readerNew["RvID"],
                            VsoftCustomerId = GetIntSafe(readerNew, "vsoftKLID")
                        });
                    }

                    // After filling customerInvoicesNew
                    // Keep only invoices that have a valid FK to VsoftCustomers
                    customerInvoicesNew = customerInvoicesNew
                        .Where(ci => ci.VsoftCustomerId > 0)
                        .ToList();

                    try
                    {
                        connection.Execute(
                            "spVsoftCustomerInvoices_InsertFull @V033, @A110, @V035, @V066, @V036, @V037, @V038, @V249, @Decv249, @V039, @Vs03, @V040, @Decv040, @V041, @V245, @V246, @A000, @B010, @DecB010, @B014, @DecB014, @B090, @DecB090, @B094, @DecB094, @V065, @Decv065, @E069, @Dece069, @E071, @E072, @V055, @Decv055, @V056, @Decv056, @V057, @Decv057, @V058, @Decv058, @V059, @Decv059, @V060, @Decv060, @V061, @Decv061, @V062, @Decv062, @V063, @Decv063, @V064, @Decv064, @RvDm, @RvXmltb2, @V405, @V406, @VsoftCustomerId",
                            customerInvoicesNew);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    readerNew.Close();


                    while (readerUpdate.Read())
                    {
                        decimal iodecv249  = GetDecimalSafe(readerUpdate, "decv249");
                        decimal iodecv040  = GetDecimalSafe(readerUpdate, "decv040");
                        decimal iodecv055  = GetDecimalSafe(readerUpdate, "decv055");
                        decimal iodecv056  = GetDecimalSafe(readerUpdate, "decv056");
                        decimal iodecv057  = GetDecimalSafe(readerUpdate, "decv057");
                        decimal iodecv058  = GetDecimalSafe(readerUpdate, "decv058");
                        decimal iodecv059  = GetDecimalSafe(readerUpdate, "decv059");
                        decimal iodecv060  = GetDecimalSafe(readerUpdate, "decv060");
                        decimal iodecv061  = GetDecimalSafe(readerUpdate, "decv061");
                        decimal iodecv062  = GetDecimalSafe(readerUpdate, "decv062");
                        decimal iodecv063  = GetDecimalSafe(readerUpdate, "decv063");
                        decimal iodecv064  = GetDecimalSafe(readerUpdate, "decv064");
                        decimal iodecB010  = GetDecimalSafe(readerUpdate, "decB010");
                        decimal iodecB014  = GetDecimalSafe(readerUpdate, "decB014");
                        decimal iodecB090  = GetDecimalSafe(readerUpdate, "decB090");
                        decimal iodecB094  = GetDecimalSafe(readerUpdate, "decB094");
                        decimal iodecv065  = GetDecimalSafe(readerUpdate, "decv065");

                        customerInvoicesUpdate.Add(new VsoftCustomerInvoice
                        {
                            // ... string fields ...

                            Decv249 = iodecv249,
                            Decv040 = iodecv040,
                            Decv055 = iodecv055,
                            Decv056 = iodecv056,
                            Decv057 = iodecv057,
                            Decv058 = iodecv058,
                            Decv059 = iodecv059,
                            Decv060 = iodecv060,
                            Decv061 = iodecv061,
                            Decv062 = iodecv062,
                            Decv063 = iodecv063,
                            Decv064 = iodecv064,
                            DecB010 = iodecB010,
                            DecB014 = iodecB014,
                            DecB090 = iodecB090,
                            DecB094 = iodecB094,
                            Decv065 = iodecv065,

                            Dece069 = readerUpdate["dece069"].ToString().Trim(' '),
                            E069    = readerUpdate["e069"].ToString().Trim(' '),
                            RvDm    = readerUpdate["rvDM"].ToString().Trim(' '),
                            V405    = readerUpdate["v405"].ToString().Trim(' '),
                            V406    = readerUpdate["v406"].ToString().Trim(' '),
                            RvXmltb2 = readerUpdate["rvXMLTB2"].ToString().Trim(' '),

                            VsoftCustomerId = GetIntSafe(readerUpdate, "vsoftKLID")
                        });
                    }

                    // After filling customerInvoicesUpdate
                    customerInvoicesUpdate = customerInvoicesUpdate
                        .Where(ci => ci.VsoftCustomerId > 0)
                        .ToList();

                    try
                    {
                        connection.Execute(
                            "spVsoftCustomerInvoices_UpdateFull @V033, @A110, @V035, @V066, @V036, @V037, @V038, @V249, @Decv249, @V039, @Vs03, @V040, @Decv040, @V041, @V245, @V246, @A000, @B010, @DecB010, @B014, @DecB014, @B090, @DecB090, @B094, @DecB094, @V065, @Decv065, @E069, @Dece069, @E071, @E072, @V055, @Decv055, @V056, @Decv056, @V057, @Decv057, @V058, @Decv058, @V059, @Decv059, @V060, @Decv060, @V061, @Decv061, @V062, @Decv062, @V063, @Decv063, @V064, @Decv064, @RvDm, @RvXmltb2, @V405, @V406, @VsoftCustomerId",
                            customerInvoicesUpdate);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    readerUpdate.Close();

                    OleDbCommand command = new OleDbCommand("UPDATE Dokumenten SET dnnSync=True, dnnSyncAlready = True where v034 like 'K%'", cnnMarJet);
                    command.ExecuteNonQuery();
                    cnnMarJet.Close();
                }
                catch (Exception)
                {
                    throw;
                }

            }

        }

        public void CustomerInvoicesSyncID(string marLocatie, string cnn)
        {
            // Load records from SQL Server into a DataTable
            DataTable sqlTable = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(cnn))
            {
                sqlConn.Open();
                string sqlQuery = "SELECT [Id], [V033] FROM VsoftCustomerInvoices"; // adjust column names accordingly
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
                    string V033 = (string)row["V033"]; // adjust as needed

                    // Check if this record already exists in the Access table
                    bool recordExists = false;
                    using (OleDbCommand checkCmd = new OleDbCommand("SELECT COUNT(*) FROM Dokumenten WHERE [V033] = ?", accessConn))
                    {
                        checkCmd.Parameters.AddWithValue("?", V033);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        recordExists = (count > 0);
                    }

                    if (!recordExists)
                    {
                        Console.WriteLine("Opzoeking vanuit SQLSERVER VsoftCustomerInvoices:\nSleutel in tabel Dokumenten niet gevonden in marIntegraal mdv: " + V033);
                    }
                    else
                    {
                        // Update the record if other fields have changed
                        using (OleDbCommand updateCmd = new OleDbCommand("UPDATE Dokumenten SET sqlID = ? WHERE [V033] = ?", accessConn))
                        {
                            updateCmd.Parameters.AddWithValue("?", sqlID);
                            updateCmd.Parameters.AddWithValue("?", V033);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            Console.WriteLine("Database sync completed successfully.");
        }

        private static decimal GetDecimalSafe(OleDbDataReader reader, string fieldName)
        {
            object value = reader[fieldName];

            if (value == null || value == DBNull.Value)
                return 0m;

            // Common numeric types
            if (value is decimal dec) return dec;
            if (value is double dbl) return Convert.ToDecimal(dbl);
            if (value is float flt) return Convert.ToDecimal(flt);
            if (value is int i) return i;
            if (value is long l) return l;
            if (value is short s16) return s16;
            if (value is byte b) return b;

            // String or other types – try to parse
            var s = value.ToString();
            if (string.IsNullOrWhiteSpace(s))
                return 0m;

            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out var result))
                return result;

            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                return result;

            // If still not parseable, treat as zero (and optionally log)
            Console.WriteLine($"GetDecimalSafe: cannot parse '{s}' from field '{fieldName}', using 0.");
            return 0m;
        }

        private static int GetIntSafe(OleDbDataReader reader, string fieldName)
        {
            object value = reader[fieldName];

            if (value == null || value == DBNull.Value)
                return 0;

            if (value is int i) return i;
            if (value is long l) return (int)l;
            if (value is short s16) return s16;
            if (value is byte b) return b;
            if (value is decimal dec) return (int)dec;
            if (value is double dbl) return (int)dbl;
            if (value is float flt) return (int)flt;

            var s = value.ToString();
            if (string.IsNullOrWhiteSpace(s))
                return 0;

            if (int.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out var result))
                return result;

            if (int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                return result;

            Console.WriteLine($"GetIntSafe: cannot parse '{s}' from field '{fieldName}', using 0.");
            return 0;
        }
    }
}
