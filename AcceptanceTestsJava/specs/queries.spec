# Happy Path Query Tests

Created by PAshby on 9/14/2017 11:20:28 AM.

Always start with a fresh system:

* Given the service has no data.
* Given known fraudsters are <file:Env/fraud2017-09-01.csv>.
* Having ingested messages <table:Env/messages1.csv>.
* Given known fraudsters are <file:Env/fraud2017-09-02.csv>.
* Having ingested messages <table:Env/messages2.csv>.

## Date Range

Date ranges apply to social message timestamps, as indivdual known fraudsters are not dated (but see tag queries below)

* Check audit records for dates between "2017-09-02T22:00:00" and "2017-09-02:23:00:00" match
    |MessageID|ActorID|Sender          |Recipients       |Timestamp          |Tags             |MessageText|
    |1002     |1      |fredthephish    |philiscool/jimbob|2017-09-02T22:07:12|                 |Sure thing Phil. BTW have you seen this: http://badsite.do.not.click/drive-by-hacks.vbs|
    |1005     |2      |pamelathespamela|philiscool       |2017-09-02T22:30:21|Removed2017-09-02|I thought you liked junk mail Phil *weg*|

## Actor ID Range

Assuming the caller has prior knowledge of actor IDs (likely as they supply them)

* Check audit records for actors in list "2,4" match
    |MessageID|ActorID|Sender          |Recipients       |Timestamp          |Tags             |MessageText|
    |1005     |2      |pamelathespamela|philiscool       |2017-09-02T22:30:21|Removed2017-09-02|I thought you liked junk mail Phil *weg*|
    |1009     |4      |droptables      |jimbob           |2017-09-02T23:49:00|                 |Eat this sucker! *Boom* `;DROP TABLE USERS;|

## Message ID Range

Assuming the caller has prior knowledge of message IDs (likely as they supply them)

* Check audit records for messages in list "1010..1020" match
    |MessageID|ActorID|Sender          |Recipients       |Timestamp          |Tags             |MessageText|
    |1013     |5      |badboyjim       |*broadcast*      |2017-09-03T08:59:59|                 |Early bird offers (for cheap loans)!|
    |1014     |1      |freddy          |killbill99       |2017-09-03T17:30:00|                 |Woot! I'm in :)|
    |1015     |0      |killbill99      |freddy           |2017-09-03T17:30:10|                 |Smooth dude now try and stay there...|

## Actor Details

Caller is trawling for names..

* Check audit records for actors with forename "Bobby" match
    |MessageID|ActorID|Sender          |Recipients       |Timestamp          |Tags             |MessageText|
    |1009     |4      |droptables      |jimbob           |2017-09-02T23:49:00|                 |Eat this sucker! *Boom* `;DROP TABLE USERS;|
    |1016     |4      |nullboy         |jimbob           |2017-09-03T22:15:38|                 |Suck it down Jimbo.. `;exec 'cat /dev/null > /etc/passwd';|

* Check audit records for actors with forename "Bobby" and surname "Drop*" match
    |MessageID|ActorID|Sender          |Recipients       |Timestamp          |Tags             |MessageText|
    |1016     |4      |nullboy         |jimbob           |2017-09-03T22:15:38|                 |Suck it down Jimbo.. `;exec 'cat /dev/null > /etc/passwd';|

* Check audit records for actors with forename "Billy'bob" match
    |MessageID|ActorID|Sender          |Recipients       |Timestamp          |Tags             |MessageText|
    |1015     |0      |killbill99      |freddy           |2017-09-03T17:30:10|                 |Smooth dude now try and stay there...|

## Tags

Caller is looking for previous records from fraudsters removed in a specific document revision

* Check audit records for tags "Removed2017-09-02" match
    |MessageID|ActorID|Sender          |Recipients       |Timestamp          |Tags             |MessageText|
    |1005     |2      |pamelathespamela|philiscool       |2017-09-02T22:30:21|Removed2017-09-02|I thought you liked junk mail Phil *weg*|
