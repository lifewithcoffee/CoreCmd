using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreProject.Services
{
    class ScaffoldingService
    {
        internal void GenerateClassFile(string className, string path)
        {
            Directory.CreateDirectory(path);    // always create the path recursively if not exist

            string filePath = $@"{path}\{className}.cs";
            using(var textWriter = File.AppendText(filePath))
            {
                string src = $@"namespace SomeNS
{{
    class {className}
    {{
        void Bar(){{ }}
    }}
}}";
                textWriter.Write(src);
            }
        }

        internal void GenerateSrc()
        {
            // for some complex source output, use a separate template file and interpolate string
            var src = new CsharpSrc();
            src.Class("public class MyClass")
                .Method("public void Foo1(string param1, double param2)")   // method body: throw new NotImplementedException();
                .Method("public int Foo2(int param)")
                .Method("public string Foo3(float param)")
            .End()
            .Class("public class MyClass2").End()
            .Class("public class MyClass3").End();
        }
    }
}
