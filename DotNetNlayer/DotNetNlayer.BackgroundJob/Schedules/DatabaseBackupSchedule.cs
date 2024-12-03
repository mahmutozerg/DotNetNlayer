using DotNetNlayer.BackgroundJob.Managers.RecurringJobs;
using Hangfire;

namespace DotNetNlayer.BackgroundJob.Schedules;

public static class DatabaseBackupSchedule
{ 
    /// <summary>
     ///
     ///    Bu not geleceğe gelsin. Bu arkadaşi program.cs tarafından bir kere çağırılıp bırakılıyor. Anladığım kadarıyla
     /// bunu çalıştıracaksın kardeşim diyor
     /// </summary>
    public static void SetupDatabaseBackupJob()
    {
        RecurringJob.RemoveIfExists(nameof(DataBaseBackupScheduleJobManager));
        RecurringJob.RemoveIfExists("test");
        RecurringJob.AddOrUpdate<DataBaseBackupScheduleJobManager>(nameof(DataBaseBackupScheduleJobManager),
            job => job.Process(), "0 */8 * * *");
    } 
}