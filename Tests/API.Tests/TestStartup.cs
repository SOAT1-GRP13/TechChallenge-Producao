using API.Setup;
using System.Text;
using Infra.Pedidos;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Application.Pedidos.AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Tests
{
    public class TestStartup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var jsonString = @"{""ConnectionString"": ""teste""}";

            var configuration = new ConfigurationBuilder()
                    .AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                    .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddDbContext<PedidosContext>(options =>
                options.UseNpgsql("User ID=fiap;Password=S3nh@L0c@lP40ducao;Host=localhost;Port=15433;Database=techChallengeProduto;Pooling=true;"));

            services.AddAutoMapper(typeof(PedidosMappingProfile));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            DependencyInjection.RegisterServices(services);

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
