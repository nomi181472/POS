using AutoMapper;
using DA.AppDbContexts;
using Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Till.Test
{
    public class Initializer
    {
        public readonly Mock<ICustomLogger> mockLogger;
        public readonly Mock<IMapper> mockMapper;
        public readonly Mock<IConfiguration> mockConfig;
        private ServiceCollection services;
        private ServiceProvider _serviceProvider;
        public CancellationToken token;
        public readonly dynamic logger;
        public readonly dynamic mapper;
        public readonly dynamic config;
        public readonly dynamic cache;
        public readonly dynamic _scope;
        public readonly dynamic _scopedServices;
        public readonly dynamic _dbContext;

        public Initializer()
        {
            services = new ServiceCollection();
            mockLogger = new Mock<ICustomLogger>();
            mockMapper = new Mock<IMapper>();
            mockConfig = new Mock<IConfiguration>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TestDb")
            );
            services.AddSingleton<ICustomLogger>(mockLogger.Object);
            services.AddSingleton<IMapper>(mockMapper.Object);
            services.AddSingleton<IConfiguration>(mockConfig.Object);

            _serviceProvider = services.BuildServiceProvider();

            logger = _serviceProvider.GetRequiredService<ICustomLogger>();
            mapper = _serviceProvider.GetRequiredService<IMapper>();
            config = _serviceProvider.GetRequiredService<IConfiguration>();
            var scope = _serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var dbContext = scopedServices.GetRequiredService<AppDbContext>();
            _scope = scope;
            _scopedServices = scopedServices;
            _dbContext = dbContext;
        }
    }
}
