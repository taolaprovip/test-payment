﻿using RePurpose_Models.Entities;
using RePurpose_Models.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Repositories.Implementations
{
    public class WalletRepository : Repository<Wallet>, IWalletRepository
    {
        public WalletRepository(RePurposeContext context) : base(context)
        {
        }
    }
}
