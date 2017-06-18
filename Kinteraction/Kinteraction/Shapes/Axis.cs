using SharpGL;

namespace Kinteraction.Shapes
{
    public class Axis : Shape
    {
        public override void Draw(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_LINES);
            gl.Color(1.0f, 1.0f, 1.0f);
            gl.Vertex(-100.0f, 0.0f, 0.0f);
            gl.Vertex(100.0f, 0.0f, 0.0f);
            gl.Vertex(0.0f, -100.0f, 0.0f);
            gl.Vertex(0.0f, 100.0f, 0.0f);
            gl.Vertex(0.0f, 0.0f, -100.0f);
            gl.Vertex(0.0f, 0.0f, 100.0f);
            gl.End();
        }
    }
}
