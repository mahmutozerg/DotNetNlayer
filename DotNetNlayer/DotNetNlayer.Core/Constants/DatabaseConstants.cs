namespace DotNetNlayer.Core.Constants;


/// <summary>
///  This file is used for Database command that generates backups
/// </summary>
public static class DatabaseConstants
{
    public const string BackupCommandText = @"BACKUP DATABASE [{0}] TO DISK = N'{1}' WITH NOFORMAT, INIT, NAME = N'{2}-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";


}