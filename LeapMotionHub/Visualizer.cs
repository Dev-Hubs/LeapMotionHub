using Leap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LeapFirst
{
    class Visualizer
    {

        public Mesh mesh;
        SolidColorBrush brush;
        DiffuseMaterial material;
        static SolidColorBrush[] brushes = { Brushes.Red, Brushes.Yellow ,Brushes.Green,Brushes.Purple,Brushes.Blue};
        DiffuseMaterial[] materials = { new DiffuseMaterial(brushes[0]), new DiffuseMaterial(brushes[1]), new DiffuseMaterial(brushes[2]), new DiffuseMaterial(brushes[3]), new DiffuseMaterial(brushes[4]) };
        public Visualizer() {
            mesh = new Mesh();
            brush = Brushes.Green;
            material = new DiffuseMaterial(brush);
        }
        public List<GeometryModel3D> drawLeftHand(Hand hand) {
            List<GeometryModel3D> list = new List<GeometryModel3D>();
            for (int i = 0; i < hand.Fingers.Count; i++)
            {
               // material = materials[i];
                Finger finger = hand.Fingers[i];
                List<Bone> allBones = new List<Bone>();

                Bone _m = finger.Bone(Leap.Bone.BoneType.TYPE_METACARPAL);
                Bone _pp = finger.Bone(Leap.Bone.BoneType.TYPE_PROXIMAL);
                Bone _ip = finger.Bone(Leap.Bone.BoneType.TYPE_INTERMEDIATE);
                Bone _dp = finger.Bone(Leap.Bone.BoneType.TYPE_DISTAL);
                allBones.Add(_m);
                allBones.Add(_pp);
                allBones.Add(_ip);
                allBones.Add(_dp);

                for (int j = 0; j < allBones.Count; j++)
                {
                    material = materials[j];
                    if (allBones[j].IsValid)
                    {
                        Vector before = allBones[j].PrevJoint;

                        GeometryModel3D sphere = new GeometryModel3D(mesh.createSphere(new Point3D(before.x, before.y, before.z)), material);
                        list.Add(sphere);





                                Vector after = allBones[j].NextJoint;
                                GeometryModel3D cylinder = new GeometryModel3D(mesh.createCylinder(new Point3D(before.x, before.y, before.z), new Point3D(after.x, after.y, after.z)), material);
                                list.Add(cylinder);

                                if (j == allBones.Count - 1)
                                {
                                    Vector last = allBones[j].NextJoint;
                                    GeometryModel3D _sphere = new GeometryModel3D(mesh.createSphere(new Point3D(last.x, last.y, last.z)), material);
                                    list.Add(_sphere);
                                }


                    }


                }



            }



            return list;

        }
        public List<GeometryModel3D> drawRightHand(Hand hand)
        {
            List<GeometryModel3D> list = new List<GeometryModel3D>();
            for (int i = 0; i < hand.Fingers.Count; i++)
            {
                //material = materials[i];
                Finger finger = hand.Fingers[i];
                List<Bone> allBones = new List<Bone>();

                Bone _m = finger.Bone(Leap.Bone.BoneType.TYPE_METACARPAL);
                Bone _pp = finger.Bone(Leap.Bone.BoneType.TYPE_PROXIMAL);
                Bone _ip = finger.Bone(Leap.Bone.BoneType.TYPE_INTERMEDIATE);
                Bone _dp = finger.Bone(Leap.Bone.BoneType.TYPE_DISTAL);
                allBones.Add(_m);
                allBones.Add(_pp);
                allBones.Add(_ip);
                allBones.Add(_dp);

                for (int j = 0; j < allBones.Count; j++)
                {
                    material = materials[j];
                    if (allBones[j].IsValid)
                    {
                        Vector before = allBones[j].PrevJoint;

                        GeometryModel3D sphere = new GeometryModel3D(mesh.createSphere(new Point3D(before.x, before.y, before.z)), material);
                        list.Add(sphere);





                        Vector after = allBones[j].NextJoint;
                        GeometryModel3D cylinder = new GeometryModel3D(mesh.createCylinder(new Point3D(before.x, before.y, before.z), new Point3D(after.x, after.y, after.z)), material);
                        list.Add(cylinder);

                        if (j == allBones.Count - 1)
                        {
                            Vector last = allBones[j].NextJoint;
                            GeometryModel3D _sphere = new GeometryModel3D(mesh.createSphere(new Point3D(last.x, last.y, last.z)), material);
                            list.Add(_sphere);
                        }


                    }


                }



            }



            return list;

        }
        public List<GeometryModel3D> drawShape() {
            List<GeometryModel3D> list = new List<GeometryModel3D>();
            list.Add(new GeometryModel3D(mesh.createSphere(new Point3D(1, 0, 8)), material));
            list.Add(new GeometryModel3D(mesh.createSphere(new Point3D(2, 3, 8)), material));
            list.Add(new GeometryModel3D(mesh.createSphere(new Point3D(4, 5, 8)), material));
            list.Add(new GeometryModel3D(mesh.createSphere(new Point3D(2, 5, 4)), material));

            return list;
        }
    }
}
