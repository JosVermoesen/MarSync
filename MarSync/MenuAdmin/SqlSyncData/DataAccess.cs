using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MarSync
{
    class DataAccess
    {
        public List<VsoftCustomer> GetCustomersByA107(string fieldToSearch, string cnn)
        {
            // using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("marDb")))
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(cnn))
            {
                try
                {
                    var output = connection.Query<VsoftCustomer>("spVsoftCustomers_GetByA107 @A107", new { A107 = fieldToSearch }).ToList();
                    return output;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public List<VsoftCustomer> GetCustomersByA108(string fieldToSearch, string cnn)
        {
            // using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("marDb")))
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(cnn))
            {

                // var output = connection.Query<VsoftCustomer>($"select * from VsoftCustomers where A108 = '{ fieldToSearch }'").ToList();                
                var output = connection.Query<VsoftCustomer>("spVsoftCustomers_GetByA108 @A108", new { A108 = fieldToSearch }).ToList();
                return output;
            }
        }


        public List<VsoftCustomer> GetCustomersById(string fieldToSearch, string cnn)
        {
            // using (System.Data.IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("marDb")))
            using (System.Data.IDbConnection connection = new System.Data.SqlClient.SqlConnection(cnn))
            {

                // var output = connection.Query<VsoftCustomer>($"select * from VsoftCustomers where A108 = '{ fieldToSearch }'").ToList();                
                var output = connection.Query<VsoftCustomer>("spVsoftCustomers_GetByA110 @Id", new { Id = fieldToSearch }).ToList();
                return output;
            }
        }


        public void InsertCustomer(string id, string a100, string a107, string a108, string cnn)
        {
            // using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("marDb")))
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(cnn))
            {
                List<VsoftCustomer> customers = new List<VsoftCustomer>
                {
                    new VsoftCustomer { A110 = id, A107 = a107, A108 = a108, A100 = a100 }
                };

                try
                {
                    // connection.Execute("dbo.spVsoftCustomers_InsertFull @Id, @A107, @A108, @A100", customers);                    
                    connection.Execute("dbo.spVsoftCustomers_InsertFull @Id, @E072, @G101, @G102, @G103, @G104, @G105, @G106, @A10c, @A104, @A105, @A106, @A107, @A108, @V149, @A109, @V150, @Vs03, @V161, @A161, @V151, @V111, @V254, @V255, @V256, @A170, @Vs04, @Vs05, @Vs06, @Vs07, @V225, @V227, @V247, @A10a, @Vs02, @V224, @A123, @A124, @A121, @A122, @V259, @V260, @E070, @E071, @V252, @A191, @A192, @A193, @A194, @A197, @_510z, @A130, @V301, @A102, @A100, @A101, @V226, @V243, @V302, @Vs01, @A125, @A127, @V002, @V257, @V258, @V244, @V251, @A120, @V201, @V202, @V203, @V204, @V205, @V206, @V207, @V208, @V209, @V210, @V211, @V245, @V246, @V253, @Uxxx, @V262, @V263, @RvID", customers);
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }

    }
}
