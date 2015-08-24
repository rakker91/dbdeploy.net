To use the examples, you'll need to build the solution or place the binaries into a bin\debug directory underneath the directory
where the samples are installed.

The rebuild.bat file assumes that you have a database server installed locally and will rebuild, from scratch, a database on the local machine.  The intent of
this is that in a build environment (Test, Dev, etc) a developer or tester can instantly spin up a clean copy of the database from scratch.
Note that the rebuild.bat file is simply calling msbuild database.targets /target:rebuild

The database.targets file contains all of the magic to make this happen.

For a manual deployment, simply run databasedeploy.exe after you have modified databasedeploy.exe.config to have the appropriate values.

The rebuild uses the MSBuild extensions pack (http://msbuildextensionpack.codeplex.com/) which is installed as a NuGet Package.
