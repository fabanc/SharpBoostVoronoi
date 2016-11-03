# C++ Wrapper

Contains the CLR class called VoronoiWrapper that is used to call the C++ boost voronoi API and retrieve the results in data types that can be read by the .NET framework. The object being returned by this class being tuples
it is advised to use this library through the C# project SharpBoostVoronoi.


# Install

There is a very well done tutorial for building Boost from source on YouTube: https://www.youtube.com/watch?v=6trC5zVXzG0

In order to build from source for Boost 1.59.0:

1. Open the documentation: http://www.boost.org/doc/libs/1_59_0/more/getting_started/windows.html
2. Download Booost in the link in section "1. Get Boost". The link should be: http://www.boost.org/users/history/version_1_59_0.html
3. Then follow the instructions to build binary from source:
3.1. Open Visual Studio Command Prompt.
3.2. Run bootstrap.bat
3.3. Run b2 install --prefix=PREFIX where PREFIX is the directory where you want Boost.Build to be installed



