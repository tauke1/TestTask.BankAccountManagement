using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TestTask.BankAccountManagement.DAL.Models.Enums;
using TestTask.BankAccountManagement.Shared.Models.Dtos.Enums;

namespace TestTask.BankAccountManagement.BAL.MappingProfiles
{
    public class SortDirectionMappingProfile : Profile
    {
        public SortDirectionMappingProfile()
        {
            CreateMap<SortDirectionDto, SortDirection>().ConvertUsing((value, _) =>
            {
                return value switch
                {
                    SortDirectionDto.Ascending => SortDirection.Ascending,
                    SortDirectionDto.Descending => SortDirection.Descending,
                    _ => SortDirection.Ascending
                };
            });
        }
    }
}
