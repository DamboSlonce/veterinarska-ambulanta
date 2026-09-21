using System.Collections.Generic;
using System.Data.SqlClient;
using SlojPodataka.Klase;

namespace SlojPodataka
{
    public class LekDB
    {
        private readonly string _connectionString;

        public LekDB()
        {
            _connectionString = Konekcija.String;
        }

        public List<Lek> GetAll()
        {
            var lekovi = new List<Lek>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Lek", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lekovi.Add(new Lek
                    {
                        IDLeka = (int)reader["IDLeka"],
                        Naziv = reader["Naziv"].ToString(),
                        Proizvodjac = reader["Proizvodjac"].ToString(),
                        Jacina = reader["Jacina"].ToString(),
                        Doziranje = reader["Doziranje"].ToString(),
                        isDeleted = (bool)reader["isDeleted"]
                    });
                }
            }
            return lekovi;
        }

        public List<Lek> GetAllActiveLek()
        {
            var lekovi = new List<Lek>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Lek where isDeleted=0", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lekovi.Add(new Lek
                    {
                        IDLeka = (int)reader["IDLeka"],
                        Naziv = reader["Naziv"].ToString(),
                        Proizvodjac = reader["Proizvodjac"].ToString(),
                        Jacina = reader["Jacina"].ToString(),
                        Doziranje = reader["Doziranje"].ToString()
                    });
                }
            }
            return lekovi;
        }

        public void Insert(Lek lek)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Lek (Naziv, Proizvodjac, Jacina, Doziranje) VALUES (@Naziv, @Proizvodjac, @Jacina, @Doziranje)", conn);

                cmd.Parameters.AddWithValue("@Naziv", lek.Naziv);
                cmd.Parameters.AddWithValue("@Proizvodjac", lek.Proizvodjac);
                cmd.Parameters.AddWithValue("@Jacina", lek.Jacina);
                cmd.Parameters.AddWithValue("@Doziranje", lek.Doziranje);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Lek lek)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Lek SET Naziv=@Naziv, Proizvodjac=@Proizvodjac, Jacina=@Jacina, Doziranje=@Doziranje WHERE IDLeka=@IDLeka", conn);

                cmd.Parameters.AddWithValue("@IDLeka", lek.IDLeka);
                cmd.Parameters.AddWithValue("@Naziv", lek.Naziv);
                cmd.Parameters.AddWithValue("@Proizvodjac", lek.Proizvodjac);
                cmd.Parameters.AddWithValue("@Jacina", lek.Jacina);
                cmd.Parameters.AddWithValue("@Doziranje", lek.Doziranje);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Lek WHERE IDLeka=@IDLeka", conn);
                cmd.Parameters.AddWithValue("@IDLeka", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void SoftDelete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Lek SET isDeleted = 1 WHERE IDLeka = @IDLeka", conn);
                cmd.Parameters.AddWithValue("@IDLeka", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
