using Klase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SlojPodataka
{
    public class TerminDB
    {
        private readonly string _connectionString;

        public TerminDB()
        {
            _connectionString = Konekcija.String;
        }

        public List<Termin> GetAll()
        {
            var lista = new List<Termin>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Termin", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(MapTermin(reader));
                    }
                }
            }

            return lista;
        }

        public void Insert(Termin termin)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Termin(Datum, Vreme, VrstaUsluge, IDPacijenta, IDVeterinara) " +
                    "VALUES(@Datum, @Vreme, @VrstaUsluge, @IDPacijenta, @IDVeterinara)", connection);

                command.Parameters.AddWithValue("@Datum", termin.Datum);
                command.Parameters.AddWithValue("@Vreme", termin.Vreme);
                command.Parameters.AddWithValue("@VrstaUsluge", termin.VrstaUsluge);
                command.Parameters.AddWithValue("@IDPacijenta", termin.IDPacijenta);
                command.Parameters.AddWithValue("@IDVeterinara", termin.IDVeterinara);
                command.ExecuteNonQuery();
            }
        }

        public void Update(Termin termin)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Termin SET Datum=@Datum, Vreme=@Vreme, VrstaUsluge=@VrstaUsluge, " +
                    "IDPacijenta=@IDPacijenta, IDVeterinara=@IDVeterinara WHERE IDTermina=@Id", connection);

                command.Parameters.AddWithValue("@Id", termin.IDTermina);
                command.Parameters.AddWithValue("@Datum", termin.Datum);
                command.Parameters.AddWithValue("@Vreme", termin.Vreme);
                command.Parameters.AddWithValue("@VrstaUsluge", termin.VrstaUsluge);
                command.Parameters.AddWithValue("@IDPacijenta", termin.IDPacijenta);
                command.Parameters.AddWithValue("@IDVeterinara", termin.IDVeterinara);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int idTermina)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Termin WHERE IDTermina=@Id", connection);
                command.Parameters.AddWithValue("@Id", idTermina);
                command.ExecuteNonQuery();
            }
        }

        public List<Termin> GetTerminiFromPacijent(int ID)
        {
            var list = new List<Termin>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Termin where IDPacijenta=@IDPacijenta", connection);
                command.Parameters.AddWithValue("@IDPacijenta", ID);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(MapTermin(reader));
                    }
                }
            }
            return list;
        }

        public bool DaLiJeVeterinarAktivan(int ID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT 1 FROM Veterinar where IDVeterinara=@IDVeterinara and IsDeleted=0", connection);
                command.Parameters.AddWithValue("@IDVeterinara", ID);

                using (var reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        private Termin MapTermin(SqlDataReader reader)
        {
            return new Termin
            {
                IDTermina = (int)reader["IDTermina"],
                Datum = (DateTime)reader["Datum"],
                Vreme = (TimeSpan)reader["Vreme"],
                VrstaUsluge = reader["VrstaUsluge"].ToString(),
                IDPacijenta = (int)reader["IDPacijenta"],
                IDVeterinara = (int)reader["IDVeterinara"]
            };
        }
    }
}
