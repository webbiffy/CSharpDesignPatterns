using System;
using System.Collections;
using System.Collections.Generic;
using static System.Console;


namespace CSharpDesignPattern.SOLID.OpenClose
{
    public class OpenClose
    {
        public enum Color
        {
            Red, Blue, Green
        }
        public enum Size
        {
            Small, Medium, Large, Huge
        }

        public class Product
        {

            public Product(string name, Color color, Size size)
            {
                Name = name;
                Color = color;
                Size = size;
            }

            public string Name { get; set; }
            public Color Color { get; set; }
            public Size Size { get; set; }
        }

        public class ProductFilter
        {
            public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
            {
                foreach (var p in products)
                {
                    if (p.Size == size)
                        yield return p;
                }
            }

            public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
            {
                foreach (var p in products)
                {
                    if (p.Color == color)
                        yield return p;
                }
            }

            public IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
            {
                foreach (var p in products)
                {
                    if (p.Size == size && p.Color == color)
                        yield return p;
                }
            }
        }

        public interface ISpecification<T>
        {
            bool IsSatisfied(T t);
        }

        public interface IFilter<T>
        {
            IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
        }

        public class ColorSpecification : ISpecification<Product>
        {
            private readonly Color _color;

            public ColorSpecification(Color color)
            {
                this._color = color;
            }
            public bool IsSatisfied(Product t)
            {
                return t.Color == _color;
            }
        }

        public class SizeSpecification : ISpecification<Product>
        {
            private readonly Size _size;

            public SizeSpecification(Size size)
            {
                this._size = size;
            }

            public bool IsSatisfied(Product t)
            {
                return t.Size == _size;
            }
        }

        public class AndSpecification<T> : ISpecification<T>
        {
            private readonly ISpecification<T> _first, _second;

            public AndSpecification(ISpecification<T> first, ISpecification<T> second)
            {
                this._first = first ?? throw new ArgumentNullException(paramName: nameof(first));
                this._second = second ?? throw new ArgumentNullException(paramName: nameof(second));
            }
            public bool IsSatisfied(T t)
            {
                return _first.IsSatisfied(t) && _second.IsSatisfied(t);
            }
        }


        public class ProductFilterNew : IFilter<Product>
        {
            public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
            {
                foreach (var i in items)
                    if (spec.IsSatisfied(i))
                        yield return i;
            }
        }

        private void Main(string[] args)
        {
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Large);

            Product[] product = { apple, tree, house };

            var pf = new ProductFilter();
            WriteLine("Green Products (old):");
            foreach (var p in pf.FilterByColor(product, Color.Green))
            {
                WriteLine($" - { p.Name } is green");
            }

            var bf = new ProductFilterNew();
            WriteLine("Green Products (new):");
            foreach (var p in bf.Filter(product, new ColorSpecification(Color.Green)))
            {
                WriteLine($" - { p.Name } is green");
            }

            WriteLine("Large Blue Items:");
            foreach (Product p in bf.Filter(
                product,
                new AndSpecification<Product>(
                    new SizeSpecification(Size.Large),
                    new ColorSpecification(Color.Blue)
                )))
            {
                WriteLine($" - { p.Name } is Large and Blue");
            }
        }
    }
}
