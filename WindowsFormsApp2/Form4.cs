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
    public partial class Form4 : Form
    {
        private string userEmail; // to store the verified email

        public Form4()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string emailToVerify = textBox1.Text;

            if (VerifyEmailExists(emailToVerify))
            {
                userEmail = emailToVerify; // Remember the verified email
                this.Hide();
                Form5 form5 = new Form5(userEmail);
                form5.Show();
            }
            else
            {
                MessageBox.Show("Email does not exist. Please enter a valid email.");
            }
        }

        private bool VerifyEmailExists(string email)
        {
            using (MySqlConnection conn = Connect.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM users WHERE Email = @Email";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error verifying email: " + ex.Message);
                    return false;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}


