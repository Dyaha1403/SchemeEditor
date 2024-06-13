using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Orientation = SchemeEditor.Infrastructure.Orientation;

namespace SchemeEditor.Controls
{
    public partial class Fuse : BaseControl
    {
        public Fuse() : base()
        {
            InitializeComponent();
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

        public Fuse(bool inToolBox) : base(inToolBox) 
        {
            InitializeComponent();
        }
    }
}
