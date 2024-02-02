using System.Timers;
using Timer = System.Threading.Timer;

namespace TestDeleF.Services
{
    public class TimerService : IHostedService, IDisposable
    {

        private Timer? _timer = null;
        public float interval = 5;

        public virtual Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(interval));

            return Task.CompletedTask;
        }

        virtual public void DoWork(object? state)
        {
            
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
    }
}
