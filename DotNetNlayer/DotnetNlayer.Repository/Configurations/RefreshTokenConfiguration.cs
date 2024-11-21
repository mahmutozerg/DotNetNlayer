using DotNetNlayer.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetNlayer.Repository.Configurations;

public class RefreshTokenConfiguration:IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.HasKey(t => t.UserId);
        builder.Property(t => t.Token).IsRequired();
    }
}