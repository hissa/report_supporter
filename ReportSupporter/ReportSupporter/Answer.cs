using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportSupporter
{
    class Answer
    {
        public int Number { get; private set; }
        public string Text { get; private set; }
        public string ReportNumber { private get; set; }
        public string Script
        {
            get
            {
<<<<<<< HEAD
=======
                //return $"$(\"#q{ReportNumber}:{Number}_answer\").val(\"{Text}\");";
>>>>>>> eb70fd2d62c74d28a5b9f677e52b6673d7c4deb1
                return $"document.getElementById(\"q{ReportNumber}:{Number}_answer\").value=\"{Text}\";";
            }
        }

        public Answer(int number, string text, string reportNumber = "000000")
        {
            Number = number;
            Text = text;
            ReportNumber = reportNumber;
        }
    }
}
