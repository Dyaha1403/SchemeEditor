using SchemeEditor.Models;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using SchemeEditor.Controls;

namespace SchemeEditor.Infrastructure
{
    // Class for displaying connectors on an element
    public class ConnectorAdorner : Adorner
    {
        #region Fields
        private double _connectorRadius;
        public CanvasItem _canvasItem { get; private set; }
        // Event for passing start position, for creating a Connector, to the ViewModel 
        public static event EventHandler<Connector>? ConnectorPressed;
        public static event EventHandler<Connector>? ConnectorButtonUp;
        #endregion

        #region Constructors
        public ConnectorAdorner(UIElement adornedElement, CanvasItem canvasItem)
            : base(adornedElement)
        {
            _canvasItem = canvasItem;
            _connectorRadius = 4;

            this.MouseEnter += ConnectorAdorner_MouseEnter;
            this.MouseLeave += ConnectorAdorner_MouseLeave;
            this.MouseMove += ConnectorAdorner_MouseMove;
            this.MouseLeftButtonUp += ConnectorAdorner_MouseLeftButtonUp;
        }
        #endregion

        #region Events
        // Used to start creating a connector
        private void ConnectorAdorner_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if(AdornedElement is BaseControl element && element.PositionnConnectorsForAdorner != null)
                {
                    Point mousePosition = e.GetPosition(this);
                    for (int i = 0; i < element.PositionnConnectorsForAdorner.Count; i++)
                    {
                        if (IsMouseOverConnector(mousePosition, element.PositionnConnectorsForAdorner[i]))
                        {
                            ConnectorPressed?.Invoke(this, _canvasItem.Connectors[i]);
                        }
                    }
                }             
            }
        }
        // Used to end creating a connector
        private void ConnectorAdorner_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (AdornedElement is BaseControl element && element.PositionnConnectorsForAdorner != null)
            {
                Point mousePosition = e.GetPosition(this);

                for(int i = 0; i < element.PositionnConnectorsForAdorner.Count; i++)
                {
                    if(IsMouseOverConnector(mousePosition, element.PositionnConnectorsForAdorner[i]))
                    {
                        ConnectorButtonUp?.Invoke(this, _canvasItem.Connectors[i]);
                    }
                }
            }
        }

        private void ConnectorAdorner_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }

        private void ConnectorAdorner_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Methods
        // Check if the cursor is on the connector 
        private bool IsMouseOverConnector(Point mousePosition, Point connectorPosition)
        {
            double distance = Math.Sqrt(Math.Pow(mousePosition.X - connectorPosition.X, 2) + Math.Pow(mousePosition.Y - connectorPosition.Y, 2));
            return distance <= _connectorRadius;
        }
        // Draws a connector on an element
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (AdornedElement is BaseControl element && element.PositionnConnectorsForAdorner != null)
            {
                base.OnRender(drawingContext);

                SolidColorBrush semiTransparentBlueBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 255));
                foreach (var centerPosition in element.PositionnConnectorsForAdorner)
                {
                    drawingContext.DrawEllipse(semiTransparentBlueBrush, null, centerPosition, _connectorRadius, _connectorRadius);
                }
            }
        }
        #endregion
    }
}
