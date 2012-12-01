# FSI as a service #
This fork has the latest compiler drop from http://fsharppowerpack.codeplex.com, FSharp.Compiler.dll is modified to include F# as a compiler service for normal F# projects. It also includes the modified FSharp.Core.dll that is required.

The modified dlls are placed in the bin directory and there is a sample showcasing the use of compiler services in the samples folder. This is a starter to have FSI like services inside an app. It can do three things now,

- Compile code to DLL
- Compile code to on-the-fly assembly and evaluate as an EXE
	- Note: This more requires a "script.fsx" to generate the main module for the script evaluation.
- Host a FSI session

#### Building the compiler tools manually

- cd src
- msbuild fsharp-proto-build.proj /p:TargetFramework=cli\4.0
- ngen install ..\Proto\cli\4.0\bin\fsc-proto.exe
- msbuild fsharp-library-build.proj  /p:TargetFramework=cli\4.0 /p:Configuration=Release
- msbuild fsharp-compiler-build.proj  /p:TargetFramework=cli\4.0 /p:Configuration=Release