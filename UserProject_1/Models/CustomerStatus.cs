using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserProject_1.Models
{
    public class CustomerStatus
    {
        [Key]
        [DisplayName("S_N.O")]
        public int Id { get; set; }
        public double Rent { get; set; }
        public double Ebill { get; set; }
        public DateTime Date { get; set; }
        public string Due { get; set; }
        public string Status { get; set; }

        public int CustomerDetailId { get; set; }
        public CustomerDetails customerDetails { get; set; }
    }
}
