using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using KennelAPI.Models;

namespace KennelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AnimalsController(IConfiguration configuration)
        {
            _config = configuration;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: api/Animal
        [HttpGet]
        public IActionResult Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Breed, Age, HasShots FROM Animal";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Animal> animals = new List<Animal>();
                    while(reader.Read())
                    {
                        animals.Add(new Animal
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Age = reader.GetInt32(reader.GetOrdinal("Age")),
                            HasShots = reader.GetBoolean(reader.GetOrdinal("HasShots"))
                        });
                    }

                    reader.Close();
                    return Ok(animals);
                }
            }
        }

        [HttpGet("{id}", Name="GetAnimal")]
        public IActionResult GetAnimal(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name, Breed, Age, HasShots FROM Animal WHERE Id = @Id";
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Animal animal = null;
                    if (reader.Read())
                    {
                        animal = new Animal
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Age = reader.GetInt32(reader.GetOrdinal("Age")),
                            HasShots = reader.GetBoolean(reader.GetOrdinal("HasShots"))
                        };
                    }

                    reader.Close();
                    return Ok(animal);
                }
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody] Animal newAnimal)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Animal (Name, Breed, Age, HasShots) 
                                           OUTPUT INSERTED.Id
                                           VALUES (@Name, @Breed, @Age, @HasShots)";
                    cmd.Parameters.Add(new SqlParameter("@Name", newAnimal.Name));
                    cmd.Parameters.Add(new SqlParameter("@Breed", newAnimal.Breed));
                    cmd.Parameters.Add(new SqlParameter("@Age", newAnimal.Age));
                    cmd.Parameters.Add(new SqlParameter("@HasShots", newAnimal.HasShots));

                    int newId = (int) cmd.ExecuteScalar();

                    newAnimal.Id = newId;
                    return CreatedAtRoute("GetAnimal", new { id = newId }, newAnimal);
                }
            } 
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Animal updatedAnimal)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Animal 
                                           SET Name = @Name, 
                                               Breed = @Breed,
                                               Age = @Age,
                                               HasShots = @HasShots
                                         WHERE id = @id";
                    cmd.Parameters.Add(new SqlParameter("@Name", updatedAnimal.Name));
                    cmd.Parameters.Add(new SqlParameter("@Breed", updatedAnimal.Breed));
                    cmd.Parameters.Add(new SqlParameter("@Age", updatedAnimal.Age));
                    cmd.Parameters.Add(new SqlParameter("@HasShots", updatedAnimal.HasShots));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();

                    return NoContent();
                }
            } 
        }
    }
}
