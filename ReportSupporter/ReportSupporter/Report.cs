﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace ReportSupporter
{
    class Report
    {
        private string _ReportNumber;
        public string ReportNumber {
            get { return _ReportNumber; }
            set
            {
                _ReportNumber = value;
                NumberChanged();
            }
        }
        public ObservableCollection<Answer> Answers { get; set; }
        public string Script
        {
            get
            {
                var script = "";
                script +=
$@"var getFirst = ()=>{{
var inputs = document.getElementsByTagName(""input"");
for(var i = 0; i < inputs.length; i++){{
if(/q\d{{6}}:1_answer/.test(inputs[i].id)) return inputs[i];
}};
}};
var getNumber = (first)=>{{
return /\d{{6}}/.exec(first.id)[0];
}};
var rNum = getNumber(getFirst());
";
                foreach (var item in Answers)
                {
                    script += item.Script + "\n";
                }
                return script;
            }
        }

        public Report()
        {
            Answers = new ObservableCollection<Answer>();
        }

        private void NumberChanged()
        {
            foreach (var item in Answers)
            {
                item.ReportNumber = ReportNumber;
            }
        }

        public void LoadFile(string path)
        {
            Answers = new ObservableCollection<Answer>();
            using (var parser = new TextFieldParser(path, Encoding.UTF8))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.HasFieldsEnclosedInQuotes = false;
                parser.TrimWhiteSpace = false;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] row = parser.ReadFields();
                    Answers.Add(new Answer(int.Parse(row[0]), row[1], ReportNumber));
                }
            }
        }
    }
}
