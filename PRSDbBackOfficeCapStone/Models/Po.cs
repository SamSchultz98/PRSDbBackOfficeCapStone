using System.ComponentModel.DataAnnotations.Schema;

namespace PRSDbBackOfficeCapStone.Models
{
    public class Po
    {
        public Vendor Vendor { get; set; }
        public IEnumerable<Poline> polines { get; set; }

        [Column(TypeName =("Decimal (11,2)"))]  //Only works when updating the database
        public decimal PoTotal { get; set; }
    }
    
}
