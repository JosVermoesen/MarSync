
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace MarSync
{
    class DataTelebibContracts
    {
        public void TelebibContractsInsertAndUpdate(string marLocatie, string sqlNew, string sqlUpdate, string cnn)
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

                    List<VsoftTelebibContract> telebibContractsNew = new List<VsoftTelebibContract>();
                    List<VsoftTelebibContract> telebibContractsUpdate = new List<VsoftTelebibContract>();

                    while (readerNew.Read())
                    {
                        string strPolis = readerNew["Polis"].ToString().Trim(' ');
                        if (strPolis == "999999999999")
                        {
                            goto IgnoreNewRec;
                        }

                        if (strPolis.Contains("+"))
                        {
                            goto IgnoreNewRec;
                        }

                        if (strPolis.Contains(":"))
                        {
                            int whereIsIt = strPolis.IndexOf(":");
                            strPolis = strPolis.Substring(0, whereIsIt);
                            // goto IgnoreNewRec;
                        }

                        telebibContractsNew.Add(new VsoftTelebibContract
                        {
                            Mij = readerNew["Mij"].ToString().Trim(' '),
                            MemoTb2 = readerNew["MemoTB2"].ToString().Trim(' '),
                            DocType = readerNew["DocType"].ToString().Trim(' '),
                            VsoftContractId = strPolis
                        });
                    IgnoreNewRec:;
                    }

                    try
                    {
                        connection.Execute("spVsoftTelebibContracts_InsertFull @Mij, @MemoTb2, @DocType, @VsoftContractId", telebibContractsNew);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    readerNew.Close();


                    while (readerUpdate.Read())
                    {
                        string strPolis = readerUpdate["Polis"].ToString().Trim(' ');
                        if (strPolis == "999999999999")
                        {
                            goto IgnoreUpdateRec;
                        }

                        telebibContractsNew.Add(new VsoftTelebibContract
                        {
                            Mij = readerUpdate["Mij"].ToString().Trim(' '),
                            MemoTb2 = readerUpdate["MemoTB2"].ToString().Trim(' '),
                            DocType = readerUpdate["DocType"].ToString().Trim(' '),
                            VsoftContractId = strPolis
                        });
                    IgnoreUpdateRec:;
                    }

                    try
                    {
                        // connection.Execute("dbo.spVsoftCustomers_InsertFull @Id, @A107, @A108, @A100", customers);                    
                        connection.Execute("spVsoftTelebibContracts_UpdateFull @Mij, @MemoTb2, @DocType, @VsoftContractId", telebibContractsUpdate);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    readerUpdate.Close();

                    OleDbCommand command = new OleDbCommand("UPDATE TB2 SET dnnSync=True, dnnSyncAlready = True WHERE DocType = '01' OR DocType = '02'", cnnMarJet);
                    command.ExecuteNonQuery();
                    cnnMarJet.Close();
                }
                catch (Exception)
                {
                    throw;
                }

            }

        }
    }
}
