using InventoryСontrol.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryСontrol.Storage.Mapping
{
    public sealed class PreOrderMap : IEntityTypeConfiguration<PreOrder>
    {
        public void Configure(EntityTypeBuilder<PreOrder> builder)
        {
            builder.ToTable("PreOrder");
            builder.HasKey(item => item.PreOrderId);

            builder
                .HasOne(item => item.Item)
                .WithMany(item => item.PreOrders)
                .HasForeignKey(item => item.ItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(item => item.User)
                .WithMany(item => item.PreOrders)
                .HasForeignKey(item => item.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}