using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class ObjectMapper : Profile
    {
        public ObjectMapper() 
        {
            CreateMap<LoginInformationResponse, User>();
            CreateMap<User, LoginInformationResponse>();

            CreateMap<UserListInformationResponse, User>();
            CreateMap<User, UserListInformationResponse>();

            CreateMap<TestApplicationListInformationResponse, TestApplication>();
            CreateMap<TestApplication, TestApplicationListInformationResponse>();

            CreateMap<CourseInformationResponse, Course>();
            CreateMap<Course, CourseInformationResponse>();

        }
    }
}
