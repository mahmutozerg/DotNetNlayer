using DotNetNlayer.Core.Services.AdminServices;
using DotNetNlayer.Core.Services.Manager;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace DotNetNlayer.BackgroundJob.Managers.RecurringJobs;

/// <summary>
///  Bu class aslında joblar için bir api controller gibi hissediyorum ne kadar doğrudur inan bilmiyorum fakat
/// </summary>
public class DataBaseBackupScheduleJobManager
{
    
    private readonly IDatabaseBackupJobService _databaseBackupJobService;
    public DataBaseBackupScheduleJobManager(IDatabaseBackupJobService databaseBackupJobService)
    {
        _databaseBackupJobService = databaseBackupJobService;
    }
  
    [DisableConcurrentExecution(28800)] 
    public async Task Process()
    {
        await _databaseBackupJobService.BackupDatabase();
    }
}