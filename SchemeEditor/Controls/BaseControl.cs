using SchemeEditor.Entities;
using SchemeEditor.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Orientation = SchemeEditor.Infrastructure.Orientation;

namespace SchemeEditor.Controls
{
    // Base class for the UserControl
    public abstract class BaseControl : UserControl
    {
        #region Fields
        // If the control is in the toolbox, field = true 
        private bool _inToolBox;
        protected ContextMenu? _contextMenu;
        // A dictionary that stores formulas for connectors with respect to orientation
        protected Dictionary<Orientation, Point>? _сontrolPointsByOrientation;
        // An event that occurs when an element is rotated
        public event EventHandler? RotateEvent;
        // An event that occurs when an item is deleted
        public event EventHandler? DeleteEvent;
        #endregion

        #region Properties
        // Position of connectors relative to the control
        public List<Point>? PositionsConnectors { get ; protected set; }
        // Connector locations for Adorner
        public List<Point>? PositionnConnectorsForAdorner { get; protected set; }
        // Orientation of connectors in relation to the canvas
        public List<Orientation>? ConnectorOrientation {  get; protected set; }
        public double Angle { get; protected set; }
        public ControlDTO ControlDTO;
        #endregion

        #region Constructors
        // Constructor for creating elements on canvas
        public BaseControl()
        {
            _inToolBox = false;
            Angle = 0;
            ControlDTO = ControlService.FromControlToControlDTO(this);

            CreateContextMenu();
        }
        // Сonstructor for creating elements on a toolbox
        public BaseControl(bool inToolBox) : this()
        {
            _inToolBox = inToolBox;
        }
        #endregion

        #region Methods

        public void ShowContextMenu()
        {
            if (_contextMenu != null)
            {
                _contextMenu.IsOpen = true;
            }
        }

        protected virtual void CreateContextMenu()
        {
            _contextMenu = new ContextMenu();

            MenuItem rotateMenuItem = new MenuItem { Header = "Rotate 90°" };
            rotateMenuItem.Click += (s, e) => RotateControl();

            MenuItem deleteMenuItem = new MenuItem { Header = "Delete" };
            deleteMenuItem.Click += (s, e) => DeleteControl();

            _contextMenu.Items.Add(rotateMenuItem);
            _contextMenu.Items.Add(deleteMenuItem);
        }

        protected void DeleteControl()
        {
            DeleteEvent?.Invoke(this, EventArgs.Empty);
        }

        public void RotateControl(double angle = 90)
        {
            for(int i = 0; i < angle / 90; i++)
            {
                _сontrolPointsByOrientation = new Dictionary<Orientation, Point>
                {
                    { Orientation.Left, new Point(0, Width / 2) },
                    { Orientation.Top, new Point(Width / 2, 0) },
                    { Orientation.Right, new Point(Height, Width / 2) },
                    { Orientation.Bottom, new Point(Width / 2, Height) }
                };
                // Rotate the element
                RotateTransform? _rotateTransform = LayoutTransform as RotateTransform;
                if (_rotateTransform == null)
                {
                    _rotateTransform = new RotateTransform(0);
                    LayoutTransform = _rotateTransform;
                }
                _rotateTransform.Angle = (_rotateTransform.Angle + 90) % 360;
                Angle = _rotateTransform.Angle;
                // Change the position and orientation of connectors
                if (ConnectorOrientation != null && PositionsConnectors != null)
                {
                    for (int j = 0; j < ConnectorOrientation.Count; j++)
                    {
                        ConnectorOrientation[j] = GetNextOrientation(ConnectorOrientation[j]);
                        PositionsConnectors[j] = _сontrolPointsByOrientation[ConnectorOrientation[j]];
                    }
                }
                // Update in database
                ControlService controlService = new ControlService(new ApplicationContext());
                controlService.UpdateAngle(ControlDTO.Id, Angle);
            }

            RotateEvent?.Invoke(this, EventArgs.Empty);
        }

        private Orientation GetNextOrientation(Orientation currentOrientation)
        {
            switch (currentOrientation)
            {
                case Orientation.Left:
                    return Orientation.Top;
                case Orientation.Top:
                    return Orientation.Right;
                case Orientation.Right:
                    return Orientation.Bottom;
                case Orientation.Bottom:
                    return Orientation.Left;
                default:
                    return currentOrientation;
            }
        }
        #endregion
    }
}
