# SharpBoostVoronoi

This project has 2 goals.
	1. Demonstrate how to call the boost voronoi library.
	2. Provides a C# that provided classes and function that make the boost library easier to use.

Links: 
	1. Boost library: http://www.boost.org/
	2. Step-by-step install guide for Boost with Visual Studio. https://www.youtube.com/watch?v=6trC5zVXzG0
	3. Boost documentation about the voronoi API.
		
The compiled C++ wrapper is provided with SharpBoostVoronoi (folder lib). If you want to compile and modify the C++ wrapper, you will need to install the boost library. Not being an expert in C++, I used the following video: https://www.youtube.com/watch?v=6trC5zVXzG0
The code is still at its early phase of developement. The C# Wrapper is not implemented yet, but the test unit show you how to call the CLR wrapper. My next steps is to implement a voronoi cell data structure in the CLR wrapper before implementing a C# wrapper.


In terms of grasping the concept of wrapping C++ with C#, this link has been really helpful: https://grevit.wordpress.com/2015/07/03/boost-c-library-in-c-revit-api/	
