using AutoMapper;
using RePurpose_Models;
using RePurpose_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Service.Implementations
{
    public class WalletService : BaseService, IWalletService
    {
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
    }
}
