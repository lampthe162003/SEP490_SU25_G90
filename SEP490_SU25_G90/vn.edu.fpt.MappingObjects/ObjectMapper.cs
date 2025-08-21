using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SEP490_SU25_G90.vn.edu.fpt.Commons;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.CarAssignment;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.LearningMaterial;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.News;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.UserDto;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class ObjectMapper : Profile
    {

        public ObjectMapper() 
        {
            CreateMap<LoginInformationResponse, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => StringUtils.GetFirstName(src.Fullname)))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => StringUtils.GetMiddleName(src.Fullname)))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => StringUtils.GetLastName(src.Fullname)));
            CreateMap<User, LoginInformationResponse>()
                .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src =>
                string.Join(" ", new[] { src.FirstName, src.MiddleName, src.LastName }
                .Where(name => !string.IsNullOrWhiteSpace(name)))));

            CreateMap<UserListInformationResponse, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => StringUtils.GetFirstName(src.FullName)))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => StringUtils.GetMiddleName(src.FullName)))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => StringUtils.GetLastName(src.FullName)));
            CreateMap<User, UserListInformationResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => 
                string.Join(" ", new[] { src.FirstName, src.MiddleName, src.LastName }
                .Where(name => !string.IsNullOrWhiteSpace(name)))));

            CreateMap<Models.TestApplication, TestApplicationListInformationResponse>()
                .ForMember(dest => dest.TestId, opt => opt.MapFrom(src => src.TestId))
                .ForMember(dest => dest.ExamDate, opt => opt.MapFrom(src => src.ExamDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Learning, opt => opt.MapFrom(src => src.Learning));


            CreateMap<Models.News, NewsListInformationResponse>();
            CreateMap<NewsFormRequest, Models.News>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.PostTime, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorId, opt => opt.Ignore());
           
            CreateMap<AccountCreationRequest, User>();
            CreateMap<User, AccountCreationRequest>();

            CreateMap<Models.LearningMaterial, LearningMaterialListInformationResponse>();
            CreateMap<LearningMaterialFormRequest, Models.LearningMaterial>()
                .ForMember(dest => dest.FileLink, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<Models.LearningMaterial, LearningMaterialFormRequest>()
                .ForMember(dest => dest.File, opt => opt.Ignore())
                .ForMember(dest => dest.OldFilePath, opt => opt.MapFrom(src => src.FileLink));

            CreateMap<Models.User, CarBorrowerInformationResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                string.Join(" ", new[] { src.FirstName, src.MiddleName, src.LastName }
                .Where(name => !string.IsNullOrWhiteSpace(name)))));
            CreateMap<CarBorrowerInformationResponse, Models.User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => StringUtils.GetFirstName(src.FullName)))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => StringUtils.GetMiddleName(src.FullName)))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => StringUtils.GetLastName(src.FullName)));

            CreateMap<Models.CarAssignment, CarAssignmentInformationResponse>();
            CreateMap<CarAssignmentInformationResponse, Models.CarAssignment>();

            CreateMap<User, UserDetailsInformationResponse>()
                .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => string.Join(" ", new[] { src.FirstName, src.MiddleName, src.LastName }
                .Where(name => !string.IsNullOrWhiteSpace(name)))));
            CreateMap<UserDetailsInformationResponse, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => StringUtils.GetFirstName(src.Fullname)))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => StringUtils.GetMiddleName(src.Fullname)))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => StringUtils.GetLastName(src.Fullname)));

            CreateMap<User, UpdateStaffRequest>()
                .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => string.Join(" ", new[] { src.FirstName, src.MiddleName, src.LastName }
                .Where(name => !string.IsNullOrWhiteSpace(name)))));
            CreateMap<UpdateStaffRequest, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => StringUtils.GetFirstName(src.Fullname)))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => StringUtils.GetMiddleName(src.Fullname)))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => StringUtils.GetLastName(src.Fullname)));

            CreateMap<UpdateStaffRequest, UserDetailsInformationResponse>();
            CreateMap<UserDetailsInformationResponse, UpdateStaffRequest>();
        }
    }
}
