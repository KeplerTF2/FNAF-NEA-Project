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

            // DEBUG, checking Dijkstras
            foreach (int i in _graph.Dijkstra(2, 13))
            {
                if (i != -1)
                {
                    Room room = _graph.GetItem(i);
                    Debug.Write(room.GetName() + ", ");
                }
            }
            Debug.WriteLine("Dijkstras performed");
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

        public static Room GetRoom(int CamNum)
        {
            return _graph.GetItem(CamNumToID(CamNum));
        }
    }
}
