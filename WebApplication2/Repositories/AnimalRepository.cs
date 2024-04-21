using System.Data.SqlClient;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private IConfiguration _configuration;

    public AnimalRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpGet]
    public IEnumerable<Animal> GetAnimals([FromQuery] string orderBy = "name")
    {
        var animals = new List<Animal>();

        using (var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            con.Open();

            string orderByColumn;
            switch (orderBy.ToLower())
            {
                case "name":
                    orderByColumn = "NAME";
                    break;
                case "description":
                    orderByColumn = "DESCRIPTION";
                    break;
                case "category":
                    orderByColumn = "CATEGORY";
                    break;
                case "area":
                    orderByColumn = "AREA";
                    break;
                default:
                    orderByColumn = "NAME"; // Domyślne sortowanie po nazwie
                    break;
            }

            using (var cmd = new SqlCommand("SELECT * FROM ANIMAL ORDER BY {orderByColumn}", con))
            {
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var animal = new Animal
                        {
                            IDANIMAL = (int)dr["IDANIMAL"],
                            NAME = dr["NAME"].ToString(),
                            DESCRIPTION = dr["DESCRIPTION"].ToString(),
                            CATEGORY = dr["CATEGORY"].ToString(),
                            AREA = dr["AREA"].ToString()
                        };
                        animals.Add(animal);
                    }
                }
            }
        }

        return animals;
    }

    [HttpPost]
    public int CreateAnimal([FromBody] Animal animal)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "INSERT INTO ANIMAL(ANIMALID, NAME, DESCRIPTION, CATEGORY, AREA) VALUES(@ANIMALIS, @NAME, @DESCRIPTION, @CATEGORY, @AREA)";
        cmd.Parameters.AddWithValue("@ANIMALID", animal.IDANIMAL);
        cmd.Parameters.AddWithValue("@NAME", animal.NAME);
        cmd.Parameters.AddWithValue("@DESCRIPTION", animal.DESCRIPTION);
        cmd.Parameters.AddWithValue("@CATEGORY", animal.CATEGORY);
        cmd.Parameters.AddWithValue("@AREA", animal.AREA);

        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }

    [HttpPut("{id}")]
    public int UpdateAnimal(Animal animal)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "UPDATE ANIMAL SET ANIMALID = @ANIMALID, NAME = @NAME, @DESCRIPTION = DESCRIPTION, CATEGORY = @CATEGORY, AREA = @AREA";
        cmd.Parameters.AddWithValue("@ANIMALID", animal.IDANIMAL);
        cmd.Parameters.AddWithValue("@NAME", animal.NAME);
        cmd.Parameters.AddWithValue("@DESCRIPTION", animal.DESCRIPTION);
        cmd.Parameters.AddWithValue("@CATEGORY", animal.CATEGORY);
        cmd.Parameters.AddWithValue("@AREA", animal.AREA);

        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }
    
    [HttpDelete("{id:int}")]
    public int DeleteAnimal(int Id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "DELETE FROM ANIMAL WHERE ANIMALID = @ANIMALID";
        cmd.Parameters.AddWithValue("@ANIMALID", Id);
        
        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }
}