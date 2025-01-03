using System.Data;
using DotNetNlayer.Core.Constants;
using DotNetNlayer.Core.DTO.Manager;
using DotNetNlayer.Core.Services.Manager;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DotnetNlayer.Service.Services.ManagerServices;

public class DatabaseBackupJobService:IDatabaseBackupJobService 
{
    private readonly DatabaseBackupJobOptionsDto _databaseBackupJobOptionsDto;
    private readonly ILogger<DatabaseBackupJobService> _logger;
    public DatabaseBackupJobService(IOptions<DatabaseBackupJobOptionsDto> databaseOptionDto, ILogger<DatabaseBackupJobService> logger)
    {
        _logger = logger;
        _databaseBackupJobOptionsDto = databaseOptionDto.Value;
    }

    public async Task BackupDatabase()
    {
        _logger.LogInformation($"Starting backup at {nameof(BackupDatabase)}");
        var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var commandText = string.Format(DatabaseConstants.BackupCommandText, _databaseBackupJobOptionsDto.DatabaseName, _databaseBackupJobOptionsDto.BackupPath+time, _databaseBackupJobOptionsDto.DatabaseName);
        
        await using var connection = new SqlConnection(_databaseBackupJobOptionsDto.ConnectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = commandText;
        command.CommandType = CommandType.Text;
        await command.ExecuteNonQueryAsync();
        _logger.LogInformation($"Backup done at {nameof(BackupDatabase)}");

    }


}