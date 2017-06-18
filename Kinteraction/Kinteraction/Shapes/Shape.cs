using SharpGL;

namespace Kinteraction.Shapes
{
    public abstract class Shape
    {
        public double[] Origin = new double[3];
        public double[] Move = new double[3] { 0, 0, 0 };
        public double[] Rotate = new double[3] { 0, 0, 0 };
        public System.Windows.Media.Color Color;

        public double R;

        //r plus
        public double Rp = 0;

        //rotate
        public double[] Ro = new double[3] { 0, 0, 0 };

        public double[] Rop = new double[3] { 0, 0, 0 };
        public Type Type;
        public bool Grabbed = false;

        public abstract void Draw(OpenGL gl);
    }
    public enum Type
    {
        Cube,
        Sphere,
        Pyramid,
        Hand
    }
}
