using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CIPHER
{
    public partial class Register : Form
    {
        DataSet ds;
        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;
        string connectionString = Connection.connectionString;
        string sql = "SELECT * FROM Users";
        public Register()
        {
            InitializeComponent();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql, connection);

                ds = new DataSet();
                adapter.Fill(ds);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataRow row = ds.Tables[0].NewRow(); // добавляем новую строку в DataTable
            ds.Tables[0].Rows.Add(row);

            string login = textBox2.Text;



            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            string message = textBox3.Text;

            StringBuilder sb = new StringBuilder(message.Length);   //СтрингБилдер для удобства преобразования стринга

            for (int i = message.Length; i-- != 0;)     //Сама операция переворота
            {
                sb.Append(message[i]);
            }
            string password = sb.ToString();  //Вывод реверсивной строки

            ////////////////////////

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string ent = "insert into users (password) values (@password)";
                    SqlCommand command = new SqlCommand(ent, connection);
                    command.Parameters.AddWithValue("@password", password);


                    command.ExecuteNonQuery();
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



            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            if (textBox2.Text.Length >= 5 & textBox3.Text.Length >= 8)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string ent = "select count(*) from users where login = @login";
                        SqlCommand command = new SqlCommand(ent, connection);
                        command.Parameters.AddWithValue("@login", login);
                        int UserExists = (int)command.ExecuteScalar();
                        if (UserExists == 0)
                        {

                            //Connection.AddUserAsync(textBox1.Text, textBox2.Text, textBox3.Text);

                            try
                            {



                                //connection.Open();
                                //string ent = "select count(*) from ciphers where text = @text";
                                string entr = "insert into users (nick, login, password) values (@nick, @login, @password)";
                                SqlCommand commandr = new SqlCommand(entr, connection);
                                commandr.Parameters.AddWithValue("@nick", textBox1.Text);
                                commandr.Parameters.AddWithValue("@login", textBox2.Text);
                                commandr.Parameters.AddWithValue("@password", password);//textBox3.Text);

                                commandr.ExecuteNonQuery();

                                MessageBox.Show("Регистрация прошла успешно!");
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
                        if (UserExists != 0)
                        {
                            MessageBox.Show("Такой логин уже есть. Придумайте другой");
                        }

                        Application.Run(new Register());
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            if (textBox2.Text.Length < 5 || textBox3.Text.Length < 8)
            {
                MessageBox.Show("Логин должен содержать более 4 символов и пароль - больше 7 символов");
            }

        }

        //private void Register_Load(object sender, EventArgs e)
        //{
        //string connectionString = (@"Data Sourсe = DESKTOP-LH734KP\SQLEXPRESS;InitialCatalog = CIPHER; Integrated Security = True"); string sql = "select * from Users";  пока не надо--
        //string connectionString = (@"Data Sourсe = 335-09\SQLEXPRESS; Initial Catalog = u2; Integrated Security = True");
        //string insertQuery = "(INSERT INTO Users (Login, Password) VALUES (@login, @password)";
        //try
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //connection.Open();

        //connection.Close();
        //Console.WriteLine("Введите логин: ");
        //string login = Console.ReadLine();

        //Console.WriteLine("Введите полироль: ");
        //string password = Console.ReadLine();

        //using (SqlCommand command = new SqlCommand(insertQuery, connection))
        //{
        //    command.Parameters.AddWithValue("@Login", login);
        //    command.Parameters.AddWithValue("@Password", password);

        //    int rowsAffected = command.ExecuteNonQuery();

        //    Console.WriteLine($"{rowsAffected} строк добавлено в таблицу.");
        //}
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Ошибка: {ex.Message}");
        //}
        //Console.WriteLine("Нажмите любую клавишу чтобы ");
        //Console.ReadKey();
        //}
    }
}
