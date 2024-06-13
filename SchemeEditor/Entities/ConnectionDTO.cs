using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchemeEditor.Entities
{
    [Table("Connection")]
    public class ConnectionDTO
    {
        [Key]
        public Guid Id { get; set; }
        public Guid StartConnectorId { get; set; }
        public Guid EndConnectorId { get; set; }
        public ConnectorDTO? StartConnector { get; set; }
        public ConnectorDTO? EndConnector { get; set; }
        public Guid SchemeId { get; set; }
        public SchemeDTO? Scheme { get; set; }
    }
}
