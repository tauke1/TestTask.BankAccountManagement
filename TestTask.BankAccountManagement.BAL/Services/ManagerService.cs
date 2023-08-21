using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TestTask.BankAccountManagement.BAL.Exceptions;
using TestTask.BankAccountManagement.BAL.SecurityHashers.Interfaces;
using TestTask.BankAccountManagement.BAL.Services.Interfaces;
using TestTask.BankAccountManagement.DAL.Models;
using TestTask.BankAccountManagement.DAL.Repositories.Interfaces;
using TestTask.BankAccountManagement.Shared.Models.Dtos;

namespace TestTask.BankAccountManagement.BAL.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepository;
        private readonly ISecurityHasher _securityHasher;
        private readonly IMapper _mapper;

        private const string ManagerNotFoundExceptionMessage = "Combination of login/password has not been found";
        private const string ManagerDoesNotExistExceptionMessage = "Manager with id {0} not found";

        public ManagerService(
            IManagerRepository managerRepository, 
            ISecurityHasher securityHasher, 
            IMapper mapper)
        {
            _managerRepository = managerRepository;
            _securityHasher = securityHasher;
            _mapper = mapper;
        }

        public async Task<ManagerDto> LoginAsync(string login, string pin)
        {
            var manager = await _managerRepository.SingleOrDefaultByFilterAsync(m => m.Login == login);
            if (manager == null)
            {
                throw new UserFriendlyMessageException(ManagerNotFoundExceptionMessage, HttpStatusCode.Unauthorized);
            }

            var pinValid = _securityHasher.Verify(pin, manager.PinHashed);
            if (!pinValid)
            {
                throw new UserFriendlyMessageException(ManagerNotFoundExceptionMessage, HttpStatusCode.Unauthorized);
            }

            return _mapper.Map<ManagerDto>(manager);
        }

        public async Task<ManagerDto> GetAsync(long id)
        {
            var manager = await _managerRepository.SingleOrDefaultByIdAsync(id);
            if (manager == null)
            {
                var exceptionMessage = string.Format(ManagerDoesNotExistExceptionMessage, id);
                throw new Exception(exceptionMessage);
            }

            return _mapper.Map<ManagerDto>(manager);
        }
    }
}
