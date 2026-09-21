using Klase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SlojPodataka
{
    public class PregledDB
    {
        private readonly string _connectionString;

        public PregledDB()
        {
            _connectionString = Konekcija.String;
        }

        public List<Pregled> GetAll()
        {
            var lista = new List<Pregled>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Pregled", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(MapPregled(reader));
                    }
                }
            }

            return lista;
        }

        public void Insert(Pregled pregled)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Pregled(Izvestaj, IDTermina, IDLeka) VALUES(@Izvestaj, @Termin, @Lek)", connection);

                command.Parameters.AddWithValue("@Izvestaj", pregled.Izvestaj);
                command.Parameters.AddWithValue("@Termin", pregled.IDTermina);
                command.Parameters.AddWithValue("@Lek", pregled.IDLeka);
                command.ExecuteNonQuery();
            }
        }

        public void Update(Pregled pregled)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Pregled SET Izvestaj=@Izvestaj, IDTermina=@Termin, IDLeka=@Lek WHERE IDPregleda=@Id", connection);

                command.Parameters.AddWithValue("@Id", pregled.IDPregleda);
                command.Parameters.AddWithValue("@Izvestaj", pregled.Izvestaj);
                command.Parameters.AddWithValue("@Termin", pregled.IDTermina);
                command.Parameters.AddWithValue("@Lek", pregled.IDLeka);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int idPregleda)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Pregled WHERE IDPregleda=@Id", connection);
                command.Parameters.AddWithValue("@Id", idPregleda);
                command.ExecuteNonQuery();
            }
        }

        public List<Pregled> GetTerminiFromPacijent(int ID)
        {
            var list = new List<Pregled>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Pregled where IDTermina=@IDTermina", connection);
                command.Parameters.AddWithValue("@IDTermina", ID);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(MapPregled(reader));
                    }
                }
            }
            return list;
        }

        public Termin GetTerminWithPregled(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Termin WHERE IDTermina = @IDTermina", connection);
                command.Parameters.AddWithValue("@IDTermina", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
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

            return null;
        }

        public bool DaLiJeLekAktivan(int ID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT 1 FROM Lek where IDLeka=@IDLeka and IsDeleted=0", connection);
                command.Parameters.AddWithValue("@IDLeka", ID);

                using (var reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        private Pregled MapPregled(SqlDataReader reader)
        {
            return new Pregled
            {
                IDPregleda = (int)reader["IDPregleda"],
                Izvestaj = (string)reader["Izvestaj"],
                IDTermina = (int)reader["IDTermina"],
                IDLeka = (int)reader["IDLeka"]
            };
        }
    }
}
