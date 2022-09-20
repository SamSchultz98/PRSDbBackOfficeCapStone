namespace PRSDbBackOfficeCapStone.Models
{
    public class RequestLine
    {
        public int Id { get; set; }

        public int RequestId { get; set; }       //FK to Request
        public virtual Request? Request { get; set; }



        public int ProductId { get; set; }       //FK to Product
        public virtual Product? Product { get; set; }




        public int Quantity { get; set; } = 1;


    }
}
