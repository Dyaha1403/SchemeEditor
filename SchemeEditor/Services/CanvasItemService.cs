using SchemeEditor.Entities;
using SchemeEditor.Models;
using System.Windows;
using SchemeEditor.Controls;

namespace SchemeEditor.Services
{
    public class CanvasItemService : Service
    { 
        public CanvasItemService(ApplicationContext context) : base(context)
        {

        }

        public static CanvasItemDTO FromCanvasItemToCanvasItemDTO(CanvasItem canvasItem)
        {
            List<ConnectorDTO> connectorDTOs = new List<ConnectorDTO>();
            foreach(var c in canvasItem.Connectors)
            {
                if(c.ConnectorDTO != null)
                {
                    connectorDTOs.Add(c.ConnectorDTO);
                }
            }

            CanvasItemDTO canvasItemDTO = new CanvasItemDTO
            {
                Id = Guid.NewGuid(),
                Control = canvasItem.Control.ControlDTO,
                PositionOnCanvasX = canvasItem.PositionOnCanvas.X,
                PositionOnCanvasY = canvasItem.PositionOnCanvas.Y,
                Connectors = connectorDTOs
            };

            return canvasItemDTO;
        }

        public static CanvasItem? FromCanvasItemDTOToCanvasItem(CanvasItemDTO canvasItemDTO)
        {
            using (var context = new ApplicationContext())
            {
                ControlDTO? controlDTO = context.Controls.FirstOrDefault(item => item.CanvasItemID == canvasItemDTO.Id);
                List<ConnectorDTO> connectorDTOs = context.Connectors.Where(c => c.CanvasItemId == canvasItemDTO.Id).ToList();
                CanvasItem? canvasItem = null;
                if (controlDTO != null)
                {
                    BaseControl control = ControlService.FromControlDTOToControl(controlDTO);
                    control.ControlDTO = controlDTO;
                    Point positionOnCanvas = new Point(canvasItemDTO.PositionOnCanvasX, canvasItemDTO.PositionOnCanvasY);
                    canvasItem = new CanvasItem(control, positionOnCanvas, canvasItemDTO);
                    canvasItem.Control.RotateControl(controlDTO.Angle);
                    for(int i = 0; i < canvasItem.Connectors.Count; i++)
                    {
                        canvasItem.Connectors[i].ConnectorDTO = connectorDTOs[i];
                    }
                }

                return canvasItem;
            }
        }

        public void UpdatePosition(Guid canvasItemDTOId, Point positionOnCanvas)
        {
            using (_context)
            {
                CanvasItemDTO? canvasItemDTO = _context.CanvasItems.FirstOrDefault(item => item.Id == canvasItemDTOId);

                if(canvasItemDTO != null)
                {
                    canvasItemDTO.PositionOnCanvasX = positionOnCanvas.X;
                    canvasItemDTO.PositionOnCanvasY = positionOnCanvas.Y;
                    _context.SaveChanges();
                }
            }
        }
    }
}
