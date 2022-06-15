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

        public string GetLineAtOffset(int offset)
        {
            return this.lines[lineNumber + offset];
        }

        public int GetLineCount()
        {
            return this.lines.Count;
        }

        public int GetLineNumber()
        {
            return lineNumber;
        }

        public string GetText()
        {
            return string.Join("\r\n", this.lines);
        }

        public void AddPrefixBetweenLinesThatContain(string prefix, string startThatContains, string endThatContains)
        {
            var startLineNumber = this.lineNumber;

            this.MoveToStart();
            this.NextThatContains(startThatContains);
            this.Next();

            while (this.lineNumber < lines.Count() && !this.lines[lineNumber].Contains(endThatContains))
            {
                if (this.lines[lineNumber].Trim().Count() > 0)
                {
                    this.lines[lineNumber] = prefix + this.lines[lineNumber];
                }
                this.lineNumber++;
            }

            this.lineNumber = startLineNumber;
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

        public StringEditor InsertIntoLine(string text)
        {
            this.SetLine(this.GetLine() + text);

            return this;
        }

        public void MoveToEnd()
        {
            this.lineNumber = this.lines.Count - 1;
        }

        public void MoveToStart()
        {
            this.lineNumber = 0;
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

        public StringEditor PrevThatContains(params string[] patterns)
        {
            this.Prev((line) => patterns.Any(pattern => line.Contains(pattern)));

            return this;
        }

        public bool ContainsInTheNextNLines(string pattern, int n)
        {
            for (int i = 1; i <= 10 && lineNumber + i < this.lines.Count; i++)
            {
                if (this.lines[lineNumber + i].Contains(pattern))
                {
                    return true;
                }
            }

            return false;
        }

        public void SetLine(string line)
        {
            this.lines[lineNumber] = line;
        }
    }
}