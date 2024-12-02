namespace DotNetNlayer.Core.Services.Manager;

public interface IDatabaseBackupJobService
{
    Task BackupDatabase();

}