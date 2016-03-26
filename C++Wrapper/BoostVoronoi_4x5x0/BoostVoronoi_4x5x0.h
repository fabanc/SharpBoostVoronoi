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


	struct c_Vertex {
		double X;
		double Y;

		c_Vertex(double x = 0, double y = 0) : X(x), Y(y) {}
	};


	struct c_Edge {
		long long start;
		long long end;

		bool isPrimary;

		size_t site;

		bool isLinear;
		bool isFinite;

		long cell;
		long twin;

		c_Edge(long long start = -1, long long end = -1, bool isPrimary = false, size_t site = -1, bool isLinear = false, bool isFinite = false, long cell = -1) {
			this->start = start;
			this->end = end;
			this->isPrimary = isPrimary;
			this->site = site;
			this->isLinear = isLinear;
			this->isFinite = isFinite;
			this->cell = cell;
		}
	};

	struct c_Cell{
		size_t cellId;
		size_t source_index;
		bool contains_point;
		bool contains_segment;
		bool is_open;

		std::vector<long> vertices;
		std::vector<long> edges;

		c_Cell(size_t cellId = -1, size_t source_index = -1, bool contains_point = false, bool contains_segment = false, bool is_open = false){
			this->cellId = cellId;
			this->source_index = source_index;
			this->contains_point = contains_point;
			this->contains_segment = contains_segment;
			this->is_open = is_open;
		}
	};

	public class Voronoi
	{
	public:
		std::vector<Point> points;
		std::vector<Segment> segments;
		voronoi_diagram<double> vd;

		//Output data storage
		std::map<const voronoi_diagram<double>::vertex_type*, long long> vertexMap;
		std::map<const voronoi_diagram<double>::edge_type*, long long> edgeMap;

		std::vector<c_Vertex> vertices;
		std::vector<c_Edge> edges;
		std::vector<c_Cell> cells;


		void AddPoint(int x, int y);
		void AddSegment(int x1, int y1, int x2, int y2);
		void ConstructVoronoi();

		List<Tuple<double, double>^>^ GetVertices();
		List<Tuple<double, double>^>^ GetVerticesUnmapped();
		List<Tuple<long, long, long, long, bool, bool, bool>^>^ GetEdges();
		List<Tuple<long, long, bool, bool, List<long>^, bool>^>^ GetCells();

	};

	void Voronoi::ConstructVoronoi()
	{
		boost::polygon::construct_voronoi(points.begin(), points.end(), segments.begin(), segments.end(), &vd);

		//Iterate through the results

		//An identifier for cells
		long long cell_identifier = 0;

		//Iterate through cells
		for (voronoi_diagram<double>::const_cell_iterator it = vd.cells().begin(); it != vd.cells().end(); ++it) {
			const voronoi_diagram<double>::cell_type &cell = *it;

			//Don't do anything if the cells is degenerate
			if (!cell.is_degenerate()){

				//Create the memory cells object
				c_Cell c_cell = c_Cell(cell_identifier, cell.source_index(), cell.contains_point(), cell.contains_segment(), false);

				//Iterate throught the edges
				const voronoi_diagram<double>::edge_type *edge = cell.incident_edge();
				if (edge != NULL)
				{
					do {
						//Get the vertices and add them to the map if required
						const voronoi_diagram<double>::vertex_type* v0 = edge->vertex0();
						const voronoi_diagram<double>::vertex_type* v1 = edge->vertex1();

						long long start_index = -1;
						if (v0 != 0){

							//Check if the vertex exists in the map
							std::map<const voronoi_diagram<double>::vertex_type*, long long>::iterator vertexMapIterator = 
								vertexMap.find(v0);

							//If the vertex is not in the map, add it to the vector and the map. If not fetch the index.
							if (vertexMapIterator == vertexMap.end()){
								start_index = vertices.size();
								c_Vertex start = c_Vertex(v0->x(), v0->y());
								vertices.push_back(start);
								vertexMap[v0] = start_index;
							}
							else{
								start_index = vertexMapIterator->second;
							}
						}

						if (start_index == -1){
							c_cell.is_open = true;
						}

						long long end_index = -1;
						if (v1 != 0){

							//Check if the vertex exists in the map
							std::map<const voronoi_diagram<double>::vertex_type*, long long>::iterator vertexMapIterator = 
								vertexMap.find(v1);

							//If the vertex is not in the map, add it to the vector and the map. If not fetch the index.
							if (vertexMapIterator == vertexMap.end()){
								end_index = vertices.size();
								c_Vertex end = c_Vertex(v1->x(), v1->y());
								vertices.push_back(end);
								vertexMap[v1] = end_index;
							}
							else{
								end_index = vertexMapIterator->second;
							}
						}

						if (end_index == -1){
							c_cell.is_open = true;
						}

						//Add the edge to the collection of edges
						std::map<const voronoi_diagram<double>::edge_type *, long long>::iterator edgeMapIterator = 
							edgeMap.find(edge);

						if (edgeMapIterator == edgeMap.end()){

							//Create memory edge object
							c_Edge cell_edge = c_Edge(start_index, end_index, edge->is_primary(), edge->cell()->source_index(), edge->is_linear(), edge->is_finite(), cell_identifier);
							
							//Add to map and vector
							long long eIndex = edges.size();
							edges.push_back(cell_edge);
							edgeMap[edge] = eIndex;
							c_cell.edges.push_back(eIndex);
						}
						else{
							c_cell.edges.push_back(edgeMapIterator->second);
						}

						//Move to the next edge
						edge = edge->next();

					} while (edge != cell.incident_edge());
				}
				cells.push_back(c_cell);
				cell_identifier++;
			}

			//Parse the list of segments and associate its twin.
			for (std::map<const voronoi_diagram<double>::edge_type *, long long>::iterator it = edgeMap.begin(); it != edgeMap.end(); ++it)
			{
				//Get the struct representing the cell
				if (edges[it->second].twin == -1)
				{
					//Set an iterator on the twin edge
					std::map<const voronoi_diagram<double>::edge_type *, long long>::iterator twinEdgeIterator =
						edgeMap.find(it->first->twin());

					//Associate the segments and their twin
					edges[it->second].twin = twinEdgeIterator->second;
					edges[twinEdgeIterator->second].twin = it->second;
				}
			}
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
		for (int i = 0; i < vertices.size(); i++) {
			Tuple<double, double>^ t = gcnew Tuple<double, double>(vertices[i].X, vertices[i].Y);
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
	List<Tuple<long, long, long, long, bool, bool, bool>^>^ Voronoi::GetEdges()
	{
		List<Tuple<long, long, long, long, bool, bool, bool>^>^ ret = gcnew List<Tuple<long, long, long, long, bool, bool, bool>^>();
		for (int i = 0; i < edges.size(); i++) {
			Tuple<long, long, long, long, bool, bool, bool>^ t = gcnew Tuple<long, long, long, long, bool, bool, bool>(i, edges[i].start, edges[i].end, 
				edges[i].site, edges[i].isPrimary, edges[i].isLinear, edges[i].isFinite);
			ret->Add(t);
		}
		return ret;
	};

	/// <summary>
	/// Return the list of cells
	/// </summary>
	List<Tuple<long, long, bool, bool, List<long>^, bool>^>^ Voronoi::GetCells()
	{
		long cell_identifier = 0;
		List<Tuple<long, long, bool, bool, List<long>^, bool>^>^ ret = gcnew List<Tuple<long, long, bool, bool, List<long>^, bool>^>();
		for (int i = 0; i < cells.size(); i++) {

			//Create the list of identifiers
			List<long>^ edge_list = gcnew List<long>();
			for (int j = 0; j < cells[i].edges.size(); j++) {
				edge_list->Add(cells[i].edges[j]);
			}

			//Populate the cells info
			Tuple<long, long, bool, bool, List<long>^, bool>^ t = gcnew Tuple <long, long, bool, bool, List<long>^, bool>(
				cells[i].cellId, 
				cells[i].source_index,
				cells[i].contains_point, 
				cells[i].contains_segment, 
				edge_list,
				cells[i].is_open);

			//Add tuple to the list
			ret->Add(t);
		}

		return ret;
	}

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

		List<Tuple<long, long, long, long, bool, bool, bool>^>^ GetEdges()
		{
			return v->GetEdges();
		};

		List<Tuple<long, long, bool, bool, List<long>^, bool>^>^ GetCells()
		{
			return v->GetCells();
		}

		List<Tuple<double, double>^>^ GetVerticesUnmapped()
		{
			return v->GetVerticesUnmapped();
		};
	};

}  // boost



