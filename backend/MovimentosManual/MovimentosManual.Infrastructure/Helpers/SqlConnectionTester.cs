using System;
using Microsoft.Data.SqlClient;

namespace MovimentosManual.Infrastructure.Helpers
{
    public static class SqlConnectionTester
    {
        /// <summary>
        /// Testa se a conexão com a string fornecida pode ser aberta com sucesso.
        /// </summary>
        /// <param name="connectionString">A string de conexão a ser testada.</param>
        /// <returns>True se a conexão foi bem-sucedida; caso contrário, False.</returns>
        public static bool Test(string? connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                return false;

            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"[SqlConnectionTester] SQL ERROR: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SqlConnectionTester] ERROR: {ex.Message}");
                return false;
            }
        }
    }
}
