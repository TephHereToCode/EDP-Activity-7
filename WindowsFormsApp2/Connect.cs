using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace WindowsFormsApp2
{
    public class Connect
    {
        // Connection String for MySQL database
        private static string myConnectionString = "server=localhost; uid=root; pwd=admin; database=act5";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(myConnectionString);
        }

        public static DataTable GetWinsData(string trainerID)
        {
            DataTable winsData = new DataTable();

            using (MySqlConnection connection = GetConnection())
            {
                string query = "SELECT Badges, NumberOfPokemon FROM trainers WHERE TrainerID = @TrainerID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@TrainerID", trainerID);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                try
                {
                    connection.Open();
                    adapter.Fill(winsData);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error fetching wins data: " + ex.Message);
                }
            }

            return winsData;
        }


        public static DataSet GetActiveTrainersData()
        {
            DataSet dataSet = new DataSet();

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    // Select active trainers and count them
                    string query = "SELECT * FROM trainers WHERE IsActive = 'Active'";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable activeTrainersTable = new DataTable();
                    adapter.Fill(activeTrainersTable);

                    // Get the count of active trainers
                    int totalActiveTrainers = activeTrainersTable.Rows.Count;

                    // Create a new DataTable to hold the total count
                    DataTable totalActiveTrainersTable = new DataTable();
                    totalActiveTrainersTable.Columns.Add("TotalActiveTrainers", typeof(int));
                    totalActiveTrainersTable.Rows.Add(totalActiveTrainers);

                    // Add both DataTables to the DataSet
                    dataSet.Tables.Add(activeTrainersTable);
                    dataSet.Tables.Add(totalActiveTrainersTable);
                }
                catch (MySqlException ex)
                {
                    
                }
            }

            return dataSet;
        }


        public static DataTable GetTrainerCountData()
        {
            DataTable trainerCountData = new DataTable();

            using (MySqlConnection connection = GetConnection())
            {
                string query = "SELECT COUNT(*) AS TotalTrainers FROM trainers";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                try
                {
                    connection.Open();
                    adapter.Fill(trainerCountData);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error fetching trainer count: " + ex.Message);
                }
            }

            return trainerCountData;
        }

        // Inside Connect.cs
        public static int GetActiveTrainerCount()
        {
            int activeTrainerCount = 0;

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM trainers WHERE IsActive = 'Active'";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    activeTrainerCount = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error fetching active trainer count: " + ex.Message);
                }
            }

            return activeTrainerCount;
        }

        public static int GetTotalTrainerCount()
        {
            int totalTrainerCount = 0;

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM trainers";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    totalTrainerCount = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error fetching total trainer count: " + ex.Message);
                }
            }

            return totalTrainerCount;
        }


    }
}


