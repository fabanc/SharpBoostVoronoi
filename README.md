# SharpBoostVoronoi

##This project has 2 goals.

1. Show how to wrap boost voronoi library using CLR. It will enable users to generate voronoi cells from line and segments in .NET.
2. Provide a C# wrapper around the CLR code to make the library easy to use in C#.

##Links:
 
1. Boost library: http://www.boost.org/
2. Step-by-step install guide for Boost with Visual Studio. https://www.youtube.com/watch?v=6trC5zVXzG0
3. Boost documentation about the voronoi API: http://www.boost.org/doc/libs/1_59_0/libs/polygon/doc/voronoi_diagram.htm

##Project status
		
The compiled C++ wrapper is provided with SharpBoostVoronoi (folder lib). If you want to compile and modify the C++ wrapper, you will need to install the boost library (cf link 2).
The code is still at its early phase of developement. The C# Wrapper is not implemented yet, but the test units show you how to call the CLR wrapper. My next steps are to implement a voronoi cell data structure in the CLR wrapper before implementing a C# wrapper.

##External links
1. A very useful link about the wrapping Boost with CLR and C# here: https://grevit.wordpress.com/2015/07/03/boost-c-library-in-c-revit-api/	
2. A similar project to SharpBoostVoronoi in python. It exposes very basic functionalities, but it does it very, very well: https://github.com/Voxel8/pyvoronoi

