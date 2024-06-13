using SchemeEditor.Entities;
using SchemeEditor.Infrastructure;
using SchemeEditor.Models;
using System.Windows;

namespace SchemeEditor.Services
{
    public class ConnectorService : Service
    {
        public ConnectorService(ApplicationContext context) : base(context)
        {
            
        }

        public static ConnectorDTO FromConnectorToConnectorDTO(Connector connector)
        {
            ConnectorDTO connectorDTO = new ConnectorDTO()
            {
                Id = Guid.NewGuid(),
                PositionOnControlX = connector.PositionOnControl.X,
                PositionOnControlY = connector.PositionOnControl.Y,
                Orientation = connector.Orientation
            };

            return connectorDTO;
        }

        public void UpdateOrientationAndPositionOnControl(Guid connectorDTOId, Orientation orientation, Point positionOnControl)
        {
            using (_context)
            {
                ConnectorDTO? connectorDTO = _context.Connectors.FirstOrDefault(item => item.Id == connectorDTOId); 

                if(connectorDTO != null)
                {
                    connectorDTO.Orientation = orientation;
                    connectorDTO.PositionOnControlX = positionOnControl.X;
                    connectorDTO.PositionOnControlY = positionOnControl.Y;
                    _context.SaveChanges();
                }
            }
        }
    }
}
