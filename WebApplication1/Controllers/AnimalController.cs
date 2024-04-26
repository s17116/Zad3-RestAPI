using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.DTO;
using WebApplication1.Sevices.Interfaces;

namespace WebApplication1.Controllers
{
	[ApiController]
	[Route("api/animal")]
	public class AnimalController : ControllerBase
	{
		private readonly IDataBaseAcces _dataBaseAcces;
		public AnimalController(IDataBaseAcces dataBaseAcces) { 
			_dataBaseAcces = dataBaseAcces;
		}

		[HttpGet("{orderBy?}")]
		public IActionResult GetAnimals(string orderBy = "name") {
			if (orderBy != "name" && orderBy != "description" && orderBy != "category" && orderBy != "area")
			{
				return BadRequest("wrong parameter");
			}
				var list = _dataBaseAcces.GetAnimals(orderBy);
				return Ok(list);
			
		}
		[HttpPost]
		public IActionResult AddAnimal(AnimalCreateDTO animalCreateDTO)
		{
			if(animalCreateDTO.Name  ==  null || animalCreateDTO.Category == null || animalCreateDTO.Area == null)
			{
				return BadRequest("wrong parameter");
			}

			if (_dataBaseAcces.AddAnimal(animalCreateDTO))
			{
				return Ok(animalCreateDTO);
			}
			return StatusCode(500, "error");
		}

		[HttpDelete("{idAnimal}")]
		public IActionResult RemoveAnimal(int idAnimal) 
		{
			if (idAnimal <= 0) 
			{
				return BadRequest("wrong parameter");
			}
			else if(!_dataBaseAcces.DoesAnimalExist(idAnimal))
			{
				return NotFound("not found");
			}
			var isOK = _dataBaseAcces.RemoveAnimal(idAnimal);
			if (isOK)
			{
				return Ok("animal deleted");
			}
			return StatusCode(500, "error");
		}

		[HttpPut("{idAnimal}")]
		public IActionResult EditAnimal(int idAnimal, AnimalEditDTO animalEditDTO)
		{
			if (idAnimal <= 0 || animalEditDTO.Name == null || animalEditDTO.Area == null || animalEditDTO.Category == null)
			{
				return BadRequest("wrong parameter");
			}
			else if (!_dataBaseAcces.DoesAnimalExist(idAnimal))
			{
				return NotFound("not found");
			}
			var isOK = _dataBaseAcces.EditAnimal(idAnimal, animalEditDTO);
			if (isOK)
			{
				return Ok("animal edited");
			}
			return StatusCode(500, "error");
		}

	}

}
