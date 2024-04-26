using System.Data.SqlClient;
using WebApplication1.Models.DTO;
using WebApplication1.Sevices.Interfaces;

namespace WebApplication1.Sevices.Implementations
{
	public class DataBaseAcces : IDataBaseAcces
	{
		private readonly IConfiguration _configuration;
		public DataBaseAcces(IConfiguration configuration) { 
			_configuration = configuration;
		}

		public List<AnimalSelectDTO> GetAnimals(string orderBy)
		{
			var connectionString = _configuration["ConnectionString"];
			var list = new List<AnimalSelectDTO>();

			using (var sqlConnection = new SqlConnection(connectionString))
			{
				sqlConnection.Open();
				string sqlQuery = "SELECT * FROM ANIMAL ORDER BY" +
					" CASE " +
					"WHEN @orderBy = 'Name' THEN Name  " +
					"WHEN @orderBy = 'Area' THEN Area  " +
					"WHEN @orderBy = 'Description' THEN Description  " +
					"WHEN @orderBy = 'Category' THEN Category " +
					"END";
				using (var command = new SqlCommand(sqlQuery, sqlConnection))
				{
					command.Parameters.AddWithValue("@orderBy", orderBy);
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							string name = reader.GetString(reader.GetOrdinal("name"));
							string? description = "brak opisu";
							if(!reader.IsDBNull(reader.GetOrdinal("description")))
							{
								description = reader.GetString(reader.GetOrdinal("description"));
							}
							string category = reader.GetString(reader.GetOrdinal("category"));
							string area = reader.GetString(reader.GetOrdinal("area"));

							var animal = new AnimalSelectDTO()
							{
								Area = area,
								Name = name,
								Description = description,
								Category = category
							};
							list.Add(animal);
						}
					}
				}	
			}
			return list;
		}

		public bool AddAnimal(AnimalCreateDTO animalCreate)
		{
			var connectionString = _configuration["ConnectionString"];

			using (var sqlConnection = new SqlConnection(connectionString))
			{
				sqlConnection.Open();
				string sqlQuery = "INSERT INTO ANIMAL(" +
					" Name, Description, Category, area) " +
					"VALUES (@name, @descriprtion, @category, @area)";
				using (var command = new SqlCommand(sqlQuery, sqlConnection))
				{
					command.Parameters.AddWithValue("@name", animalCreate.Name);
					command.Parameters.AddWithValue("@descriprtion", animalCreate.Description == null ? DBNull.Value : animalCreate.Description);
					command.Parameters.AddWithValue("@category", animalCreate.Category);
					command.Parameters.AddWithValue("@area", animalCreate.Area);
					return  command.ExecuteNonQuery() > 0;

				}
			}
			return false;
		}

		public bool DoesAnimalExist(int id)
		{
			var connectionString = _configuration["ConnectionString"];

			using (var sqlConnection = new SqlConnection(connectionString))
			{
				sqlConnection.Open();
				string sqlQuery = "select 1 from animal where idAnimal = @id";
				using (var command = new SqlCommand(sqlQuery, sqlConnection))
				{
					command.Parameters.AddWithValue("@id", id);
					using (var reader = command.ExecuteReader())
					{
						return reader.HasRows;
					}

				}
			}
			return false;
		}

		public bool RemoveAnimal(int id)
		{
			var connectionString = _configuration["ConnectionString"];

			using (var sqlConnection = new SqlConnection(connectionString))
			{
				sqlConnection.Open();
				string sqlQuery = "delete from animal where idAnimal = @id";
				using (var command = new SqlCommand(sqlQuery, sqlConnection))
				{
					command.Parameters.AddWithValue("@id", id);
					return command.ExecuteNonQuery() > 0;

				}
			}
			return false;
		}
		public bool EditAnimal(int id, AnimalEditDTO animalEditDTO)
		{
			var connectionString = _configuration["ConnectionString"];

			using (var sqlConnection = new SqlConnection(connectionString))
			{
				sqlConnection.Open();
				string sqlQuery = "update animal set name = @name, area = @area, description = @description, category = @category where idAnimal = @id ";
				using (var command = new SqlCommand(sqlQuery, sqlConnection))
				{
					command.Parameters.AddWithValue("@name", animalEditDTO.Name);
					command.Parameters.AddWithValue("@description", animalEditDTO.Description == null ? DBNull.Value : animalEditDTO.Description);
					command.Parameters.AddWithValue("@category", animalEditDTO.Category);
					command.Parameters.AddWithValue("@area", animalEditDTO.Area);
					command.Parameters.AddWithValue("@id", id);

					return command.ExecuteNonQuery() > 0;

				}
			}
			return false;
		}
	}
}
