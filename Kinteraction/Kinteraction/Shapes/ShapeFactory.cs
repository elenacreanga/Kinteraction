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
                case Type.Pyramid:
                    return new Pyramid();
                case Type.Hand:
                    return new Sphere();
                case Type.Tracker:
                    return new Tracker();
                case Type.Axis:
                    return new Axis();
                default:
                    return null;
            }
        }
    }
}
