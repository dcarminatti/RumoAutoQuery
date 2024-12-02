using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace RumoAutoQuery.Database
{
    internal class DatabaseRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["PostgresDB"].ConnectionString;

        public List<string> ExistKeys(List<string> keys)
        {
            List<string> keysFound = new List<string>();

            try
            {
                using (var connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    string stringKeys = string.Join(",", keys.Select(key => $"'{key}'"));
                    string query = $"SELECT chave FROM chave_table WHERE CHAVE IN ({stringKeys})";

                    using (var command = new OracleCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                keysFound.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyLogEvent.Writer("An error occurred while trying to query the database;");
                MyLogEvent.Writer($"Error: {ex.Message};");
            }

            return keysFound;
        }
    }
}
