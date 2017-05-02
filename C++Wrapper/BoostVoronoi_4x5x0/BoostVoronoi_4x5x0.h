// CSharpBootVoronoi.h

#pragma once
#include<boost/polygon/voronoi.hpp>
#include <boost/bimap.hpp>


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
		struct geometry_concept < Point > {
			typedef point_concept type;
		};

		template <>
		struct point_traits < Point > {
			typedef int coordinate_type;

			static inline coordinate_type get(
				const Point& point, orientation_2d orient) {
				return (orient == HORIZONTAL) ? point.a : point.b;
			}
		};

		template <>
		struct geometry_concept < Segment > {
			typedef segment_concept type;
		};

		template <>
		struct segment_traits < Segment > {
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
			//Data structure for numbering
			typedef boost::bimap<const voronoi_diagram<double>::vertex_type*, long long> vertices_bimap;
			typedef vertices_bimap::value_type vertex_position;
			vertices_bimap vertices;

			typedef boost::bimap<const voronoi_diagram<double>::edge_type*, long long> edges_bimap;
			typedef edges_bimap::value_type edge_position;
			edges_bimap edges;

			typedef boost::bimap<const voronoi_diagram<double>::cell_type*, long long> cells_bimap;
			typedef cells_bimap::value_type cell_position;
			cells_bimap cells;

			std::vector<Point> points;
			std::vector<Segment> segments;
			voronoi_diagram<double> vd;

			void AddPoint(int x, int y);
			void AddSegment(int x1, int y1, int x2, int y2);

			void Construct();
			void CreateMaps();
			void CreateVertexMap();
			void CreateEdgesMap();
			void CreateCellMap();
			long long CountVertices();
			long long CountEdges();
			long long CountCells();

			Tuple<long long, double, double>^ GetVertex(long long i);
			Tuple<long long, long long, long long, bool, bool, bool, Tuple<long long, long long>^>^ GetEdge(long long i);
			Tuple<long long, long long, short, Tuple<bool, bool, bool, bool>^, List<long long>^, List<long long>^>^ GetCell(long long i);

			long long GetVertexIndex(const voronoi_diagram<double>::vertex_type* vertex);
			long long GetEdgeIndex(const voronoi_diagram<double>::edge_type* edge);
			long long GetCellIndex(const voronoi_diagram<double>::cell_type* cell);
	};


	void Voronoi::Construct()
	{
		boost::polygon::construct_voronoi(points.begin(), points.end(), segments.begin(), segments.end(), &vd);
	}

	long long Voronoi::CountVertices()
	{
		return vd.num_vertices();
	}

	long long Voronoi::CountEdges()
	{
		return vd.num_edges();
	}

	long long Voronoi::CountCells()
	{
		return vd.num_cells();
	}

	void Voronoi::CreateVertexMap()
	{

		long long index = 0;
		for (voronoi_diagram<double>::const_vertex_iterator it = vd.vertices().begin(); it != vd.vertices().end(); ++it) {
			const voronoi_diagram<double>::vertex_type* vertex = &(*it);
			vertices.insert(vertex_position(vertex, index));
			index++;
		}
	}

	void Voronoi::CreateEdgesMap()
	{
		long long index = 0;
		for (voronoi_diagram<double>::const_edge_iterator it = vd.edges().begin(); it != vd.edges().end(); ++it) {
			const voronoi_diagram<double>::edge_type* edge = &(*it);
			edges.insert(edge_position(edge, index));
			index++;
		}
	}

	void Voronoi::CreateCellMap()
	{
		long long index = 0;
		for (voronoi_diagram<double>::const_cell_iterator it = vd.cells().begin(); it != vd.cells().end(); ++it) {
			const voronoi_diagram<double>::cell_type* cell = &(*it);
			cells.insert(cell_position(cell, index));
			index++;
		}
	}

	void Voronoi::CreateMaps()
	{
		CreateVertexMap();
		CreateEdgesMap();
		CreateCellMap();
	}

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



	Tuple<long long, double, double>^ Voronoi::GetVertex(long long index)
	{
		const voronoi_diagram<double>::vertex_type* vertex = vertices.right.at(index);
		return gcnew Tuple<long long, double, double>(index, vertex->x(), vertex->y());
	}


	Tuple<long long, long long, long long, bool, bool, bool, Tuple<long long, long long>^>^ Voronoi::GetEdge(long long index)
	{
		const voronoi_diagram<double>::edge_type* edge = edges.right.at(index);
		//Find vertex references
		const voronoi_diagram<double>::vertex_type * start = edge->vertex0();
		const voronoi_diagram<double>::vertex_type * end = edge->vertex1();

		long long start_id = GetVertexIndex(start);
		long long end_id = GetVertexIndex(end);

		//Find the twin reference using the segment object
		const voronoi_diagram<double>::edge_type * twin = edge->twin();
		long long twinIndex = -1;
		if (edge != NULL){
			twinIndex = GetEdgeIndex(twin);
		}

		//Find the cell reference using ther cell object
		long long cellIndex = GetCellIndex(edge->cell());

		//Lay out the tuple structure
		Tuple<long long, long long>^  treferences = gcnew Tuple<long long, long long>(twinIndex, cellIndex);

		Tuple<long long, long long, long long, bool, bool, bool, Tuple<long long, long long>^>^ t =
			gcnew Tuple<long long, long long, long long, bool, bool, bool, Tuple<long long, long long>^>(
			index,
			start_id,
			end_id,
			edge->is_primary(),
			edge->is_linear(),
			edge->is_finite(),
			treferences
			);

		return t;

	}


	Tuple<long long, long long, short, Tuple<bool, bool, bool, bool>^, List<long long>^, List<long long>^>^ Voronoi::GetCell(long long index)
	{
		//std::map<long long, const voronoi_diagram<double>::cell_type *>::iterator cellMapIterator = cellMap2.find(index);
		const voronoi_diagram<double>::cell_type* cell = cells.right.at(index);
		List<long long>^ edge_identifiers = gcnew List<long long>();
		List<long long>^ vertex_identifiers = gcnew List<long long>();

		bool is_open = false;

		//Identify the source type
		int source_category = -1;
		if (cell->source_category() == boost::polygon::SOURCE_CATEGORY_SINGLE_POINT){
			source_category = 0;
		}
		else if (cell->source_category() == boost::polygon::SOURCE_CATEGORY_SEGMENT_START_POINT){
			source_category = 1;
		}
		else if (cell->source_category() == boost::polygon::SOURCE_CATEGORY_SEGMENT_END_POINT){
			source_category = 2;
		}
		else if (cell->source_category() == boost::polygon::SOURCE_CATEGORY_INITIAL_SEGMENT){
			source_category = 3;
		}
		else if (cell->source_category() == boost::polygon::SOURCE_CATEGORY_REVERSE_SEGMENT){
			source_category = 4;
		}
		else if (cell->source_category() == boost::polygon::SOURCE_CATEGORY_GEOMETRY_SHIFT){
			source_category = 5;
		}
		else if (cell->source_category() == boost::polygon::SOURCE_CATEGORY_BITMASK){
			source_category = 6;
		}


		const voronoi_diagram<double>::edge_type* edge = cell->incident_edge();
		if (edge != NULL)
		{
			do {
				//Get the edge index
				long long edge_index = GetEdgeIndex(edge);
				edge_identifiers->Add(edge_index);

				if (edge->vertex0() == NULL || edge->vertex1() == NULL)
					is_open = true;

				long long edge_start = -1;
				long long edge_end = -1;

				if (edge->vertex0() == NULL){
					edge_start = GetVertexIndex(edge->vertex0());
				}

				if (edge->vertex1() == NULL){
					edge_end = GetVertexIndex(edge->vertex1());
				}

				long vertices_count = vertex_identifiers->Count;
				if (vertices_count == 0){
					vertex_identifiers->Add(edge_start);
				}
				else{
					if (vertex_identifiers[vertices_count - 1] != edge_start){
						vertex_identifiers->Add(edge_start);
					}
				}
				vertex_identifiers->Add(edge_end);
				//Move to the next edge
				edge = edge->next();

			} while (edge != cell->incident_edge());
		}


		Tuple<bool, bool, bool, bool>^ booleanInfo = gcnew Tuple<bool, bool, bool, bool>(
			cell->contains_point(),
			cell->contains_segment(),
			is_open,
			cell->is_degenerate()
			);

		return gcnew Tuple<long long, long long, short, Tuple<bool, bool, bool, bool>^, List<long long>^, List<long long>^>(
			index,
			cell->source_index(),
			source_category,
			booleanInfo,
			edge_identifiers,
			vertex_identifiers
			);
	}

	long long Voronoi::GetVertexIndex(const voronoi_diagram<double>::vertex_type* vertex){

		//Search the map and return the index
		if (vertex != NULL){
			return vertices.left.at(vertex);
		}
		return -1;
	}

	long long Voronoi::GetEdgeIndex(const voronoi_diagram<double>::edge_type* edge){

		//Search the map and return the index
		if (edge != NULL){
			return edges.left.at(edge);
		}
		return -1;
	}

	long long Voronoi::GetCellIndex(const voronoi_diagram<double>::cell_type* cell){

		//Search the map and return the index
		if (cell != NULL){
			return cells.left.at(cell);
		}
		return -1;
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

		void Construct()
		{
			v->Construct();
		}

		long long CountVertices(){
			return v->CountVertices();
		}

		long long CountEdges(){
			return v->CountEdges();
		}

		long long CountCells(){
			return v->CountCells();
		}

		void CreateVertexMap()
		{
			v->CreateVertexMap();
		}

		void CreateEdgeMap()
		{
			v->CreateEdgesMap();
		}

		void CreateCellMap()
		{
			v->CreateCellMap();
		}

		Tuple<long long, double, double>^ GetVertex(long long index)
		{
			return v->GetVertex(index);
		}

		Tuple<long long, long long, long long, bool, bool, bool, Tuple<long long, long long>^>^ GetEdge(long long index)
		{
			return v->GetEdge(index);
		}

		Tuple<long long, long long, short, Tuple<bool, bool, bool, bool>^, List<long long>^, List<long long>^>^ GetCell(long long index)
		{
			return v->GetCell(index);
		}
	};

}  // boost



