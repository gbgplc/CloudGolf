# GBG Cloud Golf: The Specification

This document provides all necessary information* to take part in the GBG Cloud Golf challenge.

## What's that?

GBG Cloud Golf is an ongoing challenge to create and deploy a (micro)service that meets the specification
below, using selected 'cloud technologies' (eg: IaaS, PaaS, FaaS, etc.), in a limited time window, within
a (small) self-contained team.

## Why?

Good question: at a cloud strategy meeting in July 2017 the team identified a need to gain a better
understanding of both our in-house capabilities with cloud technologies (existing skills, champions
and learners) and the capabilities of the technologies themselves. These are both moving targets.

Experimenting with specific technologies through time-limited hackathon style workshops was suggested
as a good way for our cloud enthusiasts / experts to self-identify, and for us all to discover the good
and bad points of each selected technology. Since we are interested in comparing cloud technologies,
it makes sense to try and create the same thing each time, provided that is representative of the
sorts of services we would be creating for real. This is reminiscent of [code golf](https://codegolf.stackexchange.com/)
where real-world related programming puzzles are attempted by multiple players (developers), although
our outcomes may be different. Thus we are playing Cloud Golf :)

A need for ongoing / repeat hackathons was identified to keep pace with both changing technology and
changing people in GBG, Working in small local self-contained teams was suggested to allow the whole
company an opportunity to take part, on their terms and timescales. This also gives us the opportunity
to practice how to operate in a more distributed, scalable technology organisation, taking ownership
of service delivery ('parenting') and having appropriate conversations with providers and consumers of
a service (in this case the challenge organisers).

## How?

We are expecting teams to volunteer when they're ready to play, engaging with the challenge organisers
so everyone is aware of the team members, time window, and the cloud technology they are going to use
(selected from a common list, or an identified new technology which is added to the list).

A shared test environment is available to all teams, containing both provider and consumer services, able
to verify the new service against the specification below, *possibly including additional success criteria
as agreed between the team and the organisers (since the specification may be incomplete...).

## Functional specification

### High level description: real time fraud matching

The service is required to match two data sources, one provides 'known fraudsters' and is updated daily,
the other provides a real time stream of 'social media posts'. The service must match posts from known
fraudsters, retaining an audit record of these matches. The service must also re-match audit records
each time known fraudsters data is updated, creating new audit records for new matches and adding a tag
to any existing audit records that no longer match, or are re-matched. This tag must retain the version
of the known fraudsters data it was applied from.

The service must provide a query interface that permits searches of the audit record, using any of:
time ranges (up to 7 days ago), audit record IDs, tag type/versions, fraudsters details.

The service must provide statistics on stream throughput (averages, loss rates), matching (rates,
fields matched, etc.), changes in fraudster data (%, change intervals), service availability and query 
stats (for SLA reporting, see below).

### Low level consumer tests

* [Gauge](https://getgauge.io/) scripts to execute in test environment,
  both [AcceptanceTestsCSharp](AcceptanceTestsCSharp/AcceptanceTests.csproj) and
  [AcceptanceTestsJava](AcceptanceTestsJava/CloudGolf.iml)
* Data providers included within test environment, driven by above scripts.

### Provider interfaces

* Known fraudsters data will be made available via an HTTPS interface to a static file, thus it will
  support last-changed information for querying/caching purposes. The file will be in CSV format, the
  first line containing column header information. The file date stamp must be used as the version.
* Social media stream data will be provided by making HTTPS calls into a service endpoint. There
  will be no opportunity to flow control this data (it's a 'firehose'). Requests will contain one or
  more JSON formatted messages (as an array within a single JSON document). Requests will be numbered
  to facilitate statistics and reporting.

## Non-functional requirements (aka quality attributes)

### SLA

* The service must provide a mean availability level of 99.95%:
  [amusing downtime calculator](https://uptime.is/99.95),
  [definition of mean availability](http://www.weibull.com/hotwire/issue79/relbasics79.htm).
* In the event of a disaster^, a recovery time objective [RTO] of 4h (~ a year's worth of downtime),
  a recovery point objective [RPO] of under 10 seconds (preferably under 1 second):
  [definitions of RPO, RTO](https://www.druva.com/blog/understanding-rpo-and-rto/)
* Consumer queries of the audit record should return within 1 second, for 99.9% of queries.
* Social media stream loss should remain below 0.1%.
* Any query that takes >1 second, or social media stream loss >0.1% must be logged and provided in
  monthly SLA reports to consumers.

^ a disaster is defined as an unplanned total loss of the service from a consumers viewpoint.

### Scaling and payments

The service is expected to process social media stream rates up to 10,000 messages/sec, up to 10Mbit/sec
raw traffic rate.

Service consumers pay for queries made that meet SLA terms (<1 second response time), and for total
social media traffic that is successfully ingested (lost traffic is not chargeable).

Optimising business value by minimising service creation & operating cost, is your challenge. Have fun :)
