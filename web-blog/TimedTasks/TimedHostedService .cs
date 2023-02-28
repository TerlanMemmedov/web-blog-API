using Microsoft.Extensions.Caching.Memory;
using web_blog.Data;
using web_blog.Data.Services;

namespace web_blog.TimedTasks
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        public IConfiguration _configuration { get; }
        protected IMemoryCache _memoryCache;
        //private readonly ArticlesService _articlesService;
        //public IHttpClientFactory _httpClientFactory;
        readonly IServiceScopeFactory _serviceScopeFactory;
        private int executionCount = 0;
        private Timer? _timer = null;


        public TimedHostedService(IConfiguration configuration,
            IMemoryCache memoryCache, IServiceScopeFactory HttpserviceScopeFactory,
            IServiceScopeFactory serviceScope)
        {
            _configuration = configuration;
            //_articlesService = articlesService;
            _memoryCache = memoryCache;
            //_httpClientFactory = httpClientFactory;
            _serviceScopeFactory = serviceScope;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            //var request = new HttpRequestMessage(HttpMethod.Get, "");
            //await _articlesService.DelTestTimer();

            //using (var scope = _serviceScopeFactory.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //}


            //var count = Interlocked.Increment(ref executionCount);

            using var scope = _serviceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ArticlesService>();

            await service.DelTestTimer();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }



        //

        internal interface IScopedProcessingService
        {
            Task DoWork(CancellationToken stoppingToken);
        }

        internal class ScopedProcessingService : IScopedProcessingService
        {
            private int executionCount = 0;

            public ScopedProcessingService() { }

            public async Task DoWork(CancellationToken stoppingToken)
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    executionCount++;

                    await Task.Delay(10000, stoppingToken);
                }
            }
        }

        public class ConsumeScopedServiceHostedService : BackgroundService
        {
            public ConsumeScopedServiceHostedService(IServiceProvider services)
            {
                Services = services;
            }

            public IServiceProvider Services { get; }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                await DoWork(stoppingToken);
            }

            private async Task DoWork(CancellationToken stoppingToken)
            {
                using (var scope = Services.CreateScope())
                {
                    var scopedProcessingService = scope.ServiceProvider
                        .GetRequiredService<IScopedProcessingService>();

                    await scopedProcessingService.DoWork(stoppingToken);
                }
            }

            public override async Task StopAsync(CancellationToken stoppingToken)
            {
                await base.StopAsync(stoppingToken);
            }
        }

    }
}
