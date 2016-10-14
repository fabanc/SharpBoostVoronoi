# SharpBoostVoronoi

##This repository has 2 projects.

1. A CLR wrapper around the boost voronoi API. It will enable users to generate voronoi cells from line and segments in .NET. The content of this project is in the directory C++ wrapper.
2. A .NET wrapper around the CLR code to make the library easy to use in C#. The content of this project is in the directory SharpBoostVoronoi. SharpBoostVoronoi references a compiled version of the CLR wrapper in its subfolder lib. 

##Links:
 
1. Boost library: http://www.boost.org/
2. Step-by-step install guide for Boost with Visual Studio. https://www.youtube.com/watch?v=6trC5zVXzG0
3. Boost documentation about the voronoi API: http://www.boost.org/doc/libs/1_59_0/libs/polygon/doc/voronoi_diagram.htm

##Project status
The project basic unit testing and demos. My plans for the next weeks are going to develop more unit tests and example, and then do some performance improvement in the C++ library.

##How to use
The C++ wrapper is included in the lib folder of SharpBoostVoronoi. If you update the C++ wrapper, paste the resulting DLL into the lib folder of SharpBoostVoronoi.
SharpBoostVoronoi is the C# library to use for your C# projects. See the documentation of this project for code samples.


##External links
1. A very useful link about the wrapping Boost with CLR and C# here: https://grevit.wordpress.com/2015/07/03/boost-c-library-in-c-revit-api/	
2. A similar project to SharpBoostVoronoi in python: https://github.com/Voxel8/pyvoronoi

##Limitations
1. As much as the API works well with input dataset less than 450,000 segments, we seem to be hitting memory issues beyond that. See the PerformanceTesting
projects in SharpBoostVoronoi and its read me to get more information about that.


