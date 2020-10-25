using System;
using System.Collections.Generic;
using System.Linq;

namespace IndentInfoLib
{
    public class Sections
    {
        private Dictionary<string, Lines> dict = new Dictionary<string, Lines>();
        private string generalIndentSpaces;

        public Sections(int indentNumber = 0)
        {
            if (indentNumber != 0)
                generalIndentSpaces = " ".PadLeft(indentNumber);
            else
                generalIndentSpaces = "";
        }

        public Lines AddSection(string name)
        {
            var lines = new Lines();
            this.dict.Add(name, lines);
            return lines;
        }

        public void Print()
        {
            const int spaceNumber = 3;
            var indentNumber = dict.Keys.Select(k => k.Length).Max() + spaceNumber;
            string indentSpaces = " ".PadLeft(indentNumber);

            foreach(var section in dict)
            {
                int counter = 0;
                foreach(var line in section.Value.LineList)
                {
                    if(counter++ == 0)
                    {
                        string header = section.Key.PadRight(indentNumber);
                        Console.WriteLine($"{generalIndentSpaces}{header}{line}");
                    }
                    else
                    {
                        Console.WriteLine($"{generalIndentSpaces}{indentSpaces}{line}");
                    }
                }
            }
        }
    }
}
