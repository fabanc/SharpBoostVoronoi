// CSharpBootVoronoi.h

#pragma once
#include<boost/polygon/voronoi.hpp>
#include <map>

using namespace System;
using namespace System::Collections::Generic;

using boost::polygon::voronoi_builder;
using boost::polygon::voronoi_diagram;
using boost::polygon::voronoi_builder;
using boost::polygon::voronoi_diagram;
using boost::polygon::x;
using boost::polygon::y;
using boost::polygon::low;
using boost::polygon::high;


/*Input struct*/
struct Point {
	double a;
	double b;
	Point(double x, double y) : a(x), b(y) {}
};

struct Segment {
	Point p0;
	Point p1;
	Segment(double x1, double y1, double x2, double y2) : p0(x1, y1), p1(x2, y2) {}
};



namespace boost {
	namespace polygon {
		template <>
		struct geometry_concept<Point> {
			typedef point_concept type;
		};

		template <>
		struct point_traits<Point> {
			typedef int coordinate_type;

			static inline coordinate_type get(
				const Point& point, orientation_2d orient) {
				return (orient == HORIZONTAL) ? point.a : point.b;
			}
		};

		template <>
		struct geometry_concept<Segment> {
			typedef segment_concept type;
		};

		template <>
		struct segment_traits<Segment> {
			typedef int coordinate_type;
			typedef Point point_type;

			static inline point_type get(const Segment& segment, direction_1d dir) {
				return dir.to_int() ? segment.p1 : segment.p0;
			}
		};
	}  // polygon




	public class Voronoi
	{
	public:
		std::vector<Point> points;
		std::vector<Segment> segments;
		voronoi_diagram<double> vd;

		//Output data storage
		std::map<const voronoi_diagram<double>::vertex_type*, long long> vertexMap;
		std::map<const voronoi_diagram<double>::edge_type*, long long> edgeMap;

		void AddPoint(int x, int y);
		void AddSegment(int x1, int y1, int x2, int y2);
		void ConstructVoronoi();

		List<Tuple<double, double>^>^ GetVertices();
		List<Tuple<double, double>^>^ GetVerticesUnmapped();
		List<Tuple<long, long, long, bool, bool, bool>^>^ GetEdges();
		//List<Tuple<long, long, bool, bool, List<long>^>^>^ GetCells();

	};

	void Voronoi::ConstructVoronoi()
	{
		boost::polygon::construct_voronoi(points.begin(), points.end(), segments.begin(), segments.end(), &vd);

		//Populate the list of results
		long long cellId = 0;
		for (voronoi_diagram<double>::const_cell_iterator it = vd.cells().begin(); it != vd.cells().end(); ++it) {
			const voronoi_diagram<double>::cell_type &cell = *it;

			//Skip the cell if it is degenerate
			//if (cell.is_degenerate()){
			//	break;
			//}

			//Iterate throught the edges
			const voronoi_diagram<double>::edge_type *edge = cell.incident_edge();
			if (edge != NULL)
			{
				do {
					//Get the vertices and add them to the map if required
					const voronoi_diagram<double>::vertex_type* v0 = edge->vertex0();
					const voronoi_diagram<double>::vertex_type* v1 = edge->vertex1();

					long long startIndex = -1;
					if (v0 != 0){
						std::map<const voronoi_diagram<double>::vertex_type*, long long>::iterator vertexMapIterator = vertexMap.find(v0);
						if (vertexMapIterator == vertexMap.end()){
							vertexMap[v0] = vertexMap.size();
						}
						else {
							startIndex = vertexMapIterator->second;
						}
					}

					long long endIndex = -1;
					if (v1 != 0){
						std::map<const voronoi_diagram<double>::vertex_type*, long long>::iterator vertexMapIterator = vertexMap.find(v1);
						if (vertexMapIterator == vertexMap.end()){
							vertexMap[v1] = vertexMap.size();
						}
						else {
							endIndex = vertexMapIterator->second;
						}
					}

					//Add the edge to the collection of edges
					edgeMap[edge] = edgeMap.size();

					//TO-DO - implement reference to cell

					//Move to the next edge
					edge = edge->next();
				} while (edge != cell.incident_edge());
			}
			cellId++;
		}
	};

	void Voronoi::AddPoint(int x, int y)
	{
		Point p(x, y);
		points.push_back(p);
	};

	void Voronoi::AddSegment(int x1, int y1, int x2, int y2)
	{
		Segment s(x1, y1, x2, y2);
		segments.push_back(s);
	};

