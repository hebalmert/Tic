using AutoMapper;
using Tic.Shared.ApiDTOs;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        { 
            
            CreateMap<ServerSaveDTOs, Server>().ReverseMap();
        }
    }
}
