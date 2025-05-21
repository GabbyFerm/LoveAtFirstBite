using Application.Commen.Dtos;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commen.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
         CreateMap<Vote, VoteDto>().ReverseMap();
        
        }
    }
}
