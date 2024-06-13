using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchemeEditor.Entities
{
    [Table("ControlElement")]
    public class ControlDTO
    {
        [Key]
        public Guid Id { get; set; }
        public string TypeName { get; set; }

        [NotMapped]
        public Type Type
        {
            get => TypeName != null ? Type.GetType(TypeName) : null;
            set => TypeName = value != null ? value.FullName : null;
        }

        public double Angle { get; set; }
        [Column("SchemeElementId")]
        public Guid CanvasItemID { get; set; }
        public CanvasItemDTO? CanvasItem { get; set; }
    }
}
