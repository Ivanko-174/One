using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using static CIPHER.Вход;
using static CIPHER.Шифратор;
using CIPHER;
using CIPHER_WF;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CIPHER_WF
{
    public partial class Дешифратор : Form
    {
        DataTable dss = new DataTable();
        DataSet ds;
        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;
        string connectionString = CIPHER.Connection.connectionString;
        string sql = "SELECT * FROM ciphers";
        string sqlid = ("SELECT * FROM ciphers where users = " + Вход.id);


        public void DATAGRIDED(string sqly)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sqly, connection);



                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns["Id"].ReadOnly = true;
            }
        }





        public Дешифратор()
        {
            InitializeComponent();

            DATAGRIDED(sqlid);
        }

        private void button1_Click(object sender, EventArgs e) //Это метод переворачивания текста наоборот
        {                                                       //По сути, обычный Reversed, но расписанный
            string message = textBox1.Text;

            StringBuilder sb = new StringBuilder(message.Length);   //СтрингБилдер для удобства преобразования стринга

            for (int i = message.Length; i-- != 0;)     //Сама операция переворота
            {
                sb.Append(message[i]);
            }
            textBox3.Text = sb.ToString();  //Вывод реверсивной строки

            ///////////////////////////////////////////////////////////////////////////////////////////////////try


            if (textBox3.Text.Length == textBox1.Text.Length)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        //string ent = "select count(*) from ciphers where text = @text";
                        string ent = "insert into ciphers (users, text, result, type, alphabet, keyw) values (@users, @text, @result, @type, @alphabet, @keyw)";
                        SqlCommand command = new SqlCommand(ent, connection);
                        command.Parameters.AddWithValue("@users", Вход.id);
                        command.Parameters.AddWithValue("@text", textBox1.Text);
                        command.Parameters.AddWithValue("@result", textBox3.Text);
                        command.Parameters.AddWithValue("@type", "Реверс");
                        command.Parameters.AddWithValue("@alphabet", textBox2.Text);
                        command.Parameters.AddWithValue("@keyw", "Задом наперед");


                        //Connection.AddCipherAsync(Вход.id, textBox1.Text, textBox3.Text, "Реверсивный", textBox2.Text);
                        command.ExecuteNonQuery();

                        DATAGRIDED(sqlid);

                        //Application.Run(new Шифратор());
                        //this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            if (textBox3.Text.Length != textBox1.Text.Length)
            {
                MessageBox.Show("Все символы, что вы шифруете должны присутствовать в алфавите, на основе которого вы вписываете текст (ключ тоже (для виженера))");
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////try

        }


        private void button2_Click(object sender, EventArgs e) //Метод Цезаря
        {                                                     //Каждый символ смещается на 3 буквы вперед по алфавиту
            string alphabet = textBox2.Text;
            
            if (alphabet == "английский" || alphabet == "eng")
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz";
            }
            if (alphabet == "русский")
            {
                alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            }
            int alphcountdown = alphabet.Length;
            string message = textBox1.Text;
            alphabet += alphabet;
            alphabet += alphabet;


            if (Convert.ToInt32(textBox5.Text) > alphcountdown || Convert.ToInt32(textBox5.Text) < 1)
            {
                MessageBox.Show("Число в ключе для Цезаря не должно превышать кол-во символов в алфавите и не должно быть меньше 1!");
            }

            if (Convert.ToInt32(textBox5.Text) <= alphcountdown & Convert.ToInt32(textBox5.Text) > 0)
            {


                string output = "";   //Это будет вывод
                alphabet = alphabet.ToLower();
                message = message.ToLower();
                for (int i = 0; i < message.Length; i++)    //Пробегаемся по каждому символу сообщения
                {

                    if (message[i] == ' ')
                    {
                        output += message[i];
                    }

                    for (int j = alphcountdown; j < alphabet.Length; j++)   //Пробегаемся по алфавиту
                    {
                        if (message[i] == alphabet[j])  //Если этот символ текста совпадает с символом алфавита
                        {
                            //output += alphabet[j - 3];
                            output += alphabet[alphcountdown + j - Convert.ToInt32(textBox5.Text)];  //То он заменяется на другой символ алфавита, который находится на 3 итерации впереди
                            break;
                        }
                    }
                }
                textBox3.Text = output;

                if (textBox3.Text.Length == textBox1.Text.Length)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            string ent = "insert into ciphers (users, text, result, type, alphabet, keyw) values (@users, @text, @result, @type, @alphabet, @keyw)";
                            SqlCommand command = new SqlCommand(ent, connection);
                            command.Parameters.AddWithValue("@users", Вход.id);
                            command.Parameters.AddWithValue("@text", textBox1.Text);
                            command.Parameters.AddWithValue("@result", textBox3.Text);
                            command.Parameters.AddWithValue("@type", "Цезарь");
                            command.Parameters.AddWithValue("@alphabet", textBox2.Text);
                            command.Parameters.AddWithValue("@keyw", "+n по алфавиту");


                            command.ExecuteNonQuery();

                            DATAGRIDED(sqlid);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
                if (textBox3.Text.Length != textBox1.Text.Length)
                {
                    MessageBox.Show("Все символы, что вы шифруете должны присутствовать в алфавите, на основе которого вы вписываете текст (ключ тоже (для виженера))");
                }

            }
        }

        private void button3_Click(object sender, EventArgs e) //Это метод Виженера
        {
            string alphabet = textBox2.Text;
            
            if (alphabet == "английский" || alphabet == "eng")
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz";
            }
            if (alphabet == "русский")
            {
                alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            }
            int alphcount = alphabet.Length;
            alphabet += alphabet;
            alphabet += alphabet;
            string message = textBox1.Text;
            string Key = textBox4.Text; char KeySymbol = '0';
            string output = "";
            alphabet = alphabet.ToLower();
            message = message.ToLower();

            //Текст -> кл.слово -> прогон по алфавиту букв текста -> прогон по алфавиту букв кл.слова -> запись по букве, сверяя с ключом -> результат
            int ii = 0; //для while ниже
            for (int i = 0; i < message.Length; i++)   //Просмотр строки введенного текста. Основное действие
            {

                if (message[i] == ' ')
                {
                    output += message[i];
                }

                while (ii < Key.Length)
                //for (int ii = 0; ii < Key.Length; ii++) //Это чтобы вычислять, на какой букве 
                {                                       //мы остановились в ключевом слове
                    KeySymbol = Key[ii];
                    for (int j = 0; j < alphabet.Length; j++)  //чтобы останавливаться на определенной букве алфавита для текста
                    {
                        if (message[i] == alphabet[j]) //соответствие буквы текста букве алфавита
                        {
                            for (int jj = 0; jj < alphabet.Length; jj++) //чтобы останавливаться на определенной букве алфавита для ключевого слова    я хочу заказать еще этих сосисок в тесте
                            {
                                if (KeySymbol == alphabet[jj]) //соответствие ключевой буквы букве алфавита
                                {
                                    output += alphabet[j - jj - 1 + alphcount];
                                    break;
                                }

                            }
                            break;
                        }

                    }
                    ii++;
                    if (ii == Key.Length)
                    { ii = 0; }
                    break;
                }
            }
            textBox3.Text = output;

            if (textBox3.Text.Length == textBox1.Text.Length)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string ent = "insert into ciphers (users, text, result, type, alphabet, keyw) values (@users, @text, @result, @type, @alphabet, @keyw)";
                        SqlCommand command = new SqlCommand(ent, connection);
                        command.Parameters.AddWithValue("@users", Вход.id);
                        command.Parameters.AddWithValue("@text", textBox1.Text);
                        command.Parameters.AddWithValue("@result", textBox3.Text);
                        command.Parameters.AddWithValue("@type", "Виженер");
                        command.Parameters.AddWithValue("@alphabet", textBox2.Text);
                        command.Parameters.AddWithValue("@keyw", textBox4.Text);

                        
                        command.ExecuteNonQuery();
                        DATAGRIDED(sqlid);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            if (textBox3.Text.Length != textBox1.Text.Length)
            {
                MessageBox.Show("Все символы, что вы шифруете должны присутствовать в алфавите, на основе которого вы вписываете текст (ключ тоже (для виженера))");
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            Главное_меню ГМ = new Главное_меню();
            ГМ.Show();
            this.Close();
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox5.KeyPress += new KeyPressEventHandler(textBox5_KeyPress);
        }
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Отменяем ввод нецифровых символов
            }
        }



        private void button4_Click(object sender, EventArgs e)
        {
            string alphabet = textBox2.Text;
            if (alphabet == "английский" || alphabet == "eng")
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz";
            }
            if (alphabet == "русский")
            {
                alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            }
            int alphcount = alphabet.Length;
            string message = textBox1.Text;
            alphabet += alphabet;

            string output = "";   //Это будет вывод
            alphabet = alphabet.ToLower();
            message = message.ToLower();







            for (int i = 0; i < message.Length; i++)    //Пробегаемся по каждому символу сообщения
            {

                if (message[i] == ' ')
                {
                    output += message[i];
                }

                for (int j = 0; j < alphabet.Length; j++)   //Пробегаемся по алфавиту
                {
                    if (message[i] == alphabet[j])  //Если этот символ текста совпадает с символом алфавита
                    {
                        //output += alphabet[j + 3];
                        output += alphabet[alphabet.Length - j - 1];  //То он заменяется на другой символ алфавита, который находится на 3 итерации впереди
                        break;
                    }
                }
            }
            textBox3.Text = output;

            if (textBox3.Text.Length == textBox1.Text.Length)
            {

            }
            if (textBox3.Text.Length != textBox1.Text.Length)
            {
                MessageBox.Show("Все символы, что вы шифруете должны присутствовать в алфавите, на основе которого вы вписываете текст (ключ тоже (для виженера))");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string alphabet = textBox2.Text;
            if (alphabet == "английский" || alphabet == "eng")
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz";
            }
            if (alphabet == "русский")
            {
                alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            }
            int alphcount = alphabet.Length;
            string message = textBox1.Text;
            alphabet += alphabet;

            string output = "";   //Это будет вывод
            alphabet = alphabet.ToLower();
            message = message.ToLower();


            for (int i = 0; i < message.Length; i++)    //Пробегаемся по каждому символу сообщения
            {

                if (message[i] == ' ')
                {
                    output += message[i];
                }



                string[] splitInput = message.Split(' ');
                StringBuilder decryptedText = new StringBuilder();
                int row = 0;

                foreach (string pair in splitInput)
                {
                    row = Convert.ToInt32(pair);
                    decryptedText.Append(alphabet[row]);
                }

                textBox3.Text = decryptedText.ToString();



                //for (int j = 0; j < alphabet.Length; j++)   //Пробегаемся по алфавиту
                //{

                //    if (message[i] == alphabet[j])  //Если этот символ текста совпадает с символом алфавита
                //    {
                //        //output += alphabet[j + 3];
                //        output += j + 1; output += " ";
                //        break;
                //    }
                //}
            }
            textBox3.Text = output;

            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string ToCopy = textBox4.Text;
            string ToInsert = "";

            for (int i = 0; i < ToCopy.Length; i++)
            {
                for (int j = 0; j < Convert.ToInt32(textBox5.Text); j++)
                {
                    ToInsert += ToCopy[i];
                }
            }
            textBox4.Text = ToInsert;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string alphabet = textBox2.Text;
            string message = textBox1.Text;
            if (alphabet == "английский" || alphabet == "eng")
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz";
            }
            if (alphabet == "русский")
            {
                alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            }


            var n = (int)Math.Ceiling(Math.Sqrt(alphabet.Length));

            char[,] square1 = new char[n, n];
            var index = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (index < alphabet.Length)
                    {
                        square1[j, i] = alphabet[index];
                        index++;
                    }
                }
            }


            string[] splitInput = message.Split(' ');
            StringBuilder decryptedText = new StringBuilder();

            foreach (string pair in splitInput)
            {
                int row = int.Parse(pair[0].ToString()) - 1;
                int col = int.Parse(pair[1].ToString()) - 1;
                decryptedText.Append(square1[row, col]);
            }

            textBox3.Text = decryptedText.ToString();
        }

        private void Шифратор_Load_2(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Description Desc = new Description();
            Desc.Show();
        }
    }
}
