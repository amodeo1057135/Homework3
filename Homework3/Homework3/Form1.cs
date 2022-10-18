using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Text;

namespace Homework3
{
    public partial class Form1 : Form
    {
        public DataTable csvTable;

        public Form1()
        {
            InitializeComponent();
            richTextBox1.HideSelection = false;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private static DataTable GenerateDataTable(string fileName, bool firstRowContainsFieldNames = true)
        {
            DataTable result = new DataTable();

            if (fileName == "")
            {
                return result;
            }

            string delimiters = ",";
            string extension = Path.GetExtension(fileName);

            if (extension.ToLower() == "txt")
                delimiters = "\t";
            else if (extension.ToLower() == "csv")
                delimiters = ",";

            using (TextFieldParser tfp = new TextFieldParser(fileName))
            {
                tfp.SetDelimiters(delimiters);

                // Get The Column Names
                if (!tfp.EndOfData)
                {
                    string[] fields = tfp.ReadFields();

                    for (int i = 0; i < fields.Count(); i++)
                    {
                        if (firstRowContainsFieldNames)
                            result.Columns.Add(fields[i]);
                        else
                            result.Columns.Add("Col" + i);
                    }

                    // If first line is data then add it
                    if (!firstRowContainsFieldNames)
                        result.Rows.Add(fields);
                }

                // Get Remaining Rows from the CSV
                while (!tfp.EndOfData)
                    result.Rows.Add(tfp.ReadFields());
            }

            return result;
        }

        //parse file button
        private void button1_Click(object sender, EventArgs e) //parse button
        {
            if (csvTable != null)
            {

                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = csvTable;
            }
            else
            {
                MessageBox.Show("No file selected!");
            }
        }

        private void button2_Click(object sender, EventArgs e) //select button
        {
            int size = -1;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                try
                {
                    csvTable = GenerateDataTable(file, true);
                    label1.Text = file;
                }
                catch (IOException)
                {
                    Console.WriteLine("Error: can't open file");
                }
            }
            Console.WriteLine(size);
            Console.WriteLine(result);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) //clear button
        {
            csvTable.Clear();
            label1.Text = "Select a CSV File...";
        }

        private void button4_Click(object sender, EventArgs e) //generate distribution button
        {
           var pkgLenght = csvTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("Length"))
               .Select(r => new
               {
                   Str = r.Key,
                   Count = r.Count()
               });
            foreach (var item in pkgLenght)
            {
                richTextBox1.AppendText($"Lenght {item.Str} = {item.Count}\n");
            }
        }
    }
}