using SchemeEditor.Controls;
using SchemeEditor.Entities;
using SchemeEditor.Infrastructure;
using SchemeEditor.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Orientation = SchemeEditor.Infrastructure.Orientation;

namespace SchemeEditor.Models
{
    // Class element on canvas
    public class CanvasItem : ObservableObject
    {
        #region Fields
        private BaseControl? _control;
        private Point _positionOnCanvas;
        private ConnectorAdorner? _connectorAdorner;
        private double _width;
        private double _height;
        // An event that occurs when an item is deleted
        public static EventHandler? DeleteEvent;
        #endregion

        #region Properties
        public BaseControl? Control
        {
            get => _control;
            set
            {
                _control = value;
                OnPropertyChanged();
            }
        }

        public Point PositionOnCanvas
        {
            get => _positionOnCanvas;
            set
            {
                _positionOnCanvas = value;
                UpdatePosition();
                OnPropertyChanged();
            }
        }

        public List<Connector> Connectors { get; private set; }
        public Rect Obstacle { get; private set; }
        public CanvasItemDTO CanvasItemDTO { get; private set; }
        #endregion

        #region Commands
        // Command to start moving controls
        public ICommand MouseLeftButtonDownCommand { get; }
        private void OnMouseLeftButtonDownCommandExecute(object parameters)
        {
            if (parameters is Canvas canvas)
            {
                DragDrop.DoDragDrop(canvas, this, DragDropEffects.Move);
            }
        }
        // Command to open the context menu
        public ICommand MouseRightButtonDownCommand { get; }
        private void OnMouseRightButtonDownCommandExecute(object parameters)
        {
            if(_control != null)
            {
                _control.ShowContextMenu();
            }
        }
        // Command to show Adorner
        public ICommand MouseEnterCommand { get; }
        private void OnMouseEnterCommandExecute(object parameters)
        {
            ShowConnectorAdorner();
        }
        // Command to hide Adorner
        public ICommand MouseLeaveCommand { get; }
        private void OnMouseLeaveCommandExecute(object parameters)
        {
            HideConnectorAdorner();
        }
        #endregion

        #region Constructors
        public CanvasItem(BaseControl control, Point positionOnCanvas, CanvasItemDTO? canvasItemDTO = null)
        {
            Connectors = new List<Connector>();
            Obstacle = new Rect(); 

            Control = control;
            PositionOnCanvas = positionOnCanvas;
            _width = Control.Width;
            _height = Control.Height;

            MouseLeftButtonDownCommand = new LambdaCommand(OnMouseLeftButtonDownCommandExecute);
            MouseRightButtonDownCommand = new LambdaCommand(OnMouseRightButtonDownCommandExecute);
            MouseEnterCommand = new LambdaCommand(OnMouseEnterCommandExecute);
            MouseLeaveCommand = new LambdaCommand(OnMouseLeaveCommandExecute);

            Control.RotateEvent += OnRotate;
            Control.DeleteEvent += OnDelete;

            if(canvasItemDTO == null)
            {
                CanvasItemDTO = CanvasItemService.FromCanvasItemToCanvasItemDTO(this);
            }
            else
            {
                CanvasItemDTO = canvasItemDTO;
            }
        }
        #endregion

        #region Events
        private void OnRotate(object? sender, EventArgs e)
        { 
            if(Connectors != null && Control != null && Control.PositionsConnectors != null && Control.ConnectorOrientation != null)
            {
                for(int i = 0; i < Connectors.Count; i++)
                {
                    Connectors[i].Update(Control.PositionsConnectors[i], Control.ConnectorOrientation[i], PositionOnCanvas);
                }
            }
            // Swap the height and width
            double temp = _width;
            _width = _height;
            _height = temp;

            AddObstacle();
        }

        private void OnDelete(object? sender, EventArgs e)
        {
            DeleteEvent?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Methods
        public CanvasItem ChangeOpacity(double newOpacity)
        {
            if(newOpacity >= 0 && newOpacity <= 1 && Control != null)
            {
                Control.Opacity = newOpacity;
            }

            return this;
        }

        public CanvasItem ShowConnectorAdorner()
        {
            if(_connectorAdorner == null && Control != null && Control.PositionsConnectors != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(Control);

                if (adornerLayer != null)
                {
                    _connectorAdorner = new ConnectorAdorner(Control, this);
                    adornerLayer.Add(_connectorAdorner);
                }
            }
            else if(_connectorAdorner != null)
            {
                _connectorAdorner.Visibility = Visibility.Visible;
            }

            return this;
        }

        public CanvasItem HideConnectorAdorner()
        {
            if(_connectorAdorner != null)
            {
                _connectorAdorner.Visibility = Visibility.Hidden;
            }

            return this;
        }
        
        private void UpdatePosition()
        {
            if(Connectors.Count == 0)
            {
                AddConnectors();
            }
            else
            {
                foreach (var connector in Connectors)
                {
                    connector.UpdatePositionOnCanvas(PositionOnCanvas);
                }
            }

            // Update position in database
            if(CanvasItemDTO != null)
            {
                CanvasItemDTO.PositionOnCanvasX = PositionOnCanvas.X;
                CanvasItemDTO.PositionOnCanvasY = PositionOnCanvas.Y;
                CanvasItemService canvasItemService = new CanvasItemService(new ApplicationContext());
                canvasItemService.UpdatePosition(CanvasItemDTO.Id, PositionOnCanvas);
            }
            AddObstacle();
        }
         
        private void AddConnectors()
        {
            if (_control != null && _control.PositionsConnectors != null && _control.ConnectorOrientation != null)
            {
                for (int i = 0; i < _control.PositionsConnectors.Count; i++)
                {
                    Connectors.Add(new Connector(PositionOnCanvas, _control.PositionsConnectors[i], _control.ConnectorOrientation[i]));             
                }
            }
        }

        private void AddObstacle()
        {
            if(_control != null)
            {
                bool hasLeft = Connectors.Any(c => c.Orientation == Orientation.Left);
                bool hasTop = Connectors.Any(c => c.Orientation == Orientation.Top);
                bool hasRight = Connectors.Any(c => c.Orientation == Orientation.Right);
                bool hasBottom = Connectors.Any(c => c.Orientation == Orientation.Bottom);

                int margin = 5;

                double offsetX = hasLeft ? margin : 0;
                double offsetY = hasTop ? margin : 0;
                double widthChange = hasLeft ? -margin : 0;
                double heightChange = hasBottom ? -margin : 0;
                
                heightChange += hasTop ? -margin : 0;
                widthChange += hasRight ? -margin : 0;

                PathFinder.Obstacles.Remove(Obstacle);

                Obstacle = new Rect(PositionOnCanvas.X - margin + offsetX, PositionOnCanvas.Y - margin + offsetY, _width + 2 * margin + widthChange, _height + 2 * margin + heightChange);

                PathFinder.Obstacles.Add(Obstacle);
            }
        }
        #endregion
    }
}
