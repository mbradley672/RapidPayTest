namespace WebApplication1.PseudoServices;

public class UniversalFeesExchangePseudoService
{
    private  readonly Random random = new();
    internal  decimal CurrentFee { get; set; }
    internal  decimal LastFee { get; set; } = 0m;
    private  DateTime? LastTimeFeeUpdated { get; set; }

    protected async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (LastTimeFeeUpdated == null || DateTime.Now - LastTimeFeeUpdated >= TimeSpan.FromHours(1))
            {
                double range = 2;
                decimal newDecimal = (decimal)random.NextDouble() * (decimal)range;
                CurrentFee = CurrentFee == 0 ? newDecimal : CurrentFee * newDecimal;
                LastTimeFeeUpdated = DateTime.Now;
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    internal void UpdateLastFee(decimal lastFee) => LastFee = lastFee;

    public decimal GetFee(decimal amount)
    {
        if (LastTimeFeeUpdated == null || DateTime.Now - LastTimeFeeUpdated >= TimeSpan.FromHours(1))
        {
            double range = 2;
            decimal newDecimal = (decimal)random.NextDouble() * (decimal)range;
            CurrentFee = CurrentFee == 0 ? newDecimal : CurrentFee * newDecimal;
            LastTimeFeeUpdated = DateTime.Now;
        }
        UpdateLastFee(CurrentFee);
        return CurrentFee;
    }
}