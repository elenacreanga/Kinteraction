using System;
using System.Windows.Media;
using SharpGL;

namespace Kinteraction.Shapes
{
    public class Cube : Shape
    {
        public Cube()
        {
            Origin = new double[3] { 0, 0, 10 };
            Color = Colors.BurlyWood;
            R = 1;
        }

        public Cube(double[] origin, Color color, double r)
        {
            Origin = origin;
            Color = color;
            R = r;
        }

        public override void Draw(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(Color.R, Color.G, Color.B, Color.A);
            var r = (R + Rp) * Math.Sqrt(3);
            var v = new double[8][];
            v[0] = new double[] { 1, 1, 1 };
            v[1] = new double[] { -1, 1, 1 };
            v[2] = new double[] { -1, -1, 1 };
            v[3] = new double[] { 1, -1, 1 };
            v[4] = new double[] { 1, 1, -1 };
            v[5] = new double[] { -1, 1, -1 };
            v[6] = new double[] { -1, -1, -1 };
            v[7] = new double[] { 1, -1, -1 };
            for (var k = 0; k < 8; k++)
            for (var j = 0; j < 3; j++)
                v[k][j] *= r / Math.Sqrt(3);
            for (var k = 0; k < 4; k++)
                gl.Vertex(v[k][0], v[k][1], v[k][2]);
            //B
            gl.Color(new[] { 0.4f, 0.2f, 0.7f, 1f });
            for (var k = 4; k < 8; k++)
                gl.Vertex(v[k][0], v[k][1], v[k][2]);
            //L
            gl.Color(new[] { 0.8f, 0.4f, 0.5f, 1f });
            gl.Vertex(v[1][0], v[1][1], v[1][2]);
            gl.Vertex(v[2][0], v[2][1], v[2][2]);
            gl.Vertex(v[6][0], v[6][1], v[6][2]);
            gl.Vertex(v[5][0], v[5][1], v[5][2]);
            //R
            gl.Color(new[] { 0.1f, 0.8f, 0.3f, 1f });
            gl.Vertex(v[0][0], v[0][1], v[0][2]);
            gl.Vertex(v[3][0], v[3][1], v[3][2]);
            gl.Vertex(v[7][0], v[7][1], v[7][2]);
            gl.Vertex(v[4][0], v[4][1], v[4][2]);
            //U
            gl.Color(new[] { 0.7f, 0.4f, 0.1f, 1f });
            gl.Vertex(v[0][0], v[0][1], v[0][2]);
            gl.Vertex(v[1][0], v[1][1], v[1][2]);
            gl.Vertex(v[5][0], v[5][1], v[5][2]);
            gl.Vertex(v[4][0], v[4][1], v[4][2]);
            //D
            gl.Color(new[] { 0.1f, 0.2f, 0.7f, 1f });
            gl.Vertex(v[3][0], v[3][1], v[3][2]);
            gl.Vertex(v[2][0], v[2][1], v[2][2]);
            gl.Vertex(v[6][0], v[6][1], v[6][2]);
            gl.Vertex(v[7][0], v[7][1], v[7][2]);

            gl.End();
        }
    }
}