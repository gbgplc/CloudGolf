using Gauge.CSharp.Lib;
using Gauge.CSharp.Lib.Attribute;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;

namespace AcceptanceTests
{
    class QuerySteps : TraceListener
    {
        private string frauddata;
        private bool verbose
        {
            get { return "true".Equals(Environment.GetEnvironmentVariable("verbose")); }
        }
        private string logpath
        {
            get {
                string root = Environment.GetEnvironmentVariable("GAUGE_PROJECT_ROOT");
                string path = Environment.GetEnvironmentVariable("logs_directory");
                return string.IsNullOrEmpty(path) ? null :
                    (path.StartsWith("/") || path.StartsWith("\\")) ? path : root + Path.DirectorySeparatorChar + path;
            }
        }
        private string logfile
        {
            get {
                string file = Environment.GetEnvironmentVariable("logfile");
                if (logpath != null && !string.IsNullOrEmpty(file))
                    return logpath + Path.DirectorySeparatorChar + file;
                return null;
            }
        }

        [BeforeSpec()]
        public void Before()
        {
            Trace.Listeners.Add(this);
        }

        [AfterStep()]
        public void After()
        {
            Trace.Flush();
        }

        [Step("Given the service has no data.")]
        public void GivenTheServiceHasNoData()
        {
            // Special call to clear all storage from the service under test, we also nuke known fraudsters data
            Trace.WriteLine("GivenTheServiceHasNoData");
            frauddata = null;
        }

        [Step("Given known fraudsters are <csvfile>.")]
        public void GivenKnownFraudstersAre(string csvfile)
        {
            // Populate 'static' file returned as known fraudster data
            Trace.WriteLine("GivenKnownFraudstersAre:");
            Trace.WriteLine(csvfile);
            frauddata = csvfile;
        }

        [Step("Having ingested messages <table>.")]
        public void HavingIngestedMessages(Table table)
        {
            // Push messages from the table as JSON documents into service under test
            Trace.WriteLine("HavingIngestedMessages:");
            List<string> cols = table.GetColumnNames();
            List<string> docs = new List<string>();
            bool fst;
            foreach (TableRow tr in table.GetTableRows())
            {
                string doc = "\t{";
                fst = true;
                foreach(string col in cols)
                {
                    string val = tr.GetCell(col).Replace('\"', '\'');   // paranoia, shouldn't have any quotes but...
                    if (fst)
                    {
                        doc = doc + "\n\t\t";
                        fst = false;
                    }
                    else
                    {
                        doc = doc + ",\n\t\t";
                    }
                    doc = doc + "\"" + col + "\": \"" + val + "\"";
                }
                doc = doc + "\n\t}";
                docs.Add(doc);
            }
            string body = "{\n\t\"messages\": [";
            fst = true;
            foreach (string doc in docs)
            {
                if (fst)
                {
                    body = body + "\n";
                    fst = false;
                }
                else
                {
                    body = body + ",\n";
                }
                body = body + doc;
            }
            body = body + "\t]\n}\n";
            // TODO: send to service via HTTPS!
            Trace.WriteLine(body);
        }

        [Step("Check audit records for dates between <start> and <end> match <table>")]
        public void CheckAuditDateRange(string start, string end, Table results)
        {
            Trace.WriteLine("CheckAuditDateRange");
            // TODO
        }

        [Step("Check audit records for actors in list <list> match <table>")]
        public void CheckAuditActorIDs(string list, Table results)
        {
            Trace.WriteLine("CheckAuditActorIDs");
            // TODO
        }

        [Step("Check audit records for messages in list <list> match <table>")]
        public void CheckAuditMessageIDs(string list, Table results)
        {
            Trace.WriteLine("CheckAuditMessageIDs");
            // TODO
        }

        [Step("Check audit records for actors with forename <forename> match <table>")]
        public void CheckAuditActorForename(string forename, Table results)
        {
            Trace.WriteLine("CheckAuditActorForename");
            // TODO
        }

        [Step("Check audit records for actors with forename <forename> and surname <surname> match <table>")]
        public void CheckAuditActorForenameSurname(string forename, string surname, Table results)
        {
            Trace.WriteLine("CheckAuditActorForenameSurname");
            // TODO
        }

        [Step("Check audit records for tags <tags> match <table>")]
        public void CheckAuditTags(string tags, Table results)
        {
            Trace.WriteLine("CheckAuditTags");
            // TODO
        }

        public override void Write(string message)
        {
            if (verbose)
                Console.Write(message);
            WriteLog(message);
        }

        public override void WriteLine(string message)
        {
            if (verbose)
                Console.WriteLine(message);
            WriteLog(message);
        }

        private void WriteLog(string message)
        {
            if (logfile != null)
            {
                Directory.CreateDirectory(logpath);
                using (StreamWriter wr = new StreamWriter(logfile, true))
                {
                    wr.WriteLine(DateTime.UtcNow.ToString("o") + ": " + message.Replace('\n',' ').Replace('\r',' ').Replace('\t',' '));
                }
            }
        }
    }
}
