using SchemeEditor.Infrastructure;
using System.Windows.Shapes;
using SchemeEditor.Entities;

namespace SchemeEditor.Models
{
    public class Connection : ObservableObject
    {
        #region Fields
        private Path? _path;
        private Connector? _startConnector;
        private Connector? _endConnector;
        #endregion

        #region Properties
        public Path? Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }

        public Connector? StartConnector
        {
            get => _startConnector;
            set
            {
                if (_startConnector != null)
                {
                    _startConnector.PositionChanged -= OnConnectorPositionChanged;
                }

                _startConnector = value;

                if (_startConnector != null)
                {
                    _startConnector.PositionChanged += OnConnectorPositionChanged;
                }

                UpdatePath();
            }
        }
        public Connector? EndConnector
        {
            get => _endConnector;
            set
            {
                if (_endConnector != null)
                {
                    _endConnector.PositionChanged -= OnConnectorPositionChanged;
                }

                _endConnector = value;

                if (_endConnector != null)
                {
                    _endConnector.PositionChanged += OnConnectorPositionChanged;
                }

                UpdatePath();
            }
        }
        public ConnectionDTO? ConnectionDTO;
        #endregion

        #region Constructors
        public Connection()
        {
            StartConnector = null;
            EndConnector = null;
        }
        #endregion

        #region Events
        private void OnConnectorPositionChanged(object? sender, EventArgs e)
        {
            UpdatePath();
        }
        #endregion  

        #region Methods
        public void UpdatePath()
        {
            if (_endConnector != null && _startConnector != null && _endConnector != _startConnector)
            {
                PathFinder pathFinder = new PathFinder(_startConnector, _endConnector);
                Path = pathFinder.FindPath();
            }
        }
        #endregion
    }
}
