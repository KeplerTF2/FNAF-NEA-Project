using NEA_Project.Engine;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    // A graph that contains nodes and edges. Edges can have varying values. A value of 0 indicates no edge
    public class Graph
    {
        private int Size;
        private Dictionary<int, dynamic> ItemDict = new Dictionary<int, dynamic>();
        private Dictionary<int, Dictionary<int, float>> ConnectionDict = new Dictionary<int, Dictionary<int, float>>();

        public Graph() { }

        public Graph(dynamic[] items)
        {
            foreach (dynamic item in items)
                AddItem(item);
        }

        // Find the shortest path from the source to the target
        public List<int> Dijkstra(int SourceID, int TargetID)
        {
            Dictionary<int, float> Distance = new Dictionary<int, float>();
            Dictionary<int, int> Previous = new Dictionary<int, int>();
            List<int> UncheckedVertices = new List<int>();

            foreach (int Vertex in ItemDict.Keys)
            {
                Distance[Vertex] = float.PositiveInfinity;
                Previous[Vertex] = -1;
                UncheckedVertices.Add(Vertex);
            }
            Distance[SourceID] = 0;

            while (UncheckedVertices.Count > 0)
            {
                // Finds the closest vertex to the source
                int ClosestVertex = -1;
                float MinDist = float.PositiveInfinity;
                foreach (int V in UncheckedVertices)
                {
                    if (Distance[V] < MinDist)
                    {
                        MinDist = Distance[V];
                        ClosestVertex = V;
                    }
                }

                // Quit loop if we find our target vertex
                if (ClosestVertex == TargetID) break;

                // Removes from unchecked vertices
                UncheckedVertices.Remove(ClosestVertex);

                // Calculates distances to next node
                foreach (int V in ConnectionDict[ClosestVertex].Keys)
                {
                    // Only consider vertices that haven't been checked already
                    if (UncheckedVertices.Contains(V))
                    {
                        // If alternate distance found is shorter, use that one
                        if (ConnectionDict[ClosestVertex][V] != 0)
                        {
                            float AltDist = Distance[ClosestVertex] + ConnectionDict[ClosestVertex][V];
                            if (AltDist < Distance[V])
                            {
                                Distance[V] = AltDist;
                                Previous[V] = ClosestVertex;
                            }
                        }
                    }
                }
            }

            // Find shortest path once all distances found
            int CurrentVertex = TargetID;
            List<int> Path = new List<int>();
            if (Previous[CurrentVertex] != -1 || CurrentVertex == SourceID)
            {
                while (CurrentVertex != -1)
                {
                    Path.Add(CurrentVertex);
                    CurrentVertex = Previous[CurrentVertex];
                }
            }

            Path.Reverse();

            return Path;
        }

        public dynamic GetItem(int ID)
        {
            return ItemDict[ID];
        }

        public void AddItem(dynamic item)
        {
            // Gives an ID to the item
            int ID = Size;

            // Increments the graph's size
            Size++;

            // Adds the item to the item dictionary
            ItemDict.Add(ID, item);

            // Gives every node a connection to the new item
            foreach (Dictionary<int, float> value in ConnectionDict.Values)
            {
                value.Add(ID, 0f);
            }

            // Adds the item to the connection dictionary
            ConnectionDict.Add(ID, new Dictionary<int, float>());

            // Gives the item a connection to every node
            for (int i = 0; i < Size; i++)
            {
                ConnectionDict[ID].Add(i, 0f);
            }
        }

        public void SetConnection(int ID1, int ID2, float Value)
        {
            // Sets value between the two nodes
            ConnectionDict[ID1][ID2] = Value;
            ConnectionDict[ID2][ID1] = Value;
        }

        public void SetConnection(int ID1, int ID2, float ValueFrom1, float ValueFrom2)
        {
            // Sets value between the two nodes. ValueFrom1 is from Node ID1 to Node ID2, and vice-versa
            ConnectionDict[ID1][ID2] = ValueFrom1;
            ConnectionDict[ID2][ID1] = ValueFrom2;
        }

        public void SetConnection(int ID, Dictionary<int, float> Connections)
        {
            foreach (int ID2 in Connections.Keys)
            {
                ConnectionDict[ID][ID2] = Connections[ID2];
                ConnectionDict[ID2][ID] = Connections[ID2];
            }
        }

        public void SetConnection(int ID, float[] Connections)
        {
            for (int i = 0; i < Connections.Length; i++)
            {
                ConnectionDict[ID][i] = Connections[i];
                ConnectionDict[i][ID] = Connections[i];
            }
        }

        public int GetSize()
        {
            return Size;
        }

        public float GetConnection(int ID1, int ID2)
        {
            return ConnectionDict[ID1][ID2];
        }

        public int GetID(dynamic item)
        {
            foreach (int ID in ItemDict.Keys)
            {
                if (ItemDict[ID] == item) return ID;
            }
            return -1;
        }

        public Dictionary<int, dynamic> GetItemDict() { return ItemDict; }

        public Dictionary<int, Dictionary<int, float>> GetConnectionDict() { return ConnectionDict; }
    }
}
