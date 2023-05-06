using Microsoft.Data.SqlClient;
using PROJECT_ES.Data;

namespace PROJECT_ES.Services;

public class CompetitionRepository
{
    public static List<Competition> getCompetition()
    {
        List<Competition> competitions = new List<Competition>();
        using (SqlConnection connection = new SqlConnection("Server=tcp:engesoft.database.windows.net,1433;Initial Catalog=X;Persist Security Info=False;User ID=engsoft;Password=dnaXHaP6W5KV2v4f;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
        {
            string sql = "SELECT * FROM Competition";
            SqlCommand command = new SqlCommand(sql, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Competition competition = new Competition();
                    competition.Id = Convert.ToInt32(reader["Id"]);
                    competition.Name = reader["Name"].ToString();
                    competition.Description = reader["Description"].ToString();
                    competition.data_inicio = Convert.ToDateTime(reader["data_inicio"].ToString());
                    competition.data_fim = Convert.ToDateTime(reader["data_fim"].ToString());
                    competition.n_participantes = Convert.ToInt32(reader["n_participantes"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        return competitions;
    }
}