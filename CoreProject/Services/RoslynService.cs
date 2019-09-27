using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreProject.Services
{
    class RoslynService
    {
        /// <summary>
        /// Changes the namespace for the given code.
        /// </summary>
        public async Task<string> ChangeNamespaceAsync(string code, string @namespace)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = await tree.GetRootAsync().ConfigureAwait(false) as CompilationUnitSyntax;

            // Get the namespace declaration.
            var oldNamespace = root.Members.Single(m => m is NamespaceDeclarationSyntax) as NamespaceDeclarationSyntax;

            // Get all class declarations inside the namespace.
            var classDeclarations = oldNamespace.Members.Where(m => m is ClassDeclarationSyntax);

            // Create a new namespace declaration.
            var newNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(@namespace)).NormalizeWhitespace();

            // Add the class declarations to the new namespace.
            newNamespace = newNamespace.AddMembers(classDeclarations.Cast<MemberDeclarationSyntax>().ToArray());

            // Replace the oldNamespace with the newNamespace and normailize.
            root = root.ReplaceNode(oldNamespace, newNamespace).NormalizeWhitespace();

            return root.ToFullString();
        }
    }
}
