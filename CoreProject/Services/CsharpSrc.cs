using System;
using System.Collections.Generic;
using System.Text;

namespace CoreProject.Services
{
    class ClassDeclaration
    {
        public CsharpSrc CsharpSrc { get; set; }
        public string ClassDecl { get; set; }
        private List<string> methodDecls = new List<string>();

        public ClassDeclaration Method(string method_declaration)
        {
            methodDecls.Add(method_declaration);
            return this;
        }

        public CsharpSrc End()
        {
            return this.CsharpSrc;
        }
    }

    class CsharpSrc
    {
        string fileNamespace;
        List<string> using_namespaces = new List<string>();
        List<ClassDeclaration> classDecls = new List<ClassDeclaration>();

        public CsharpSrc Using(string _namespace)
        {
            this.using_namespaces.Add(_namespace);
            return this;
        }

        public CsharpSrc Namespace(string file_namespace)
        {
            fileNamespace = file_namespace;
            return this;
        }

        public ClassDeclaration Class(string class_declaration)
        {
            var classInfo = new ClassDeclaration { CsharpSrc = this, ClassDecl =  class_declaration};
            this.classDecls.Add(classInfo);
            return classInfo;
        }
    }
}
