/******************************************************************
* Licensed Materials - Property of HCL
* (c) Copyright HCL Technologies Ltd. 2015, 2016.
* Note to U.S. Government Users Restricted Rights.
******************************************************************/
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using AppScan.Utilities;
using Microsoft.Win32;

namespace BurpTrafficImporter
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Environment.CurrentDirectory =
                    (string) Registry.LocalMachine.OpenSubKey(@"SOFTWARE\IBM\AppScan Standard\").GetValue("AppScanDir");
                Directory.SetCurrentDirectory(Environment.CurrentDirectory);
            }
            catch
            { //
            }

            if (args.Length == 0)
            {
                PrintHelp();
                return;
            }
            string sourceFile = args[0];
            string targetFile = args.Length > 1 ? args[1] : null;
            string charsetOverride = args.Length > 2 ? args[2] : "UTF8";
            string htmlEncodingOverride = args.Length > 3 ? args[3] : null;

            BurpToExd b2e = new BurpToExd(charsetOverride, htmlEncodingOverride);

            if (targetFile == null)
            {
                b2e.Convert(sourceFile, new ConsoleOutStream());
            }
            else
            {
                b2e.Convert(sourceFile, targetFile);
            }
        }

        private static void PrintHelp()
        {
            Console.Out.WriteLine("BurpTrafficImporter by IBM(c)");
            Console.Out.WriteLine("Usage: BurpTrafficImporter.exe <source_file> {<target_file>} ");
            Console.Out.WriteLine("   Required:");
            Console.Out.WriteLine("       source_file            - full path to the source Burp traffic file.");
            Console.Out.WriteLine("   Optional:");
            Console.Out.WriteLine("       target_file            - full path to the EXD format output location.");
            Console.Out.WriteLine("   Exmaples:");
            Console.Out.WriteLine("       BurpTrafficImporter.exe \"c:\\My Traffic Files\\traffic.xml\"");
            Console.Out.WriteLine("       BurpTrafficImporter.exe \"c:\\My Traffic Files\\traffic.xml\" \"c:\\My Traffic Files\\traffic.exd\"");
        }
    }

    class ConsoleOutStream : Stream
    {
        public override void Flush()
        {
            Console.Out.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Console.Out.Write(Encoding.Unicode.GetString(buffer,0,count));
        }

        public override bool CanRead
        {
            get { return false;
            }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position { get; set; }
    }
}
