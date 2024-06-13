using SchemeEditor.Controls;
using SchemeEditor.Infrastructure;
using SchemeEditor.Models;
using SchemeEditor.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchemeEditor.ViewModels
{
    public class CanvasViewModel : ObservableObject
    {
        #region Fields
        // UserControls field collection
        private ObservableCollection<CanvasItem>? _controls;
        // Connection field collection
        private ObservableCollection<Connection>? _connections;
        private CanvasItem? _draggerItem;
        private Connection? _draggerConnection;
        private double _scaleX;
        private double _scaleY;
        #endregion

        #region Properties
        // UserControls properties collection
        public ObservableCollection<CanvasItem>? Controls
        {
            get => _controls;
            set
            {
                _controls = value;
                OnPropertyChanged();
            }
        }
        // Connection properties collection
        public ObservableCollection<Connection>? Connections
        {
            get => _connections;
            set
            {
                _connections = value;
                OnPropertyChanged();
            }
        }

        public double ScaleX
        {
            get => _scaleX;
            set
            {
                _scaleX = value;
                OnPropertyChanged();
            }
        }

        public double ScaleY
        {
            get => _scaleY;
            set
            {
                _scaleY = value;
                OnPropertyChanged();
            }
        }

        public readonly Guid SchemeId;
        #endregion

        #region Commands
        public ICommand DragOverCommand { get; }
        private void OnDragOverCommand(object parameters)
        {
            if(parameters is DragEventArgs dragEventArgs)
            {
                if(dragEventArgs.Source is Canvas canvas)
                {
                    Point position = dragEventArgs.GetPosition(canvas);
                    // Add new canvasItem
                    if (dragEventArgs.Data.GetData(typeof(BaseControl)) is BaseControl sourceControl)
                    {
                        if(_draggerItem == null)
                        {
                            _draggerItem = AddControl(sourceControl, position);
                            if (_draggerItem != null)
                            {
                                _draggerItem.ChangeOpacity(0.5);
                            }
                            return;
                        }

                        _draggerItem.PositionOnCanvas = position;
                    }
                    else if(dragEventArgs.Data.GetData(typeof(CanvasItem)) is CanvasItem item)
                    {
                        // Change position canvasItem
                        item.PositionOnCanvas = position;
                    }

                    // Update all connections on canvas
                    if(Connections != null)
                    {
                        foreach (var connection in Connections)
                        {
                            connection.UpdatePath();
                        }
                    }
                }
            }
        }
        
        public ICommand DropCommand { get; }
        private void OnDropCommandExecute(object parameters)
        {
            if(_draggerItem != null)
            {
                _draggerItem.ChangeOpacity(1);
                // Add to database new canvasItem
                SchemeService schemeService = new SchemeService(new ApplicationContext());
                schemeService.AddCanvasItemDTO(SchemeId, _draggerItem.CanvasItemDTO);

                _draggerItem = null;
            }
        }

        public ICommand MouseMoveCommand { get; }
        private void OnMouseMoveCommandExecute(object parameters)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && _draggerConnection != null 
                && parameters is MouseEventArgs e && Connections != null)
            {
                var canvas = e.Source as Canvas;
                if (canvas != null)
                {
                    Point endPoint = e.GetPosition(canvas);
                    Connector endConnector = new Connector(endPoint);
                    _draggerConnection.EndConnector = endConnector;
                    if (!Connections.Contains(_draggerConnection))
                    {
                        Connections.Add(_draggerConnection);
                    }
                }
            }
        }

        public ICommand MouseLeftButtonUpCommand { get; }
        private void OnMouseLeftButtonUpCommandExecute(object parameters)
        {
            if (_draggerConnection != null && Connections != null)
            {
                if(_draggerConnection.ConnectionDTO != null)
                {
                    SchemeService schemeService = new SchemeService(new ApplicationContext());
                    schemeService.RemoveConnection(SchemeId, _draggerConnection.ConnectionDTO.Id);
                }

                Connections.Remove(_draggerConnection);
                _draggerConnection = null;
            }
        }
        // Zoom canvas command
        public ICommand ZoomCommand { get; }
        private void Zoom(object parameter)
        {
            if(parameter is MouseWheelEventArgs e)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    if (e.Delta > 0)
                    {
                        if(ScaleX < 2.0)
                        {
                            ScaleX *= 1.1;
                        }

                        if (ScaleY < 2.0)
                        {
                            ScaleY *= 1.1;
                        }
                    }
                    else
                    {
                        if (ScaleX > 0.7)
                        {
                            ScaleX /= 1.1;
                        }

                        if(ScaleY > 0.7)
                        {
                            ScaleY /= 1.1;
                        }
                    }
                }
            }
        }
        #endregion

        #region Constructors
        // Constructor without parameters
        public CanvasViewModel(Guid schemeId)
        {

            SchemeId = schemeId;
            SchemeService schemeService = new SchemeService(new ApplicationContext());
            Controls = new ObservableCollection<CanvasItem>(schemeService.LoadCanvasItems(schemeId));

            schemeService = new SchemeService(new ApplicationContext());
            Connections = new ObservableCollection<Connection>(schemeService.LoadConnection(schemeId, Controls.ToList()));

            DragOverCommand = new LambdaCommand(OnDragOverCommand);
            DropCommand = new LambdaCommand(OnDropCommandExecute);
            MouseMoveCommand = new LambdaCommand(OnMouseMoveCommandExecute);
            MouseLeftButtonUpCommand = new LambdaCommand(OnMouseLeftButtonUpCommandExecute);
            ZoomCommand = new LambdaCommand(Zoom);

            ConnectorAdorner.ConnectorPressed += OnConnectorPressed;
            ConnectorAdorner.ConnectorButtonUp += OnConnectorButtonUp;
            CanvasItem.DeleteEvent += OnDeleteControl;

            ScaleX = 1.0;
            ScaleY = 1.0;
        }
        #endregion

        #region Methods
        private CanvasItem AddControl(BaseControl sourceControl, Point position)
        {
            BaseControl newControl = (BaseControl)Activator.CreateInstance(sourceControl.GetType())!;
            CanvasItem newItem = new CanvasItem(newControl, position);
            Controls?.Add(newItem);
            return newItem;
        }

        private bool IsEndConnectorSourceControl(Connector endConnector, Connector startConnector, List<Connector> allControlConnector)
        {
            if (allControlConnector.Count > 0)
            {
                if (allControlConnector.Contains(endConnector) && allControlConnector.Contains(startConnector))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Events
        private void OnConnectorPressed(object? sender, Connector c)
        {
            if(_draggerConnection == null && _connections != null)
            {
                // Changing an existing connection
                Connection? connection = _connections.FirstOrDefault(connections => connections.StartConnector == c || connections.EndConnector == c);
                if (connection != null)
                {
                    _draggerConnection = connection;
                    _draggerConnection.StartConnector = _draggerConnection.EndConnector;
                    _draggerConnection.EndConnector = c;
                    return;
                }
                _draggerConnection = new Connection();
                _draggerConnection.StartConnector = c;
            }
        }

        private void OnConnectorButtonUp(object? sender, Connector c)
        {
            if (_draggerConnection != null && _draggerConnection.StartConnector != null && Connections != null && sender is ConnectorAdorner adorner)
            {
                SchemeService schemeService;
                _draggerConnection.EndConnector = c;
                List<Connector> allConnectorControl = adorner._canvasItem.Connectors;
                if (IsEndConnectorSourceControl(_draggerConnection.StartConnector, c, allConnectorControl))
                {
                    if (_draggerConnection.ConnectionDTO != null)
                    {
                        schemeService = new SchemeService(new ApplicationContext());
                        schemeService.RemoveConnection(SchemeId, _draggerConnection.ConnectionDTO.Id);
                    }

                    Connections.Remove(_draggerConnection);
                    _draggerConnection = null;
                    return;
                }
                // Add to database new canvasItem
                schemeService = new SchemeService(new ApplicationContext());
                if(_draggerConnection.ConnectionDTO != null)
                {
                    ConnectionService connectionService = new ConnectionService(new ApplicationContext());
                    connectionService.Update(_draggerConnection.ConnectionDTO.Id,
                        _draggerConnection.StartConnector.ConnectorDTO.Id,
                        _draggerConnection.EndConnector.ConnectorDTO.Id);
                }
                else
                {
                    _draggerConnection.ConnectionDTO = schemeService.AddConnectionDTO(SchemeId, ConnectionService.FromConnectionToConnectionDTO(_draggerConnection));
                }

                _draggerConnection = null;
            }
        }

        private void OnDeleteControl(object? sender, EventArgs e)
        {
            if (sender is CanvasItem control && Controls != null && Connections != null)
            {
                SchemeService schemeService;
                for (int i = 0; i < Connections.Count; i++)
                {
                    if(Connections[i].StartConnector != null && Connections[i].EndConnector != null)
                    {
                        if (control.Connectors.Contains(Connections[i].StartConnector) || control.Connectors.Contains(Connections[i].EndConnector))
                        {
                            schemeService = new SchemeService(new ApplicationContext());
                            schemeService.RemoveConnection(SchemeId, Connections[i].ConnectionDTO.Id);

                            Connections.Remove(Connections[i]);
                            i--;
                        }
                    }
                }

                schemeService = new SchemeService(new ApplicationContext());
                schemeService.RemoveCanvasItem(SchemeId, control.CanvasItemDTO.Id);
                Controls.Remove(control);
            }
        }
        #endregion
    }
}
