using System;

namespace LowLevelDesign.WinTrace
{
    interface ITraceOutput
    {
        void Write(double timeStampRelativeInMSec, int processId, int threadId, string eventName, string details);

        void WriteSummary(string title, string eventsSummary);
    }

    class WTraceOutput : ITraceOutput
    {
        private readonly string eventNameFilter;
        private readonly string outputFilename;
        private System.IO.StreamWriter fh = null;

        public WTraceOutput(string eventNameFilter, string outputFilename)
        {
            this.eventNameFilter = eventNameFilter;
            this.outputFilename = outputFilename;

            if (this.outputFilename != null) {
                this.fh = new System.IO.StreamWriter(outputFilename);
            }
        }

        public void Write(double timeStampRelativeInMSec, int processId, int threadId, string eventName, string details)
        {
            if (eventNameFilter == null || 
                eventName.IndexOf(eventNameFilter, StringComparison.OrdinalIgnoreCase) >= 0) {
                var msg = $"{timeStampRelativeInMSec:0.0000} ({processId}.{threadId}) {eventName} {details}";
                if (this.fh != null)
                {
                    this.fh.WriteLine(msg);
                }
                else {
                    Console.WriteLine(msg);
                }
            }
        }

        public void WriteSummary(string title, string eventsSummary)
        {
            var separator = "--------------------------------";
            var newLine = "\r\n";
            var space = Math.Max(0, (separator.Length - title.Length) / 2);

            var msg = new System.Text.StringBuilder();
            msg.Append(newLine);
            msg.Append(separator + newLine);
            msg.Append("".PadRight(space) + newLine);
            msg.Append(title + newLine);
            msg.Append(separator + newLine);
            msg.Append(eventsSummary + newLine);

            if (this.fh != null)
            {
                this.fh.WriteLine(msg);
            }
            else {
                Console.WriteLine(msg);
            }

            Console.WriteLine();
            Console.WriteLine(separator);
            Console.Write("".PadRight(space));
            Console.WriteLine(title);
            Console.WriteLine("--------------------------------");
            Console.WriteLine(eventsSummary);
        }
    }
}
