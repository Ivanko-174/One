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

namespace CIPHER
{
    public class Connection
    {
        public static string connectionString = @"Data Source=DESKTOP-LH734KP\SQLEXPRESS; Initial Catalog = CIPHER; Integrated Security=True";
        //public static string connectionString = @"Data Source=510-03; Initial Catalog = CIPHER; Integrated Security=True";

        public static async Task AddUserAsync(string nick, string login, string password)
        {
            // название процедуры
            string sqlExpression = "sp_CreateUser";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = CommandType.StoredProcedure;
                // параметр для ввода имени
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@nick",
                    Value = nick
                };
                // добавляем параметр
                command.Parameters.Add(nameParam);
                // параметр для ввода возраста
                SqlParameter ageParam = new SqlParameter
                {
                    ParameterName = "@login",
                    Value = login
                };
                command.Parameters.Add(ageParam);

                SqlParameter passParam = new SqlParameter
                {
                    ParameterName = "@password",
                    Value = password
                };
                command.Parameters.Add(passParam);

                // выполняем процедуру
                var id = await command.ExecuteScalarAsync();
                // если нам не надо возвращать id
                //var id = await command.ExecuteNonQueryAsync();

                Console.WriteLine($"Id добавленного объекта: {id}");
            }
        }

        public static async Task AddCipherAsync(int user, string text, string result, string type, string alphabet)
        {
            // название процедуры
            string sqlExpression = "sp_coded";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter userParam = new SqlParameter///////////////////////////////////////
                {
                    ParameterName = "@user",
                    Value = user
                };
                command.Parameters.Add(userParam);////////////////////////////////////////

                // параметр для ввода имени
                SqlParameter textParam = new SqlParameter
                {
                    ParameterName = "@text",
                    Value = text
                };
                // добавляем параметр
                command.Parameters.Add(textParam);
                // параметр для ввода возраста
                SqlParameter resParam = new SqlParameter
                {
                    ParameterName = "@result",
                    Value = result
                };
                command.Parameters.Add(resParam);

                SqlParameter typeParam = new SqlParameter
                {
                    ParameterName = "@type",
                    Value = type
                };
                command.Parameters.Add(typeParam);

                SqlParameter alphParam = new SqlParameter
                {
                    ParameterName = "@alphabet",
                    Value = alphabet
                };
                command.Parameters.Add(alphParam);

                // выполняем процедуру
                var id = await command.ExecuteScalarAsync();
                // если нам не надо возвращать id
                //var id = await command.ExecuteNonQueryAsync();

                Console.WriteLine($"Id добавленного объекта: {id}");
            }
        }
    }
}
