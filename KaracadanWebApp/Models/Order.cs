using KaracadanWebApp.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KaracadanWebApp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int No { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int StatusId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }


    }
}