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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Orientation = SchemeEditor.Infrastructure.Orientation;

namespace SchemeEditor.Controls
{
    public partial class Generator : BaseControl
    {
        public Generator() : base()
        {
            InitializeComponent();
            PositionsConnectors = new List<Point>()
            {
                new Point(Width / 2, Height)
            };

            ConnectorOrientation = new List<Orientation>()
            {
                Orientation.Bottom
            };

            PositionnConnectorsForAdorner = new List<Point>(PositionsConnectors);
        }
        public Generator(bool inToolBox) : base(inToolBox)
        {
            InitializeComponent();
        }
    }
}
