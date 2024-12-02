using Hangfire;
using Hangfire.Storage;

namespace DotNetNlayer.BackgroundJob;

public static class JobRetriever
{
    public static void GetAllRecurringJobs()
    {
        using var connection = JobStorage.Current.GetConnection();
        var recurringJobs = connection.GetRecurringJobs();

        foreach (var job in recurringJobs)
        {
            Console.WriteLine($"Job ID: {job.Id}");
            Console.WriteLine($"Cron Expression: {job.Cron}");
            Console.WriteLine($"Next Execution: {job.NextExecution}");
            Console.WriteLine($"Last Execution: {job.LastExecution}");
            Console.WriteLine($"Method: {job.Job.Method.Name}");
            Console.WriteLine("----------------------------");
        }
    }
}