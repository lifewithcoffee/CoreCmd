using System;
using System.Collections.Generic;

namespace IndentInfoLib
{
    public class Lines
    {
        public List<string> LineList { get; } = new List<string>();

        private string generalIndentSpaces;

        public Lines(int indentNumber = 0)
        {
            if (indentNumber != 0)
                generalIndentSpaces = " ".PadLeft(indentNumber);
            else
                generalIndentSpaces = "";
        }

        public Lines AddLine(string line)
        {
            LineList.Add(line);
            return this;
        }

        public void Print()
        {
            foreach(var line in LineList)
            {
                Console.WriteLine($"{generalIndentSpaces}{line}");
            }
        }
    }
}
