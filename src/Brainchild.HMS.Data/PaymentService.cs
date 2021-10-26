using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Brainchild.HMS.Data.DTOs;
using Brainchild.HMS.Core.Models;

namespace Brainchild.HMS.Data
{

    public interface IPaymentService
    {
        void AddPayments(PaymentDTO payment);
    }
    public class PaymentService : IPaymentService
    {
        private readonly string connectionString;
        public PaymentService(string connection)
        {
            connectionString = connection;
        }
        public void AddPayments(PaymentDTO payment)
        {
            //creating an sqlconnection object.
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //opening the connection.
            sqlConnection.Open();
            //SQL query for inserting the payments
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO Payments VALUES(@paymentTypeId,@paymentAmount,@paymentDescription,@billingId,@paymentDate);SELECT SCOPE_IDENTITY()", sqlConnection);
            //Adding parameters
            sqlCommand.Parameters.AddWithValue("@paymentTypeId",payment.PaymentTypeId);
            sqlCommand.Parameters.AddWithValue("@paymentAmount", payment.PaymentAmount);
            sqlCommand.Parameters.AddWithValue("@paymentDescription", payment.PaymentDescription);
            sqlCommand.Parameters.AddWithValue("@billingId", payment.BillingId);
            sqlCommand.Parameters.AddWithValue("@paymentDate", payment.PaymentDate);
            //Executing the query and storing the paymentId 
            payment.PaymentId=Convert.ToInt32(sqlCommand.ExecuteScalar());
            //Closing the connection
            sqlConnection.Close();

        }
    }
}
