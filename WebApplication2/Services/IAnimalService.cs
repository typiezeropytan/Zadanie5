using WebApplication2.Models;

namespace WebApplication2.Services;

public interface IAnimalService
{
    IEnumerable<Animal> GetAnimals();
    int CreateAnimal(Animal animal);
    int UpdateAnimal(Animal animal);
    int DeleteAnimal(int Id);
}