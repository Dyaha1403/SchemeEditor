using System.Windows;
using SchemeEditor.Entities;
using SchemeEditor.Services;
using Orientation = SchemeEditor.Infrastructure.Orientation;

namespace SchemeEditor.Models
{
    public class Connector
    {
        public Point PositionOnCanvas {  get; private set; }

        public Point PositionOnControl {  get; private set; }

        public Orientation Orientation { get; private set; }
        public ConnectorDTO ConnectorDTO;

        public event EventHandler? PositionChanged;

        public Connector(Point positionControlOnCanvas, Point positionConnectorOnControl, Orientation orientation)
        {
            PositionOnControl = positionConnectorOnControl;
            Orientation = orientation;

            UpdatePositionOnCanvas(positionControlOnCanvas);
            ConnectorDTO = ConnectorService.FromConnectorToConnectorDTO(this);
        }

        public Connector(Point positionOnCanvas)
        {
            PositionOnCanvas = positionOnCanvas;
        }

        public void UpdatePositionOnCanvas(Point positionControlOnCanvas)
        {
            PositionOnCanvas = new Point(positionControlOnCanvas.X + PositionOnControl.X,
                                         positionControlOnCanvas.Y + PositionOnControl.Y);
            PositionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Update(Point newPositionOnControl, Orientation newOrientation, Point positionControlOnCanvas)
        {
            PositionOnControl = newPositionOnControl;
            Orientation = newOrientation;

            ConnectorService connectionService = new ConnectorService(new ApplicationContext());
            connectionService.UpdateOrientationAndPositionOnControl(ConnectorDTO.Id, Orientation, PositionOnControl);

            UpdatePositionOnCanvas(positionControlOnCanvas);
        }
    }
}
