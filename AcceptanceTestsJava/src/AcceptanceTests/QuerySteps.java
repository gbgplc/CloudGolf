package AcceptanceTests;

import com.thoughtworks.gauge.*;

import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Handler;
import java.util.logging.LogRecord;
import java.util.logging.Logger;

public class QuerySteps extends Handler {
    private String frauddata = null;

    private Logger logger() { Logger l = Logger.getLogger("querysteps"); l.setUseParentHandlers(false); return l; }
    private String logpath() {
        String r = System.getenv("GAUGE_PROJECT_ROOT");
        String d = System.getenv("logs_directory");
        return null == d ? null : (d.startsWith("/") || d.startsWith("\\")) ? d : r + File.separator + d;
    }
    private String logfile() {
        String d = logpath();
        String f = System.getenv("logfile");
        if (d != null && f != null) {
            return d + File.separator + f;
        }
        return null;
    }

    @BeforeSpec()
    public void Before() {
        logger().addHandler(this);
    }

    @Step("Given the service has no data.")
    public void GivenTheServicehashNoData() {
        frauddata = null;
    }

    @Step("Given known fraudsters are <csvfile>.")
    public void GivenKnownFraudstersAre(String csvfile)
    {
        // Populate 'static' file returned as known fraudster data
        logger().info("GivenKnownFraudstersAre:");
        logger().info(csvfile);
        frauddata = csvfile;
    }

    @Step("Having ingested messages <table>.")
    public void HavingIngestedMessages(Table table)
    {
        // Push messages from the table as JSON documents into service under test
        logger().info("HavingIngestedMessages:");
        List<String> cols = table.getColumnNames();
        List<String> docs = new ArrayList<>();
        boolean fst;
        for (TableRow tr: table.getTableRows())
        {
            String doc = "\t{";
            fst = true;
            for (String col: cols)
            {
                String val = tr.getCell(col).replace('\"', '\'');   // paranoia, shouldn't have any quotes but...
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
            docs.add(doc);
        }
        String body = "{\n\t\"messages\": [";
        fst = true;
        for (String doc: docs)
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
        logger().info(body);
    }

    @Step("Check audit records for dates between <start> and <end> match <table>")
    public void CheckAuditDateRange(String start, String end, Table results)
    {
        logger().info("CheckAuditDateRange");
        // TODO
    }

    @Step("Check audit records for actors in list <list> match <table>")
    public void CheckAuditActorIDs(String list, Table results)
    {
        logger().info("CheckAuditActorIDs");
        // TODO
    }

    @Step("Check audit records for messages in list <list> match <table>")
    public void CheckAuditMessageIDs(String list, Table results)
    {
        logger().info("CheckAuditMessageIDs");
        // TODO
    }

    @Step("Check audit records for actors with forename <forename> match <table>")
    public void CheckAuditActorForename(String forename, Table results)
    {
        logger().info("CheckAuditActorForename");
        // TODO
    }

    @Step("Check audit records for actors with forename <forename> and surname <surname> match <table>")
    public void CheckAuditActorForenameSurname(String forename, String surname, Table results)
    {
        logger().info("CheckAuditActorForenameSurname");
        // TODO
    }

    @Step("Check audit records for tags <tags> match <table>")
    public void CheckAuditTags(String tags, Table results)
    {
        logger().info("CheckAuditTags");
        // TODO
    }

    // Handler implementation
    public void publish(LogRecord record) {
        if ("true".equals(System.getenv("verbose")))
            System.err.println(record.getMessage());
        if (logfile() != null) {
            FileWriter w = null;
            try {
                new File(logpath()).mkdirs();
                w = new FileWriter(logfile(), true);
                w.write(record.getMessage() + "\n");
                w.close();
            } catch (IOException ex) {
                System.err.println("Unable to write to log file: " + logfile() + ": " + ex.getMessage());
            } finally {
                try {
                    if (w != null) w.close();
                } catch (IOException e) {}
            }
        }
    }
    public void close() {}
    public void flush() { System.err.flush(); }
}
