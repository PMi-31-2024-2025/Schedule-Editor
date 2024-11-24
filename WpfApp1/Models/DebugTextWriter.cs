using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class DebugTextWriter : System.IO.TextWriter
    {
        public override void WriteLine(string value)
        {
            Debug.WriteLine(value); // Відправляє дані в "Output"
        }

        public override void Write(char value)
        {
            Debug.Write(value);
        }

        public override Encoding Encoding => Encoding.Default;
    }

}
