using System;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core.Helpers
{
    internal class StringEditor
    {
        private int lineNumber;
        private readonly List<string> lines;

        public StringEditor(string text)
        {
            this.lines = text.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None).ToList();
            lineNumber = 0;
        }

        public string GetLine()
        {
            return this.lines[lineNumber];
        }

        public int GetLineNumber()
        {
            return lineNumber;
        }

        public string GetText()
        {
            return string.Join("\r\n", this.lines);
        }

        public StringEditor InsertLine(string line)
        {
            var splittedLines = line.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None);

            foreach (var splittedLine in splittedLines)
            {
                this.lines.Insert(this.lineNumber, splittedLine);
                this.lineNumber++;
            }

            return this;
        }

        public StringEditor InsertNewLine()
        {
            this.lines.Insert(this.lineNumber, "");
            this.lineNumber++;

            return this;
        }

        public void MoveToEnd()
        {
            this.lineNumber = this.lines.Count - 1;
        }

        public StringEditor Next(Predicate<string> predicate)
        {
            do
            {
                this.lineNumber++;
            } while (this.lineNumber < lines.Count() && !predicate(this.lines[lineNumber]));

            return this;
        }

        public StringEditor Next()
        {
            if (this.lineNumber < lines.Count())
            {
                this.lineNumber++;
            }

            return this;
        }

        public StringEditor NextThatContains(string pattern)
        {
            this.Next((line) => line.Contains(pattern));

            return this;
        }

        public StringEditor NextThatStartsWith(string pattern)
        {
            this.Next((line) => line.StartsWith(pattern));

            return this;
        }

        public StringEditor Prev(Predicate<string> predicate)
        {
            do
            {
                this.lineNumber--;
            } while (this.lineNumber > 0 && !predicate(this.lines[lineNumber]));

            return this;
        }

        public StringEditor Prev()
        {
            if (this.lineNumber > 0)
            {
                this.lineNumber--;
            }

            return this;
        }

        public StringEditor PrevThatContains(string pattern)
        {
            this.Prev((line) => line.Contains(pattern));

            return this;
        }
    }
}