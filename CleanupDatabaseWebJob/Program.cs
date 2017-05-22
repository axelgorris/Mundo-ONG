using System;
using System.Configuration;
using System.Data.SqlClient;

namespace CleanupDatabaseWebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        static void Main()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    Console.WriteLine("[CleanupDatabaseWebJob] Initiating SQL Connection");
                    sqlConnection.Open();

                    Console.WriteLine("[CleanupDatabaseWebJob] Executing Organizations SQL Statement");
                    sqlCommand.CommandText = "DELETE FROM [dbo].[Organizations] WHERE [deleted] = 1 AND [updatedAt] < DATEADD(day, -7, SYSDATETIMEOFFSET())";
                    var organizationRowsAffected = sqlCommand.ExecuteNonQuery();
                    Console.WriteLine($"[CleanupDatabaseWebJob] Organizations: {organizationRowsAffected} rows deleted.");

                    Console.WriteLine("[CleanupDatabaseWebJob] Executing Announcements SQL Statement");
                    sqlCommand.CommandText = "DELETE FROM [dbo].[Announcements] WHERE [deleted] = 1 AND [updatedAt] < DATEADD(day, -7, SYSDATETIMEOFFSET())";
                    var announcementRowsAffected = sqlCommand.ExecuteNonQuery();
                    Console.WriteLine($"[CleanupDatabaseWebJob] Announcements: {announcementRowsAffected} rows deleted.");

                    sqlConnection.Close();
                }
            }
        }
    }
}
