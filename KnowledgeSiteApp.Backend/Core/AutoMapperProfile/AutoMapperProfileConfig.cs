using AutoMapper;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Dto;
using KnowledgeSiteApp.Models.Entities;

namespace KnowledgeSiteApp.Backend.Core.AutoMapperProfile
{
    public class AutoMapperProfileConfig : Profile
    {
        public AutoMapperProfileConfig()
        {
            // User
            CreateMap<Dto.RegisterUserDto, User>();
            CreateMap<LoginUserDto, User>();
            CreateMap<UpdatePasswordDto, User>();
            CreateMap<ValidateUserDto, User>();
            CreateMap<UpdateUserDetailsDto, User>();

            // Training 
            CreateMap<Dto.TrainingCreateDto, Training>();

            // Topic
            CreateMap<TopicCreateDto, Topic>();
            CreateMap<TopicUpdateDto, Topic>();

            // Category
            CreateMap<TrainingCategoryCreateDto, TrainingCategory>();
            CreateMap<TrainingCategoryUpdateDto, TrainingCategory>();
        }
    }
}
