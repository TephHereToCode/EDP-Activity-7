// adding comment for pushing to main
﻿using System;
using System.Data;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using System.Linq;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {
        private string userID = "";

        public Form2()
        {
            InitializeComponent();
            DisplayTrainersData();

            // Attach event handlers
            textBox2.TextChanged += textBox2_TextChanged;

            // Create and configure the "Generate Report" button
            Button buttonGenerateReport = new Button();
            buttonGenerateReport.Text = "Generate Report";
            buttonGenerateReport.Location = new System.Drawing.Point(50, 40); // Adjust the position as needed
            buttonGenerateReport.Click += buttonGenerateReport_Click; // Attach the click event handler
            this.Controls.Add(buttonGenerateReport); // Add the button to the form
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            // Create an instance of Form6
            Form6 form6 = new Form6(userID);

            // Show Form6
            form6.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            // Create an instance of Form2
            Form3 form3 = new Form3();

            // Show Form2
            form3.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = this.dataGridView1.Rows[e.RowIndex];
                userID = selectedRow.Cells["TrainerID"].Value?.ToString();
            }
        }

        private void DisplayTrainersData()
        {
            using (MySqlConnection conn = Connect.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Select all columns from the Trainers table
                    string query = "SELECT TrainerID, TrainerName, IsActive, Gender, Address, Region, Badges, NumberOfPokemon FROM trainers";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dataTable;
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show("Error connecting to the database: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            // Get the search text from the TextBox
            string searchText = textBox2.Text.Trim();

            // Execute the query with the filter condition
            string query = $"SELECT TrainerID, TrainerName, IsActive, Gender, Address, Region, Badges, NumberOfPokemon FROM trainers WHERE TrainerName LIKE '%{searchText}%' OR TrainerID = '{searchText}'";

            using (MySqlConnection conn = Connect.GetConnection())
            {
                try
                {
                    conn.Open();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the filtered DataTable to the DataGridView
                        dataGridView1.DataSource = dataTable;
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show("Error connecting to the database: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Get the TrainerID or TrainerName from the TextBox
            string searchText = textBox2.Text.Trim();

            // Check if the searchText is a number (assumed to be TrainerID)
            if (int.TryParse(searchText, out int trainerID))
            {
                // Update IsActive to "Inactive" based on TrainerID
                UpdateIsActiveToInactive(trainerID);
            }
            else
            {
                // Update IsActive to "Inactive" based on TrainerName
                UpdateIsActiveToInactiveByName(searchText);
            }

            // Refresh the data in the DataGridView
            DisplayTrainersData();
        }

        private void UpdateIsActiveToInactive(int trainerID)
        {
            using (MySqlConnection conn = Connect.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Prepare the SQL query to update IsActive to "Inactive" by TrainerID
                    string query = "UPDATE trainers SET IsActive = 'Inactive' WHERE TrainerID = @TrainerID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Set the parameter value for TrainerID
                        cmd.Parameters.AddWithValue("@TrainerID", trainerID);

                        // Execute the query
                        cmd.ExecuteNonQuery();

                        // Display a success message or perform any additional actions
                        MessageBox.Show("Trainer status updated to Inactive!");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error updating trainer status: " + ex.Message);
                }
            }
        }

        private void UpdateIsActiveToInactiveByName(string trainerName)
        {
            using (MySqlConnection conn = Connect.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Prepare the SQL query to update IsActive to "Inactive" by TrainerName
                    string query = "UPDATE trainers SET IsActive = 'Inactive' WHERE TrainerName = @TrainerName";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Set the parameter value for TrainerName
                        cmd.Parameters.AddWithValue("@TrainerName", trainerName);

                        // Execute the query
                        cmd.ExecuteNonQuery();

                        // Display a success message or perform any additional actions
                        MessageBox.Show("Trainer status updated to Inactive!");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error updating trainer status: " + ex.Message);
                }
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();

        }

        private void buttonGenerateReport_Click(object sender, EventArgs e)
        {
            // Check if a trainer is selected
            if (string.IsNullOrEmpty(userID))
            {
                MessageBox.Show("Please select a trainer first.");
                return;
            }

            // Get the name of the selected trainer
            string trainerName = "";
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["TrainerID"].Value?.ToString() == userID)
                {
                    trainerName = row.Cells["TrainerName"].Value?.ToString();
                    break;
                }
            }

            try
            {
                // Load the template file
                string templatePath = @"C:\Users\Joseph\Documents\Custom Office Templates\Template.xltx";
                if (!System.IO.File.Exists(templatePath))
                {
                    MessageBox.Show("Template file not found at the specified path.");
                    return;
                }

                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = true;
                Excel.Workbook workbook = excelApp.Workbooks.Open(templatePath);

                // Get the counts of active and total trainers
                int activeTrainerCount = Connect.GetActiveTrainerCount();
                int totalTrainerCount = Connect.GetTotalTrainerCount();

                // Get the DataSet containing active trainers data
                DataSet activeTrainersDataSet = Connect.GetActiveTrainersData();

                // Extract the DataTable from the DataSet
                DataTable activeTrainersDataTable = activeTrainersDataSet.Tables[0];

                // Modify the existing sheets with data
                ModifyTemplateSheet(workbook, "Wins Report", $"No. of Wins of {trainerName}", Connect.GetWinsData(userID), activeTrainerCount, totalTrainerCount);
                ModifyTemplateSheet(workbook, "Active Trainers Report", "List of Active Trainers", activeTrainersDataTable, activeTrainerCount, totalTrainerCount);
                ModifyTemplateSheet(workbook, "Trainer Count Report", "Total Number of Trainers", Connect.GetTrainerCountData(), activeTrainerCount, totalTrainerCount);

                // Check if the ModifyTemplateSheet method is called for "Sheet1"
                Console.WriteLine("Modifying Sheet1...");
                ModifyTemplateSheet(workbook, "Sheet1", "Active vs Inactive Trainers", null, activeTrainerCount, totalTrainerCount);

                // Generate bar graphs for each report on Sheet1
                GenerateBarGraphs(workbook, "Wins Report", "Sheet1");
                GenerateBarGraphs(workbook, "Active Trainers Report", "Sheet1");
                GenerateBarGraphs(workbook, "Trainer Count Report", "Sheet1");

                // Save the workbook
                string savePath = @"C:\Users\Joseph\Documents\TrainerReports.xlsx";
                workbook.SaveAs(savePath);

                // Close the workbook
                workbook.Close();
                excelApp.Quit();
                releaseObject(workbook);
                releaseObject(excelApp);

                MessageBox.Show("Reports generated successfully and saved at: " + savePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report: " + ex.Message);
            }
        }





        private void GenerateBarGraphs(Excel.Workbook workbook, string reportSheetName, string graphSheetName)
        {
            // Retrieve the existing report worksheet by name
            Excel.Worksheet reportWorksheet = workbook.Sheets[reportSheetName] as Excel.Worksheet;

            // Get the "Sheet1" worksheet
            Excel.Worksheet graphWorksheet = workbook.Sheets[graphSheetName];

            // Add chart objects
            Excel.ChartObjects chartObjects = (Excel.ChartObjects)graphWorksheet.ChartObjects(Type.Missing);
            Excel.ChartObject chartObject = chartObjects.Add(100, 100, 300, 200);
            Excel.Chart chart = chartObject.Chart;

            // Set chart data range
            Excel.Range dataRange = reportWorksheet.Range[reportWorksheet.Cells[5, 1], reportWorksheet.Cells[reportWorksheet.UsedRange.Rows.Count, reportWorksheet.UsedRange.Columns.Count]];
            chart.SetSourceData(dataRange);

            // Set chart type to bar chart
            chart.ChartType = Excel.XlChartType.xlColumnClustered;

            // Set chart title
            chart.HasTitle = true;
            chart.ChartTitle.Text = reportSheetName;

            // Move chart to desired location on the "Sheet1" worksheet
            chartObject.Top = 20;
            chartObject.Left = 20;
        }

        private void ModifyTemplateSheet(Excel.Workbook workbook, string sheetName, string header, DataTable data, int activeTrainerCount, int totalTrainerCount)
        {
            // Retrieve the existing worksheet by name
            Excel.Worksheet worksheet = workbook.Sheets[sheetName] as Excel.Worksheet;

            // If the worksheet is found, populate it with data
            if (worksheet != null)
            {
                // Add header information
                worksheet.Cells[3, 1] = header;

                // Add data if available
                if (data != null)
                {
                    int rowCount = data.Rows.Count;
                    int columnCount = data.Columns.Count;

                    // Add column headers
                    for (int i = 0; i < columnCount; i++)
                    {
                        worksheet.Cells[5, i + 1] = data.Columns[i].ColumnName;
                    }

                    // Add data rows
                    for (int i = 0; i < rowCount; i++)
                    {
                        for (int j = 0; j < columnCount; j++)
                        {
                            worksheet.Cells[i + 6, j + 1] = data.Rows[i][j].ToString();
                        }
                    }

                    // Resize the range to accommodate the data
                    Excel.Range range = worksheet.Range[worksheet.Cells[5, 1], worksheet.Cells[rowCount + 5, columnCount]];
                    range.Columns.AutoFit();
                }
            }

            // Add a bar graph for active and inactive trainers in "Sheet1"
            if (sheetName == "Sheet1")
            {
                GenerateTrainerCountBarGraph(workbook, worksheet, activeTrainerCount, totalTrainerCount);
            }
        }





        private void GenerateTrainerCountBarGraph(Excel.Workbook workbook, Excel.Worksheet worksheet, int activeTrainerCount, int totalTrainerCount)
        {
            // Add chart objects
            Excel.ChartObjects chartObjects = (Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
            Excel.ChartObject chartObject = chartObjects.Add(400, 100, 600, 400); // Adjust size and position as needed
            Excel.Chart chart = chartObject.Chart;

            // Create a new data series for active and inactive trainers
            Excel.Series seriesActive = chart.SeriesCollection().NewSeries();
            Excel.Series seriesInactive = chart.SeriesCollection().NewSeries();

            // Create arrays for the data and categories
            object[] values = { activeTrainerCount, totalTrainerCount - activeTrainerCount };
            object[] categories = { "Active", "Inactive" };

            // Add data points for active and inactive counts
            seriesActive.Values = new object[] { activeTrainerCount };
            seriesInactive.Values = new object[] { totalTrainerCount - activeTrainerCount };
            seriesActive.XValues = new object[] { "Active" };
            seriesInactive.XValues = new object[] { "Inactive" };

            // Set chart type to clustered column
            chart.ChartType = Excel.XlChartType.xlColumnClustered;

            // Set chart title
            chart.HasTitle = true;
            chart.ChartTitle.Text = "Active vs Inactive Trainers";

            // Set axis titles
            chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary).HasTitle = true;
            chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary).AxisTitle.Text = "Trainer Status";
            chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary).HasTitle = true;
            chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary).AxisTitle.Text = "Count";

            // Set legend
            chart.HasLegend = true;
            chart.Legend.Position = Excel.XlLegendPosition.xlLegendPositionBottom;

            // Set series names
            seriesActive.Name = "Active";
            seriesInactive.Name = "Inactive";

        }








        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occurred while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
