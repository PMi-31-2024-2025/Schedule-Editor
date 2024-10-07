using System;
using System.Data;
using Npgsql;

namespace ScheduleEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            // NOTE: Make sure to set your database connection string here with your own credentials
            string connectionString = "Host=localhost;Username=postgres;Password=your_password;Database=ScheduleEditor";

            // Display data from the Lecturer table
            Console.WriteLine("Displaying Lecturers:");
            DisplayData(connectionString, "SELECT * FROM Lecturer");

            // Display data from the Department table
            Console.WriteLine("\nDisplaying Departments:");
            DisplayData(connectionString, "SELECT * FROM Department");

            // Insert random data into the tables
            //Console.WriteLine("\nInserting random data...");
            //InsertRandomData(connectionString);

            //Console.WriteLine("\nRandom data inserted successfully!");
        }

        // Method to display data from the specified query
        static void DisplayData(string connectionString, string query)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    DataTable schemaTable = reader.GetSchemaTable();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{schemaTable.Rows[i]["ColumnName"]}: {reader[i]} ");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        // Method to insert random data into tables
        static void InsertRandomData(string connectionString)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                Random random = new Random();

                // Insert random data into the Lecturer table
                for (int i = 0; i < 30; i++)
                {
                    string insertLecturerQuery = "INSERT INTO Lecturer (name, department_id, total_hours, hours_teaching, hours_research, hours_others, rank) " +
                                                 "VALUES (@name, @department_id, @total_hours, @hours_teaching, @hours_research, @hours_others, @rank)";
                    using (var command = new NpgsqlCommand(insertLecturerQuery, connection))
                    {
                        command.Parameters.AddWithValue("@name", $"Lecturer_{i + 1}");
                        command.Parameters.AddWithValue("@department_id", random.Next(1, 4));  // Assuming we have 3 departments
                        command.Parameters.AddWithValue("@total_hours", random.Next(30, 50));
                        command.Parameters.AddWithValue("@hours_teaching", random.Next(10, 20));
                        command.Parameters.AddWithValue("@hours_research", random.Next(5, 10));
                        command.Parameters.AddWithValue("@hours_others", random.Next(5, 10));
                        command.Parameters.AddWithValue("@rank", "Professor");

                        command.ExecuteNonQuery();
                    }
                }

                // Insert random data into the Department table
                for (int i = 0; i < 3; i++)
                {
                    string insertDepartmentQuery = "INSERT INTO Department (name) VALUES (@name)";
                    using (var command = new NpgsqlCommand(insertDepartmentQuery, connection))
                    {
                        command.Parameters.AddWithValue("@name", $"Department_{i + 1}");
                        command.ExecuteNonQuery();
                    }
                }

                // Insert random data into the Subject table
                for (int i = 0; i < 30; i++)
                {
                    string insertSubjectQuery = "INSERT INTO Subject (name, specialization_id, credits, is_mandatory) " +
                                                "VALUES (@name, @specialization_id, @credits, @is_mandatory)";
                    using (var command = new NpgsqlCommand(insertSubjectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@name", $"Subject_{i + 1}");
                        command.Parameters.AddWithValue("@specialization_id", random.Next(1, 4));  // Assuming we have 3 specializations
                        command.Parameters.AddWithValue("@credits", random.Next(3, 5));
                        command.Parameters.AddWithValue("@is_mandatory", random.Next(0, 2) == 1);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
