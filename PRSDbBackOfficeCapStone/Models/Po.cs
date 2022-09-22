namespace PRSDbBackOfficeCapStone.Models
{
    public class Po
    {
        public Vendor Vendor { get; set; }
        public IEnumerable<Poline> polines { get; set; }
        public decimal PoTotal { get; set; }
    }
    public class Poline
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal LineTotal { get; set; }
    }
}
