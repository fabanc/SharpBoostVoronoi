# SharpBoostVoronoi

## This .NET solution has 3 project

1. SharpBoostVoronoi is the library that exposes the C++ wrapper through a set of C# classes. If you want to use the boost voronoi library in .NET, this is the project you need to import.
2. SampleWPFApp are two applications that illustrate the use of SharpBoostVoronoi. SampleWPFApp allows you to visualize some test data and the results computed by the boost library.
3. SampleApp is an older example that demonstrates how to call the SharpBoosVoronoi library from a console application.


## IDisposable Interface

The C# wrapper implements the IDisposable interface. This ensures that the C++ ressource are released every time the wrapper BoostVoronoi is collected by the garbage collector. Because there is no way to completly
clear the C++ object between conscecutive analysis, it is not advised to reuse a object BoostVoronoi for multiple analysis. Consider destrying the object once you are done with 1 analysis.

```

            using (BoostVoronoi bv = new BoostVoronoi())
            {
				//Run your voronoi analysis
			}
			
```
