using Biblio.Data.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Biblio.Data
{
    public class GenericRepository<T> : IDisposable, IRepository<T> where T : class, new()
    {
        private readonly string _connectionString;
        public GenericRepository()
        {
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
                    var properties = typeof(T).GetProperties().Where(p => !string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase));
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

                    var tableName = typeof(T).Name.EndsWith("s") ? typeof(T).Name : typeof(T).Name + "s";
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

                    var tableName = typeof(T).Name.EndsWith("s") ? typeof(T).Name : typeof(T).Name + "s";
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

                    var tableName = typeof(T).Name.EndsWith("s") ? typeof(T).Name : typeof(T).Name + "s";
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
                    var tableName = typeof(T).Name.EndsWith("s") ? typeof(T).Name : typeof(T).Name + "s";
                    var properties = typeof(T).GetProperties();
                    var query = $"SELECT * FROM {tableName}";

                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var entity = new T();
                                foreach (var property in properties)
                                {
                                    property.SetValue(entity, reader[property.Name]);
                                }
                                entities.Add(entity);
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
        public T GetStoredProcedure(int id)
        {
            var entity = new T();

            try
            {
                using (var cnn = new SqlConnection(_connectionString))
                {
                    var tableName = typeof(T).Name.EndsWith("s") ? typeof(T).Name : typeof(T).Name + "s";
                    var properties = typeof(T).GetProperties();
                  
                    using (var cmd = new SqlCommand("GetStoredProcedure", cnn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", id));

                        cnn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                foreach (var property in properties)
                                {
                                    property.SetValue(entity, reader[property.Name]);
                                }
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

            return entity;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

