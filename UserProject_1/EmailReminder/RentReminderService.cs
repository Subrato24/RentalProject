using UserProject_1.Connection;
using UserProject_1.EmailReminder;
using Microsoft.EntityFrameworkCore;

namespace UserProject_1.EmailRminder
{
    public class RentReminderService: BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RentReminderService> _logger;

        public RentReminderService(IServiceScopeFactory scopeFactory, ILogger<RentReminderService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {

                        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        var customers = db.CustStatus
                            .Where(c => c.Date <= DateTime.Today && c.Status != "Paid")
                            .Include(c => c.customerDetails)
                            .ToList();

                        foreach (var c in customers)
                        {
                            // Send email (write a helper method here)
                            _logger.LogInformation($"Sending reminder to {c.customerDetails.Email}");
                            await EmailHelper.SendReminderAsync(c.customerDetails.Email, c.customerDetails.Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in RentReminderService");
                }
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken); // Run daily
            }
        }
    }
}
