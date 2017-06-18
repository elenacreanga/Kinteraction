using System.Windows.Media;
using SharpGL;

namespace Kinteraction.Shapes
{
    public class Sphere : Shape
    {
        public Sphere()
        {
            Origin = new double[3] { 0, 0, 0 };
            Color = Colors.DarkCyan;
            R = 2;
        }

        public Sphere(double[] origin, Color color, double r)
        {
            Origin = origin;
            Color = color;
            R = r;
        }

        public override void Draw(OpenGL gl)
        {
            gl.Color(Color.R, Color.G, Color.B, Color.A);
            gl.PushMatrix();
            gl.Translate(Origin[0], Origin[1], Origin[2]);
            var sphere = gl.NewQuadric();
            if (false)
                gl.QuadricDrawStyle(sphere, OpenGL.GL_LINES);
            gl.QuadricDrawStyle(sphere, OpenGL.GL_QUADS);
            gl.QuadricNormals(sphere, OpenGL.GLU_NONE); //GLU_NONE,GLU_FLAT,GLU_SMOOTH
            gl.QuadricOrientation(sphere, (int)OpenGL.GLU_OUTSIDE); //GLU_OUTSIDE,GLU_INSIDE
            gl.QuadricTexture(sphere, (int)OpenGL.GLU_TRUE); //GL_TRUE,GLU_FALSE
            gl.Sphere(sphere, R + Rp, (int)(R + 1) * 10, (int)(R + 1) * 10);
            gl.DeleteQuadric(sphere);
            gl.PopMatrix();
        }
    }
}