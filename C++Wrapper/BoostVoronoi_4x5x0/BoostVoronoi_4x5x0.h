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
		long start;
		long end;

		bool isPrimary;

		long site;

		bool isLinear;
		bool isFinite;

		long cell;
		long twin;

		c_Edge(long start = -1, long end = -1, bool isPrimary = false, long site = -1, bool isLinear = false, bool isFinite = false, long cell = -1, long twin = -1) {
			this->start = start;
			this->end = end;
			this->isPrimary = isPrimary;
			this->site = site;
			this->isLinear = isLinear;
			this->isFinite = isFinite;
			this->cell = cell;
			this->twin = twin;
		}
	};

	struct c_Cell{
		long cellId;
		long source_index;
		bool contains_point;
		bool contains_segment;
		bool is_open;

		//std::vector<long> vertices;
		std::vector<long> edges;

		short source_category;

		c_Cell(long cellId = -1, long source_index = -1, bool contains_point = false, bool contains_segment = false, bool is_open = false, short source_category = -1){
			this->cellId = cellId;
			this->source_index = source_index;
			this->contains_point = contains_point;
			this->contains_segment = contains_segment;
			this->is_open = is_open;
			this->source_category = source_category;
		}
	};

	public class Voronoi
	{
	public:

		//Data structure for numbering
		std::map<const voronoi_diagram<double>::vertex_type*, long long> vertexMap;
		std::map<const voronoi_diagram<double>::edge_type*, long long> edgeMap;
		std::map<const voronoi_diagram<double>::cell_type*, long long> cellMap;


		std::map<long long, const voronoi_diagram<double>::vertex_type*> vertexMap2;
		std::map<long long, const voronoi_diagram<double>::edge_type*> edgeMap2;
		std::map<long long, const voronoi_diagram<double>::cell_type*> cellMap2;

		std::vector<Point> points;
		std::vector<Segment> segments;
		voronoi_diagram<double> vd;

		//Output data storage
		std::vector<c_Vertex> vertices;
		std::vector<c_Edge> edges;
		std::vector<c_Cell> cells;

		void AddPoint(int x, int y);
		void AddSegment(int x1, int y1, int x2, int y2);
		void ConstructVoronoi();

		void Construct();
		void CreateMaps();
		void CreateVertexMap();
		void CreateEdgesMap();
		void CreateCellMap();
		long long CountVertices();
		long long CountEdges();
		long long CountCells();

		List<Tuple<double, double>^>^ GetVertices();
		List<Tuple<long, long, long, long, Tuple<bool, bool, bool, long, long>^>^>^ GetEdges();
		List<Tuple<long, long, bool, bool, List<long>^, bool, short>^>^ GetCells();

		Tuple<long long, double, double>^ GetVertex(long long i);
		Tuple<long long, long long, long long, bool, bool, bool, Tuple<long long, long long>^>^ GetEdge(long long i);
		Tuple<long long, long, short, Tuple<bool, bool, bool, bool>^, List<long long>^>^ GetCell(long long i);

		// Cell Index
		// Source Index
		// Source Category
		// Contains Point
		// Contains Line
		// Is Degenerate
		// List of segments
		
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
			vertexMap2[index] = vertex;
			vertexMap[vertex] = index;
			index++;
		}
	}

	//void Voronoi::CreateVertexMap()
	//{
	//	for (voronoi_diagram<double>::const_edge_iterator it = vd.edges().begin();
	//		it != vd.edges().end(); ++it) {

	//		//Get the vertices and add them to the map if required
	//		const voronoi_diagram<double>::vertex_type* v0 = it->vertex0();
	//		const voronoi_diagram<double>::vertex_type* v1 = it->vertex1();

	//		long start_index = -1;
	//		if (v0 != 0){

	//			//Check if the vertex exists in the map
	//			std::map<const voronoi_diagram<double>::vertex_type*, long>::iterator vertexMapIterator =
	//				vertexMap.find(v0);

	//			//If the vertex is not in the map, add it to the vector and the map. If not fetch the index.
	//			if (vertexMapIterator == vertexMap.end()){
	//				start_index = vertices.size();
	//				c_Vertex start = c_Vertex(v0->x(), v0->y());
	//				vertices.push_back(start);
	//				vertexMap[v0] = start_index;
	//			}
	//			else{
	//				start_index = vertexMapIterator->second;
	//			}
	//		}


	//		long end_index = -1;
	//		if (v1 != 0){

	//			//Check if the vertex exists in the map
	//			std::map<const voronoi_diagram<double>::vertex_type*, long>::iterator vertexMapIterator =
	//				vertexMap.find(v1);

	//			//If the vertex is not in the map, add it to the vector and the map. If not fetch the index.
	//			if (vertexMapIterator == vertexMap.end()){
	//				end_index = vertices.size();
	//				c_Vertex end = c_Vertex(v1->x(), v1->y());
	//				vertices.push_back(end);
	//				vertexMap[v1] = end_index;
	//			}
	//			else{
	//				end_index = vertexMapIterator->second;
	//			}
	//		}
	//	}
	//}

	void Voronoi::CreateEdgesMap()
	{
		long long index = 0;
		for (voronoi_diagram<double>::const_edge_iterator it = vd.edges().begin(); it != vd.edges().end(); ++it) {
			const voronoi_diagram<double>::edge_type* edge = &(*it);
			edgeMap2[index] = edge;
			edgeMap[edge] = index;
			index++;
		}
	}

	void Voronoi::CreateCellMap()
	{
		long long index = 0;
		for (voronoi_diagram<double>::const_cell_iterator it = vd.cells().begin(); it != vd.cells().end(); ++it) {
			const voronoi_diagram<double>::cell_type* cell = &(*it);
			cellMap2[index] = cell;
			cellMap[cell] = index;
			index++;
		}
	}

	void Voronoi::CreateMaps()
	{
		CreateVertexMap();
		CreateEdgesMap();
		CreateCellMap();
	}

	void Voronoi::ConstructVoronoi()
	{
		voronoi_diagram<double> vd;
		boost::polygon::construct_voronoi(points.begin(), points.end(), segments.begin(), segments.end(), &vd);

		//Data structure for numbering
		std::map<const voronoi_diagram<double>::vertex_type*, long> vertexMap;
		//std::map<const voronoi_diagram<double>::edge_type*, long long> edgeMap;

		//Initialize collections
		cells.reserve(vd.num_cells());
		edges.reserve(vd.num_edges());
		vertices.reserve(vd.num_vertices());

		//An identifier for cells
		long cell_identifier = 0;

		//Iterate through cells
		for (voronoi_diagram<double>::const_cell_iterator it = vd.cells().begin(); it != vd.cells().end(); ++it) {
			const voronoi_diagram<double>::cell_type &cell = *it;

			//Don't do anything if the cells is degenerate
			if (!cell.is_degenerate()){

				//Identify the source type
				int source_category = -1;
				if (cell.source_category() == boost::polygon::SOURCE_CATEGORY_SINGLE_POINT){
					source_category = 0;
				}
				else if (cell.source_category() == boost::polygon::SOURCE_CATEGORY_SEGMENT_START_POINT){
					source_category = 1;
				}
				else if (cell.source_category() == boost::polygon::SOURCE_CATEGORY_SEGMENT_END_POINT){
					source_category = 2;
				}
				else if (cell.source_category() == boost::polygon::SOURCE_CATEGORY_INITIAL_SEGMENT){
					source_category = 3;
				}
				else if (cell.source_category() == boost::polygon::SOURCE_CATEGORY_REVERSE_SEGMENT){
					source_category = 4;
				}
				else if (cell.source_category() == boost::polygon::SOURCE_CATEGORY_GEOMETRY_SHIFT){
					source_category = 5;
				}
				else if (cell.source_category() == boost::polygon::SOURCE_CATEGORY_BITMASK){
					source_category = 6;
				}


				//Create the memory cells object
				c_Cell c_cell = c_Cell(cell_identifier, cell.source_index(), cell.contains_point(), cell.contains_segment(), false, source_category);

				//Iterate throught the edges
				const voronoi_diagram<double>::edge_type *edge = cell.incident_edge();
				if (edge != NULL)
				{
					do {
						//Get the vertices and add them to the map if required
						const voronoi_diagram<double>::vertex_type* v0 = edge->vertex0();
						const voronoi_diagram<double>::vertex_type* v1 = edge->vertex1();

						long start_index = -1;
						if (v0 != 0){

							//Check if the vertex exists in the map
							std::map<const voronoi_diagram<double>::vertex_type*, long>::iterator vertexMapIterator = 
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


						long end_index = -1;
						if (v1 != 0){

							//Check if the vertex exists in the map
							std::map<const voronoi_diagram<double>::vertex_type*, long>::iterator vertexMapIterator = 
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

						if (start_index == -1 || end_index == -1){
							c_cell.is_open = true;
						}

						//Add the edge to the collection of edges
						c_Edge cell_edge = c_Edge(start_index, end_index, edge->is_primary(), edge->cell()->source_index(), edge->is_linear(), edge->is_finite(), cell_identifier, -1);
							
						//Add to map and vector
						long long eIndex = edges.size();
						edges.push_back(cell_edge);
						edgeMap[edge] = eIndex;
						c_cell.edges.push_back(eIndex);


						//Move to the next edge
						edge = edge->next();

					} while (edge != cell.incident_edge());
				}
				cells.push_back(c_cell);
				cell_identifier++;
			}
		}

		//Second iteration for twins
		//This part can probably optimized - TBD
		for (voronoi_diagram<double>::const_cell_iterator it = vd.cells().begin(); it != vd.cells().end(); ++it) {
			const voronoi_diagram<double>::cell_type &cell = *it;

			//Don't do anything if the cells is degenerate
			if (!cell.is_degenerate()){
				//Iterate throught the edges
				const voronoi_diagram<double>::edge_type *edge = cell.incident_edge();
				if (edge != NULL)
				{
					do {
						long long edge_id = -1;
						std::map<const voronoi_diagram<double>::edge_type *, long long>::iterator edgeMapIterator = edgeMap.find(edge);
						if (edgeMapIterator != edgeMap.end()){
							edge_id = edgeMapIterator->second;
						}

						if (edge_id != -1){
							edgeMapIterator = edgeMap.find(edge->twin());
							if (edgeMapIterator != edgeMap.end()){
								edges[edge_id].twin = edgeMapIterator->second;
							}
						}
						//Move to the next edge
						edge = edge->next();
					} while (edge != cell.incident_edge());
				}
			}
		}

		vertexMap.clear();
		edgeMap.clear();
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
		List<Tuple<double, double>^>^ ret = gcnew List<Tuple<double, double>^>(vertices.size());
		for (size_t i = 0; i < vertices.size(); i++) {
			Tuple<double, double>^ t = gcnew Tuple<double, double>(vertices[i].X, vertices[i].Y);
			ret->Add(t);
		}
		return ret;
	};

	Tuple<long long, double, double>^ Voronoi::GetVertex(long long index)
	{
		std::map<long long, const voronoi_diagram<double>::vertex_type *>::iterator mapIterator = vertexMap2.find(index);
		if (mapIterator != vertexMap2.end()){
			double x = mapIterator->second->x();
			double y = mapIterator->second->y();
			return gcnew Tuple<long long, double, double>(mapIterator->first, x, y);
		}
		return gcnew Tuple<long long, double, double>(-1, -1 ,-1);
	}

	/// <summary>
	/// Return the list of edges
	/// </summary>
	List<Tuple<long, long, long, long, Tuple<bool, bool, bool, long, long>^>^>^ Voronoi::GetEdges()
	{
		List<Tuple<long, long, long, long, Tuple<bool, bool, bool, long, long>^>^>^ ret = 
			gcnew List<Tuple<long, long, long, long, Tuple<bool, bool, bool, long, long>^>^>(edges.size());

		for (size_t i = 0; i < edges.size(); i++) {
			Tuple<long, long, long, long, Tuple<bool, bool, bool, long, long>^>^ t = 
				gcnew Tuple<long, long, long, long, Tuple<bool, bool, bool, long, long>^>(i, edges[i].start, edges[i].end,
				edges[i].site, gcnew Tuple<bool, bool, bool, long, long>(edges[i].isPrimary, edges[i].isLinear, edges[i].isFinite, edges[i].cell, edges[i].twin));
			ret->Add(t);
		}
		return ret;
	};


	Tuple<long long, long long, long long, bool, bool, bool, Tuple<long long, long long>^>^ Voronoi::GetEdge(long long index)
	{

		std::map<long long, const voronoi_diagram<double>::edge_type *>::iterator edgeMapIterator = edgeMap2.find(index);
		if (edgeMapIterator != edgeMap2.end()){
			
			//Shorten the edge reference
			const voronoi_diagram<double>::edge_type * edge = edgeMapIterator->second;

			//Find vertex references
			const voronoi_diagram<double>::vertex_type * start = edge->vertex0();
			const voronoi_diagram<double>::vertex_type * end = edge->vertex1();

			long start_id = GetVertexIndex(start);
			long end_id = GetVertexIndex(end);

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
				edgeMapIterator->second->is_primary(),
				edgeMapIterator->second->is_linear(),
				edgeMapIterator->second->is_finite(),
				treferences
				);

			//Tuple<long long, Tuple<long long, double, double>^, Tuple<long long, double, double>^, bool, bool, bool, Tuple<long long, long long>^>^ t =
			//	gcnew Tuple<long long, Tuple<long long, double, double>^, Tuple<long long, double, double>^, bool, bool, bool, Tuple<long long, long long>^>(
			//	index,
			//	gcnew Tuple<long long, double, double>(-1,-1,-1),
			//	gcnew Tuple<long long, double, double>(-1, -1, -1),
			//	edgeMapIterator->second->is_primary(),
			//	edgeMapIterator->second->is_linear(),
			//	edgeMapIterator->second->is_finite(),
			//	gcnew Tuple<long long, long long>(-1, -1)
			//	);

			return t;
		}
	}


	Tuple<long long, long, short, Tuple<bool, bool, bool, bool>^, List<long long>^>^ Voronoi::GetCell(long long index)
	{
		std::map<long long, const voronoi_diagram<double>::cell_type *>::iterator cellMapIterator = cellMap2.find(index);
		List<long long>^ edge_identifiers = gcnew List<long long>();

		if (cellMapIterator != cellMap2.end()){
			const voronoi_diagram<double>::cell_type* cell = cellMapIterator->second;
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

			return gcnew Tuple<long long, long, short, Tuple<bool, bool, bool, bool>^, List<long long>^>(
				index,
				cell->source_index(),
				source_category,
				booleanInfo,
				edge_identifiers
			);
		}
	}

	long long Voronoi::GetVertexIndex(const voronoi_diagram<double>::vertex_type* vertex){

		//Search the map and return the index
		if (vertex != NULL){
			std::map<const voronoi_diagram<double>::vertex_type *, long long>::iterator vertexMapIterator = vertexMap.find(vertex);
			if (vertexMapIterator != vertexMap.end()){
				return vertexMapIterator->second;
			}
		}
		return -1;
	}

	long long Voronoi::GetEdgeIndex(const voronoi_diagram<double>::edge_type* edge){

		//Search the map and return the index
		if (edge != NULL){
			std::map<const voronoi_diagram<double>::edge_type *, long long>::iterator edgeMapIterator = edgeMap.find(edge);
			if (edgeMapIterator != edgeMap.end()){
				return edgeMapIterator->second;
			}
		}
		return -1;
	}

	long long Voronoi::GetCellIndex(const voronoi_diagram<double>::cell_type* cell){

		//Search the map and return the index
		if (cell != NULL){
			std::map<const voronoi_diagram<double>::cell_type *, long long>::iterator cellMapIterator = cellMap.find(cell);
			if (cellMapIterator != cellMap.end()){
				return cellMapIterator->second;
			}
		}
		return -1;
	}

	/// <summary>
	/// Return the list of cells
	/// </summary>
	List<Tuple<long, long, bool, bool, List<long>^, bool, short>^>^ Voronoi::GetCells()
	{
		long cell_identifier = 0;
		List<Tuple<long, long, bool, bool, List<long>^, bool, short>^>^ ret = gcnew List<Tuple<long, long, bool, bool, List<long>^, bool, short>^>(cells.size());
		for (size_t i = 0; i < cells.size(); i++) {

			//Create the list of identifiers
			List<long>^ edge_list = gcnew List<long>(cells[i].edges.size());
			for (size_t j = 0; j < cells[i].edges.size(); j++) {
				edge_list->Add(cells[i].edges[j]);
			}

			//Populate the cells info
			Tuple<long, long, bool, bool, List<long>^, bool, short>^ t = gcnew Tuple <long, long, bool, bool, List<long>^, bool, short>(
				cells[i].cellId, 
				cells[i].source_index,
				cells[i].contains_point, 
				cells[i].contains_segment, 
				edge_list,
				cells[i].is_open,
				cells[i].source_category);

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

		List<Tuple<long, long, long, long, Tuple<bool, bool, bool, long, long>^>^>^ GetEdges()
		{
			return v->GetEdges();
		};

		List<Tuple<long, long, bool, bool, List<long>^, bool, short>^>^ GetCells()
		{
			return v->GetCells();
		}


		//New Stuffs to integrate in SharpBoostVoronoi
		void Construct()
		{
			v->Construct();
		}

		long CountVertices(){
			return v->CountVertices();
		}

		long CountEdges(){
			return v->CountEdges();
		}

		long CountCells(){
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

		Tuple<long long, long, short, Tuple<bool, bool, bool, bool>^, List<long long>^>^ GetCell(long long index)
		{
			return v->GetCell(index);
		}
	};

}  // boost



