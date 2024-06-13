using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchemeEditor.Entities
{
    [Table("Scheme")]
    public class SchemeDTO
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<CanvasItemDTO>? CanvasItems { get; set; }
        public ICollection<ConnectionDTO>? Connections { get; set; }
    }
}
