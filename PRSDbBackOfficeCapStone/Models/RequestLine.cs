using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace PRSDbBackOfficeCapStone.Models
{
    public class RequestLine
    {
        public int Id { get; set; }

        public int RequestId { get; set; }//FK to Request

        [JsonIgnore]
        public virtual Request? Request { get; set; }       //BackReference



        public int ProductId { get; set; }       //FK to Product
        public virtual Product? Product { get; set; }




        public int Quantity { get; set; } = 1;


    }
}
