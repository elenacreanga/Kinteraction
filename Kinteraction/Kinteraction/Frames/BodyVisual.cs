using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Kinect;

namespace Kinteraction.Frames
{
    internal class BodyVisual
    {
        public readonly List<Tuple<JointType, JointType>> Connections = new List<Tuple<JointType, JointType>>
        {
            // Torso
            new Tuple<JointType, JointType>(JointType.Head, JointType.Neck),
            new Tuple<JointType, JointType>(JointType.Neck, JointType.SpineShoulder),
            new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.SpineMid),
            new Tuple<JointType, JointType>(JointType.SpineMid, JointType.SpineBase),
            new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderRight),
            new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderLeft),
            new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipRight),
            new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipLeft),

            // Right Arm
            new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight),
            new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.WristRight),
            new Tuple<JointType, JointType>(JointType.WristRight, JointType.HandRight),
            new Tuple<JointType, JointType>(JointType.HandRight, JointType.HandTipRight),
            new Tuple<JointType, JointType>(JointType.WristRight, JointType.ThumbRight),

            // Left Arm
            new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft),
            new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.WristLeft),
            new Tuple<JointType, JointType>(JointType.WristLeft, JointType.HandLeft),
            new Tuple<JointType, JointType>(JointType.HandLeft, JointType.HandTipLeft),
            new Tuple<JointType, JointType>(JointType.WristLeft, JointType.ThumbLeft),

            // Right Leg
            new Tuple<JointType, JointType>(JointType.HipRight, JointType.KneeRight),
            new Tuple<JointType, JointType>(JointType.KneeRight, JointType.AnkleRight),
            new Tuple<JointType, JointType>(JointType.AnkleRight, JointType.FootRight),

            // Left Leg
            new Tuple<JointType, JointType>(JointType.HipLeft, JointType.KneeLeft),
            new Tuple<JointType, JointType>(JointType.KneeLeft, JointType.AnkleLeft),
            new Tuple<JointType, JointType>(JointType.AnkleLeft, JointType.FootLeft)
        };

        public BodyVisual()
        {
            Joints = new Dictionary<JointType, Ellipse>();
            Bones = new Dictionary<Tuple<JointType, JointType>, Line>();
        }

        public ulong TrackingId { get; set; }

        public Dictionary<JointType, Ellipse> Joints { get; set; }

        public Dictionary<Tuple<JointType, JointType>, Line> Bones { get; set; }

        public void Clear()
        {
            Joints.Clear();
            Bones.Clear();
        }

        public void AddJoint(JointType joint, double radius, Brush brush)
        {
            Joints.Add(joint, new Ellipse
            {
                Width = radius,
                Height = radius,
                Fill = brush
            });
        }

        public void AddBone(Tuple<JointType, JointType> joints, double thickness, Brush brush)
        {
            Bones.Add(joints, new Line
            {
                StrokeThickness = thickness,
                Stroke = brush
            });
        }

        public void UpdateJoint(JointType joint, Point point)
        {
            var ellipse = Joints[joint];

            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);
        }

        public void UpdateBone(Tuple<JointType, JointType> bone, Point first, Point second)
        {
            var line = Bones[bone];

            line.X1 = first.X;
            line.Y1 = first.Y;
            line.X2 = second.X;
            line.Y2 = second.Y;
        }

        public static BodyVisual Create(ulong trackingId, IEnumerable<JointType> joints, double jointRadius,
            Brush jointBrush, double boneThickness, Brush boneBrush)
        {
            var bodyVisual = new BodyVisual
            {
                TrackingId = trackingId
            };

            foreach (var joint in joints)
                bodyVisual.AddJoint(joint, jointRadius, jointBrush);

            foreach (var bone in bodyVisual.Connections)
                bodyVisual.AddBone(bone, boneThickness, boneBrush);

            return bodyVisual;
        }
    }
}