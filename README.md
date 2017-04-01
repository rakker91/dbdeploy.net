# dbdeploy.net
A NuGet package is now available at http://nuget.org/packages/DbDeployNet2/

For usage instructions, please see the /docs folder.  There's a help file and some more detailed instructions on usage.

One of the major challenges that many products face is around versioning of databases between development and production. DbDeploy was developed to meet this need (see http://code.google.com/p/dbdeploy/). 

In 2007, DbDeploy.Net was ported from the DbDeploy version 2.0. (see http://sourceforge.net/projects/dbdeploy-net/). The project appears to have seen little changes since then.

Recently, we wanted to use DbDeploy against Microsoft's Sql Azure platform and discovered that it simply doesn't work.

We investigated what it'd take to update DbDeploy.Net to a version that would support it and because of the code base, we decided that rewriting would be a better strategy.

This version is a complete rewrite from the ground up. While many of the concepts and ideas are similar, we've added the following:
Full Unit Testing with NUnit 
Support for Azure (and easy maintenance of the Sql used in the application 
Similar functionality to version 3.0 of DbDeploy (elimination of ChangeSets, simplification of the changelog table) 
Stylecop Compliance with the default StyleCop rules 
Dependency injection and service architecture 
Better MS Build integration and easier command line integration 
Sandcastle Help Documentation

We plan on keeping this project active and making changes as needed based on suggestions made by you. Some things that we plan on adding in the near future:
Support for database servers other than MsSql

Currently, this version is compatible with scripts made for other version of DbDeploy, but only Microsoft Sql Server is supported at this time! If you'd like to help us out with other DBMS', please drop us a line.

We appreciate your feedback.
