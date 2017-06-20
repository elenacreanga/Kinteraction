using Kinteraction.Kinteract.Poses.Helpers;
using Microsoft.Kinect;

namespace Kinteraction.Kinteract.Poses.Selectors
{
    public class JointSelector
    {
        public JointSelector(JointType joint)
        {
            Joint = joint;
        }
        public JointType Joint { get; private set; }

        public virtual Posture Above(JointType other)
        {
            return new FunctionalPosture((body) =>
                RatioCalculator.DifferenceDistanceRatio(
                    body.YDiff(Joint, other),
                    body.PathLengthBetween(Joint, other))
            );
        }

        public virtual Posture Below(JointType other)
        {
            return new FunctionalPosture((body) =>
                RatioCalculator.DifferenceDistanceRatio(
                    body.YDiff(other, Joint),
                    body.PathLengthBetween(other, Joint)));
        }

        public virtual Posture AtTheSameHeightOf(JointType other)
        {
            return new FunctionalPosture((body) =>
                RatioCalculator.AbsoluteDifferenceDistanceRatio(
                    body.YDiff(Joint, other),
                    body.PathLengthBetween(Joint, other)));
        }

        public virtual Posture After(JointType other)
        {
            return new FunctionalPosture((body) =>
                RatioCalculator.DifferenceDistanceRatio(
                    body.ZDiff(Joint, other),
                    body.PathLengthBetween(Joint, other)));
        }

        public virtual Posture Before(JointType other)
        {
            return new FunctionalPosture((body) =>
                RatioCalculator.DifferenceDistanceRatio(
                    body.ZDiff(Joint, other),
                    body.PathLengthBetween(other, Joint)));
        }

        public virtual Posture AtTheSameDepthOf(JointType other)
        {
            return new FunctionalPosture((body) =>
                RatioCalculator.AbsoluteDifferenceDistanceRatio(
                    body.ZDiff(Joint, other),
                    body.PathLengthBetween(Joint, other)));
        }

        public virtual Posture ToTheLeftOf(JointType other)
        {
            return new FunctionalPosture((body) =>
                RatioCalculator.DifferenceDistanceRatio(
                    body.XDiff(Joint, other),
                    body.PathLengthBetween(Joint, other)));
        }

        public virtual Posture ToTheRightOf(JointType other)
        {
            return new FunctionalPosture((body) =>
                RatioCalculator.DifferenceDistanceRatio(
                    body.XDiff(other, Joint),
                    body.PathLengthBetween(other, Joint)));
        }

        public virtual Posture AtTheSameLengthOf(JointType other)
        {
            return new FunctionalPosture((body) =>
                RatioCalculator.AbsoluteDifferenceDistanceRatio(
                    body.XDiff(Joint, other),
                    body.PathLengthBetween(Joint, other)));
        }
    }
}
