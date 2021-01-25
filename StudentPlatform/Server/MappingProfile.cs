using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentPlatform.Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Group, GroupDto>();
            CreateMap<GroupForCreationDto, Group>();
            CreateMap<Group, GroupForUpdateDto>().ReverseMap();

            CreateMap<User, UserDto>().ForMember(c => c.FullName, 
                opt => opt.MapFrom(src => string.Join(' ', src.LastName, src.FirstName, src.Patronymic))).ReverseMap();
            CreateMap<UserForCreationDto, User>();
            CreateMap<User, UserForUpdateDto>().ReverseMap();

            CreateMap<Tag, TagDto>();
            CreateMap<TagForCreationDto, Tag>();
            CreateMap<Tag, TagForUpdateDto>().ReverseMap();

            CreateMap<Material, MaterialDto>();
            CreateMap<MaterialForCreationDto, Material>();
            CreateMap<Material, MaterialForUpdateDto>().ReverseMap();

            CreateMap<Test, TestDto>();
            CreateMap<TestForCreationDto, Test>();
            CreateMap<Test, TestForUpdateDto>();
        }
    }
}
