using System;
using System.Data;
using System.Data.SqlClient;

namespace CafeDatabaseExample
{
    class Program
    {
        // ������ ����������� � ���� ������
        static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Git\cafe2.mdf;Integrated Security=True;Connect Timeout=30";

        static void Main(string[] args)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("����������� � ���� ������ �������!");

                    // 1. ������� �������, ���� ��� �� ����������
                    CreateTableIfNotExists(connection);

                    // 2. ��������� ������ ������
                    InsertData(connection, "Coffee", 150);

                    // 3. ������ � ������� ������ �� �������
                    ReadData(connection);

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("������: " + ex.Message);
            }

            Console.WriteLine("������� ����� ������� ��� ������...");
            Console.ReadKey();
        }

        static void CreateTableIfNotExists(SqlConnection conn)
        {
            string query = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
                CREATE TABLE Products (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(100) NOT NULL,
                    Price INT NOT NULL
                )";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("������� 'Products' ������� ��� ��� ����������.");
            }
        }

        static void InsertData(SqlConnection conn, string name, int price)
        {
            string query = "INSERT INTO Products (Name, Price) VALUES (@name, @price)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@price", price);
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine($"��������� �������: {rows}");
            }
        }

        static void ReadData(SqlConnection conn)
        {
            string query = "SELECT Id, Name, Price FROM Products";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.WriteLine("������ �� ������� Products:");
                while (reader.Read())
                {
                    Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}, Price: {reader["Price"]}");
                }
            }
        }
    }
}
