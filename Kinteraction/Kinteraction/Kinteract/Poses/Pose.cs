using Kinteraction.Kinteract.Poses.Selectors;
using Microsoft.Kinect;

namespace Kinteraction.Kinteract.Poses
{
    public class Pose
    {
        public static readonly JointSelector Head = new JointSelector(JointType.Head);
        public static readonly JointSelector Neck = new JointSelector(JointType.Neck);
        public static readonly JointSelector SpineShoulder = new JointSelector(JointType.SpineShoulder);
        public static readonly JointSelector SpineMid = new JointSelector(JointType.SpineMid);
        public static readonly JointSelector HipCenter = new JointSelector(JointType.SpineBase);

        public static readonly JointSelector RightShoulder = new JointSelector(JointType.ShoulderRight);
        public static readonly JointSelector RightElbow = new JointSelector(JointType.ElbowRight);
        public static readonly JointSelector RightWrist = new JointSelector(JointType.WristRight);
        public static readonly JointSelector RightHand = new JointSelector(JointType.HandRight);
        public static readonly JointSelector RightHandTip = new JointSelector(JointType.HandTipRight);
        public static readonly JointSelector RightThumb = new JointSelector(JointType.ThumbRight);

        public static readonly JointSelector LeftShoulder = new JointSelector(JointType.ShoulderLeft);
        public static readonly JointSelector LeftElbow = new JointSelector(JointType.ElbowLeft);
        public static readonly JointSelector LeftWrist = new JointSelector(JointType.WristLeft);
        public static readonly JointSelector LeftHand = new JointSelector(JointType.HandLeft);
        public static readonly JointSelector LeftHandTip = new JointSelector(JointType.HandTipLeft);
        public static readonly JointSelector LeftThumb = new JointSelector(JointType.ThumbLeft);

        public static readonly JointSelector RightHip = new JointSelector(JointType.HipRight);
        public static readonly JointSelector RightKnee = new JointSelector(JointType.KneeRight);
        public static readonly JointSelector RightAnkle = new JointSelector(JointType.AnkleRight);
        public static readonly JointSelector RightFoot = new JointSelector(JointType.FootRight);

        public static readonly JointSelector LeftHip = new JointSelector(JointType.HipLeft);
        public static readonly JointSelector LeftKnee = new JointSelector(JointType.KneeLeft);
        public static readonly JointSelector LeftAnkle = new JointSelector(JointType.AnkleLeft);
        public static readonly JointSelector LeftFoot = new JointSelector(JointType.FootLeft);
    }
}