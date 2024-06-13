using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchemeEditor.Entities
{
    [Table("SchemeElement")]
    public class CanvasItemDTO
    {
        [Key]
        public Guid Id { get; set; }
        public ControlDTO? Control { get; set; }
        public double PositionOnCanvasX { get; set; }
        public double PositionOnCanvasY { get; set; }
        public ICollection<ConnectorDTO>? Connectors { get; set; }
        public Guid SchemeId { get; set; }
        public SchemeDTO? Scheme { get; set; }
    }
}
