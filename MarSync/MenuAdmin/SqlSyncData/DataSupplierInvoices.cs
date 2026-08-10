using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;   // <-- add this
using System.Linq;           // <-- add if you later want filtering like with customers

namespace MarSync
{
    class DataSupplierInvoices
    {
        public void SupplierInvoicesInsertAndUpdate(string marLocatie, string sqlNew, string sqlUpdate, string cnn)
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

                    List<VsoftSupplierInvoice> supplierInvoicesNew = new List<VsoftSupplierInvoice>();
                    List<VsoftSupplierInvoice> supplierInvoicesUpdate = new List<VsoftSupplierInvoice>();

                    while (readerNew.Read())
                    {
                        decimal iodecv249 = GetDecimalSafe(readerNew, "decv249");
                        decimal iodecv040 = GetDecimalSafe(readerNew, "decv040");
                        decimal iodecv042 = GetDecimalSafe(readerNew, "decv042");
                        decimal iodecv043 = GetDecimalSafe(readerNew, "decv043");
                        decimal iodecv044 = GetDecimalSafe(readerNew, "decv044");
                        decimal iodecv045 = GetDecimalSafe(readerNew, "decv045");
                        decimal iodecv046 = GetDecimalSafe(readerNew, "decv046");
                        decimal iodecv047 = GetDecimalSafe(readerNew, "decv047");
                        decimal iodecv048 = GetDecimalSafe(readerNew, "decv048");
                        decimal iodecv049 = GetDecimalSafe(readerNew, "decv049");
                        decimal iodecv050 = GetDecimalSafe(readerNew, "decv050");
                        decimal iodecv051 = GetDecimalSafe(readerNew, "decv051");
                        decimal iodecv052 = GetDecimalSafe(readerNew, "decv052");
                        decimal iodecv053 = GetDecimalSafe(readerNew, "decv053");
                        decimal iodecv054 = GetDecimalSafe(readerNew, "decv054");

                        supplierInvoicesNew.Add(new VsoftSupplierInvoice
                        {
                            A110 = readerNew["v034"].ToString().Substring(1).Trim(' '),
                            V033 = readerNew["v033"].ToString().Trim(' '),
                            V035 = readerNew["v035"].ToString().Trim(' '),
                            V066 = readerNew["v066"].ToString().Trim(' '),
                            V036 = readerNew["v036"].ToString().Trim(' '),
                            V037 = readerNew["v037"].ToString().Trim(' '),
                            V038 = readerNew["v038"].ToString().Trim(' '),
                            V249 = readerNew["v249"].ToString().Trim(' '),

                            Decv249 = iodecv249,
                            Decv040 = iodecv040,

                            V039 = readerNew["v039"].ToString().Trim(' '),
                            Vs03 = readerNew["vs03"].ToString().Trim(' '),
                            V040 = readerNew["v040"].ToString().Trim(' '),

                            V041 = readerNew["v041"].ToString().Trim(' '),
                            V245 = readerNew["v245"].ToString().Trim(' '),
                            V246 = readerNew["v246"].ToString().Trim(' '),

                            RvDm = readerNew["rvDM"].ToString().Trim(' '),

                            V042 = readerNew["v042"].ToString().Trim(' '),
                            V043 = readerNew["v043"].ToString().Trim(' '),
                            V044 = readerNew["v044"].ToString().Trim(' '),
                            V045 = readerNew["v045"].ToString().Trim(' '),
                            V046 = readerNew["v046"].ToString().Trim(' '),
                            V047 = readerNew["v047"].ToString().Trim(' '),
                            V048 = readerNew["v048"].ToString().Trim(' '),
                            V049 = readerNew["v049"].ToString().Trim(' '),
                            V050 = readerNew["v050"].ToString().Trim(' '),
                            V051 = readerNew["v051"].ToString().Trim(' '),
                            V052 = readerNew["v052"].ToString().Trim(' '),
                            V053 = readerNew["v053"].ToString().Trim(' '),
                            V054 = readerNew["v054"].ToString().Trim(' '),

                            Decv042 = iodecv042,
                            Decv043 = iodecv043,
                            Decv044 = iodecv044,
                            Decv045 = iodecv045,
                            Decv046 = iodecv046,
                            Decv047 = iodecv047,
                            Decv048 = iodecv048,
                            Decv049 = iodecv049,
                            Decv050 = iodecv050,
                            Decv051 = iodecv051,
                            Decv052 = iodecv052,
                            Decv053 = iodecv053,
                            Decv054 = iodecv054,

                            VsoftSupplierId = GetIntSafe(readerNew, "vsoftKLID")
                        });
                    }

