using AutoMapper;
using RePurpose_Models.Entities;
using RePurpose_Models.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Transaction, TransactionViewModel>();
            CreateMap<Wallet, WalletViewModel>();
        }   

        
    }
}
