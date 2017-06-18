using System.Windows.Media;
using SharpGL;

namespace Kinteraction.Shapes
{
    public abstract class Shape
    {
        public Color Color;
        public bool Grabbed = false;
        public double[] Move = new double[3] {0, 0, 0};
        public double[] Origin = new double[3];

        public double R;

        //rotate
        public double[] Ro = new double[3] {0, 0, 0};

        public double[] Rop = new double[3] {0, 0, 0};
        public double[] Rotate = new double[3] {0, 0, 0};

        //r plus
        public double Rp = 0;

        public Type Type;

        public abstract void Draw(OpenGL gl);
    }

    public enum Type
    {
        Cube,
        Sphere,
        Pyramid,
        Hand,
        Tracker,
        Axis
    }
}