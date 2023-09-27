using Microsoft.EntityFrameworkCore.Storage;
using RePurpose_Models.Entities;
using RePurpose_Models.Repositories.Implementations;
using RePurpose_Models.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RePurposeContext _context;
        public IMemberRepository _member = null!;
        public IRefreshTokenRepository _refreshToken = null!;
        public ILocationRepository _location = null!;
        public IItemRepository _item = null!;
        public IImageRepository _image = null!;
        public ITransactionRepository _transaction = null!;
        public IWalletRepository _wallet = null!;

        public UnitOfWork(RePurposeContext context)
        {
            _context = context;
        }
        public IWalletRepository Wallet
        {
            get { return _wallet ??= new WalletRepository(_context); }
        }
       
        public ITransactionRepository TransactionDb
        {
            get { return _transaction ??= new TransactionRepository(_context); }
        }
        public IImageRepository Image
        {
            get { return _image ??= new ImageRepository(_context); }
        }
        public IItemRepository Item
        {
            get { return _item ??= new ItemRepository(_context); }
        }
        public ILocationRepository Location
        {
            get { return _location ??= new LocationRepository(_context); }
        }
        public IMemberRepository Member
        {
            get { return _member ??= new MemberRepository(_context); } 
        }
        public IRefreshTokenRepository RefreshToken
        {
            get { return _refreshToken ??= new RefreshTokenRepository(_context); } 
        }
        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public IDbContextTransaction Transaction()
        {
            return _context.Database.BeginTransaction();
        }
    }
}
