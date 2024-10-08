﻿using Autofac;
using AutoMapper;
using DigitalMarket.Business.CQRS.Commands.CategoryCommands;
using DigitalMarket.Business.Infrastructure.Mapping.AutoMapper;
using DigitalMarket.Business.Services.PaymentService;
using DigitalMarket.Business.Services.TokenService;
using DigitalMarket.Data.Context;
using DigitalMarket.Data.UnitOfWork;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace DigitalMarket.Business.Infrastructure.DependencyResolvers
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Unit of Work
            builder.RegisterGeneric(typeof(UnitOfWork<>))
                   .As(typeof(IUnitOfWork<>))
                   .InstancePerLifetimeScope();

            builder.RegisterType<PaymentService>()
                   .As<IPaymentService>()
                   .InstancePerLifetimeScope();

            // FluentValidation
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                   .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            // AutoMapper
            builder.Register(ctx =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new MapperConfig());
                });

                return config.CreateMapper();
            }).As<IMapper>()
              .InstancePerLifetimeScope();

            // MediatR
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
       .AsImplementedInterfaces();

            // MediatR'nun handler'larını kaydetme
            builder.RegisterAssemblyTypes(typeof(CreateCategoryCommandHandler).Assembly)
                   .AsImplementedInterfaces();


            // Redis
            builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();
                var configuration = ConfigurationOptions.Parse(config.GetSection("Redis:ConnectionString").Value, true);
                return ConnectionMultiplexer.Connect(configuration);
            }).As<IConnectionMultiplexer>().SingleInstance();

        }
    }
}
