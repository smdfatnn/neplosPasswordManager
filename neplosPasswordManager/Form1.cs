using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace neplosPasswordManager
{
    public partial class Form1 : Form
    {
        string[] row;
        string hash;
        string fileName = "";
        string filePath = "";
        bool pwned = false;
        string lowercaseSymbols = "qwertyuiopasdfghjklzxcvbnm";
        string uppercaseSymbols = "QWERTYUIOPASDFGHJKLZXCVBNM";
        string numbers = "1234567890";
        string specialSymbols = "!@#$%^&*()_+=-}{[]|;:<,>.?/";
        string unsafeChars = "‰‡ƒ…†ŠŒ•œ™Ÿ¡¥§µ¶¸¼½¾Ñÿþý";
        string genString = "";
        List<string> generatedPasswords = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void neplosButton2_Click(object sender, EventArgs e)
        {
            using (AesManaged aes = new AesManaged())
            {
                byte[] enc = Encrypt(textBox6.Text, aes.Key, aes.IV);

                returnEncrypted(textBox6.Text);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void neplosButton1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Please fill text boxes with your credential information");
                return;
            }

            row = new string[] { textBox1.Text, textBox2.Text, hash, textBox3.Text, pwned.ToString() }; Thread.Sleep(500);
            dataGridView1.Rows.Add(row);
        }

        public static string returnEncrypted(string raw)
        {
            using (AesManaged aes = new AesManaged())
            {
                return Encoding.UTF8.GetString(Encrypt(raw, aes.Key, aes.IV));
            }
        }

        private static byte[] Encrypt(string raw, object p1, object p2)
        {
            throw new NotImplementedException();
        }

        static byte[] Encrypt(string plain, byte[] key, byte[] iv)
        {
            byte[] encrypted;

            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plain);
                            encrypted = ms.ToArray();
                        }
                    }
                }
            }

            return encrypted;
        }

        private void neplosButton5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;

                File.WriteAllText("data.txt", fileName);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists("data.txt"))
                File.Create("data.txt");

            filePath = File.ReadAllText("data.txt");
            string data = File.ReadAllText(filePath);
            string[] lines = File.ReadAllLines(filePath);
            if (data.Contains(":"))
            {
                foreach (var line in lines)
                {
                    string[] array = line.Split(':');

                    string service = array[0];
                    string username = array[1];
                    string password = array[2];
                    string hash = array[3];

                    row = new string[] { username, password, hash, service, "False" };
                    dataGridView1.Rows.Add(row);
                }
            }
        }

        public static string GeneratePassword(string finalString, string weird_length)
        {
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();

            int length = Convert.ToInt32(weird_length);

            while (0 < length--)
            {
                res.Append(finalString[rnd.Next(finalString.Length)]);
            }

            return res.ToString();
        }

        private void neplosButton2_Click_1(object sender, EventArgs e)
        {
            if((textBox6.Text == "" || textBox7.Text == ""))
            {
                MessageBox.Show("Please set-up your password generator!");
                return;
            }

            if (checkBox3.Checked)
                genString += uppercaseSymbols;

            if (checkBox4.Checked)
                genString += lowercaseSymbols;

            if (checkBox5.Checked)
                genString += numbers;

            if (checkBox6.Checked)
                genString += specialSymbols;

            string generated = GeneratePassword(genString, textBox9.Text);
            row = new string[] { textBox7.Text, generated, hash, textBox6.Text, pwned.ToString() };

            dataGridView1.Rows.Add(row);
            generatedPasswords.Add(generated);
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void neplosButton4_Click(object sender, EventArgs e)
        {
            if ((textBox6.Text == "" || textBox7.Text == ""))
            {
                MessageBox.Show("Please set-up your password generator!");
                return;
            }

            genString += unsafeChars;
            string generated_unsafe = GeneratePassword(genString, textBox9.Text);
            row = new string[] { textBox7.Text, generated_unsafe, hash, textBox6.Text, pwned.ToString() };

            dataGridView1.Rows.Add(row);
            generatedPasswords.Add(generated_unsafe);
        }

        private void neplosButton7_Click(object sender, EventArgs e)
        {
            if (fileName != "")
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    string username, password, service;
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        try
                        {
                            username = dataGridView1.Rows[i].Cells[0].Value.ToString();
                            password = dataGridView1.Rows[i].Cells[1].Value.ToString();
                            service = dataGridView1.Rows[i].Cells[3].Value.ToString();

                            //File.WriteAllText(fileName, service + ":" + username + ":" + password + ":");

                            {
                                sw.WriteLine(service + ":" + username + ":" + password + ":", i);
                            }
                        }
                        catch { }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please specify / create a file before saving the password");
                return;
            }
        }

        private void neplosButton8_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void neplosButton3_Click(object sender, EventArgs e)
        {
            foreach(var gen in generatedPasswords)
            {

            }
        }
    }
}
