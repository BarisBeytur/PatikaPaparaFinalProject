﻿using DigitalMarket.Base.Entity;
using DigitalMarket.Data.Context;
using DigitalMarket.Data.GenericRepository;
using System.Reflection.Metadata.Ecma335;

namespace DigitalMarket.Data.UnitOfWork;

public class UnitOfWork<TEntity> : IDisposable, IUnitOfWork<TEntity> where TEntity : BaseEntity
{
    private readonly DigitalMarketDbContext _context;
    private IGenericRepository<TEntity> _repository;

    public UnitOfWork(DigitalMarketDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns the repository for the entity
    /// </summary>
    public IGenericRepository<TEntity> Repository
    {
        get
        {
            return _repository ??= new GenericRepository<TEntity>(_context);
        }
    }


    /// <summary>
    /// Completes the unit of work
    /// </summary>
    /// <returns></returns>
    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }


    /// <summary>
    /// Completes the unit of work with a transaction
    /// </summary>
    /// <returns></returns>
    public async Task CommitWithTransaction()
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }
    }


    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}   
