using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumoAutoQuery.Database
{
    internal class DatabaseRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["OracleDB"].ConnectionString;
        public DatabaseRepository() { }

        public List<string> ExistKeys(List<string> keys)
        {
            List<string> keysFound = new List<string>();

            try
            {
                using (var connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT CHAVE FROM TABELA WHERE CHAVE IN (:keys)";

                    using (var command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("keys", string.Join(",", keys.Select((key, index) => $":key{index}"))));
                        for (int i = 0; i < keys.Count; i++)
                        {
                            command.Parameters.Add(new OracleParameter($":key{i}", keys[i]));
                        }

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
