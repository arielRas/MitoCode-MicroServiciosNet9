using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastBuy.Orders.Repository.Database.Entities;

[Table("ORDER_STATE")]
public partial class OrderState : SagaStateMachineInstance
{
    [Key]
    [Column("CORRELATION_ID")]
    public Guid CorrelationId { get; set; }

    [Column("LAST_UPDATE")]
    public DateTime LastUpdate { get; set; }

    [Column("CURRENT_STATE")]
    [StringLength(50)]
    [Unicode(false)]
    public string CurrentState { get; set; } = null!;

    [Column("REASON_REJECTION")]
    [Unicode(false)]
    public string? ReasonRejection { get; set; }

    [ForeignKey("CorrelationId")]
    [InverseProperty("OrderState")]
    public virtual Order Order { get; set; } = null!;
}
