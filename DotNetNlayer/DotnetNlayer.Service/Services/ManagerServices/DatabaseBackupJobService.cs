using System.Data;
using DotNetNlayer.Core.Constants;
using DotNetNlayer.Core.DTO.Manager;
using DotNetNlayer.Core.Services.Manager;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace DotnetNlayer.Service.Services.ManagerServices;

public class DatabaseBackupJobService:IDatabaseBackupJobService 
{
    private readonly DatabaseBackupJobOptionsDto _databaseBackupJobOptionsDto;

    public DatabaseBackupJobService(IOptions<DatabaseBackupJobOptionsDto> databaseOptionDto)
    {
        _databaseBackupJobOptionsDto = databaseOptionDto.Value;
    }

    public async Task BackupDatabase()
    {
        var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var commandText = string.Format(DatabaseConstants.BackupCommandText, _databaseBackupJobOptionsDto.DatabaseName, _databaseBackupJobOptionsDto.BackupPath+time, _databaseBackupJobOptionsDto.DatabaseName);
        
        await using SqlConnection connection = new SqlConnection(_databaseBackupJobOptionsDto.ConnectionString);
        await connection.OpenAsync();
        await using SqlCommand command = connection.CreateCommand();
        command.CommandText = commandText;
        command.CommandType = CommandType.Text;
        await command.ExecuteNonQueryAsync();

    }


}