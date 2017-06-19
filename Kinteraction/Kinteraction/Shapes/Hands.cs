using System.Windows.Media;
using SharpGL;

namespace Kinteraction.Shapes
{
    public class Hands
    {
        public Hand Left;
        public Hand Right;

        public Hands(ShapeFactory shapeFactory)
        {
            Left = new Hand(shapeFactory);
            Right = new Hand(shapeFactory);
        }

        public void Draw(OpenGL gl)
        {
            DrawLeft(gl);
            DrawRight(gl);
        }

        private void DrawRight(OpenGL gl)
        {
            Hand.AdjustOrigin(Right);
            Right.Draw(gl, Colors.Orchid, Right);
        }

        private void DrawLeft(OpenGL gl)
        {
            Hand.AdjustOrigin(Left);
            Left.Draw(gl, Colors.Aqua, Left);
        }

        



    }
}