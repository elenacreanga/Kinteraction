using Kinteract.Poses.Selectors;
using Microsoft.Kinect;

namespace Kinteract.Poses
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
        public static readonly JointSelector RightFootTip = new JointSelector(JointType.FootRight);

        public static readonly JointSelector LeftHip = new JointSelector(JointType.HipLeft);
        public static readonly JointSelector LeftKnee = new JointSelector(JointType.KneeLeft);
        public static readonly JointSelector LeftAnkle = new JointSelector(JointType.AnkleLeft);
        public static readonly JointSelector LeftFootTip = new JointSelector(JointType.FootLeft);

        public static readonly BoneSelector UpperNeck = new BoneSelector(JointType.Head, JointType.Neck);
        public static readonly BoneSelector LowerNeck = new BoneSelector(JointType.Neck, JointType.SpineShoulder);

        public static readonly BoneSelector LeftClavicle =
            new BoneSelector(JointType.ShoulderLeft, JointType.SpineShoulder);
        public static readonly BoneSelector RightClavicle =
            new BoneSelector(JointType.ShoulderRight, JointType.SpineShoulder);

        public static readonly BoneSelector LeftForearm = new BoneSelector(JointType.ElbowLeft, JointType.WristLeft);
        public static readonly BoneSelector RightForearm = new BoneSelector(JointType.ElbowRight, JointType.WristRight);

        public static readonly BoneSelector LeftUpperarm = new BoneSelector(JointType.ShoulderLeft, JointType.ElbowLeft);
        public static readonly BoneSelector RightUpperarm = new BoneSelector(JointType.ShoulderRight, JointType.ElbowRight);

        public static readonly BoneSelector LeftFoot = new BoneSelector(JointType.AnkleLeft, JointType.FootLeft);
        public static readonly BoneSelector RightFoot = new BoneSelector(JointType.AnkleRight, JointType.FootRight);

        public static readonly BoneSelector LeftShin = new BoneSelector(JointType.KneeLeft, JointType.AnkleLeft);
        public static readonly BoneSelector RightShin = new BoneSelector(JointType.KneeRight, JointType.AnkleRight);

        public static readonly BoneSelector LeftThigh = new BoneSelector(JointType.HipLeft, JointType.KneeLeft);
        public static readonly BoneSelector RightThigh = new BoneSelector(JointType.HipRight, JointType.KneeRight);

        public static readonly BoneSelector LeftLowerHand = new BoneSelector(JointType.HandLeft, JointType.HandTipLeft);
        public static readonly BoneSelector RightLowerHand = new BoneSelector(JointType.HandRight, JointType.HandTipRight);

        public static readonly BoneSelector LeftPelvis = new BoneSelector(JointType.SpineBase, JointType.HipLeft);
        public static readonly BoneSelector RightPelvis = new BoneSelector(JointType.SpineBase, JointType.HipRight);

        public static readonly BoneSelector LeftThumbBone = new BoneSelector(JointType.ThumbLeft, JointType.WristLeft);
        public static readonly BoneSelector RightThumbBone = new BoneSelector(JointType.ThumbRight, JointType.WristRight);

        public static readonly BoneSelector LeftUpperHand = new BoneSelector(JointType.HandLeft, JointType.WristLeft);
        public static readonly BoneSelector RightUpperHand = new BoneSelector(JointType.HandRight, JointType.WristRight);

        public static readonly BoneSelector LowerBack = new BoneSelector(JointType.SpineBase, JointType.SpineMid);

        public static readonly BoneSelector UpperBack = new BoneSelector(JointType.SpineMid, JointType.SpineShoulder);
    }
}