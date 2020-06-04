using System; //Ip-81-304352 ip-81-007
using System.Data;
using System.Data.SqlClient;

namespace zova1
{
    class Program
    {
        static void test(string[] args)
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=userdb;Integrated Security=True;";

            Console.WriteLine("1. Добавить запись \n2. Изменить запись \n3.Удалить запись \n4. Запросы");
            int key = int.Parse(Console.ReadLine());
            switch (key)
            {
                case 1:
                    {
                        Console.Write("Имя доктора");
                        string name = Console.ReadLine();
                        Console.Write("ID пациента");
                        int patientID = int.Parse(Console.ReadLine());
                        Console.Write("Номер кабинета");
                        int cabinet = int.Parse(Console.ReadLine());
                        Console.Write("Дата");
                        string data = Console.ReadLine();
                        Console.Write("Время");
                        string time = Console.ReadLine();


                        string sqlExpression7 = String.Format("INSERT INTO appointments (docName, patientID, cabinet,time) VALUES ('{0}',{1},{2},'{3}')", name, patientID, cabinet, time);
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlExpression7, connection);
                            int number = command.ExecuteNonQuery();
                            Console.WriteLine("Добавлено объектов: {0}", number);

                        }
                        Console.Read();
                        break;
                    }
                case 2:
                    {
                        Console.Write("ID записи: ");
                        int ID = int.Parse(Console.ReadLine());
                        Console.Write("Изменить время: ");
                        string time = Console.ReadLine();
                        string sqlExpression17 = String.Format("UPDATE appointments SET time = '{0}' WHERE Id = {1}",time,ID);
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlExpression17, connection);
                            int number = command.ExecuteNonQuery();
                            Console.WriteLine("Changed: {0}", number);

                        }
                        break;
                    }
                case 3:
                    {
                        Console.Write("ID записи");
                        int ID = int.Parse(Console.ReadLine());
                        string sqlExpression7 = String.Format("DELETE FROM appointments WHERE Id = {0}", ID);
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlExpression7, connection);
                            int number = command.ExecuteNonQuery();
                            Console.WriteLine("Удалено: {0}", number);

                        }
                        break;
                    }
                case 4:
                    break;
            }

                

           
            Console.WriteLine("1. Все поля из таблицы пациентов");
            string sqlExpression = "SELECT * FROM patients";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) 
                {
       
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", reader.GetName(0), reader.GetName(1), reader.GetName(2),reader.GetName(3));

                    while (reader.Read()) // построчно считываем данные
                    {
                        object name = reader.GetValue(0);
                        object cardNumber = reader.GetValue(1);
                        object adress = reader.GetValue(2);
                        object procedureID = reader.GetValue(3);

                        Console.WriteLine("{0} \t{1} \t{2} \t{3}", name, cardNumber, adress, procedureID);
                    }
                }

                reader.Close();
            }

            Console.Read();
            

Console.WriteLine("2. Все поля из таблицы медикаментов");

            string sql = "SELECT * FROM medicines";
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
 
        DataTable dt = ds.Tables[0];
 
        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
        adapter.Update(ds);
        ds.Clear();
        adapter.Fill(ds);
 
        foreach (DataColumn column in dt.Columns)
            Console.Write("\t{0}", column.ColumnName);
        Console.WriteLine();

        foreach (DataRow row in dt.Rows)
        {
       
            var cells = row.ItemArray;
            foreach (object cell in cells)
                Console.Write("\t{0}", cell);
            Console.WriteLine();
        }
    }
    Console.Read();


            Console.WriteLine("3. Join таблиц");
            string sqlExpression11 = "SELECT patients.name, procedures.name from patients INNER JOIN procedures on procedureID=procedures.ID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression11, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t", reader.GetName(0), reader.GetName(1));

                    while (reader.Read())
                    {
                        object patientName = reader.GetValue(0);
                        object procedureName = reader.GetValue(1);

                        Console.WriteLine("{0} \t{1}", patientName, procedureName);
                    }
                }

                reader.Close();
            }

            Console.Read();

             
Console.WriteLine("4. Записи в поликлинику");
                string sqlExpression76 = "SELECT * FROM appointments";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression76, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3), reader.GetName(4));

                    while (reader.Read()) // построчно считываем данные
                    {
                        object docName = reader.GetValue(0);
                        object patientID = reader.GetValue(1);
                        object cabinet = reader.GetValue(2);
                        object date = reader.GetValue(3);
                        object time = reader.GetValue(4);

                        Console.WriteLine("{0} \t{1} \t{2} \t{3} \t{4}", docName, patientID, cabinet, date, time);
                    }
                }

                reader.Close();
            }

            Console.Read();


            Console.WriteLine("5. Where");
            string sqlExpression112 = "SELECT docName, cabinet FROM appointments WHERE cabinet > 5";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression112, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t", reader.GetName(0), reader.GetName(1));

                    while (reader.Read()) 
                    {
                        object docName = reader.GetValue(0);
                        object cabinet = reader.GetValue(1);

                        Console.WriteLine("{0} \t{1}", docName, cabinet);
                    }
                }

                reader.Close();
            }

            Console.Read();

Console.WriteLine("6. Колличество записей в таблице медикаментов");
Console.WriteLine("7. Максимальная цена медикамента");
string sqlExpression4 = "SELECT COUNT(*) FROM medicines";
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    SqlCommand command = new SqlCommand(sqlExpression4, connection);
    object count = command.ExecuteScalar();
 
    command.CommandText = "SELECT MIN(price) FROM medicines";
    object minAge = command.ExecuteScalar();
 
    Console.WriteLine("В таблице {0} объектов", count);
    Console.WriteLine("Минимальная цена: {0}", minAge);
}

Console.WriteLine("8. Order By упорядочивание");
    string sql1 = "SELECT * FROM medicines ORDER BY price";
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        SqlDataAdapter adapter = new SqlDataAdapter(sql1, connection);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
 
        DataTable dt = ds.Tables[0];
 
        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
        adapter.Update(ds);

        ds.Clear();
        adapter.Fill(ds);
 
        foreach (DataColumn column in dt.Columns)
            Console.Write("\t{0}", column.ColumnName);
        Console.WriteLine();
        foreach (DataRow row in dt.Rows)
        {
            var cells = row.ItemArray;
            foreach (object cell in cells)
                Console.Write("\t{0}", cell);
            Console.WriteLine();
        }
    }
Console.WriteLine("9. Время процедур от 10 до 40 минут");
    string sqlExpression66 = "SELECT name,time FROM procedures WHERE time>10 AND time<40";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression66, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t", reader.GetName(0), reader.GetName(1));

                    while (reader.Read())
                    {
                        object name = reader.GetValue(0);
                        object time = reader.GetValue(1);

                        Console.WriteLine("{0} \t{1}", name, time);
                    }
                }

                reader.Close();
            }

            Console.Read();

Console.WriteLine("10. Уникальные номера кабинетов");
    
            string sql9 = "SELECT DISTINCT cabinet FROM appointments ORDER BY cabinet";
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        SqlDataAdapter adapter = new SqlDataAdapter(sql9, connection);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
 
        DataTable dt = ds.Tables[0];

        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
        adapter.Update(ds);

        ds.Clear();
        adapter.Fill(ds);
 
        foreach (DataColumn column in dt.Columns)
            Console.Write("\t{0}", column.ColumnName);
        Console.WriteLine();
 
        foreach (DataRow row in dt.Rows)
        {
        
            var cells = row.ItemArray;
            foreach (object cell in cells)
                Console.Write("\t{0}", cell);
            Console.WriteLine();
        }
    }
    Console.Read();

        }
    }
}
