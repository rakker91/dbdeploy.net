@echo off
IF %processor_architecture% == AMD64 C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe database.targets /target:rebuild

IF %processor_architecture% == x86 C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe database.targets /target:rebuild

pause

