using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Job_Service.Services
{
    public class CVRepository :  ICVRepository
    {
        private readonly IConfiguration _configuration;
        public CVRepository(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }
        public Task<IEnumerable<dynamic>> GetAllCvData()
        {
            throw new NotImplementedException();
        }
        public async ValueTask<dynamic> GetCVData(string commandText)
        {
            string connectionstringname = "DBConnection";
            string _connectionString = _configuration.GetConnectionString(connectionstringname);

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_connectionString);

            using (var con = new SqlConnection(builder.ConnectionString))
            {
                DataTable dt = null;
                SqlCommand sqlCmd = new SqlCommand();

                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Connection = con;

                sqlCmd.CommandText = commandText;

                using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                {
                    dt = new DataTable { TableName = "record" };
                    da.Fill(dt);
                }
                return dt;
            }
        }

        public async ValueTask<dynamic> AddEmployee(string title,string description,int  locationId,int departmentId,DateTime closingDate)
        {
            string connectionstringname = "DBConnection";
            string _connectionString = _configuration.GetConnectionString(connectionstringname);

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_connectionString);

            using (var con = new SqlConnection(builder.ConnectionString))
            {
                DataTable dt = null;
                SqlCommand sqlCmd = new SqlCommand();

                sqlCmd.CommandType = CommandType.Text;
                 sqlCmd.Connection = con;

                string CommandText = "INSERT INTO DBname.dbo.JobDataTable( title, description, locationId, departmentId, closingDate) VALUES ('" 
                    +  title+"','"+ description+"',"+ locationId+"," +departmentId+",'"+ closingDate+"')";

                using (SqlConnection connection = new SqlConnection(
               _connectionString))
                {
                    SqlCommand command = new SqlCommand(CommandText, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
                
                return dt;
            }
        }

        public async ValueTask<dynamic> UpdateEmployee(int id,string title, string description, int locationId, int departmentId, DateTime closingDate)
        {
            string connectionstringname = "DBConnection";
            string _connectionString = _configuration.GetConnectionString(connectionstringname);

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_connectionString);

            using (var con = new SqlConnection(builder.ConnectionString))
            {
                DataTable dt = null;
                SqlCommand sqlCmd = new SqlCommand();

                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Connection = con;

                string CommandText = "UPDATE DBname.dbo.JobDataTable SET title='" +
                    title+"',description='"+ description + "', locationId ="+  locationId+ ",departmentId ="+ departmentId
                    + ",closingDate = '"+ closingDate + "' where id=" +id;


                using (SqlConnection connection = new SqlConnection(
               _connectionString))
                {
                    SqlCommand command = new SqlCommand(CommandText, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }

                return dt;
            }
        }


    }
}
