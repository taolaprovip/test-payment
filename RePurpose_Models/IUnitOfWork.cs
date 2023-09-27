using Microsoft.EntityFrameworkCore.Storage;
using RePurpose_Models.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models
{
    public interface IUnitOfWork
    {
        public IMemberRepository Member { get; }
        public IRefreshTokenRepository RefreshToken { get; }
        public ILocationRepository Location { get; }
        public IItemRepository Item { get; }
        public IImageRepository Image { get; }
        public ITransactionRepository TransactionDb { get; }
        public IWalletRepository Wallet { get; }
        Task<int> SaveChanges();
        IDbContextTransaction Transaction();
    }
}
