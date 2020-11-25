using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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
        string specialSymbols = "!@#$%^&*()_+=-}{[]|;<,>.?/";
        string unsafeChars = "‰‡ƒ…†ŠŒ•œ™Ÿ¡¥§µ¶¸¼½¾Ñÿþý";
        string genString = "";
        List<string> generatedPasswords = new List<string>();
        byte[] key = { 7, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };

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

        public static void returnEncrypted(string raw)
        {
            try
            {
                using (AesManaged aes = new AesManaged())
                {
                    byte[] encrypted = Encrypt(raw, aes.Key, aes.IV);
                    MessageBox.Show(System.Text.Encoding.UTF8.GetString(encrypted));
                }
            }
            catch { }
        }

        private static byte[] Encrypt(string raw, object p1, object p2)
        {
            throw new NotImplementedException();
        }

        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
            {
                csp.KeySize = 256;
                csp.BlockSize = 128;
                csp.Key = key;
                csp.Padding = PaddingMode.PKCS7;
                csp.Mode = CipherMode.ECB;
                ICryptoTransform encrypter = csp.CreateEncryptor();
                return encrypter.TransformFinalBlock(data, 0, data.Length);
            }
        }

        private static string getString(byte[] b)
        {
            return Encoding.UTF8.GetString(b);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists("neplosData.db"))
                File.Create("neplosData.db");

            filePath = File.ReadAllText("neplosData.db");

            string[] lines = File.ReadAllLines("neplosData.db");

            if (lines != null)
            {
                if (filePath.Contains(":"))
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
            if ((textBox6.Text == "" || textBox7.Text == ""))
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
            fileName = "neplosData.db";
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

                        {
                            sw.WriteLine(service + ":" + username + ":" + password + ":", i);
                        }
                    }
                    catch { }
                }
            }
        }

        private void neplosButton8_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void neplosButton3_Click(object sender, EventArgs e)
        {
            foreach (var gen in generatedPasswords)
            {

            }
        }

        private void neplosButton5_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.UTF8.GetBytes(textBox8.Text);
            byte[] enc = Encrypt(data, key);

            MessageBox.Show(getString(enc));
        }
    }
}
