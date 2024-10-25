using System.Diagnostics;

namespace RiotKiller
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private string _leagueClientProcessName = "LeagueClient";
        private string _riotClientProcessName = "RiotClientServices";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var leagueProcess = Process.GetProcessesByName(_leagueClientProcessName).FirstOrDefault();

                if (leagueProcess != null)
                {
                    _logger.LogInformation("Watching process: {ProcessName}", _leagueClientProcessName);
                    RegisterExit(leagueProcess);

                    await Task.Run(() => leagueProcess.WaitForExit(), stoppingToken);
                }
                else
                {
                    _logger.LogInformation("Process {ProcessName} is not currently running. Checking again soon...", _leagueClientProcessName);
                }

                // Wait a bit before checking again
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private void RegisterExit(Process clientProcess)
        {
            clientProcess.EnableRaisingEvents = true;

            clientProcess.Exited += (sender, e) =>
            {
                _logger.LogInformation("Process {ProcessName} has exited at {Time}.", _leagueClientProcessName, DateTime.Now);

                CloseRiotClient();
            };
        }

        private void CloseRiotClient()
        {
            var riotClients = Process.GetProcessesByName(_riotClientProcessName);

            foreach (var riotClient in riotClients)
            {
                _logger.LogInformation("Killing process {ProcessName}:{PID} at {Time}", _riotClientProcessName, riotClient.Id, DateTime.Now);
                riotClient.Kill();

            }
        }
    }
}