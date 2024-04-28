using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox3.PasswordChar = '*';
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
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

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Assuming you have usernameTextBox and passwordTextBox for user input
            string email = textBox1.Text.Trim(); // Assuming email is used as the username
            string password = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password");
                return; // Stop further execution
            }

            using (MySqlConnection conn = Connect.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Check if the provided email and password exist in the database and if the user is active
                    string query = "SELECT t.IsActive FROM users u " +
                                   "LEFT JOIN trainers t ON u.UserId = t.UserId " +
                                   "WHERE u.Email = @email AND u.Password = @password";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", password);

                        object isActiveObj = cmd.ExecuteScalar();

                        if (isActiveObj != null && isActiveObj != DBNull.Value)
                        {
                            string isActive = isActiveObj.ToString();

                            if (isActive == "Active")
                            {
                                MessageBox.Show("Login successful");

                                // Continue with any additional code related to the successful login
                                this.Hide();
                                Form2 form2 = new Form2();
                                form2.Show();
                            }
                            else
                            {
                                MessageBox.Show("User is not active. Please contact your administrator.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid email or password");
                        }
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show("Error connecting to the database: " + ex.Message);
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form4 form4 = new Form4();
            form4.Show();
        }
    }
}

