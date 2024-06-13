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
    public partial class Disconnect : BaseControl
    {
        private Thickness MarginRect
        {
            get { return (Thickness)GetValue(MarginRectProperty);}
            set { SetValue(MarginRectProperty, value); }
        }

        private static readonly DependencyProperty MarginRectProperty =
                   DependencyProperty.Register("MarginRect", typeof(Thickness), typeof(Disconnect), new PropertyMetadata(new Thickness(1, 0, 0, 8)));

        private double AngleRect
        {
            get { return (double)GetValue(AngleRectProperty); }
            set { SetValue(AngleRectProperty, value); }
        }

        private static readonly DependencyProperty AngleRectProperty =
                   DependencyProperty.Register("AngleRect", typeof(double), typeof(Disconnect), new PropertyMetadata(15.0));

        public Disconnect() : base()
        {
            InitializeComponent();
            DataContext = this;

            SetValue(AngleRectProperty, 15.0);
            SetValue(MarginRectProperty, new Thickness(1, 0, 0, 8));

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

        public Disconnect(bool inToolBox) : base(inToolBox)
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
            if (AngleRect == 15)
            {
                SetValue(AngleRectProperty, 0.0);
                SetValue(MarginRectProperty, new Thickness(0, 0, 0, 8));
                return;
            }

            SetValue(AngleRectProperty, 15.0);
            SetValue(MarginRectProperty, new Thickness(1, 0, 0, 8));
        }
    }
}
