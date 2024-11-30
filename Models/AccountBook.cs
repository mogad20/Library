using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class AccountBook
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayName("Start Date")]
        //public DateTime startDate { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayName("End Date")]
        //public DateTime endDate { get; set; }

        //public bool IsReturned { get; set; }
    }
}