	/// <summary>
	/// Return the list of points
	/// </summary>
	List<Tuple<double, double>^>^ Voronoi::GetVertices()
	{
		List<Tuple<double, double>^>^ ret = gcnew List<Tuple<double, double>^>();
		std::map<const voronoi_diagram<double>::vertex_type*, long long>::iterator it;
		for (it = vertexMap.begin(); it != vertexMap.end(); ++it){
			const voronoi_diagram<double>::vertex_type &v = *it->first;
			Tuple<double, double>^ t = gcnew Tuple<double, double>(it->first->x(), it->first->y());
			ret->Add(t);
		}
		return ret;
	};

	List<Tuple<double, double>^>^ Voronoi::GetVerticesUnmapped()
	{
		List<Tuple<double, double>^>^ ret = gcnew List<Tuple<double, double>^>();
		for (voronoi_diagram<double>::const_vertex_iterator it = vd.vertices().begin(); it != vd.vertices().end(); ++it) {
			Tuple<double, double>^ t = gcnew Tuple<double, double>(it->x(), it->y());
			ret->Add(t);
		}
		return ret;
	}

	/// <summary>
	/// Return the list of edges
	/// </summary>
	List<Tuple<long, long, long, bool, bool, bool>^>^ Voronoi::GetEdges()
	{
		List<Tuple<long, long, long, bool, bool, bool>^>^ ret = gcnew List<Tuple<long, long, long, bool, bool, bool>^>();
		std::map<const voronoi_diagram<double>::edge_type*, long long>::iterator it;
		for (it = edgeMap.begin(); it != edgeMap.end(); ++it){
			const voronoi_diagram<double>::edge_type &edge = *it->first;

			const voronoi_diagram<double>::vertex_type* v0 = edge.vertex0();
			const voronoi_diagram<double>::vertex_type* v1 = edge.vertex1();


			//Get the index of the vertices
			std::map<const voronoi_diagram<double>::vertex_type *, long long>::iterator vertexMapIterator = vertexMap.find(v0);
			long start = -1;
			if (vertexMapIterator != vertexMap.end()){
				start = vertexMapIterator->second;
			}

			
			vertexMapIterator = vertexMap.find(v1);
			long end = -1;
			if (vertexMapIterator != vertexMap.end()){
				end = vertexMapIterator->second;
			}

			const voronoi_diagram<double>::cell_type* cell = edge.cell();
			Tuple<long, long, long, bool, bool, bool>^ t = gcnew Tuple<long, long, long, bool, bool, bool>(start, end, cell->source_index(), edge.is_primary(), edge.is_linear(), edge.is_finite());
			ret->Add(t);
		}
		return ret;
	};

	/// <summary>
	/// Return the list of cells
	/// </summary>
	//Voronoi::List<Tuple<long, long, bool, bool, List<long>^>^>^ GetCells()
	//{
	//	long long cellId = 0;
	//	for (voronoi_diagram<double>::const_cell_iterator it = vd.cells().begin(); it != vd.cells().end(); ++it) {
	//		const voronoi_diagram<double>::cell_type &cell = *it;
	//
	//		//Skip the cell if it is degenerate
	//		if (cell.is_degenerate()){
	//			break;
	//		}
	//
	//		//Iterate throught the edges
	//		List<long>^ edge_identifiers = gcnew List<long>();
	//		const voronoi_diagram<double>::edge_type *edge = cell.incident_edge();
	//		if (edge != NULL)
	//		{
	//			do {
	//				//Fetch the edge identifier
	//				std::map<const voronoi_diagram<double>::edge_type *, long long>::iterator edgeMapIterator = edgeMap.find(edge);
	//				if (vertexMapIterator == vertexMap.end()){
	//					throw "Start vertex not found";
	//				}
	//
	//				//Move to the next edge
	//				edge = edge->next();
	//			} while (edge != cell.incident_edge());
	//		}
	//
	//	}
	//}

	public ref class VoronoiWrapper
	{
		// TODO: Add your methods for this class here.
	public:
		Voronoi *v;
		VoronoiWrapper() : v(new Voronoi()){};
		void AddPoint(int x, int y)
		{
			v->AddPoint(x, y);
		};

		void AddSegment(int x1, int y1, int x2, int y2)
		{
			v->AddSegment(x1, y1, x2, y2);
		};

		void ConstructVoronoi()
		{
			v->ConstructVoronoi();
		};

		List<Tuple<double, double>^>^ GetVertices()
		{
			return v->GetVertices();
		};

		List<Tuple<long, long, long, bool, bool, bool>^>^ GetEdges()
		{
			return v->GetEdges();
		};

		List<Tuple<double, double>^>^ GetVerticesUnmapped()
		{
			return v->GetVerticesUnmapped();
		};
	};

}  // boost



