using SharpGL;

namespace Kinteraction.Shapes
{
    public class Tracker : Shape
    {
        public override void Draw(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_LINES);

            gl.LineWidth(2);
            gl.Color(Color.R, Color.G, Color.B, Color.A);

            gl.Vertex(0, 0, Origin[2]);
            gl.Vertex(0, Origin[1], Origin[2]);
            gl.Vertex(0, Origin[1], Origin[2]);
            gl.Vertex(Origin[0], Origin[1], Origin[2]);
            gl.Vertex(Origin[0], Origin[1], Origin[2]);
            gl.Vertex(Origin[0], 0, Origin[2]);
            gl.Vertex(Origin[0], 0, Origin[2]);
            gl.Vertex(0, 0, Origin[2]);

            gl.Vertex(Origin[0], Origin[1], Origin[2]);
            gl.Vertex(Origin[0], Origin[1], 0);
            gl.Vertex(Origin[0], Origin[1], 0);
            gl.Vertex(0, Origin[1], 0);
            gl.Vertex(0, Origin[1], 0);
            gl.Vertex(0, Origin[1], Origin[2]);

            gl.Vertex(Origin[0], Origin[1], 0);
            gl.Vertex(Origin[0], 0, 0);
            gl.Vertex(Origin[0], 0, 0);
            gl.Vertex(Origin[0], 0, Origin[2]);
            gl.End();
        }
    }
}
