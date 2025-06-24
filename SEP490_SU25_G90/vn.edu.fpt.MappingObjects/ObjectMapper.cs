using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SEP490_SU25_G90.vn.edu.fpt.Commons;
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

            CreateMap<TestApplicationListInformationResponse, TestApplication>();
            CreateMap<TestApplication, TestApplicationListInformationResponse>();

            CreateMap<News, NewsListInformationResponse>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? string.Join(" ", src.Author.FirstName, src.Author.MiddleName, src.Author.LastName) : ""))
                .ForMember(dest => dest.ShortContent, opt => opt.MapFrom(src => src.NewsContent.Length > 100 ? src.NewsContent.Substring(0,100)+ "..." : src.NewsContent));
        }
    }
}
