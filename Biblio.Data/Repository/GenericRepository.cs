using Biblio.Data.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace Biblio.Data
{
    public class GenericRepository<T> : IDisposable, IRepository<T> where T : class, new()
    {
        readonly string _connectionString;

        public GenericRepository()
        {
            //_connectionString = Environment.GetEnvironmentVariable("BIBLIO_CONNSTRING");
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        public bool Create(T entity)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    var tableName = typeof(T).Name.EndsWith("s") ? typeof(T).Name : typeof(T).Name + "s";
                    var properties = typeof(T).GetProperties();
                    var columns = string.Join(", ", properties.Select(p => $"[{p.Name}]"));
                    var parameters = string.Join(", ", properties.Select(p => "@" + p.Name));

                    var query = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        foreach (var property in properties)
                        {
                            var value = property.GetValue(entity) ?? DBNull.Value;
                            cmd.Parameters.AddWithValue("@" + property.Name, value);
                        }

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Erreur SQL] {ex.Message}");
                return false;
            }
        }
        public T Read(int id)
        {
            T entity = new T();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    var tableName = typeof(T).Name + "s";
                    var query = $"SELECT * FROM {tableName} WHERE id = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                foreach (var property in typeof(T).GetProperties())
                                {
                                    if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                                    {
                                        property.SetValue(entity, reader[property.Name]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Erreur SQL] {ex.Message}");
            }

            return entity;
        }
        public bool Update(T entity)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    var tableName = typeof(T).Name + "s";
                    var properties = typeof(T).GetProperties();
                    var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

                    var query = $"UPDATE {tableName} SET {setClause} WHERE id = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        foreach (var property in properties)
                        {
                            var value = property.GetValue(entity) ?? DBNull.Value;
                            cmd.Parameters.AddWithValue("@" + property.Name, value);
                        }

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Erreur SQL] {ex.Message}");
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    var tableName = typeof(T).Name + "s";
                    var query = $"DELETE FROM {tableName} WHERE id = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Erreur SQL] {ex.Message}");
                return false;
            }
        }
        public List<T> GetAll()
        {
            var entities = new List<T>();
       
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    var query = string.Empty;
                    conn.Open();

                    if (typeof(T) == typeof(Livre))
                    {
                        query = @"
                            SELECT l.id, l.titre, l.pages, l.image, l.resume, l.id_auteur, a.nom AS AuteurNom 
                            FROM Livres l 
                            INNER JOIN Auteurs a ON l.id_auteur = a.id";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                if (typeof(T) == typeof(Livre))
                                {
                                    var livre = new Livre
                                    {
                                        Id = Convert.ToInt32(reader["id"]),
                                        Titre = reader["titre"].ToString(),
                                        Pages = Convert.ToInt32(reader["pages"]),
                                        Image = reader["image"].ToString(),
                                        Resume = reader["resume"].ToString(),
                                        AuteurId = Convert.ToInt32(reader["id_auteur"]),
                                        AuteurNom = reader["AuteurNom"].ToString()
                                    };
                               
                                    entities.Add((T)(object)livre);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Erreur SQL] {ex.Message}");
            }

            return entities;
    
        }
        public Livre GetSP(int id)
        {
            Livre livre = null;
   
            try
            {
                using (var cnn = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("GetStoredProcedure", cnn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", id));

                        cnn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                livre = new Livre
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Titre = reader["titre"].ToString(),
                                    Resume = reader["resume"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine($"[Erreur SQL] {sqlEx.Message} - Code erreur : {sqlEx.Number}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Erreur Générale] {ex.Message}");
                throw;
            }

            return livre;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

