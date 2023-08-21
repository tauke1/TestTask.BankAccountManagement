using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TestTask.BankAccountManagement.DAL.Models;
using TestTask.BankAccountManagement.Shared.Models.Dtos;

namespace TestTask.BankAccountManagement.BAL.MappingProfiles
{
    public class CountryMappingProfile : Profile
    {
        public CountryMappingProfile()
        {
            CreateMap<Country, CountryDto>(MemberList.Destination);
        }
    }
}
