using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    // Contains a graph of all rooms
    public class Building
    {
        public static Graph _graph;
        public static Building _building;

        public Building()
        {
            _graph = new Graph();

            // Creates rooms
            _graph.AddItem(new Room('B', "Parts & Service", 1)); // 0 - Cam 01 - Parts & Service
            _graph.AddItem(new Room('B', "Side Stage Area"));    // 1 - Side Stage Area
            _graph.AddItem(new Room('A', "Stage"));              // 2 - Stage
            _graph.AddItem(new Room('A', "Dining Area", 3));     // 3 - Cam 03 - Dining Area
            _graph.AddItem(new Room('A', "Bathrooms"));          // 4 - Bathrooms
            _graph.AddItem(new Room('F', "Kitchen", 2));         // 5 - Cam 02 - Kitchen
            _graph.AddItem(new Room('C', "Main Hallway", 4));    // 6 - Cam 04 - Main Hallway
            _graph.AddItem(new Room('G', "Supply Room", 5));     // 7 - Cam 05 - Supply Room
            _graph.AddItem(new Room('E', "Left Hallway", 6));    // 8 - Cam 06 - Left Hallway
            _graph.AddItem(new Room('E', "Left Entrance"));      // 9 - Left Door Entrance
            _graph.AddItem(new Room('C', "Main Entrance"));      // 10 - Hallway Entrance
            _graph.AddItem(new Room('D', "Right Entrance"));     // 11 - Right Door Entrance
            _graph.AddItem(new Room('D', "Right Hallway", 7));   // 12 - Cam 07 - Right Hallway
            _graph.AddItem(new Room('_', "Office"));             // 13 - Office

            // Sets connections
            _graph.SetConnection(0, 1, 0.5f);
            _graph.SetConnection(1, 2, 0.5f);
            _graph.SetConnection(2, 3, 0.5f);
            _graph.SetConnection(3, 4, 1f);
            _graph.SetConnection(3, 12, 3f);
            _graph.SetConnection(5, 8, 1f);
            _graph.SetConnection(6, 0, 1f);
            _graph.SetConnection(6, 1, 1.5f);
            _graph.SetConnection(6, 3, 2f);
            _graph.SetConnection(6, 5, 1f);
            _graph.SetConnection(6, 7, 1f);
            _graph.SetConnection(6, 8, 2f);
            _graph.SetConnection(6, 10, 1f);
            _graph.SetConnection(6, 12, 1.5f);
            _graph.SetConnection(7, 8, 1f);
            _graph.SetConnection(8, 9, 1f);
            _graph.SetConnection(11, 12, 1f);
            _graph.SetConnection(13, 9, 0f, 1f);
            _graph.SetConnection(13, 10, 0f, 1f);
            _graph.SetConnection(13, 11, 0f, 1f);

            _building = this;
        }

        public static int CamNumToID(int ID)
        {
            switch (ID)
            {
                case 1: return 0;
                case 2: return 5;
                case 3: return 3;
                case 4: return 6;
                case 5: return 7;
                case 6: return 8;
                case 7: return 12;
            }
            return -1;
        }

        public static int IDToCamNum(int CamNum)
        {
            switch (CamNum)
            {
                case 0: return 1;
                case 5: return 2;
                case 3: return 3;
                case 6: return 4;
                case 7: return 5;
                case 8: return 6;
                case 12: return 7;
            }
            return -1;
        }

        public static Room GetRoom(int CamNum)
        {
            return _graph.GetItem(CamNumToID(CamNum));
        }

        public static int GetNextRoom(int SourceID, int TargetID = 13)
        {
            if (_building.GetTempGraph().Dijkstra(SourceID, TargetID).Count > 1)
                return _building.GetTempGraph().Dijkstra(SourceID, TargetID)[1];
            else if (_building.GetTempGraph().Dijkstra(SourceID, TargetID).Count == 1)
                return _building.GetTempGraph().Dijkstra(SourceID, TargetID)[0];
            else
                return -1;
        }

        public static float GetTempRoomTime(int Room1, int Room2)
        {
            return _building.GetTempGraph().GetConnection(Room1, Room2);
        }

        // Returns a graph that takes temperature of rooms into account
        private Graph GetTempGraph()
        {
            Graph graph = new Graph();

            // Creates clone of graph
            foreach (int Node1 in _graph.GetConnectionDict().Keys)
                graph.AddItem(_graph.GetItem(Node1));

            // Set connections after adding items, or else the graph will try assigning to non-existant rooms
            foreach (int Node1 in _graph.GetConnectionDict().Keys)
                graph.SetConnection(Node1, _graph.GetConnectionDict()[Node1]);

            // Applies temperature modifiers
            foreach (int Node1 in graph.GetConnectionDict().Keys)
            {
                foreach (int Node2 in graph.GetConnectionDict()[Node1].Keys)
                {
                    Room Room1 = graph.GetItem(Node1);
                    Room Room2 = graph.GetItem(Node2);

                    // Get temperature movement multiplers
                    float Room1Mult = (TemperatureGroups.GetTemperature(Room1.GetTempGroup()) / -2f) + 1f;
                    float Room2Mult = (TemperatureGroups.GetTemperature(Room2.GetTempGroup()) / -2f) + 1f;

                    // Get overall multipler
                    float Value = (Room1Mult + Room2Mult) / 2f;

                    // Applies multiplier to graph (making sure not to apply multiplier twice)
                    if (Node1 != 13 && Node2 != 13)
                        graph.SetConnection(Node1, Node2, graph.GetConnection(Node1, Node2) * Value, graph.GetConnection(Node2, Node1));
                    else
                        graph.SetConnection(Node1, Node2, graph.GetConnection(Node1, Node2) * Value, graph.GetConnection(Node2, Node1));
                }
            }

            return graph;
        }
    }
}
