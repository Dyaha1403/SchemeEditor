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
    public partial class DrawOutCircuitBreakerImproved : BaseControl
    {
        private Brush ColorRect
        {
            get { return (Brush)GetValue(ColorRectProperty); }
            set { SetValue(ColorRectProperty, value); }
        }

        private static readonly DependencyProperty ColorRectProperty =
            DependencyProperty.Register("ColorRect", typeof(Brush), typeof(DrawOutCircuitBreakerImproved), new PropertyMetadata(Brushes.Transparent));

        public DrawOutCircuitBreakerImproved() : base()
        {
            InitializeComponent();
            DataContext = this;

            SetValue(ColorRectProperty, Brushes.Red);

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

        public DrawOutCircuitBreakerImproved(bool inToolBox) : base(inToolBox)
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
            if (ColorRect == Brushes.Red)
            {
                SetValue(ColorRectProperty, Brushes.Green);
                return;
            }

            SetValue(ColorRectProperty, Brushes.Red);
        }
    }
}
