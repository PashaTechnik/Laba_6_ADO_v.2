using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ConsoleTables;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql;


namespace Laba_6_ADO_v._2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            string StringConnection = "Host=localhost;Port=5432;Database=new_database;Username=admin;Password=1234";


            Console.WriteLine("1. Добавить запись \n2. Изменить запись \n3. Удалить запись \n4. Запросы");
            int key = int.Parse(Console.ReadLine());
            switch (key)
            {
                case 1:
                    {
                        Console.WriteLine("ID:");
                        int id = int.Parse(Console.ReadLine());
                        Console.WriteLine("Название Химиката:");
                        string name = Console.ReadLine();
                        Console.WriteLine("Вид Химиката:");
                        string kind = Console.ReadLine();
                        Console.WriteLine("Предназначение Химиката:");
                        string app = Console.ReadLine();
                        Console.WriteLine("Компания-Производитель:");
                        string company = Console.ReadLine();
                        Console.WriteLine("Количество:");
                        int quantity = int.Parse(Console.ReadLine());
                        Console.WriteLine("Цена:");
                        int price = int.Parse(Console.ReadLine());

                        string sql = String.Format("INSERT INTO \"chemical\" VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", 
                            id,name, kind, app, company, quantity, price);
                        
                        using (NpgsqlConnection connection = new NpgsqlConnection(StringConnection))
                        {
                            connection.Open();
                            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                            int number = command.ExecuteNonQuery();
                            Console.WriteLine("Добавлено объектов: {0}", number);
                        }
                        break;
                    }
                case 2:
                    {
                        Console.Write("ID Химиката: ");
                        int ID = int.Parse(Console.ReadLine());
                        Console.Write("Изменить цену: ");
                        int price = int.Parse(Console.ReadLine());
                        string sql1 = String.Format("UPDATE \"chemical\" SET price = '{0}' WHERE chemicalId = {1}",price,ID);
                        using (NpgsqlConnection connection = new NpgsqlConnection(StringConnection))
                        {
                            connection.Open();
                            NpgsqlCommand command = new NpgsqlCommand(sql1, connection);
                            int number = command.ExecuteNonQuery();
                            Console.WriteLine("Changed: {0}", number);
                        }
                        break;
                    }
                case 3:
                    {
                        Console.Write("ID Химиката:");
                        int ID = int.Parse(Console.ReadLine());
                        string sql2 = String.Format("DELETE FROM \"chemical\" WHERE chemicalId = {0}", ID);
                        using (NpgsqlConnection connection = new NpgsqlConnection(StringConnection))
                        {
                            connection.Open();
                            NpgsqlCommand command = new NpgsqlCommand(sql2, connection);
                            int number = command.ExecuteNonQuery();
                            Console.WriteLine("Удалено: {0}", number);

                        }
                        break;
                    }
                case 4:
                    break;
            }
            
            Console.WriteLine("1. Все поля из таблицы Химикатов");
            string sqlExpression = "SELECT * FROM chemical";
            using (NpgsqlConnection connection = new NpgsqlConnection(StringConnection))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sqlExpression, connection);
                NpgsqlDataReader reader = command.ExecuteReader();
                DisplayReadTable(reader);

                reader.Close();
            }
            
            Console.WriteLine("2. Все поля из таблицы Химикатов отсортированы по Цене");
            
            using (NpgsqlConnection connection = new NpgsqlConnection(StringConnection))
            {
                connection.Open();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("SELECT * FROM chemical ORDER BY price", connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                adapter.Update(ds);
                ds.Clear();
                adapter.Fill(ds);
                DisplayTable(dt);
                
            }
            
            Console.WriteLine("3. Только удобрения с таблицы Химикатов");
            
            using (NpgsqlConnection connection = new NpgsqlConnection(StringConnection))
            {
                connection.Open();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("SELECT * FROM chemical WHERE kindofchemical = 'Feltilizer'", connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                adapter.Update(ds);
                ds.Clear();
                adapter.Fill(ds);
                DisplayTable(dt);
            }

            Console.WriteLine("4. Групировка");
            
            string sql3 = "SELECT companymanufacturer, COUNT(*) AS CompanyCount FROM chemical GROUP BY companymanufacturer";
            using (NpgsqlConnection connection = new NpgsqlConnection(StringConnection))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql3, connection);
                NpgsqlDataReader reader = command.ExecuteReader();
                var t = new TablePrinter(reader.GetName(0), reader.GetName(1));
                while (reader.Read())
                {
                    object id = reader.GetValue(0);
                    object count = reader.GetValue(1);
                    t.AddRow(id, count);
                }
                t.Print();
                reader.Close();
            }
            
            Console.WriteLine("5.Запрос с Like");
            string sql4 = "SELECT * FROM chemical WHERE companymanufacturer LIKE 'U%'";
            using (NpgsqlConnection connection = new NpgsqlConnection(StringConnection))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql4, connection);
                NpgsqlDataReader reader = command.ExecuteReader();
                DisplayReadTable(reader);
                reader.Close();
            }
            
            Console.WriteLine("5.Запрос с Where Between");
            string sql5 = "SELECT * FROM \"chemical\" WHERE quantity BETWEEN 2000 AND 5000";
            using (NpgsqlConnection connection = new NpgsqlConnection(StringConnection))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql5, connection);
                NpgsqlDataReader reader = command.ExecuteReader();
                DisplayReadTable(reader);
                reader.Close();
            }

            Console.WriteLine("6.Join трех таблиц");
            string sql6 = "SELECT OD.OrderId,CompanyBuyer,OD.ChemicalId,OD.Quantity,OD.Price,name,KindOfChemical,Application,CompanyManufacturer " +
                          "FROM " +
                          "Orders " +
                          "INNER JOIN " +
                          "OrderDetails OD ON Orders.OrderId = OD.OrderId " +
                          "INNER JOIN Chemical C on OD.ChemicalId = C.ChemicalId";
            using (NpgsqlConnection connection = new NpgsqlConnection(StringConnection))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql6, connection);
                NpgsqlDataReader reader = command.ExecuteReader();
                var t = new TablePrinter(reader.GetName(0), reader.GetName(1), reader.GetName(2),reader.GetName(3), reader.GetName(4), reader.GetName(5),reader.GetName(6), reader.GetName(7), reader.GetName(8));
                while (reader.Read())
                {
                    t.AddRow(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2),reader.GetValue(3), reader.GetValue(4), reader.GetValue(5),reader.GetValue(6), reader.GetValue(7), reader.GetValue(8));
                }
                t.Print();

                reader.Close();
            }
            
            Console.WriteLine("7.Union двух таблиц");
            string sql7 = "SELECT * FROM Chemical UNION ALL SELECT * FROM Chemical";
            using (NpgsqlConnection connection = new NpgsqlConnection(StringConnection))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql7, connection);
                NpgsqlDataReader reader = command.ExecuteReader();
                DisplayReadTable(reader);
                reader.Close();
            }
            
            Console.WriteLine("8.Запрос с Case");
            string sql8 = "SELECT chemicalid, name, kindofchemical,application,companymanufacturer,quantity, " +
                          "CASE " +
                          "WHEN price >= 15 THEN 'Expensive' " +
                          "WHEN price < 15 THEN 'Cheap' " +
                          "END AS Price " +
                          "FROM Chemical";
            using (NpgsqlConnection connection = new NpgsqlConnection(StringConnection))
            {
                connection.Open();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql8, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                adapter.Update(ds);
                ds.Clear();
                adapter.Fill(ds);
                DisplayTable(dt);
                connection.Close();
            }
            
            
            string sql9 = "SELECT COUNT(*) FROM orders";
            using (NpgsqlConnection connection = new NpgsqlConnection(StringConnection))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql9, connection);
                object count = command.ExecuteScalar();
 
                command.CommandText = "SELECT MAX(price) FROM orderdetails";
                object maxPrice = command.ExecuteScalar();
 
                Console.WriteLine("9.Колличество заказов:");
                Console.WriteLine("В таблице {0} объектов", count);
                Console.WriteLine("10.Цена максимального заказа химиката");
                Console.WriteLine("Максимальная цена: {0}", maxPrice);
            }
            
            
            
        }
        
        

        public static void DisplayTable(DataTable dt)
        {
            IEnumerable<Tuple<int, string, string, string, string, string, string>> authors = 
                new Tuple<int, string, string, string, string, string, string>[]{};
            List<Tuple<int, string, string, string, string, string, string>> list = new List<Tuple<int, string, string, string, string, string, string>>();
            
            List<string> Names = new List<string>();
            
            foreach (DataColumn column in dt.Columns)
                Names.Add(column.ColumnName);
            
            foreach (DataRow row in dt.Rows)
            {
                var cells = row.ItemArray;
                Tuple<int, string, string, string, string, string, string> temp = new Tuple<int, string, string, string, string, string, string>(int.Parse($"{cells[0]}"), $"{cells[1]}", $"{cells[2]}", $"{cells[3]}", $"{cells[4]}", $"{cells[5]}", $"{cells[6]}");
                list.Add(temp);
            }
            authors = (IEnumerable<Tuple<int, string, string, string, string, string, string>>)list;
            
            Console.WriteLine(authors.ToStringTable(
                Names.ToArray(),
                a => a.Item1, a => a.Item2, a => a.Item3, a => a.Item4, a => a.Item5, a => a.Item6, a => a.Item7));
        }
        
        
        public static void DisplayReadTable(NpgsqlDataReader reader)
        {
            IEnumerable<Tuple<int, string, string, string, string, int, int>> authors = 
                new Tuple<int, string, string, string, string, int, int>[]{};
            List<Tuple<int, string, string, string, string, int, int>> list = new List<Tuple<int, string, string, string, string, int, int>>();
            
            List<string> Names = new List<string>(){reader.GetName(0), reader.GetName(1), reader.GetName(2),reader.GetName(3), reader.GetName(4), reader.GetName(5),reader.GetName(6)};
            
            
            while (reader.Read()) // построчно считываем данные
            {
                object id = reader.GetValue(0);
                object name = reader.GetValue(1);
                object kind = reader.GetValue(2);
                object app = reader.GetValue(3);
                object company = reader.GetValue(4);
                object quality = reader.GetValue(5);
                object price = reader.GetValue(6);
                
                Tuple<int, string, string, string, string, int, int> temp = new Tuple<int, string, string, string, string, int, int>((int)id, (string)name, (string)kind, (string)app, (string)company, (int)quality, (int)price);
                list.Add(temp);
                
            }
            authors = (IEnumerable<Tuple<int, string, string, string, string, int, int>>)list;
            
            Console.WriteLine(authors.ToStringTable(
                Names.ToArray(),
                a => a.Item1, a => a.Item2, a => a.Item3, a => a.Item4, a => a.Item5, a => a.Item6, a => a.Item7));
        }

    }
}