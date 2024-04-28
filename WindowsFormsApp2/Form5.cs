using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp2
{
    public partial class Form5 : Form
    {
        private string userEmail; // to store the email received from Form4

        public Form5(string email)
        {
            InitializeComponent();
            userEmail = email;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // You can handle any additional logic when the password text changes, if needed
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // You can handle any additional logic when the confirmation password text changes, if needed
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get the new password and confirmation password from the TextBoxes
            string newPassword = textBox3.Text;
            string confirmPassword = textBox1.Text;

            // Check if the passwords match
            if (newPassword == confirmPassword)
            {
                // Perform the password change logic here using userEmail
                UpdatePassword(userEmail, newPassword);

                MessageBox.Show("Password changed successfully!");
                this.Hide();
                Form1 form1 = new Form1();
                form1.Show();
            }
            else
            {
                MessageBox.Show("Passwords do not match. Please re-enter the passwords.");
            }
        }

        // Method to update the password in the database
        private void UpdatePassword(string email, string newPassword)
        {
            using (MySqlConnection conn = Connect.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Prepare the SQL query to update the password
                    string query = "UPDATE users SET Password = @Password WHERE Email = @Email";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Set the parameter values
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", newPassword);

                        // Execute the query
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error updating password: " + ex.Message);
                }
            }
        }
    }
}


