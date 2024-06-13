using SchemeEditor.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchemeEditor.Entities
{
    [Table("Connector")]
    public class ConnectorDTO
    {
        [Key]
        public Guid Id { get; set; }
        public double PositionOnControlX { get; set; }
        public double PositionOnControlY { get; set; }
        public Orientation Orientation { get; set; }
        [Column("SchemeElementId")]
        public Guid CanvasItemId { get; set; }
        public CanvasItemDTO? CanvasItem { get; set; }
    }
}
