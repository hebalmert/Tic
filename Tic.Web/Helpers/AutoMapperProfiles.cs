using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tic.Shared.ApiDTOs;
using Tic.Shared.Entites;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        { 
            
            CreateMap<ServerSaveDTOs, Server>().ReverseMap();
            CreateMap<TicketTimeDTOs, TicketTime>().ReverseMap();
            CreateMap<PlanSaveDTOs, Plan>().ReverseMap();
        }
    }
}
