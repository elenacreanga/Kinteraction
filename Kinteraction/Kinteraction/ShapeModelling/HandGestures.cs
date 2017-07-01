using System;
using System.ComponentModel;
using Kinteraction.Helpers;
using Kinteraction.ShapeModelling.Shapes;

namespace Kinteraction.ShapeModelling
{
    internal class HandGestures
    {
        internal Mod Mod;
        internal Mod PreviousMod = Mod.FREE;
        private readonly Hands _hands;
        internal double[] Angle;
        internal float Dist;
        private string _modText;

        public HandGestures(Hands hands)
        {
            _hands = hands;
        }
        public void IsGrabbed(Shape s)
        {
            if (Mod == Mod.FREE)
            {
                if (!_hands.Right.IsOpen)
                    if (!s.IsGrabbed)
                        if (s.Origin[0] - s.R < _hands.Right.Origin[0] && s.Origin[0] + s.R > _hands.Right.Origin[0] &&
                            s.Origin[1] - s.R < _hands.Right.Origin[1] && s.Origin[1] + s.R > _hands.Right.Origin[1] &&
                            s.Origin[2] - s.R < _hands.Right.Origin[2] && s.Origin[2] + s.R > _hands.Right.Origin[2])
                        {
                            s.IsGrabbed = true;
                            Mod = Mod.GRAB;
                        }
            }
            else
            {
                if (_hands.Right.IsOpen)
                    if (s.IsGrabbed)
                    {
                        s.IsGrabbed = false;
                        Mod = Mod.FREE;
                    }
            }

            ModText = Enum.GetName(typeof(Mod), Mod);
            Console.WriteLine(Mod);
        }

        public void IsZoom(Shape s)
        {
            if (Mod == Mod.GRAB)
            {
                if (!_hands.Left.IsOpen)
                {
                    Mod = Mod.ZOOM;
                    Dist = Euclid.Calculate(_hands.Left.Origin, _hands.Right.Origin);
                    Angle = new double[3]
                    {
                        _hands.Left.Origin[0] - _hands.Right.Origin[0], _hands.Left.Origin[1] - _hands.Right.Origin[1],
                        _hands.Left.Origin[2] - _hands.Right.Origin[2]
                    };
                }
            }
            else
            {
                if (_hands.Left.IsOpen)
                {
                    Mod = Mod.GRAB;
                    s.R += s.Rp;
                    s.Rp = 0;
                    for (var k = 0; k < 3; k++)
                    {
                        s.Ro[k] += s.Rop[k];
                        s.Rop[k] = 0;
                    }
                    if (s.R < 1) s.R = 1;
                }
            }
        }

        public string ModText
        {
            get => _modText;
            set
            {
                _modText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ModText"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    internal enum Mod
    {
        FREE,
        GRAB,
        ZOOM,
        LASSO
    }
}
