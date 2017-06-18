using System.Windows.Media;
using SharpGL;

namespace Kinteraction.Shapes
{
    public class Pyramid : Shape
    {
        public Pyramid()
        {
            Origin = new double[3] { -10, 5, 2 };
            Color = Colors.YellowGreen;
            R = 2;
        }

        public override void Draw(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(Color.R, Color.G, Color.B, Color.A);
            var r = (float)(R + Rp);
            gl.Vertex(0, r, 0);
            gl.Vertex(-r, -r, r);
            gl.Vertex(r, -r, r);
            gl.Color(new[] { 0.2f, 0.4f, 0.2f, 1f });
            gl.Vertex(0, r, 0);
            gl.Vertex(r, -r, r);
            gl.Vertex(r, -r, -r);
            gl.Color(new[] { 0.1f, 0.9f, 0.7f, 1f });
            gl.Vertex(0, r, 0);
            gl.Vertex(r, -r, -r);
            gl.Vertex(-r, -r, -r);
            gl.Color(new[] { 0.8f, 0.5f, 0.2f, 1f });
            gl.Vertex(0, r, 0);
            gl.Vertex(-r, -r, -r);
            gl.Vertex(-r, -r, r);
            gl.End();

            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(new[] { 0.7f, 0.2f, 0.4f, 1f });
            gl.Vertex(-r, -r, r);
            gl.Vertex(r, -r, r);
            gl.Vertex(r, -r, -r);
            gl.Vertex(-r, -r, -r);
            gl.End();
        }
    }
}