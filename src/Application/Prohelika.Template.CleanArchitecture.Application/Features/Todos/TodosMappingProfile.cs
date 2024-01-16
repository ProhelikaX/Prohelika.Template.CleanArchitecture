using AutoMapper;
using Prohelika.Template.CleanArchitecture.Domain.Entities;

namespace Prohelika.Template.CleanArchitecture.Application.Features.Todos;

public class TodosMappingProfile : Profile
{
    public TodosMappingProfile()
    {
        CreateMap<Todo, TodoDto>();
          
        CreateMap<TodoCreateDto, Todo>();
        CreateMap<TodoDto, Todo>();
    }
}