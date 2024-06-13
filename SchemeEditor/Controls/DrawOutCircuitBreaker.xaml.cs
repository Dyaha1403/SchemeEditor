using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Orientation = SchemeEditor.Infrastructure.Orientation;

namespace SchemeEditor.Controls
{
    public partial class DrawOutCircuitBreaker : BaseControl
    {
        private Thickness MarginEllipse
        {
            get { return (Thickness)GetValue(MarginEllipseProperty); }
            set { SetValue(MarginEllipseProperty, value); }
        }

        private static readonly DependencyProperty MarginEllipseProperty =
                   DependencyProperty.Register("MarginEllipse", typeof(Thickness), typeof(DrawOutCircuitBreaker), new PropertyMetadata(new Thickness(4, 0, 0, 0)));

        public DrawOutCircuitBreaker() : base()
        {
            InitializeComponent();
            DataContext = this;

            PositionsConnectors = new List<Point>()
            {
                new Point(Width / 2, 0),
                new Point(Width / 2, Height)
            };

            ConnectorOrientation = new List<Orientation>()
            {
                Orientation.Top,
                Orientation.Bottom,
            };

            PositionnConnectorsForAdorner = new List<Point>(PositionsConnectors);
        }

        public DrawOutCircuitBreaker(bool inToolBox) : base(inToolBox)
        {
            InitializeComponent();
        }

        protected override void CreateContextMenu()
        {
            base.CreateContextMenu();

            MenuItem changeStatusMenuItem = new MenuItem { Header = "On/Off" };
            changeStatusMenuItem.Click += (s, e) => ChangeStatus();

            if (_contextMenu != null)
            {
                _contextMenu.Items.Add(changeStatusMenuItem);
            }
        }

        private void ChangeStatus()
        {
            if (MarginEllipse.Left == 4)
            {
                SetValue(MarginEllipseProperty, new Thickness(1, 0, 0, 0));
                return;
            }

            SetValue(MarginEllipseProperty, new Thickness(4, 0, 0, 0));
        }
    }
}
