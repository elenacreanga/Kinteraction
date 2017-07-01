using System.Collections.Generic;
using Microsoft.Kinect;

namespace Kinteract.Poses.Distance
{
    public class BodyGraph
    {
        private readonly Node _ankleLeft = new Node
        {
            Name = JointType.AnkleLeft.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.KneeLeft.ToString(), new List<string> {JointType.KneeLeft.ToString()}},
                {JointType.FootLeft.ToString(), new List<string> {JointType.FootLeft.ToString()}}
            },
            Visited = false
        };

        private readonly Node _ankleRight = new Node
        {
            Name = JointType.AnkleRight.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.KneeRight.ToString(), new List<string> {JointType.KneeRight.ToString()}},
                {JointType.FootRight.ToString(), new List<string> {JointType.FootRight.ToString()}}
            },
            Visited = false
        };

        private readonly Node _elbowLeft = new Node
        {
            Name = JointType.ElbowLeft.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.ShoulderLeft.ToString(), new List<string> {JointType.ShoulderLeft.ToString()}},
                {JointType.WristLeft.ToString(), new List<string> {JointType.WristLeft.ToString()}}
            },
            Visited = false
        };

        private readonly Node _elbowRight = new Node
        {
            Name = JointType.ElbowRight.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.ShoulderRight.ToString(), new List<string> {JointType.ShoulderRight.ToString()}},
                {JointType.WristRight.ToString(), new List<string> {JointType.WristRight.ToString()}}
            },
            Visited = false
        };

        private readonly Node _footLeft = new Node
        {
            Name = JointType.FootLeft.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.AnkleLeft.ToString(), new List<string> {JointType.AnkleLeft.ToString()}}
            },
            Visited = false
        };

        private readonly Node _handLeft = new Node
        {
            Name = JointType.HandLeft.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.WristLeft.ToString(), new List<string> {JointType.WristLeft.ToString()}},
                {JointType.HandTipLeft.ToString(), new List<string> {JointType.HandTipLeft.ToString()}}
            },
            Visited = false
        };

        private readonly Node _handRight = new Node
        {
            Name = JointType.HandRight.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.WristRight.ToString(), new List<string> {JointType.WristRight.ToString()}},
                {JointType.HandTipRight.ToString(), new List<string> {JointType.HandTipRight.ToString()}}
            },
            Visited = false
        };

        private readonly Node _handTipLeft = new Node
        {
            Name = JointType.HandTipLeft.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.HandLeft.ToString(), new List<string> {JointType.HandLeft.ToString()}}
            },
            Visited = false
        };

        private readonly Node _handTipRight = new Node
        {
            Name = JointType.HandTipRight.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.HandRight.ToString(), new List<string> {JointType.HandRight.ToString()}}
            },
            Visited = false
        };

        private readonly Node _head = new Node
        {
            Name = JointType.Head.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.Neck.ToString(), new List<string> {JointType.Neck.ToString()}}
            },
            Visited = false
        };

        private readonly Node _hipLeft = new Node
        {
            Name = JointType.HipLeft.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.SpineBase.ToString(), new List<string> {JointType.SpineBase.ToString()}},
                {JointType.KneeLeft.ToString(), new List<string> {JointType.KneeLeft.ToString()}}
            },
            Visited = false
        };

        private readonly Node _hipRight = new Node
        {
            Name = JointType.HipRight.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.SpineBase.ToString(), new List<string> {JointType.SpineBase.ToString()}},
                {JointType.KneeRight.ToString(), new List<string> {JointType.KneeRight.ToString()}}
            },
            Visited = false
        };

        private readonly Node _kneeLeft = new Node
        {
            Name = JointType.KneeLeft.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.HipLeft.ToString(), new List<string> {JointType.HipLeft.ToString()}},
                {JointType.AnkleLeft.ToString(), new List<string> {JointType.AnkleLeft.ToString()}}
            },
            Visited = false
        };

        private readonly Node _kneeRight = new Node
        {
            Name = JointType.KneeRight.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.HipRight.ToString(), new List<string> {JointType.HipRight.ToString()}},
                {JointType.AnkleRight.ToString(), new List<string> {JointType.AnkleRight.ToString()}}
            },
            Visited = false
        };

        private readonly Node _neck = new Node
        {
            Name = JointType.Neck.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.Head.ToString(), new List<string> {JointType.Head.ToString()}},
                {JointType.SpineShoulder.ToString(), new List<string> {JointType.SpineShoulder.ToString()}}
            },
            Visited = false
        };

        private readonly Node _shoulderLeft = new Node
        {
            Name = JointType.ShoulderLeft.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.ElbowLeft.ToString(), new List<string> {JointType.ElbowLeft.ToString()}},
                {JointType.SpineShoulder.ToString(), new List<string> {JointType.SpineShoulder.ToString()}}
            },
            Visited = false
        };

        private readonly Node _shoulderRight = new Node
        {
            Name = JointType.ShoulderRight.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.SpineShoulder.ToString(), new List<string> {JointType.SpineShoulder.ToString()}},
                {JointType.ElbowRight.ToString(), new List<string> {JointType.ElbowRight.ToString()}}
            },
            Visited = false
        };

        private readonly Node _spineBase = new Node
        {
            Name = JointType.SpineBase.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.SpineMid.ToString(), new List<string> {JointType.SpineMid.ToString()}},
                {JointType.HipLeft.ToString(), new List<string> {JointType.HipLeft.ToString()}},
                {JointType.HipRight.ToString(), new List<string> {JointType.HipRight.ToString()}}
            },
            Visited = false
        };

        private readonly Node _spineMid = new Node
        {
            Name = JointType.SpineMid.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.SpineBase.ToString(), new List<string> {JointType.SpineBase.ToString()}},
                {JointType.SpineShoulder.ToString(), new List<string> {JointType.SpineShoulder.ToString()}}
            },
            Visited = false
        };

        private readonly Node _spineShoulder = new Node
        {
            Name = JointType.SpineShoulder.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.SpineMid.ToString(), new List<string> {JointType.SpineMid.ToString()}},
                {JointType.Neck.ToString(), new List<string> {JointType.Neck.ToString()}},
                {JointType.ShoulderLeft.ToString(), new List<string> {JointType.ShoulderLeft.ToString()}},
                {JointType.ShoulderRight.ToString(), new List<string> {JointType.ShoulderRight.ToString()}}
            },
            Visited = false
        };

        private readonly Node _thumbLeft = new Node
        {
            Name = JointType.ThumbLeft.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.WristLeft.ToString(), new List<string> {JointType.WristLeft.ToString()}}
            },
            Visited = false
        };

        private readonly Node _thumbRight = new Node
        {
            Name = JointType.ThumbRight.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.WristRight.ToString(), new List<string> {JointType.WristRight.ToString()}}
            },
            Visited = false
        };

        private readonly Node _wristLeft = new Node
        {
            Name = JointType.WristLeft.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.ElbowLeft.ToString(), new List<string> {JointType.ElbowLeft.ToString()}},
                {JointType.HandLeft.ToString(), new List<string> {JointType.HandLeft.ToString()}},
                {JointType.ThumbLeft.ToString(), new List<string> {JointType.ThumbLeft.ToString()}}
            },
            Visited = false
        };

        private readonly Node _wristRight = new Node
        {
            Name = JointType.WristRight.ToString(),
            DistanceDict = new Dictionary<string, List<string>>
            {
                {JointType.ElbowRight.ToString(), new List<string> {JointType.ElbowRight.ToString()}},
                {JointType.HandRight.ToString(), new List<string> {JointType.HandRight.ToString()}},
                {JointType.ThumbLeft.ToString(), new List<string> {JointType.ThumbRight.ToString()}}
            },
            Visited = false
        };

        public BodyGraph()
        {
            Graph = new List<Node>
            {
                _spineBase,
                _spineMid,
                _neck,
                _head,
                _shoulderLeft,
                _elbowLeft,
                _wristLeft,
                _handLeft,
                _shoulderRight,
                _elbowRight,
                _wristRight,
                _handRight,
                _hipLeft,
                _kneeLeft,
                _ankleLeft,
                _footLeft,
                _hipRight,
                _kneeRight,
                _ankleRight,
                _spineShoulder,
                _handTipLeft,
                _thumbLeft,
                _handTipRight,
                _thumbRight
            };
        }

        public List<Node> Graph { get; set; }
    }
}