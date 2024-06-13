using SchemeEditor.Models;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace SchemeEditor.Infrastructure
{
    // Need to upgrade
    public class PathFinder
    {
        #region Fields
        private List<Point> _nodes;
        private List<Line> _lines;
        private Connector _startConnector;
        private Connector _endConnector;
        private double _canvasWidth;
        private double _canvasHeight;
        #endregion

        #region Properties
        public static List<Rect> Obstacles { get; private set; } = new List<Rect>();
        #endregion

        #region Constructors
        public PathFinder(Connector startConnector, Connector endConnector) 
        {
            _endConnector = endConnector;
            _startConnector = startConnector;
            _canvasWidth = 2000;
            _canvasHeight = 2000;
            _nodes = new List<Point>();
            _lines = new List<Line>();


            // Adding route lines from obstacles
            foreach (var o in Obstacles)
            {
                AddLineWithPoint(o.TopLeft);
                AddLineWithPoint(o.TopRight);
                AddLineWithPoint(o.BottomLeft);
                AddLineWithPoint(o.BottomRight);
            }

            AddLineWithPoint(_startConnector.PositionOnCanvas, _startConnector.Orientation);
            AddLineWithPoint(_endConnector.PositionOnCanvas, _endConnector.Orientation);
        }
        #endregion

        #region Methods
        // Finding the line route
        public Path? FindPath()
        {
            Path? resultPath = null;
            // Adding route lines from end point

            // Adding route points
            for (int i = 0; i < _lines.Count; i++)
            {
                for (int j = i + 1; j < _lines.Count; j++)
                {
                    Point intersection;
                    // Find the intersection of each line with each
                    if (LineIntersectsLine(new Point(_lines[i].X1, _lines[i].Y1),
                        new Point(_lines[i].X2, _lines[i].Y2), new Point(_lines[j].X1, _lines[j].Y1),
                        new Point(_lines[j].X2, _lines[j].Y2), out intersection))
                    {
                        _nodes.Add(intersection);
                    }
                }
            }
            // Finding the best route
            List<Point> resultPoint = FindOrthogonalPath(_startConnector.PositionOnCanvas, _endConnector.PositionOnCanvas, _nodes);
            // Creates a Path from points
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure();
            figure.StartPoint = _startConnector.PositionOnCanvas;

            foreach (var p in resultPoint)
            {
                figure.Segments.Add(new LineSegment(p, true));
            }

            geometry.Figures.Add(figure);

            resultPath = new Path();
            resultPath.Data = geometry;

            return resultPath;
        }
        // Adding lines from points
        private void AddLineWithPoint(Point point, Orientation? orientation = Orientation.None)
        {

            Line rigthLine = new Line()
            {
                X1 = point.X,
                Y1 = point.Y,
                X2 = _canvasWidth,
                Y2 = point.Y
            };

            Line leftLine = new Line()
            {
                X1 = point.X,
                Y1 = point.Y,
                X2 = 0,
                Y2 = point.Y
            };

            Line topLine = new Line()
            {
                X1 = point.X,
                Y1 = point.Y,
                X2 = point.X,
                Y2 = 0,
            };

            Line bottomLine = new Line()
            {
                X1 = point.X,
                Y1 = point.Y,
                X2 = point.X,
                Y2 = _canvasHeight
            };

            UpdateLine(leftLine);
            UpdateLine(rigthLine);
            UpdateLine(topLine);
            UpdateLine(bottomLine);

            if(orientation != Orientation.Right)
            {
                _lines.Add(leftLine);
            }
            if (orientation != Orientation.Left)
            {
                _lines.Add(rigthLine);
            }
            if (orientation != Orientation.Bottom)
            {
                _lines.Add(topLine);
            }
            if (orientation != Orientation.Top)
            {
                _lines.Add(bottomLine);
            }
        }
        // Change the end point of a line 
        private void UpdateLine(Line line)
        {
            Point newEndPoint = FindIntersectionLineWithRects(line, Obstacles);
            line.X2 = newEndPoint.X;
            line.Y2 = newEndPoint.Y;
        }

        // Finds the intersection of a line with obstacles and returns a new end point
        private Point FindIntersectionLineWithRects(Line line, List<Rect> rects)
        {
            Point start = new Point(line.X1, line.Y1);
            Point end = new Point(line.X2, line.Y2);

            Point? closestIntersection = null;
            double closestDistance = double.MaxValue;

            foreach (var rect in rects)
            {
                Point? intersection = FindIntersectionLineWithRect(start, end, rect);

                if (intersection == start && intersection != _startConnector.PositionOnCanvas)
                {
                    continue;
                }

                if (intersection.HasValue)
                {
                    double distance = Distance(start, intersection.Value);

                    if (distance <= closestDistance)
                    {
                        closestDistance = distance;
                        closestIntersection = intersection;
                    }
                }
            }

            return closestIntersection ?? end;
        }
        // Finds the point of intersection with an obstacle
        private Point? FindIntersectionLineWithRect(Point start, Point end, Rect rect)
        {
            var intersections = new List<Point>();

            if (LineIntersectsLine(start, end, new Point(rect.X, rect.Y), new Point(rect.X, rect.Y + rect.Height), out Point intersection))
            {
                intersections.Add(intersection);
            }

            if (LineIntersectsLine(start, end, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height), out intersection))
            {
                intersections.Add(intersection);
            }

            if (LineIntersectsLine(start, end, new Point(rect.X, rect.Y), new Point(rect.X + rect.Width, rect.Y), out intersection))
            {
                intersections.Add(intersection);
            }

            if (LineIntersectsLine(start, end, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height), out intersection))
            {
                intersections.Add(intersection);
            }

            if (intersections.Count == 0)
            {
                return null;
            }

            intersections.Sort((p1, p2) => Distance(start, p1).CompareTo(Distance(start, p2)));
            return intersections[0];
        }
        // Finds the point of intersection of two lines
        private bool LineIntersectsLine(Point p1, Point p2, Point p3, Point p4, out Point intersection)
        {
            intersection = new Point();
            // Calculating the direction vectors of segments
            double dx1 = p2.X - p1.X;
            double dy1 = p2.Y - p1.Y;
            double dx2 = p4.X - p3.X;
            double dy2 = p4.Y - p3.Y;
            // Calculating the denominator denom
            double denom = dx1 * dy2 - dy1 * dx2;
            // Check for parallelism
            if (denom == 0)
            {
                return false;
            }
            // Calculating the intersection point
            double ua = (dx2 * (p1.Y - p3.Y) - dy2 * (p1.X - p3.X)) / denom;
            double ub = (dx1 * (p1.Y - p3.Y) - dy1 * (p1.X - p3.X)) / denom;
            // Checking for intersections
            if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
            {
                double x = p1.X + ua * dx1;
                double y = p1.Y + ua * dy1;

                intersection = new Point(x, y);
                return true;
            }

            return false;

        }
        // Implements the A* path search algorithm for finding an orthogonal path
        private List<Point> FindOrthogonalPath(Point start, Point end, List<Point> points)
        {
            var allPoints = new List<Point> { start };
            allPoints.AddRange(points);
            allPoints.Add(end);

            var openSet = new HashSet<Point> { start };
            var cameFrom = new Dictionary<Point, Point>();
            var gScore = new Dictionary<Point, double> { { start, 0 } };
            var fScore = new Dictionary<Point, double> { { start, Distance(start, end) } };

            while (openSet.Any())
            {
                var current = openSet.OrderBy(p => fScore[p]).First();

                if (current == end)
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);

                foreach (var neighbor in GetNeighbors(current, allPoints))
                {
                    var tentativeGScore = gScore[current] + Distance(current, neighbor);

                    if (tentativeGScore < gScore.GetValueOrDefault(neighbor, double.PositiveInfinity))
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = tentativeGScore + Distance(neighbor, end);

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            return new List<Point>();
        }

        // Returns a list of point neighbors that are on the same axis.
        private IEnumerable<Point> GetNeighbors(Point point, List<Point> allPoints)
        {
            return allPoints.Where(p => p != point && (p.X == point.X || p.Y == point.Y));
        }
        // Restores the path from the end point to the original one
        private List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            var path = new List<Point> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Insert(0, current);
            }
            return path;
        }
        // Calculates the distance between points
        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }
        #endregion
    }
}
