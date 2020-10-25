using IndentInfoLib;
using System;
using System.Collections.Generic;

namespace DependentConsoleApp
{
    class IndentInfoCommand
    {

        private void AddSectionContent(Sections sections)
        {
            sections.AddSection("file")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla");

            sections.AddSection("project")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla");

            sections.AddSection("extension")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla");

            sections.AddSection("beautiful-window")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla");

            sections.AddSection("analyze")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla");
        }

        private void AddLinesContent(Lines lines)
        {
            lines.AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                 .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                 .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla");
            lines.Print();
        }

        public void Sections()
        {
            Sections sections = new Sections();
            AddSectionContent(sections);
            sections.Print();

            Console.WriteLine("----------------------------");

            sections = new Sections(8);
            AddSectionContent(sections);
            sections.Print();
        } 

        public void Lines()
        {
            Lines lines = new Lines();
            AddLinesContent(lines);
            lines.Print();

            Console.WriteLine("----------------------------");

            lines = new Lines(4);
            AddLinesContent(lines);
            lines.Print();
        }
    }
}