                    // Only keep invoices that have a valid supplier FK
                    supplierInvoicesNew = supplierInvoicesNew
                        .Where(si => si.VsoftSupplierId > 0)
                        .ToList();

                    var skipped = supplierInvoicesNew.Where(si => si.VsoftSupplierId <= 0).ToList();
                    Console.WriteLine($"Skipped {skipped.Count} supplier invoices without valid VsoftSupplierId.");

                    try
                    {
                        connection.Execute(
                            "spVsoftSupplierInvoices_InsertFull @V033, @A110, @V035, @V066, @V036, @V037, @V038, @V249, @Decv249, @V039, @Vs03, @V040, @Decv040, @V041, @V245, @V246, @RvDm, @V042, @V043, @V044, @V045, @V046, @V047, @V048, @V049, @V050, @V051, @V052, @V053, @V054, @Decv042, @Decv043, @Decv044, @Decv045, @Decv046, @Decv047, @Decv048, @Decv049, @Decv050, @Decv051, @Decv052, @Decv053, @Decv054, @VsoftSupplierId",
                            supplierInvoicesNew);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    readerNew.Close();


                    while (readerUpdate.Read())
                    {
                        decimal iodecv249 = GetDecimalSafe(readerUpdate, "decv249");
                        decimal iodecv040 = GetDecimalSafe(readerUpdate, "decv040");
                        decimal iodecv042 = GetDecimalSafe(readerUpdate, "decv042");
                        decimal iodecv043 = GetDecimalSafe(readerUpdate, "decv043");
                        decimal iodecv044 = GetDecimalSafe(readerUpdate, "decv044");
                        decimal iodecv045 = GetDecimalSafe(readerUpdate, "decv045");
                        decimal iodecv046 = GetDecimalSafe(readerUpdate, "decv046");
                        decimal iodecv047 = GetDecimalSafe(readerUpdate, "decv047");
                        decimal iodecv048 = GetDecimalSafe(readerUpdate, "decv048");
                        decimal iodecv049 = GetDecimalSafe(readerUpdate, "decv049");
                        decimal iodecv050 = GetDecimalSafe(readerUpdate, "decv050");
                        decimal iodecv051 = GetDecimalSafe(readerUpdate, "decv051");
                        decimal iodecv052 = GetDecimalSafe(readerUpdate, "decv052");
                        decimal iodecv053 = GetDecimalSafe(readerUpdate, "decv053");
                        decimal iodecv054 = GetDecimalSafe(readerUpdate, "decv054");

                        supplierInvoicesUpdate.Add(new VsoftSupplierInvoice
                        {
                            A110 = readerUpdate["v034"].ToString().Substring(1).Trim(' '),
                            V033 = readerUpdate["v033"].ToString().Trim(' '),
                            V035 = readerUpdate["v035"].ToString().Trim(' '),
                            V066 = readerUpdate["v066"].ToString().Trim(' '),
                            V036 = readerUpdate["v036"].ToString().Trim(' '),
                            V037 = readerUpdate["v037"].ToString().Trim(' '),
                            V038 = readerUpdate["v038"].ToString().Trim(' '),
                            V249 = readerUpdate["v249"].ToString().Trim(' '),

                            Decv249 = iodecv249,
                            Decv040 = iodecv040,

                            V039 = readerUpdate["v039"].ToString().Trim(' '),
                            Vs03 = readerUpdate["vs03"].ToString().Trim(' '),
                            V040 = readerUpdate["v040"].ToString().Trim(' '),

                            V041 = readerUpdate["v041"].ToString().Trim(' '),
                            V245 = readerUpdate["v245"].ToString().Trim(' '),
                            V246 = readerUpdate["v246"].ToString().Trim(' '),

                            RvDm = readerUpdate["rvDM"].ToString().Trim(' '),

                            V042 = readerUpdate["v042"].ToString().Trim(' '),
                            V043 = readerUpdate["v043"].ToString().Trim(' '),
                            V044 = readerUpdate["v044"].ToString().Trim(' '),
                            V045 = readerUpdate["v045"].ToString().Trim(' '),
                            V046 = readerUpdate["v046"].ToString().Trim(' '),
                            V047 = readerUpdate["v047"].ToString().Trim(' '),
                            V048 = readerUpdate["v048"].ToString().Trim(' '),
                            V049 = readerUpdate["v049"].ToString().Trim(' '),
                            V050 = readerUpdate["v050"].ToString().Trim(' '),
                            V051 = readerUpdate["v051"].ToString().Trim(' '),
                            V052 = readerUpdate["v052"].ToString().Trim(' '),
                            V053 = readerUpdate["v053"].ToString().Trim(' '),
                            V054 = readerUpdate["v054"].ToString().Trim(' '),

                            Decv042 = iodecv042,
                            Decv043 = iodecv043,
                            Decv044 = iodecv044,
                            Decv045 = iodecv045,
                            Decv046 = iodecv046,
                            Decv047 = iodecv047,
                            Decv048 = iodecv048,
                            Decv049 = iodecv049,
                            Decv050 = iodecv050,
                            Decv051 = iodecv051,
                            Decv052 = iodecv052,
                            Decv053 = iodecv053,
                            Decv054 = iodecv054,

                            VsoftSupplierId = GetIntSafe(readerUpdate, "vsoftKLID")
                        });
                    }

                    // Only keep invoices that have a valid supplier FK
                    supplierInvoicesUpdate = supplierInvoicesUpdate
                        .Where(si => si.VsoftSupplierId > 0)
                        .ToList();

                    try
                    {
                        connection.Execute(
                            "spVsoftSupplierInvoices_UpdateFull @V033, @A110, @V035, @V066, @V036, @V037, @V038, @V249, @Decv249, @V039, @Vs03, @V040, @Decv040, @V041, @V245, @V246, @RvDm, @V042, @V043, @V044, @V045, @V046, @V047, @V048, @V049, @V050, @V051, @V052, @V053, @V054, @Decv042, @Decv043, @Decv044, @Decv045, @Decv046, @Decv047, @Decv048, @Decv049, @Decv050, @Decv051, @Decv052, @Decv053, @Decv054, @VsoftSupplierId",
                            supplierInvoicesUpdate);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    readerUpdate.Close();

                    OleDbCommand command = new OleDbCommand("UPDATE Dokumenten SET dnnSync=True, dnnSyncAlready = True where v034 like 'L%'", cnnMarJet);
                    command.ExecuteNonQuery();
                    cnnMarJet.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public void SupplierInvoicesSyncID(string marLocatie, string cnn)
        {
            // Load records from SQL Server into a DataTable
            DataTable sqlTable = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(cnn))
            {
                sqlConn.Open();
                string sqlQuery = "SELECT [Id], [V033] FROM VsoftSupplierInvoices"; // adjust column names accordingly
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
                        Console.WriteLine("Opzoeking vanuit SQLSERVER VsoftSupplierInvoices:\nSleutel in tabel Dokumenten niet gevonden in marIntegraal mdv: " + V033);
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

            if (value is decimal dec) return dec;
            if (value is double dbl) return Convert.ToDecimal(dbl);
            if (value is float flt) return Convert.ToDecimal(flt);
            if (value is int i) return i;
            if (value is long l) return l;
            if (value is short s16) return s16;
            if (value is byte b) return b;

            var s = value.ToString();
            if (string.IsNullOrWhiteSpace(s))
                return 0m;

            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out var result))
                return result;

            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                return result;

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
