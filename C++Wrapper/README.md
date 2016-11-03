# C++ Wrapper

Contains the CLR class called VoronoiWrapper that is used to call the C++ boost voronoi API and retrieve the results in data types that can be read by the .NET framework. The object being returned by this class being tuples
it is advised to use this library through the C# project SharpBoostVoronoi.


# Install

There is a very well done tutorial for building Boost using Visual Studio from source on YouTube: https://www.youtube.com/watch?v=6trC5zVXzG0

In order to build from source for Boost 1.59.0 using the same installation than I did. I am running Windows 7 (64 bits), and I am compiling using Visual Studio 2013. Deploying and compiling Boost
in the same folder than I did should allow to compile this project without changing the confguration of the C++ wrapper.

##Common steps for 32 bits and 64 bits OS

1. Open the documentation: http://www.boost.org/doc/libs/1_59_0/more/getting_started/windows.html
2. Download Boost version 1_59_0 on the page http://www.boost.org/users/history/version_1_59_0.html
3. Create a folder C:\Boost\1.59.0\VC11_x32. This folder will be used to compile for 32 bits

##Steps for 32 bits
4. Create a folder C:\Boost\1.59.0\VC11_x32. 
5. Unzip boost_1_59_0.zip into a folder C:\Boost\1.59.0\VC11_x32. After unzipping you should find the file b2.exe right under C:\Boost\1.59.0\VC11_x32.
6. Open Visual Studio Command Prompt.
7. Run bootstrap.bat
8. Run b2 install the tool b2 to compile. The command line is: b2 toolset=msvc address-model=64 --build-type=complete stage


##Steps for 64 bits
4. Create a folder C:\Boost\1.59.0\VC11_x64. 
5. Unzip boost_1_59_0.zip into a folder C:\Boost\1.59.0\VC11_x64. After unzipping you should find the file b2.exe right under C:\Boost\1.59.0\VC11_x64.
6. Open Visual Studio Command Prompt.
7. Run bootstrap.bat
8. Run b2 install the tool b2 to compile. The command line is: b2 toolset=msvc address-model=64 --build-type=complete stage



