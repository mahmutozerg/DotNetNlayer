namespace DotNetNlayer.Core.DTO.Manager;

public class DatabaseBackupJobOptionsDto
{
    public string BackupPath { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}