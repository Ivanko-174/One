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
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using CIPHER_WF;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlTypes;

namespace CIPHER
{

    public partial class Вход : Form
    {
        public static int id;
        string connectionString = Connection.connectionString;

        public Вход()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Register Reg = new Register();
            Reg.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void button2_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password2 = textBox2.Text;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            string message = textBox2.Text;

            StringBuilder sb = new StringBuilder(message.Length);   //СтрингБилдер для удобства преобразования стринга

            for (int i = message.Length; i-- != 0;)     //Сама операция переворота
            {
                sb.Append(message[i]);
            }
            string password = sb.ToString();  //Вывод реверсивной строки

            ////////////////////////



            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    try
            //    {
            //        connection.Open();
            //        string ent = "insert into users (password) values (@password)";
            //        SqlCommand command = new SqlCommand(ent, connection);
            //        command.Parameters.AddWithValue("@password", password);


            //        command.ExecuteNonQuery();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //    finally
            //    {
            //        connection.Close();
            //    }
            //}



            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //string password = textBox2.Text;

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                try
                {
                    cn.Open();
                    string ent = "select count(*) from users where login = @login and password = @password";
                    SqlCommand command = new SqlCommand(ent, cn);

                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);
                    int UserExists = (int)command.ExecuteScalar();
                    if (UserExists > 0)
                    {
                        Главное_меню CTIME = new Главное_меню();
                        MessageBox.Show("Connected!");

                        //var Iid = command.ExecuteScalarAsync();


                        string sqlExpression = "SELECT id FROM Users where login = @login and password = @password";
                        SqlCommand command2 = new SqlCommand(sqlExpression, cn);
                        command2.Parameters.AddWithValue("@login", login);
                        command2.Parameters.AddWithValue("@password", password);
                        int Iid = (int)command2.ExecuteScalar();



                        id = Iid;
                        Console.WriteLine($"Ваше id: {Iid}");

                        CTIME.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Неверное имя пользователя или пароль!", "Ошибка!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Not connected, error: {ex.Message}");
                }
            }
        }
    }
}
