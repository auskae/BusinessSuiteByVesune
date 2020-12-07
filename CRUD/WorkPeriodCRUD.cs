using BusinessSuiteByVesune.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace BusinessSuiteByVesune.CRUD
{
    public class WorkPeriodCRUD
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["BusinessSuiteByVesune.Properties.Settings.BusinessSuiteVesuneConnectionString"].ToString();

        public int Create(WorkPeriod period)
        {
            int result = 0;
            string textCommand = "INSERT INTO tbl_WorkPeriods (UserId, StartDate, EndDate) OUTPUT INSERTED.WorkPeriodId VALUES (@UserId, @StartDate, @EndDate)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(textCommand, connection);

                command.Parameters.AddWithValue("@UserId", period.UserId);
                command.Parameters.AddWithValue("@StartDate", period.Start);

                if (period.End == null)
                {
                    command.Parameters.AddWithValue("@EndDate", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@EndDate", (DateTime)period.End);
                }


                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = Convert.ToInt32(reader["WorkPeriodId"].ToString());
                    }
                    connection.Close();
                }
            }
            return result;
        }
        public WorkPeriod Read(int periodId)
        {
            WorkPeriod period = new WorkPeriod();
            string textCommand = "SELECT * FROM tbl_WorkPeriods WHERE WorkPeriodId = @WorkPeriodId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(textCommand, connection);

                command.Parameters.AddWithValue("@WorkPeriodId", periodId);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        period.WorkPeriodId = Convert.ToInt32(reader["WorkPeriodId"].ToString());
                        period.UserId = Convert.ToInt32(reader["UserId"].ToString());
                        period.Start = Convert.ToDateTime(reader["StartDate"].ToString());
                        period.End = Convert.IsDBNull(reader["EndDate"]) ? null : (DateTime?)reader["EndDate"];
                    }
                    connection.Close();
                }
            }
            return period;
        }
        public int Update(WorkPeriod period)
        {
            int result = 0;
            string textCommand = "UPDATE tbl_WorkPeriods SET UserId = @UserId, StartDate = @StartDate, EndDate = @EndDate WHERE UserId = @UserId AND WorkPeriodId = @WorkPeriodId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(textCommand, connection);

                command.Parameters.AddWithValue("@WorkPeriodId", period.WorkPeriodId);
                command.Parameters.AddWithValue("@UserId", period.UserId);
                command.Parameters.AddWithValue("@StartDate", period.Start);
                command.Parameters.AddWithValue("@EndDate", period.End);

                connection.Open();

                result = command.ExecuteNonQuery();

                connection.Close();
            }
            return result;
        }
        public int Destroy(int periodId)
        {
            int result = 0;
            string textCommand = "DELETE FROM tbl_WorkPeriods WHERE WorkPeriodId = @WorkPeriodId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(textCommand, connection);

                command.Parameters.AddWithValue("@WorkPeriodId", periodId);

                connection.Open();

                result = command.ExecuteNonQuery();

                connection.Close();
            }
            return result;
        }
        public List<WorkPeriod> GetWorkPeriodsByUserId(int userId, DateTime startDate, DateTime endDate)
        {
            List<WorkPeriod> periods = new List<WorkPeriod>();
            string textCommand = "SELECT * FROM tbl_WorkPeriods WHERE UserId = @UserId AND StartDate BETWEEN @StartDate AND @EndDate";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(textCommand, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        WorkPeriod period = new WorkPeriod
                        {
                        WorkPeriodId = Convert.ToInt32(reader["WorkPeriodId"].ToString()),
                        UserId = Convert.ToInt32(reader["UserId"].ToString()),
                        Start = Convert.ToDateTime(reader["StartDate"].ToString()),
                        End = reader["EndDate"] as DateTime?,
                    };
                        periods.Add(period);
                    }
                    connection.Close();
                }
            }
            return periods;
        }
    }
}
