using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp2
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
        }

        private void label7_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2
            Form2 form2 = new Form2();

            // Show Form2
            form2.Show();

            // Close the current form
            this.Hide();
            AddTrainer();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2
            Form2 form2 = new Form2();

            // Show Form2
            form2.Show();

            // Close the current form
            this.Hide();
        }

        // Add a new trainer to the database
        private void AddTrainer()
        {
            using (MySqlConnection conn = Connect.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Start a transaction to ensure atomicity
                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Prepare the SQL query to insert a new user
                            string userQuery = "INSERT INTO users (Email, Password) " +
                                               "VALUES (@Email, @Password); SELECT LAST_INSERT_ID();";

                            // Execute the user query and get the last inserted user ID
                            int userId;
                            using (MySqlCommand userCmd = new MySqlCommand(userQuery, conn, transaction))
                            {
                                userCmd.Parameters.AddWithValue("@Email", textBox8.Text);
                                userCmd.Parameters.AddWithValue("@Password", textBox10.Text); // You should hash the password for security

                                userId = Convert.ToInt32(userCmd.ExecuteScalar());
                            }

                            // Prepare the SQL query to insert a new trainer
                            string trainerQuery = "INSERT INTO trainers (UserID, TrainerName, IsActive, Gender, Region, Badges, Address, NumberOfPokemon) " +
                                                  "VALUES (@UserID, @TrainerName, @IsActive, @Gender, @Region, @Badges, @Address, @NumberOfPokemon)";

                            // Execute the trainer query with the retrieved user ID
                            using (MySqlCommand trainerCmd = new MySqlCommand(trainerQuery, conn, transaction))
                            {
                                trainerCmd.Parameters.AddWithValue("@UserID", userId);
                                trainerCmd.Parameters.AddWithValue("@TrainerName", textBox9.Text);
                                trainerCmd.Parameters.AddWithValue("@IsActive", textBox3.Text); // Assuming char field
                                trainerCmd.Parameters.AddWithValue("@Gender", textBox2.Text);   // Assuming char field
                                trainerCmd.Parameters.AddWithValue("@Region", textBox5.Text);
                                trainerCmd.Parameters.AddWithValue("@Badges", Convert.ToInt32(textBox6.Text)); // Assuming int field
                                trainerCmd.Parameters.AddWithValue("@Address", textBox1.Text);
                                trainerCmd.Parameters.AddWithValue("@NumberOfPokemon", Convert.ToInt32(textBox7.Text)); // Assuming int field

                                // Execute the trainer query
                                trainerCmd.ExecuteNonQuery();
                            }

                            // If everything is successful, commit the transaction
                            transaction.Commit();

                            // Display a success message or perform any additional actions
                            MessageBox.Show("Trainer added successfully!");
                        }
                        catch (Exception ex)
                        {
                            // An error occurred, rollback the transaction
                            transaction.Rollback();
                            MessageBox.Show("Error adding trainer: " + ex.Message);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error connecting to the database: " + ex.Message);
                }
            }
        }



        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.Enabled = false;

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

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}
