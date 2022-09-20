using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRSDbBackOfficeCapStone.Models
{
    public class Request
    {
        public int Id { get; set; }                             //PK

        [StringLength(80)]
        public string Description { get; set; }

        [StringLength(80)]
        public string Justification { get; set; }

        [StringLength(80)]
        public string? RejectionReason { get; set; }

        [StringLength(20)]
        public string DeliveryMode { get; set; } = "Pickup";

        [StringLength(10)]
        public string Status { get; set; } = "NEW";

        [Column(TypeName ="Decimal (11,2)")]
        public decimal Total { get; set; } = 0;


        public int UserId { get; set; }                     //Fk to User
        public virtual User? User { get; set; }


    }
}
