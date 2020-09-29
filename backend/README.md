# Corporate Carpooling as a Service 
This service allows the employees of a
company to organize their own car-pooling network, thus minimizing the parking
space required at the company premises, and reducing car emissions to the
environment. It allows users (i.e. employees) to register their place of living and
select the one of working (i.e. one of the locations of the offices or factories of the
company), offer their vehicle for car pooling, look for other users with suitable
commuting routes, and arrange for car pooling with them through a dedicated
web application front end. The service is to be designed and implemented under
the assumption that multiple companies can use it at the same time, allowing
them to customize the front end web application to their liking and to pre-define
company locations for which the service is available, while clearly isolating data
from different companies. Companies using the service will be charged on the
basis of the number of user requests to the service.

# Minimal Project Requirements
## Docker
You need to be able to present an application that runs on docker
engine. This includes your own application, but also all the 3rd party
applications you’re using. Everything should run in docker.
## Docker life cycle
You should be able to demonstrate and understand the
docker life cycle. That is, creating images, starting / stopping containers,
etc.
## Single Page Application
You should provide a single page application
(SPA) that interacts with the backend application. This should include
techniques like cascading style sheets (CSS), Ajax, server-side events,
futures / promises pattern, modular pieces of code, and a minimum level
of testing.
## Basic Fault tolerance
Whenever a part of the whole application ecosystem stops, the remaining parts of the application should keep functioning
properly (to the extent still possible).
## Back-end / database
Each project should at least use one NoSQL
database of your choice (e.g., Cassandra, MongoDB, Neo4J, OrientDB,
Gizzard, Riak, Redis, etc.)

# Project requirements for a grade ranging from 8 to 9
## Admin dashboard
Implement an admin dashboard that shows the
current state of the infrastructure (i.e., which containers (or pods) are
running or have problems). It should also show an event log that shows
the events happening in your infrastructure (containers going down, etc.).
## Fault tolerance
the application should be fully fault tolerant. Whenever
containers / applications get stopped, your infrastructure should keep
functioning and should restart those containers when needed (perhaps on
a different server). Some sort of service discovery will be necessary for this.
Note that despite this fault tolerance, your application should still be able
to run in the traditional docker infrastructure. I.e., you should still be able
to stop containers (and make sure they are not restarted automatically, in
order to demonstrate the fault tolerance of the application).
## Asynchronisity
the asynchronous nature of your application should be
clear and demonstrated.
## Databases
you should use at least two different types of NoSQL databases
(which should, of course, be fault tolerant).
## Orchestrator life cycle
You should be able to demonstrate your understanding of the life cycle of the orchestrator of your choice, if you have adopted one. This includes for example stopping/removing specific
containers, reconfiguring the orchestrator on the fly to cope with different
usage profiles, etc.
