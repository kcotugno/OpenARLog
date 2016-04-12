# OpenARLog
Open source Amateur Radio logging Application for Windows.

[![Build status](https://ci.appveyor.com/api/projects/status/pby7cme7v0t98a96?svg=true)](https://ci.appveyor.com/project/kcotugno/openarlog)

Work is done in my spare time while attending school. Thus it is coming slowly but surely. I am
aiming for an alpha relase this summer. C# and the .Net Framework are new to me so the code will
be cleaned over time as I learn more. Feel free to send a Pull-Request if you wish to contribute.


OpenARLog: This project contains the front-end code, including the UI. So far, only a basic
non-functional interface has been implemented.

OpenARLog.ADIF: This project contains the library for importing and exporting from the ADIF file
format.

OpenARLog.Data: This project contains the library for handling the data internally and interfacing
with the SQLite database.

These are still in their early development. The UI is unusable, Data is only partially developed.
Work will be focused on finishing the back end before the UI will be completed.

#Dependencies
EntityFramework

System.Data.SQLite

Get both from VS NuGet Package Manager.

#Building
Just open the solution in Visual Studio, build and run.

#Branches

* master - Main development branch.
* ui-dev - UI development branch.



Kevin

W6KMC

Previously KI6LOQ
