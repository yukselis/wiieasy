using Microsoft.VisualBasic;
using System;
using System.Text;
using System.Web;
using Arwend;

namespace Arwend.Web.Application.Monitoring
{
    public class PerformanceMonitor
    {
        private bool bEnabled = false;
        private string sFileDirectory = "/ApplicationLogs/PerformanceMonitor/";
        private StringBuilder ContentBuilder = new StringBuilder();
        private DateTime LastLogTime;
        private int ResetCount = 0;
        private void AddHeader()
        {
            this.ContentBuilder.Append(Strings.Chr(34) + "ControlName" + Strings.Chr(34));
            this.ContentBuilder.Append(";");
            this.ContentBuilder.Append(Strings.Chr(34) + "FunctionName" + Strings.Chr(34));
            this.ContentBuilder.Append(";");
            this.ContentBuilder.Append(Strings.Chr(34) + "Time" + Strings.Chr(34));
            this.ContentBuilder.Append(";");
            this.ContentBuilder.Append(Strings.Chr(34) + "LastTimeDifference" + Strings.Chr(34));
            this.ContentBuilder.Append(";");
            this.ContentBuilder.AppendLine();
        }
        private void AddContent(string controlName, string functionName)
        {
            this.ContentBuilder.Append(Strings.Chr(34) + controlName + Strings.Chr(34));
            this.ContentBuilder.Append(";");
            this.ContentBuilder.Append(Strings.Chr(34) + functionName + Strings.Chr(34));
            this.ContentBuilder.Append(";");
            this.ContentBuilder.Append(Strings.Chr(34) + DateAndTime.Now.Subtract(DateTime.MinValue).TotalMilliseconds.ToString() + Strings.Chr(34));
            this.ContentBuilder.Append(";");
            this.ContentBuilder.Append(Strings.Chr(34) + DateAndTime.Now.Subtract(LastLogTime).TotalMilliseconds.ToString() + Strings.Chr(34));
            this.ContentBuilder.Append(";");
            this.ContentBuilder.AppendLine();
            LastLogTime = DateAndTime.Now;
        }
        public void Start()
        {
            this.AddHeader();
            this.AddContent("PerformanceMonitor", "Start");
        }
        public void Reset()
        {
            this.Close();
            this.ResetCount = this.ResetCount + 1;
            this.Start();
        }
        public void Close()
        {
            this.FileDirectory = HttpContext.Current.Server.MapPath(".") + this.FileDirectory;
            this.AddContent("PerformanceMonitor", "Close");
            if (!System.IO.Directory.Exists(this.FileDirectory))
            {
                System.IO.Directory.CreateDirectory(this.FileDirectory);
            }
            System.IO.StreamWriter oFile = new System.IO.StreamWriter(FileName, false, System.Text.Encoding.GetEncoding(1254));
            oFile.Write(ContentBuilder);
            oFile.Close();
            oFile.Dispose();
            oFile = null;
            ContentBuilder = null;
        }
        public bool Enabled
        {
            get { return this.bEnabled; }
            set { this.bEnabled = value; }
        }
        public string FileDirectory
        {
            get { return this.sFileDirectory; }
            set { this.sFileDirectory = value; }
        }
        public string FileName
        {
            get
            {
                if (this.ResetCount == 0)
                {
                    return DateTime.Now.AsFileName(FileDirectory) + ".csv";
                }
                return DateTime.Now.AsFileName(FileDirectory) + "_" + this.ResetCount + ".csv";
            }
        }
        public void Log(string identifier, string Description)
        {
            this.AddContent(identifier, Description);
        }
    }
}

