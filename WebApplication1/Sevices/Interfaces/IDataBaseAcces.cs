using WebApplication1.Models.DTO;

namespace WebApplication1.Sevices.Interfaces
{
	public interface IDataBaseAcces
	{
		public List<AnimalSelectDTO> GetAnimals(string orderBy);
		public bool AddAnimal(AnimalCreateDTO animalCreate);
		public bool DoesAnimalExist(int  id);
		public bool RemoveAnimal(int id);

		public bool EditAnimal(int id, AnimalEditDTO animalEditDTO);
	}
}
