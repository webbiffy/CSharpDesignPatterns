using CSharpDesignPattern.Builder.html;
using System;


namespace CSharpDesignPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            var words = new[] { "hello", "world" };
            var builder = new HtmlBuilder("ul");

            builder.AddChild("li", "hello");
            builder.AddChild("li", "world");

            Console.WriteLine(builder.ToString());
        }
    }
}
