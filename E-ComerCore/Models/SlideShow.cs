using System.ComponentModel.DataAnnotations.Schema;

namespace E_ComerCore.Models
{
    [Table("SlideShow")]
    public partial class SlideShow
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
