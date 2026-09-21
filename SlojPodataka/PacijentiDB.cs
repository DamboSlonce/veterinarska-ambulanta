using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SlojPodataka.Klase;

namespace SlojPodataka
{
    public class PacijentiDB
    {
        private readonly string _connectionString;

        public PacijentiDB()
        {
            _connectionString = Konekcija.String;
        }

        public List<Pacijenti> GetAll()
        {
            var pacijenti = new List<Pacijenti>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Pacijent", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pacijenti.Add(new Pacijenti
                    {
                        IDPacijenta = (int)reader["IDPacijenta"],
                        Ime = reader["Ime"].ToString().Trim(),
                        Vlasnik = reader["Vlasnik"].ToString().Trim(),
                        BrojCipa = reader["BrojCipa"].ToString().Trim(),
                        BrojTelefona = reader["BrojTelefona"].ToString().Trim(),
                        Pol = reader["Pol"].ToString().Trim(),
                        Alergije = reader["Alergije"].ToString().Trim(),
                        Sterilisan = reader["Sterilisan"] != DBNull.Value && (bool)reader["Sterilisan"],
                        TezinaKg = reader["TezinaKg"] != DBNull.Value ? (int)reader["TezinaKg"] : 0,
                        IDVeterinara = reader["IDVeterinara"] != DBNull.Value ? (int)reader["IDVeterinara"] : 0
                    });
                }
            }
            return pacijenti;
        }

        public void Insert(Pacijenti pacijent)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Pacijent (Ime, Vlasnik, BrojCipa, BrojTelefona, Pol, Alergije, Sterilisan, TezinaKg, IDVeterinara) 
                      VALUES (@Ime, @Vlasnik, @BrojCipa, @BrojTelefona, @Pol, @Alergije, @Sterilisan, @TezinaKg, @IDVeterinara)", conn);

                cmd.Parameters.AddWithValue("@Ime", pacijent.Ime);
                cmd.Parameters.AddWithValue("@Vlasnik", pacijent.Vlasnik);
                cmd.Parameters.AddWithValue("@BrojCipa", pacijent.BrojCipa);
                cmd.Parameters.AddWithValue("@BrojTelefona", pacijent.BrojTelefona);
                cmd.Parameters.AddWithValue("@Pol", pacijent.Pol);
                cmd.Parameters.AddWithValue("@Alergije", pacijent.Alergije ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Sterilisan", pacijent.Sterilisan);
                cmd.Parameters.AddWithValue("@TezinaKg", pacijent.TezinaKg);
                cmd.Parameters.AddWithValue("@IDVeterinara", pacijent.IDVeterinara);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Pacijenti pacijent)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Pacijent
                      SET Ime=@Ime, Vlasnik=@Vlasnik, BrojCipa=@BrojCipa, BrojTelefona=@BrojTelefona, Pol=@Pol, 
                          Alergije=@Alergije, Sterilisan=@Sterilisan, TezinaKg=@TezinaKg, IDVeterinara=@IDVeterinara 
                      WHERE IDPacijenta=@IDPacijenta", conn);

                cmd.Parameters.AddWithValue("@IDPacijenta", pacijent.IDPacijenta);
                cmd.Parameters.AddWithValue("@Ime", pacijent.Ime);
                cmd.Parameters.AddWithValue("@Vlasnik", pacijent.Vlasnik);
                cmd.Parameters.AddWithValue("@BrojCipa", pacijent.BrojCipa);
                cmd.Parameters.AddWithValue("@BrojTelefona", pacijent.BrojTelefona);
                cmd.Parameters.AddWithValue("@Pol", pacijent.Pol);
                cmd.Parameters.AddWithValue("@Alergije", pacijent.Alergije ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Sterilisan", pacijent.Sterilisan);
                cmd.Parameters.AddWithValue("@TezinaKg", pacijent.TezinaKg);
                cmd.Parameters.AddWithValue("@IDVeterinara", pacijent.IDVeterinara);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Pacijent WHERE IDPacijenta=@IDPacijenta", conn);
                cmd.Parameters.AddWithValue("@IDPacijenta", id);
                cmd.ExecuteNonQuery();
            }
        }

        public bool checkIfBrojCipaExists(string brojCipa)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM Pacijent WHERE BrojCipa = @brojCipa", conn);
                cmd.Parameters.AddWithValue("@brojCipa", brojCipa);
                SqlDataReader nadjen = cmd.ExecuteReader();
                return nadjen.Read();
            }
        }

        public Pacijenti findPacijent(string brojCipa)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM Pacijent WHERE BrojCipa = @brojCipa", conn);
                cmd.Parameters.AddWithValue("@brojCipa", brojCipa);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapPacijent(reader);
                    }
                }
            }
            return null;
        }

        public Pacijenti findPacijentByID(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM Pacijent WHERE IDPacijenta = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapPacijent(reader);
                    }
                }
            }
            return null;
        }

        private Pacijenti MapPacijent(SqlDataReader reader)
        {
            return new Pacijenti
            {
                IDPacijenta = (int)reader["IDPacijenta"],
                Ime = reader["Ime"].ToString().Trim(),
                Vlasnik = reader["Vlasnik"].ToString().Trim(),
                BrojCipa = reader["BrojCipa"].ToString().Trim(),
                BrojTelefona = reader["BrojTelefona"].ToString().Trim(),
                Pol = reader["Pol"].ToString().Trim(),
                Alergije = reader["Alergije"].ToString().Trim(),
                Sterilisan = reader["Sterilisan"] != DBNull.Value ? (bool?)reader["Sterilisan"] : null,
                TezinaKg = reader["TezinaKg"] != DBNull.Value ? (int)reader["TezinaKg"] : 0,
                IDVeterinara = reader["IDVeterinara"] != DBNull.Value ? (int)reader["IDVeterinara"] : 0
            };
        }
    }
}
