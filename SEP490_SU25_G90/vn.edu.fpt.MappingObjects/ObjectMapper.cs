using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SEP490_SU25_G90.vn.edu.fpt.Commons;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.LearningMaterial;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.News;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class ObjectMapper : Profile
    {
        public ObjectMapper() 
        {
            CreateMap<LoginInformationResponse, User>();
            CreateMap<User, LoginInformationResponse>();

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


            CreateMap<Models.News, NewsListInformationResponse>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? string.Join(" ", src.Author.FirstName, src.Author.MiddleName, src.Author.LastName) : ""))
                .ForMember(dest => dest.ShortContent, opt => opt.MapFrom(src => src.NewsContent.Length > 100 ? src.NewsContent.Substring(0,100)+ "..." : src.NewsContent));
            CreateMap<NewsFormRequest, Models.News>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.PostTime, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorId, opt => opt.Ignore());
           
            CreateMap<AccountCreationRequest, User>();
            CreateMap<User, AccountCreationRequest>();

            CreateMap<Models.LearningMaterial, LearningMaterialListInformationResponse>()
                .ForMember(dest => dest.LicenceTypeName, opt => opt.MapFrom(src => src.LicenceType != null ? src.LicenceType.LicenceCode : null));
        }
    }
}
