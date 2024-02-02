namespace TestDeleF.Services
{
    
    public class PlayerDisposer : TimerService
    {
        private readonly PlayerService _playerService;
        private readonly ILogger _logger;

        public PlayerDisposer(PlayerService playerService, ILogger<PlayerDisposer> logger)
        {
            _playerService = playerService;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken stoppingToken)
        {
            interval = 1;
            return base.StartAsync(stoppingToken);
        }

        public async override void DoWork(object? state)
        {
            await _playerService.RemoveOutdatedAsync();
            _logger.LogDebug("Remoed outdated");
        }
    }
}
