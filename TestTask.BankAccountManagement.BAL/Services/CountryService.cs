using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TestTask.BankAccountManagement.BAL.Services.Interfaces;
using TestTask.BankAccountManagement.DAL.Repositories.Interfaces;
using TestTask.BankAccountManagement.Shared.Models.Dtos;

namespace TestTask.BankAccountManagement.BAL.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryService(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<List<CountryDto>> SelectAsync(string search)
        {
            search ??= "";
            var countries =
                await _countryRepository.SelectByFilterAsync(
                    c => c.Name.Contains(search) || c.Code.Contains(search), 
                    q => q.OrderBy(c => c.Name));

            return countries.Select(_mapper.Map<CountryDto>).ToList();
        }
    }
}
