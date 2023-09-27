using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RePurpose_Models;
using RePurpose_Models.Models.View;
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
        public async Task<Guid?> GetWalletById1(Guid id)
        {
            var tran = await _unitOfWork.Wallet.GetMany(product => product.MemberId.Equals(id)).FirstOrDefaultAsync();
            if (tran != null)
            {
                return tran.WalletId;
            }
            return null;
        }

        public async Task<IActionResult> GetWalletById(Guid id)
        {
            var tran = await _unitOfWork.Wallet.GetMany(product => product.MemberId.Equals(id))
                .ProjectTo<WalletViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (tran != null)
            {
                return new JsonResult(tran);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }

        public async Task<IActionResult> GetAllWalletById()
        {
            var tran = await _unitOfWork.Wallet.GetAll()
                .ProjectTo<WalletViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            if (tran != null)
            {
                return new JsonResult(tran);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }
    }
}
