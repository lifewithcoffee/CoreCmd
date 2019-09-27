using CoreProject.Services;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreProject.Commands
{
#if DEBUG
    class RoslynCommand
    {
        RoslynService _svc = new RoslynService();
        public void TestReplacingNamespace()
        {
           var code = @"
                using System; 

                namespace OldNamespace 
                { 
                    public class Person
                    {
                        public string Name { get; set; }
                        public int Age {get; set; }
                    }
                }";

            // Use Task to call ChangeNamespaceAsync
            Console.WriteLine("Demo of replacing namespace, new code:\n");
            string newCode = _svc.ChangeNamespaceAsync(code, "NamespaceChangedUsingRoslyn").Result;
            Console.WriteLine(newCode);

            //File.WriteAllText("Person.cs", root.ToFullString());
        }

        public void TestCreatingClass()
        {
            // Create a namespace: (namespace CodeGenerationSample)
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("CodeGenerationSample")).NormalizeWhitespace();

            // Add System using statement: (using System)
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")));

            //  Create a class: (class Order)
            var classDeclaration = SyntaxFactory.ClassDeclaration("Order");

            // Add the public modifier: (public class Order)
            classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            // Inherit BaseEntity<T> and implement IHaveIdentity: (public class Order : BaseEntity<T>, IHaveIdentity)
            classDeclaration = classDeclaration.AddBaseListTypes(
                SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("BaseEntity<Order>")),
                SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("IHaveIdentity")));

            // Create a string variable: (bool canceled;)
            var variableDeclaration = SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName("bool"))
                .AddVariables(SyntaxFactory.VariableDeclarator("canceled"));

            // Create a field declaration: (private bool canceled;)
            var fieldDeclaration = SyntaxFactory.FieldDeclaration(variableDeclaration)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));

            // Create a Property: (public int Quantity { get; set; })
            var propertyDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName("int"), "Quantity")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

            // Create a stament with the body of a method.
            var syntax = SyntaxFactory.ParseStatement("canceled = true;");

            // Create a method
            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "MarkAsCanceled")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block(syntax));

            // Add the field, the property and method to the class.
            classDeclaration = classDeclaration.AddMembers(fieldDeclaration, propertyDeclaration, methodDeclaration);

            // Add the class to the namespace.
            @namespace = @namespace.AddMembers(classDeclaration);

            // Normalize and get code as string.
            var code = @namespace
                .NormalizeWhitespace()
                .ToFullString();

            // Output new code to the console.
            Console.WriteLine(code);
        }
    }
#endif
}
