using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UserProject_1.Models
{
    public class CustomerDetails
    {
        [Key]
        public int CustomerDetailId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone {  get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int  Deposit { get; set; }
        [Required]
        public string JoiningDate { get; set; }
        public string ProfileImageUrl { get; set; }
        public string IdProof { get; set; }

        public ICollection<CustomerStatus> CustomerStatuses { get; set; }
    }
}
