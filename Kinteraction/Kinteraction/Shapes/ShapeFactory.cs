using SharpGL.SceneGraph.Quadrics;

namespace Kinteraction.Shapes
{
    public class ShapeFactory
    {
        //use getShape method to get object of type shape 
        public Shape GetShape(Type shapeType)
        {
            switch (shapeType)
            {
                case Type.Cube:
                    return new Cube();
                case Type.Sphere:
                    return new Sphere();
                default:
                    return null;
            }
        }
    }
}
