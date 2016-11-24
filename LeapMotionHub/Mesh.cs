        using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LeapFirst
{
    class Mesh
    {

        // The main object model group.
        //private Model3DGroup MainModel3Dgroup = new Model3DGroup();

        // The camera.
        //private PerspectiveCamera TheCamera;

        // The camera's current location.
        //private double CameraPhi = Math.PI / 6.0;       // 30 degrees
        //private double CameraTheta = Math.PI / 6.0;     // 30 degrees
        //private double CameraR = 3.0;

        // The change in CameraPhi when you press the up and down arrows.
        //private const double CameraDPhi = 0.1;

        // The change in CameraTheta when you press the left and right arrows.
        //private const double CameraDTheta = 0.1;

        // The change in CameraR when you press + or -.
        //private const double CameraDR = 0.1;

        //private Viewport3DVisual MainViewport = new Viewport3DVisual();

        public Mesh() { }

        public void init()
        {
            // TheCamera = new PerspectiveCamera();
            // TheCamera.FieldOfView = 60;
            // MainViewport.Camera = TheCamera;
            //PositionCamera();

            // Define lights.
            //DefineLights();

            // Create the model.
            // DefineModel(MainModel3Dgroup);

            // Add the group of models to a ModelVisual3D.
            //ModelVisual3D model_visual = new ModelVisual3D();

            //model_visual.Content = MainModel3Dgroup;

            // Display the main visual to the viewportt.
            //MainViewport.Children.Add(model_visual);
            

        }


      
        public MeshGeometry3D createSphere(Point3D center)
        {
            center.X *= -1;
            double radius = 4;
            int num_phi = 5;
            int num_theta = 10;
            double dphi = Math.PI / num_phi;
            double dtheta = 2 * Math.PI / num_theta;
            double phi0 = 0;
            double theta0 = 0;
            double y0 = radius * Math.Cos(phi0);
            double r0 = radius * Math.Sin(phi0);
            MeshGeometry3D _mesh = new MeshGeometry3D();
            for (int i = 0; i < num_phi; i++)
            {
                double phi1 = phi0 + dphi;
                double y1 = radius * Math.Cos(phi1);
                double r1 = radius * Math.Sin(phi1);

                // Point ptAB has phi value A and theta value B.
                // For example, pt01 has phi = phi0 and theta = theta1.
                // Find the points with theta = theta0.

                Point3D pt00 = new Point3D(
                    center.X + r0 * Math.Cos(theta0),
                    center.Y + y0,
                    center.Z + r0 * Math.Sin(theta0));
                Point3D pt10 = new Point3D(
                    center.X + r1 * Math.Cos(theta0),
                    center.Y + y1,
                    center.Z + r1 * Math.Sin(theta0));
                for (int j = 0; j < num_theta; j++)
                {
                    // Find the points with theta = theta1.
                    double theta1 = theta0 + dtheta;
                    Point3D pt01 = new Point3D(
                        center.X + r0 * Math.Cos(theta1),
                        center.Y + y0,
                        center.Z + r0 * Math.Sin(theta1));
                    Point3D pt11 = new Point3D(
                        center.X + r1 * Math.Cos(theta1),
                        center.Y + y1,
                        center.Z + r1 * Math.Sin(theta1));

                    // Create the triangles.
                    AddTriangle(_mesh, pt00, pt11, pt10);
                    AddTriangle(_mesh, pt00, pt01, pt11);

                    // Move to the next value of theta.
                    theta0 = theta1;
                    pt00 = pt01;
                    pt10 = pt11;
                }

                // Move to the next value of phi.
                phi0 = phi1;
                y0 = y1;
                r0 = r1;
            }
            return _mesh;
        }

        private void addPoint(MeshGeometry3D mesh, Point3D center)
        {

            AddSphere(mesh, center, 1, 5, 10);

        }
        private void AddSphere(MeshGeometry3D mesh, Point3D center, double radius, int num_phi, int num_theta)
        {
            double phi0, theta0;
            double dphi = Math.PI / num_phi;
            double dtheta = 2 * Math.PI / num_theta;

            phi0 = 0;
            double y0 = radius * Math.Cos(phi0);
            double r0 = radius * Math.Sin(phi0);
            for (int i = 0; i < num_phi; i++)
            {
                double phi1 = phi0 + dphi;
                double y1 = radius * Math.Cos(phi1);
                double r1 = radius * Math.Sin(phi1);

                // Point ptAB has phi value A and theta value B.
                // For example, pt01 has phi = phi0 and theta = theta1.
                // Find the points with theta = theta0.
                theta0 = 0;
                Point3D pt00 = new Point3D(
                    center.X + r0 * Math.Cos(theta0),
                    center.Y + y0,
                    center.Z + r0 * Math.Sin(theta0));
                Point3D pt10 = new Point3D(
                    center.X + r1 * Math.Cos(theta0),
                    center.Y + y1,
                    center.Z + r1 * Math.Sin(theta0));
                for (int j = 0; j < num_theta; j++)
                {
                    // Find the points with theta = theta1.
                    double theta1 = theta0 + dtheta;
                    Point3D pt01 = new Point3D(
                        center.X + r0 * Math.Cos(theta1),
                        center.Y + y0,
                        center.Z + r0 * Math.Sin(theta1));
                    Point3D pt11 = new Point3D(
                        center.X + r1 * Math.Cos(theta1),
                        center.Y + y1,
                        center.Z + r1 * Math.Sin(theta1));

                    // Create the triangles.
                    AddTriangle(mesh, pt00, pt11, pt10);
                    AddTriangle(mesh, pt00, pt01, pt11);

                    // Move to the next value of theta.
                    theta0 = theta1;
                    pt00 = pt01;
                    pt10 = pt11;
                }

                // Move to the next value of phi.
                phi0 = phi1;
                y0 = y1;
                r0 = r1;
            }
        }


        // Add a cylinder with smooth sides.
        private void AddSmoothCylinder(MeshGeometry3D mesh, Point3D end_point, Vector3D axis, double radius, int num_sides)
        {
            // Get two vectors perpendicular to the axis.
            Vector3D v1;
            if ((axis.Z < -0.01) || (axis.Z > 0.01))
                v1 = new Vector3D(axis.Z, axis.Z, -axis.X - axis.Y);
            else
                v1 = new Vector3D(-axis.Y - axis.Z, axis.X, axis.X);
            Vector3D v2 = Vector3D.CrossProduct(v1, axis);

            // Make the vectors have length radius.
            v1 *= (radius / v1.Length);
            v2 *= (radius / v2.Length);

            // Make the top end cap.
            // Make the end point.
            int pt0 = mesh.Positions.Count; // Index of end_point.
            mesh.Positions.Add(end_point);

            // Make the top points.
            double theta = 0;
            double dtheta = 2 * Math.PI / num_sides;
            for (int i = 0; i < num_sides; i++)
            {
                mesh.Positions.Add(end_point +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2);
                theta += dtheta;
            }

            // Make the top triangles.
            int pt1 = mesh.Positions.Count - 1; // Index of last point.
            int pt2 = pt0 + 1;                  // Index of first point in this cap.
            for (int i = 0; i < num_sides; i++)
            {
                mesh.TriangleIndices.Add(pt0);
                mesh.TriangleIndices.Add(pt1);
                mesh.TriangleIndices.Add(pt2);
                pt1 = pt2++;
            }

            // Make the bottom end cap.
            // Make the end point.
            pt0 = mesh.Positions.Count; // Index of end_point2.
            Point3D end_point2 = end_point + axis;
            mesh.Positions.Add(end_point2);

            // Make the bottom points.
            theta = 0;
            for (int i = 0; i < num_sides; i++)
            {
                mesh.Positions.Add(end_point2 +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2);
                theta += dtheta;
            }

            // Make the bottom triangles.
            theta = 0;
            pt1 = mesh.Positions.Count - 1; // Index of last point.
            pt2 = pt0 + 1;                  // Index of first point in this cap.
            for (int i = 0; i < num_sides; i++)
            {
                mesh.TriangleIndices.Add(num_sides + 1);    // end_point2
                mesh.TriangleIndices.Add(pt2);
                mesh.TriangleIndices.Add(pt1);
                pt1 = pt2++;
            }

            // Make the sides.
            // Add the points to the mesh.
            int first_side_point = mesh.Positions.Count;
            theta = 0;
            for (int i = 0; i < num_sides; i++)
            {
                Point3D p1 = end_point +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                mesh.Positions.Add(p1);
                Point3D p2 = p1 + axis;
                mesh.Positions.Add(p2);
                theta += dtheta;
            }

            // Make the side triangles.
            pt1 = mesh.Positions.Count - 2;
            pt2 = pt1 + 1;
            int pt3 = first_side_point;
            int pt4 = pt3 + 1;
            for (int i = 0; i < num_sides; i++)
            {
                mesh.TriangleIndices.Add(pt1);
                mesh.TriangleIndices.Add(pt2);
                mesh.TriangleIndices.Add(pt4);

                mesh.TriangleIndices.Add(pt1);
                mesh.TriangleIndices.Add(pt4);
                mesh.TriangleIndices.Add(pt3);

                pt1 = pt3;
                pt3 += 2;
                pt2 = pt4;
                pt4 += 2;
            }
        }




        public MeshGeometry3D createCylinder(Point3D startPoint, Point3D endPoint)
        {
            startPoint.X *= -1;
            endPoint.X *= -1;
            MeshGeometry3D _mesh = new MeshGeometry3D();
            //Vector3D axis = new Vector3D(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y, endPoint.Z - startPoint.Z);
            Vector3D axis = new Vector3D(startPoint.X - endPoint.X, startPoint.Y - endPoint.Y, startPoint.Z - endPoint.Z);
            double radius = 1;
            double num_sides = 5;
            Vector3D v1;
            if ((axis.Z < -0.01) || (axis.Z > 0.01))
                v1 = new Vector3D(axis.Z, axis.Z, -axis.X - axis.Y);
            else
                v1 = new Vector3D(-axis.Y - axis.Z, axis.X, axis.X);
            Vector3D v2 = Vector3D.CrossProduct(v1, axis);

            // Make the vectors have length radius.
            v1 *= (radius / v1.Length);
            v2 *= (radius / v2.Length);

            // Make the top end cap.
            double theta = 0;
            double dtheta = 2 * Math.PI / num_sides;
            for (int i = 0; i < num_sides; i++)
            {
                Point3D p1 = endPoint +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                theta += dtheta;
                Point3D p2 = endPoint +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                AddTriangle(_mesh, endPoint, p1, p2);
            }

            // Make the bottom end cap.
            Point3D end_point2 = endPoint + axis;
            theta = 0;
            for (int i = 0; i < num_sides; i++)
            {
                Point3D p1 = end_point2 +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                theta += dtheta;
                Point3D p2 = end_point2 +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                AddTriangle(_mesh, end_point2, p2, p1);
            }

            // Make the sides.
            theta = 0;
            for (int i = 0; i < num_sides; i++)
            {
                Point3D p1 = endPoint +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                theta += dtheta;
                Point3D p2 = endPoint +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;

                Point3D p3 = p1 + axis;
                Point3D p4 = p2 + axis;

                AddTriangle(_mesh, p1, p3, p2);
                AddTriangle(_mesh, p2, p3, p4);
            }

            return _mesh;
        }
        private void connectPoints(MeshGeometry3D mesh, Point3D startPoint, Point3D endPoint)
        {
            Vector3D line = new Vector3D(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y, endPoint.Z - startPoint.Z);
            AddCylinder(mesh, endPoint, line, 1, 5);
        }
        // Add a cylinder.
        private void AddCylinder(MeshGeometry3D mesh, Point3D end_point, Vector3D axis, double radius, int num_sides)
        {
            // Get two vectors perpendicular to the axis.
            Vector3D v1;
            if ((axis.Z < -0.01) || (axis.Z > 0.01))
                v1 = new Vector3D(axis.Z, axis.Z, -axis.X - axis.Y);
            else
                v1 = new Vector3D(-axis.Y - axis.Z, axis.X, axis.X);
            Vector3D v2 = Vector3D.CrossProduct(v1, axis);

            // Make the vectors have length radius.
            v1 *= (radius / v1.Length);
            v2 *= (radius / v2.Length);

            // Make the top end cap.
            double theta = 0;
            double dtheta = 2 * Math.PI / num_sides;
            for (int i = 0; i < num_sides; i++)
            {
                Point3D p1 = end_point +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                theta += dtheta;
                Point3D p2 = end_point +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                AddTriangle(mesh, end_point, p1, p2);
            }

            // Make the bottom end cap.
            Point3D end_point2 = end_point + axis;
            theta = 0;
            for (int i = 0; i < num_sides; i++)
            {
                Point3D p1 = end_point2 +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                theta += dtheta;
                Point3D p2 = end_point2 +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                AddTriangle(mesh, end_point2, p2, p1);
            }

            // Make the sides.
            theta = 0;
            for (int i = 0; i < num_sides; i++)
            {
                Point3D p1 = end_point +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                theta += dtheta;
                Point3D p2 = end_point +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;

                Point3D p3 = p1 + axis;
                Point3D p4 = p2 + axis;

                AddTriangle(mesh, p1, p3, p2);
                AddTriangle(mesh, p2, p3, p4);
            }
        }


        // Add a triangle to the indicated mesh.
        // Do not reuse points so triangles don't share normals.
        private void AddTriangle(MeshGeometry3D mesh, Point3D point1, Point3D point2, Point3D point3)
        {
            // Create the points.
            int index1 = mesh.Positions.Count;
            mesh.Positions.Add(point1);
            mesh.Positions.Add(point2);
            mesh.Positions.Add(point3);

            // Create the triangle.
            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1);
        }


        // Set the vector's length.
        private Vector3D ScaleVector(Vector3D vector, double length)
        {
            double scale = length / vector.Length;
            return new Vector3D(
                vector.X * scale,
                vector.Y * scale,
                vector.Z * scale);
        }

        private void PositionCamera()
        {
            // Calculate the camera's position in Cartesian coordinates.

            double _x = -1;//-1.94503604799004E-16;
            double _y = -141.699999999996;
            double _z = -8;// -8.67415557992157E-15;
           // double y = CameraR * Math.Sin(CameraPhi);
           // double hyp = CameraR * Math.Cos(CameraPhi);
           // double x = hyp * Math.Cos(CameraTheta);
           // double z = hyp * Math.Sin(CameraTheta);

           // Console.WriteLine("x : " + x + " Y : " + y + " Z : " + z + " HYP : " + hyp);

            if (false)
            {
               // TheCamera.Position = new Point3D(x, y, z);
             //   TheCamera.LookDirection = new Vector3D(-x, -y, -z);
            }
            else
            {
                //TheCamera.Position = new Point3D(_x, _y, _z);
                //TheCamera.LookDirection = new Vector3D(-_x, -_y, -_z);
            }


            // Look toward the origin.


            // Set the Up direction.
            //TheCamera.UpDirection = new Vector3D(0, 1, 0);

            // Console.WriteLine("Camera.Position: (" + x + ", " + y + ", " + z + ")");
        }



        // Add a cage.
        private void AddCage(MeshGeometry3D mesh)
        {
            // Top.
            Vector3D up = new Vector3D(0, 1, 0);
            AddSegment(mesh, new Point3D(1, 1, 1), new Point3D(1, 1, -1), up, true);
            AddSegment(mesh, new Point3D(1, 1, -1), new Point3D(-1, 1, -1), up, true);
            AddSegment(mesh, new Point3D(-1, 1, -1), new Point3D(-1, 1, 1), up, true);
            AddSegment(mesh, new Point3D(-1, 1, 1), new Point3D(1, 1, 1), up, true);

            // Bottom.
            AddSegment(mesh, new Point3D(1, -1, 1), new Point3D(1, -1, -1), up, true);
            AddSegment(mesh, new Point3D(1, -1, -1), new Point3D(-1, -1, -1), up, true);
            AddSegment(mesh, new Point3D(-1, -1, -1), new Point3D(-1, -1, 1), up, true);
            AddSegment(mesh, new Point3D(-1, -1, 1), new Point3D(1, -1, 1), up, true);

            // Sides.
            Vector3D right = new Vector3D(1, 0, 0);
            AddSegment(mesh, new Point3D(1, -1, 1), new Point3D(1, 1, 1), right);
            AddSegment(mesh, new Point3D(1, -1, -1), new Point3D(1, 1, -1), right);
            AddSegment(mesh, new Point3D(-1, -1, 1), new Point3D(-1, 1, 1), right);
            AddSegment(mesh, new Point3D(-1, -1, -1), new Point3D(-1, 1, -1), right);
        }

        // Make a thin rectangular prism between the two points.
        // If extend is true, extend the segment by half the
        // thickness so segments with the same end points meet nicely.
        private void AddSegment(MeshGeometry3D mesh,
            Point3D point1, Point3D point2, Vector3D up)
        {
            AddSegment(mesh, point1, point2, up, false);
        }

        private void AddSegment(MeshGeometry3D mesh,
          Point3D point1, Point3D point2, Vector3D up,
          bool extend)
        {
            const double thickness = 0.25;

            // Get the segment's vector.
            Vector3D v = point2 - point1;

            if (extend)
            {
                // Increase the segment's length on both ends by thickness / 2.
                Vector3D n = ScaleVector(v, thickness / 2.0);
                point1 -= n;
                point2 += n;
            }

            // Get the scaled up vector.
            Vector3D n1 = ScaleVector(up, thickness / 2.0);

            // Get another scaled perpendicular vector.
            Vector3D n2 = Vector3D.CrossProduct(v, n1);
            n2 = ScaleVector(n2, thickness / 2.0);

            // Make a skinny box.
            // p1pm means point1 PLUS n1 MINUS n2.
            Point3D p1pp = point1 + n1 + n2;
            Point3D p1mp = point1 - n1 + n2;
            Point3D p1pm = point1 + n1 - n2;
            Point3D p1mm = point1 - n1 - n2;
            Point3D p2pp = point2 + n1 + n2;
            Point3D p2mp = point2 - n1 + n2;
            Point3D p2pm = point2 + n1 - n2;
            Point3D p2mm = point2 - n1 - n2;

            // Sides.
            AddTriangle(mesh, p1pp, p1mp, p2mp);
            AddTriangle(mesh, p1pp, p2mp, p2pp);

            AddTriangle(mesh, p1pp, p2pp, p2pm);
            AddTriangle(mesh, p1pp, p2pm, p1pm);

            AddTriangle(mesh, p1pm, p2pm, p2mm);
            AddTriangle(mesh, p1pm, p2mm, p1mm);

            AddTriangle(mesh, p1mm, p2mm, p2mp);
            AddTriangle(mesh, p1mm, p2mp, p1mp);

            // Ends.
            AddTriangle(mesh, p1pp, p1pm, p1mm);
            AddTriangle(mesh, p1pp, p1mm, p1mp);

            AddTriangle(mesh, p2pp, p2mp, p2mm);
            AddTriangle(mesh, p2pp, p2mm, p2pm);
        }

        private void DefineLights()
        {
            AmbientLight ambient_light = new AmbientLight(Colors.Gray);
            DirectionalLight directional_light =
                new DirectionalLight(Colors.Gray, new Vector3D(-1.0, -3.0, -2.0));
            //MainModel3Dgroup.Children.Add(ambient_light);
            //MainModel3Dgroup.Children.Add(directional_light);
        }



        private void DefineModel(Model3DGroup model_group)
        {
#if ONE_BIG_SPHERE
            MeshGeometry3D mesh1 = new MeshGeometry3D();
            AddSphere(mesh1, new Point3D(0, 0, 0), 1, 5, 10);
            SolidColorBrush brush1 = Brushes.Red;
            DiffuseMaterial material1 = new DiffuseMaterial(brush1);
            GeometryModel3D model1 = new GeometryModel3D(mesh1, material1);
            model_group.Children.Add(model1);
#else
            // Make spheres centered at (+/-1, 0, 0).
            MeshGeometry3D mesh1 = new MeshGeometry3D();
            //AddSphere(mesh1, new Point3D(1, 0, 0), 1, 5, 10);
            //AddSphere(mesh1, new Point3D(-1, 0, 0), 1, 5, 10);
            //AddSphere(mesh1, new Point3D(-2, 0, 0), 1, 5, 10);

            AddSphere(mesh1, new Point3D(-14.5542, 116.19, -5.69549), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-63.6711, 103.178, 80.3472), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-25.8321, 112.989, 13.1959), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-35.001, 114.261, 44.2358), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-17.5137, 115.224, -1.52006), 1, 5, 10);

            AddSphere(mesh1, new Point3D(-33.5582, 119.263, -64.7558), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-76.6519, 120.427, 73.9379), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-39.6399, 127.51, -52.168), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-47.5629, 134.137, -31.6606), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-35.3336, 121.026, -61.7393), 1, 5, 10);

            AddSphere(mesh1, new Point3D(-64.9179, 109.659, -79.5005), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-87.615, 119.003, 70.1146), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-70.606, 119.24, -65.5517), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-76.4143, 127.938, -40.6442), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-66.3874, 111.838, -76.1128), 1, 5, 10);

            AddSphere(mesh1, new Point3D(-90.0995, 109.268, -74.931), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-98.1045, 114.716, 67.3212), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-95.1058, 116.752, -59.6348), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-99.2971, 122.538, -34.3061), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-91.488, 111.098, -71.1918), 1, 5, 10);

            AddSphere(mesh1, new Point3D(-122.883, 93.1236, -52.0597), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-106.41, 104.106, 66.1985), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-125.32, 100.92, -37.8673), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-124.51, 106.596, -20.1933), 1, 5, 10);
            AddSphere(mesh1, new Point3D(-123.565, 95.272, -48.5776), 1, 5, 10);




            AddSphere(mesh1, new Point3D(8.8013, 140.481, 4.47819), 1, 5, 10);
            AddSphere(mesh1, new Point3D(91.0871, 110.232, 57.3497), 1, 5, 10);
            AddSphere(mesh1, new Point3D(27.0213, 133.634, 15.6422), 1, 5, 10);
            AddSphere(mesh1, new Point3D(51.1141, 126.246, 36.4644), 1, 5, 10);
            AddSphere(mesh1, new Point3D(13.0569, 139.433, 7.37985), 1, 5, 10);

            AddSphere(mesh1, new Point3D(15.9446, 139.743, -73.1181), 1, 5, 10);
            AddSphere(mesh1, new Point3D(100.159, 129.328, 49.0103), 1, 5, 10);
            AddSphere(mesh1, new Point3D(25.5098, 145.02, -60.9114), 1, 5, 10);
            AddSphere(mesh1, new Point3D(39.1375, 148.268, -42.4519), 1, 5, 10);
            AddSphere(mesh1, new Point3D(18.0758, 140.52, -70.0895), 1, 5, 10);

            AddSphere(mesh1, new Point3D(45.9067, 130.963, -99.1028), 1, 5, 10);
            AddSphere(mesh1, new Point3D(109.701, 129.346, 42.0584), 1, 5, 10);
            AddSphere(mesh1, new Point3D(54.2492, 138.017, -84.776), 1, 5, 10);
            AddSphere(mesh1, new Point3D(65.7439, 143.69, -60.7134), 1, 5, 10);
            AddSphere(mesh1, new Point3D(47.695, 132.141, -95.5373), 1, 5, 10);

            AddSphere(mesh1, new Point3D(81.2356, 131.28, -104.2), 1, 5, 10);
            AddSphere(mesh1, new Point3D(119.343, 126.383, 35.9766), 1, 5, 10);
            AddSphere(mesh1, new Point3D(87.4116, 136.36, -88.1706), 1, 5, 10);
            AddSphere(mesh1, new Point3D(94.7153, 139.525, -62.832), 1, 5, 10);
            AddSphere(mesh1, new Point3D(82.5451, 132.031, -100.281), 1, 5, 10);

            AddSphere(mesh1, new Point3D(124.962, 112.97, -89.4394), 1, 5, 10);
            AddSphere(mesh1, new Point3D(127.935, 116.746, 31.5605), 1, 5, 10);
            AddSphere(mesh1, new Point3D(126.513, 119.336, -74.2678), 1, 5, 10);
            AddSphere(mesh1, new Point3D(125.985, 123.852, -56.075), 1, 5, 10);
            AddSphere(mesh1, new Point3D(125.01, 113.987, -85.7004), 1, 5, 10);




            SolidColorBrush brush1 = Brushes.Red;
            DiffuseMaterial material1 = new DiffuseMaterial(brush1);
            GeometryModel3D model1 = new GeometryModel3D(mesh1, material1);
            model_group.Children.Add(model1);

            // Make spheres centered at (0, +/-1, 0).
            MeshGeometry3D mesh2 = new MeshGeometry3D();
            AddSphere(mesh2, new Point3D(0, 1, 0), 1, 5, 10);
            AddSphere(mesh2, new Point3D(0, -1, 0), 1, 5, 10);
            AddSphere(mesh2, new Point3D(0, -2, 0), 1, 5, 10);
            SolidColorBrush brush2 = Brushes.Green;
            DiffuseMaterial material2 = new DiffuseMaterial(brush2);
            GeometryModel3D model2 = new GeometryModel3D(mesh2, material2);
            model_group.Children.Add(model2);

            // Make spheres centered at (0, 0, +/-1).
            MeshGeometry3D mesh3 = new MeshGeometry3D();
            AddSphere(mesh3, new Point3D(0, 0, 1), 0.25, 5, 10);
            AddSphere(mesh3, new Point3D(0, 0, -1), 0.25, 5, 10);
            SolidColorBrush brush3 = Brushes.Blue;
            DiffuseMaterial material3 = new DiffuseMaterial(brush3);
            GeometryModel3D model3 = new GeometryModel3D(mesh3, material3);
            model_group.Children.Add(model3);

            // Make a cylinder along the X axis.
            MeshGeometry3D mesh4 = new MeshGeometry3D();
            AddCylinder(mesh4, new Point3D(1, 0, 0),
                new Vector3D(-2, 0, 0), 0.1, 10);
            SolidColorBrush brush4 = Brushes.HotPink;
            DiffuseMaterial material4 = new DiffuseMaterial(brush4);
            GeometryModel3D model4 = new GeometryModel3D(mesh4, material4);
            model_group.Children.Add(model4);

            // Make a cylinder along the Y axis.
            MeshGeometry3D mesh5 = new MeshGeometry3D();
            AddCylinder(mesh5, new Point3D(0, 1, 0),
                new Vector3D(0, -2, 0), 0.1, 10);
            SolidColorBrush brush5 = Brushes.LightGreen;
            DiffuseMaterial material5 = new DiffuseMaterial(brush5);
            GeometryModel3D model5 = new GeometryModel3D(mesh5, material5);
            model_group.Children.Add(model5);

            // Make a cylinder along the Z axis.
            MeshGeometry3D mesh6 = new MeshGeometry3D();
            AddCylinder(mesh6, new Point3D(0, 0, 1),
                new Vector3D(0, 0, -2), 0.1, 10);
            SolidColorBrush brush6 = Brushes.LightBlue;
            DiffuseMaterial material6 = new DiffuseMaterial(brush6);
            GeometryModel3D model6 = new GeometryModel3D(mesh6, material6);
            model_group.Children.Add(model6);
#endif

            Console.WriteLine(
                mesh1.Positions.Count +
                mesh2.Positions.Count +
                mesh3.Positions.Count +
                mesh4.Positions.Count +
                mesh5.Positions.Count +
                mesh6.Positions.Count +
                " points");
            Console.WriteLine(
                (mesh1.TriangleIndices.Count +
                 mesh2.TriangleIndices.Count +
                 mesh3.TriangleIndices.Count +
                 mesh4.TriangleIndices.Count +
                 mesh5.TriangleIndices.Count +
                 mesh6.TriangleIndices.Count) / 3 + " triangles");
            Console.WriteLine();
        }


    }
}