using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form6 : Form
    {
        private string trainerID = "";

        // Parameterless constructor
        public Form6(string trainer)
        {
            InitializeComponent();
            trainerID = trainer;
            DisplayTrainersData();

        }

        private void DisplayTrainersData()
        {
            using (MySqlConnection conn = Connect.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Select Trainer and User information for a specific TrainerID
                    string query = "SELECT t.TrainerID, t.TrainerName, t.IsActive, t.Gender, t.Address, t.Region, t.Badges, t.NumberOfPokemon, u.Email, u.Password " +
                                   "FROM trainers t " +
                                   "JOIN users u ON t.UserID = u.UserID " +
                                   "WHERE t.TrainerID = @trainerID ";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@trainerID", trainerID);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Assign values to textboxes
                                textBox4.Text = reader["TrainerID"].ToString();
                                textBox9.Text = reader["TrainerName"].ToString();
                                textBox1.Text = reader["Address"].ToString();
                                textBox2.Text = reader["Gender"].ToString();
                                textBox3.Text = reader["IsActive"].ToString();
                                textBox5.Text = reader["Region"].ToString();
                                textBox6.Text = reader["Badges"].ToString();
                                textBox7.Text = reader["NumberOfPokemon"].ToString();
                                textBox8.Text = reader["Email"].ToString();
                                textBox10.Text = reader["Password"].ToString();

                                // Repeat for other columns as needed
                            }
                            else
                            {
                                // Handle case when no records are found
                                MessageBox.Show("Trainer not found");
                            }
                        }
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show("Error connecting to the database: " + ex.Message);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            UpdateTrainerData();

        }

        private void UpdateTrainerData()
        {
            using (MySqlConnection conn = Connect.GetConnection())
            {
                try
                {
                    conn.Open();

                    string updateQuery = "UPDATE trainers SET TrainerName = @trainerName, Address = @address, Gender = @gender, IsActive = @isActive, Region = @region, Badges = @badges, NumberOfPokemon = @numberOfPokemon WHERE TrainerID = @trainerID";

                    using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@trainerName", textBox9.Text);
                        updateCmd.Parameters.AddWithValue("@address", textBox1.Text);
                        updateCmd.Parameters.AddWithValue("@gender", textBox2.Text);
                        updateCmd.Parameters.AddWithValue("@isActive", textBox3.Text);
                        updateCmd.Parameters.AddWithValue("@region", textBox5.Text);
                        updateCmd.Parameters.AddWithValue("@badges", textBox6.Text);
                        updateCmd.Parameters.AddWithValue("@numberOfPokemon", textBox7.Text);
                        updateCmd.Parameters.AddWithValue("@trainerID", textBox4.Text);

                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Trainer data updated successfully");
                        }
                        else
                        {
                            MessageBox.Show("No rows were updated. Trainer not found or values unchanged.");
                        }
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show("Error connecting to the database: " + ex.Message);
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }
    }
}