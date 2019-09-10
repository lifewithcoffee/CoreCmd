using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace CoreProject.Services
{
    class CsprojFileService
    {
        /// <param name="csprojFilePath">
        /// The full absolute path of a .csproj file.
        /// </param>
        /// <returns>
        /// Find the <RootNamespace> element in a .csproj file and return its InnerText value;
        /// If the element is not found, return the .csproj file's name.
        /// </returns>
        public string GetRootNamespace(string csprojFilePath)
        {
            string result = null;
            try { 
                var doc = new XmlDocument();
                doc.Load(csprojFilePath);

                var root_namespace_node = doc.SelectSingleNode("//RootNamespace");
                if (root_namespace_node != null)
                    result = root_namespace_node.InnerText;
                else
                    result = Path.GetFileNameWithoutExtension(csprojFilePath);
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }

        /// <summary>
        /// Start from the current directory and search recursively to the driver root for a .csproj file.
        /// </summary>
        /// <returns>The full path of the 1st found .csproj file or null if nothing found.</returns>
        public string FindCsprojFile()
        {
            string result = null;

            var currentPath = Directory.GetCurrentDirectory();
            result = FindFirstCsprojFile(currentPath);
            while(result == null)
            {
                var parentPath = Directory.GetParent(currentPath);
                if (parentPath == null) // the driver root
                    break;
                else
                {
                    currentPath = parentPath.FullName;
                    result = FindFirstCsprojFile(currentPath);
                } 
            }
            return result;
        }

        /// <returns>The full path of the 1st found .csproj file or null if nothing found.</returns>
        private string FindFirstCsprojFile(string path)
        {
            string result = null;
            var found = Directory.GetFiles(path, "*.csproj");
            if (found.Length > 0)
                result = found[0];
            return result;
        }

    }
}
