using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Kinteract.Poses.Distance;
using Kinteract.Poses.Helpers;
using Microsoft.Kinect;

namespace Kinteract.Poses.Selectors
{
    public class BoneSelector
    {
        public BoneSelector(JointType from, JointType to)
        {
            To = to;
            _bodyGraph = new BodyGraph();
            From = from;
        }

        public JointType To { get; }

        public JointType From { get; }

        private BodyGraph _bodyGraph;

        public virtual Posture PointingLeft()
        {
            return new FunctionalPosture(body =>
                RatioCalculator.DifferenceDistanceRatio(
                    body.XDiff(To, From),
                    body.DistanceBetween(To, From)));
        }

        public virtual Posture PointingRight()
        {
            return new FunctionalPosture(body =>
                RatioCalculator.DifferenceDistanceRatio(
                    body.XDiff(From, To),
                    body.DistanceBetween(To, From)));
        }

        public virtual Posture PointingUp()
        {
            return new FunctionalPosture(body =>
                RatioCalculator.DifferenceDistanceRatio(
                    body.YDiff(To, From),
                    body.DistanceBetween(To, From)));
        }

        public virtual Posture PointingDown()
        {
            return new FunctionalPosture(body =>
                RatioCalculator.DifferenceDistanceRatio(
                    body.YDiff(From, To),
                    body.DistanceBetween(To, From)));
        }

        public virtual Posture PointingBackwards()
        {
            return new FunctionalPosture(body =>
                RatioCalculator.DifferenceDistanceRatio(
                    body.ZDiff(To, From),
                    body.DistanceBetween(To, From)));
        }

        public virtual Posture PointingForward()
        {
            return new FunctionalPosture(body =>
                RatioCalculator.DifferenceDistanceRatio(
                    body.YDiff(From, To),
                    body.DistanceBetween(To, From)));
        }

        public virtual Posture InStraightAngleWith(JointType joint)
        {
            const double straightAngle = 180.00;

            return InAngleWith(joint, straightAngle);
        }
        public virtual Posture InRightAngleWith(JointType joint)
        {
            const double rightAngle = 90.00;

            return InAngleWith(joint, rightAngle);
        }

        public virtual Posture InObtuseAngleWith(JointType joint)
        {
            const double obtuseAngle = 130.00;

            return InAngleWith(joint, obtuseAngle);
        }

        public virtual Posture InAcuteAngleWith(JointType joint)
        {
            const double acuteAngle = 45.00;

            return InAngleWith(joint, acuteAngle);
        }

        public virtual Posture InReflexAngleWith(JointType joint)
        {
            const double acuteAngle = 300.00;

            return InAngleWith(joint, acuteAngle);
        }

        private Posture InAngleWith(JointType joint, double angle)
        {
            var jointNode = _bodyGraph.Graph.FirstOrDefault(x => x.Name == joint.ToString());
            if (jointNode.DistanceDict.Any(x => x.Key == From.ToString()))
            {
                return new FunctionalPosture(body =>
                    RatioCalculator.AngleDifference(angle, body.GetAngleBetween(To, From, joint)));
            }
            if (jointNode.DistanceDict.Any(x => x.Key == To.ToString()))
            {
                return new FunctionalPosture(body =>
                    RatioCalculator.AngleDifference(angle, body.GetAngleBetween(From, To, joint)));
            }
            return new FunctionalPosture(body => 0);
        }
    }
}
