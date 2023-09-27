using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Service.Interfaces
{
    public interface IWalletService
    {
        Task<IActionResult> GetWalletById(Guid id);
        Task<IActionResult> GetAllWalletById();
        Task<Guid?> GetWalletById1(Guid id);
    }
}
