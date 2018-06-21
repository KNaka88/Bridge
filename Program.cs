using System;
using Autofac;

namespace Bridge
{

    // Bridge Pattern http://www.dofactory.com/net/bridge-design-pattern


    // Implementor abstract interface
    public interface IRenderer
    {
        void RenderCircle(float radius);
    }

    // ConcreteImplementor
    public class VectorRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            Console.WriteLine($"Drawing a circle of radius {radius}");
        }
    }

    // ConcreteImplementor
    public class RasterRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            Console.WriteLine($"Drawing pixels for circle with radius {radius}");
        }
    }


    // Abstraction
    public abstract class Shape
    {
        protected IRenderer renderer;

        protected Shape(IRenderer renderer)
        {
            this.renderer = renderer ?? throw new ArgumentNullException();
        }

        public abstract void Draw();
        public abstract void Resize(float factor);
    }

    // Refined Abstraction class
    public class Circle : Shape
    {
        private float radius;

        public Circle(IRenderer renderer, float radius) : base(renderer)
        {
            this.radius = radius;
        }
        public override void Draw()
        {
            renderer.RenderCircle(radius);
        }

        public override void Resize(float factor)
        {
            radius *= factor;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //var renderer = new RasterRenderer();
            //var renderer = new VectorRenderer();
            //var circle = new Circle(renderer, 5.5f);
            //circle.Draw();
            //circle.Resize(2);
            //circle.Draw();

            var cb = new ContainerBuilder();
            cb.RegisterType<VectorRenderer>().As<IRenderer>().SingleInstance();

            cb.Register(((c, p) => new Circle(c.Resolve<IRenderer>(), p.Positional<float>(0))));

            using (var c = cb.Build())
            {
                var circle = c.Resolve<Circle>(
                    new PositionalParameter(0, 5.0f)
                );
                circle.Draw();
                circle.Resize(2);
                circle.Draw();
            }
        }
    }
}
