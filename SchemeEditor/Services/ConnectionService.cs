using SchemeEditor.Entities;
using SchemeEditor.Models;

namespace SchemeEditor.Services
{
    public class ConnectionService : Service
    {
        public ConnectionService(ApplicationContext context) : base(context)
        {

        }

        public static ConnectionDTO FromConnectionToConnectionDTO(Connection connection)
        {
            ConnectionDTO connectionDTO = new ConnectionDTO()
            {
                Id = Guid.NewGuid(),
                StartConnectorId = connection.StartConnector.ConnectorDTO.Id,
                EndConnectorId = connection.EndConnector.ConnectorDTO.Id
            };
            
            return connectionDTO;
        }

        public static Connection? FromConnectionDTOToConnectio(ConnectionDTO connectionDTO, List<CanvasItem> canvasItems)
        {
            Connector? startConnector = null;
            Connector? endConnector = null;
            Connection? connection = null;

            foreach (var canvasItem in canvasItems)
            {
                if(startConnector == null)
                {
                    startConnector = canvasItem.Connectors.FirstOrDefault(c => c.ConnectorDTO.Id == connectionDTO.StartConnectorId);
                }

                if(endConnector == null)
                {
                    endConnector = canvasItem.Connectors.FirstOrDefault(c => c.ConnectorDTO.Id == connectionDTO.EndConnectorId);
                }

                if(startConnector != null && endConnector != null)
                {
                    break;
                }
            }

            if(startConnector != null && endConnector != null && startConnector != endConnector)
            {
                connection = new Connection();
                connection.ConnectionDTO = connectionDTO;
                connection.StartConnector = startConnector;
                connection.EndConnector = endConnector;
            }

            return connection;
        }

        public void Update(Guid connectionId, Guid startConnectorId, Guid endConnectorId)
        {
            using (_context)
            {
                ConnectionDTO? connectionDTO = _context.Connections.FirstOrDefault(item => item.Id == connectionId);

                if (connectionDTO != null)
                {
                    connectionDTO.StartConnectorId = startConnectorId;
                    connectionDTO.EndConnectorId = endConnectorId;
                    _context.SaveChanges();
                }
            }
        }
    }
}
