using System.Collections.Generic;
using System.Data.SqlClient;
using SlojPodataka.Klase;

namespace SlojPodataka
{
    public class VeterinarDB
    {
        private readonly string _connectionString;

        public VeterinarDB()
        {
            _connectionString = Konekcija.String;
        }

        public List<Veterinar> GetAll()
        {
            var lista = new List<Veterinar>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Veterinar", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(MapVeterinar(reader, true));
                }
            }
            return lista;
        }

        public List<Veterinar> GetAllActiveVeterinar()
        {
            var lista = new List<Veterinar>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Veterinar where IsDeleted = 0", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(MapVeterinar(reader, false));
                }
            }
            return lista;
        }

        public void Insert(Veterinar veterinar)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Veterinar (Ime, Prezime, JMBG, BrojTelefona, Email) " +
                    "VALUES (@Ime, @Prezime, @JMBG, @BrojTelefona, @Email)", conn);

                cmd.Parameters.AddWithValue("@Ime", veterinar.Ime);
                cmd.Parameters.AddWithValue("@Prezime", veterinar.Prezime);
                cmd.Parameters.AddWithValue("@JMBG", veterinar.JMBG);
                cmd.Parameters.AddWithValue("@BrojTelefona", veterinar.BrojTelefona);
                cmd.Parameters.AddWithValue("@Email", veterinar.Email);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Veterinar veterinar)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Veterinar SET Ime=@Ime, Prezime=@Prezime, JMBG=@JMBG, " +
                    "BrojTelefona=@BrojTelefona, Email=@Email WHERE IDVeterinara=@IDVeterinara", conn);

                cmd.Parameters.AddWithValue("@IDVeterinara", veterinar.IDVeterinara);
                cmd.Parameters.AddWithValue("@Ime", veterinar.Ime);
                cmd.Parameters.AddWithValue("@Prezime", veterinar.Prezime);
                cmd.Parameters.AddWithValue("@JMBG", veterinar.JMBG);
                cmd.Parameters.AddWithValue("@BrojTelefona", veterinar.BrojTelefona);
                cmd.Parameters.AddWithValue("@Email", veterinar.Email);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Veterinar WHERE IDVeterinara=@IDVeterinara", conn);
                cmd.Parameters.AddWithValue("@IDVeterinara", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void SoftDelete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Veterinar SET isDeleted = 1 WHERE IDVeterinara = @IDVeterinara", conn);
                cmd.Parameters.AddWithValue("@IDVeterinara", id);
                cmd.ExecuteNonQuery();
            }
        }

        public bool checkIfJMBGExists(string JMBG)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM Veterinar WHERE JMBG = @jmbg", conn);
                cmd.Parameters.AddWithValue("@jmbg", JMBG);
                SqlDataReader nadjen = cmd.ExecuteReader();
                return nadjen.Read();
            }
        }

        public bool checkIfEmailForVeterinarExists(string email)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM Veterinar WHERE Email = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);
                SqlDataReader nadjen = cmd.ExecuteReader();
                return nadjen.Read();
            }
        }

        public int GetAppointmentCountFromVeterinar(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Termin WHERE IDVeterinara = @VeterinarId", conn);
                cmd.Parameters.AddWithValue("@VeterinarId", id);
                return (int)cmd.ExecuteScalar();
            }
        }

        public Veterinar findVeterinaraByID(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Veterinar WHERE IDVeterinara = @IDVeterinara", conn);
                cmd.Parameters.AddWithValue("@IDVeterinara", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return MapVeterinar(reader, true);
                }
            }
            return null;
        }

        private Veterinar MapVeterinar(SqlDataReader reader, bool includeDeleted)
        {
            var veterinar = new Veterinar
            {
                IDVeterinara = (int)reader["IDVeterinara"],
                Ime = reader["Ime"].ToString().Trim(),
                Prezime = reader["Prezime"].ToString().Trim(),
                JMBG = reader["JMBG"].ToString().Trim(),
                BrojTelefona = reader["BrojTelefona"].ToString().Trim(),
                Email = reader["Email"].ToString().Trim()
            };

            if (includeDeleted)
                veterinar.isDeleted = (bool)reader["isDeleted"];

            return veterinar;
        }
    }
}
