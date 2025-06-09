using AutoMapper;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class ObjectMapper : Profile
    {
        public ObjectMapper() 
        {
            CreateMap<LoginInformationRequest, User>();
            CreateMap<User, LoginInformationRequest>();
        }
    }
}
