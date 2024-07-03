using Microsoft.AspNetCore.Http.HttpResults;
using TodoApp.Models;

namespace TodoApp.Dtos
{
    public class TodoDto
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }

    public static class MappingDtos 
    {
        public static Todo ToEntity(this TodoDto dto)
        {
            return new Todo()
            {
                Title = dto.Title,
                IsCompleted = dto.IsCompleted,
            };
        }

        public static Todo ToEntity(this TodoDto dto, int id)
        {
            return new Todo()
            {
                Id = id,
                Title = dto.Title,
                IsCompleted = dto.IsCompleted,
            };

        }
    }
}
