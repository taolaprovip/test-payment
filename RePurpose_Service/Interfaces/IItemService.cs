using Microsoft.AspNetCore.Mvc;
using RePurpose_Models.Models.Requests.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Service.Interfaces
{
    public interface IItemService
    {
        Task<IActionResult> PostItem(ItemCreateModel itemCreateModel, Guid id);
    }
}
