# OpenARLog
Open source Amateur Radio logging Application for Windows.

[![Build status](https://ci.appveyor.com/api/projects/status/pby7cme7v0t98a96?svg=true)](https://ci.appveyor.com/project/kcotugno/openarlog)

# Overview

Alpha release comeing soon...

Designed to be a simplistic and powerful Amateur Radio logging application. Currently, there no plans to 
have advanced network features. It will just be a simple easy to use logger capable of ADIF export 
and import, portable database, simplified interface, and supporting all the necessary log data.


# Projects

OpenARLog: This project contains the front-end code, including the UI. So far, only a basic
non-functional interface has been implemented.

OpenARLog.ADIF: This project contains the library for importing and exporting from the ADIF file
format.

OpenARLog.Data: This project contains the library for handling the data internally and interfacing
with the SQLite database.

These are still in their early development. Both the UI and data management are partially
implemented.

#Dependencies

System.Data.SQLite

Get from VS NuGet Package Manager.

#Building
Just open the solution in Visual Studio, build and run.
