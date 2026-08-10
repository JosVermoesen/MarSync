using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace MarSync
{
    class DataYearSettings
    {
        public void YearSettingsDeleteAndInsert(string cnn)
        {
            string sql = "DELETE FROM vsoft_YearSettings";

            using (SqlConnection connection = new SqlConnection(cnn))
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            // TODO: use C# !
            //    Dim Tellertje As Integer
            //Dim TellertjeBlok As Integer
            //Dim sqlJAAR As String
            //Dim sqlDeel As String = ""

            //For Teljaar = 1985 To 2030
            //    sqlDeel = "jr" + Trim(Teljaar.ToString)
            //    sqlJAAR = "select * from " & sqlDeel

            //    Try
            //        adoRS.Open(sqlJAAR, dbMAR, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
            //        Do While Not adoRS.EOF
            //            Tellertje = Tellertje + 1
            //            TellertjeBlok = TellertjeBlok + 1

            //            'If Tellertje = 44 Then Stop
            //            frmExportToSQL.Label23.Text = Tellertje.ToString
            //            frmExportToSQL.Label24.Text = TellertjeBlok.ToString
            //            frmExportToSQL.Label25.Text = Teljaar.ToString
            //            frmExportToSQL.Refresh()

            //            Dim myjrTeller As New vsoft_YearSetting
            //            With myjrTeller

            //                Try
            //                    .v071 = System.Convert.ToString(adoRS.Fields("v071").Value)
            //                    .v217 = System.Convert.ToString(adoRS.Fields("v217").Value)
            //                    .bkjr = Trim(Teljaar.ToString)

            //                    mycontext.vsoft_YearSettings.InsertOnSubmit(myjrTeller)
            //                    If TellertjeBlok = 100 Then
            //                        TellertjeBlok = 0
            //                        Try
            //                            mycontext.SubmitChanges()
            //                        Catch ex As Exception
            //                            MsgBox(ex.Message)
            //                            Exit Function
            //                        End Try
            //                    End If

            //                Catch ex As Exception
            //                    frmExportToSQL.Label27.Text = ex.Message

            //                End Try
            //            End With
            //            adoRS.MoveNext()
            //        Loop

            //        Try
            //            mycontext.SubmitChanges()
            //        Catch ex As Exception
            //            MsgBox(ex.Message)
            //            Exit Function
            //        End Try
            //        adoRS.Close()

            //    Catch ex As Exception
            //        'niets doen gewoon teller laten vervolgen
            //    End Try
            //Next

            //mycontext.SubmitChanges()
            //Try
            //    dbMAR.Close()
            //    getYearSettings = True
            //Catch ex As Exception
            //    frmExportToSQL.Label27.Text = ex.Message
            //    getYearSettings = False
            //End Try
        }
    }
}
