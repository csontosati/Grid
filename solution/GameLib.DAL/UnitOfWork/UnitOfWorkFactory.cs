using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GameLib.DAL.UnitOfWork;

public class UnitOfWorkFactory(
    IDbContextFactory<GameLibDbContext> dbContextFactory,
    IServiceProvider serviceProvider) : IUnitOfWorkFactory
{
    private readonly IDbContextFactory<GameLibDbContext> _dbContextFactory = dbContextFactory;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IUnitOfWork Create()
    {
        var dbContext = _dbContextFactory.CreateDbContext();

        var scope = _serviceProvider.CreateScope();

        return new UnitOfWork(dbContext, scope);
    }
}