using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace FNAF_NEA_Project.Engine
{
    // Makes it so that you don't have to call the methods for every instance of a class in the scene,
    // but rather you just add the object to this manager on initialisation and have it all done automatically

    public static class MonogameIManager
    {
        private static List<IMonogame> ObjectList = new List<IMonogame>();

        // Adds an object implementing IMonogame to the list
        public static void AddObject(IMonogame obj)
        {
            ObjectList.Add(obj);
        }

        // Removes an object implementing IMongoame from the list
        public static void RemoveObject(IMonogame obj)
        {
            ObjectList.Remove(obj);
        }

        // Clears the list, used on scene transitions
        public static void Clear()
        {
            ObjectList.Clear();
        }

        // Calls the Draw function for every object
        public static void Draw(GameTime gameTime)
        {
            foreach (IMonogame obj in ObjectList) { obj.Draw(gameTime); }
        }

        // Calls the Initialize function for every object
        public static void Initialize()
        {
            foreach (IMonogame obj in ObjectList) { obj.Initialize(); }
        }

        // Calls the LoadContent function for every object
        public static void LoadContent()
        {
            foreach (IMonogame obj in ObjectList) { obj.LoadContent(); }
        }

        // Calls the Update function for every object
        public static void Update(GameTime gameTime)
        {
            foreach (IMonogame obj in ObjectList) { obj.Update(gameTime); }
        }
    }
}
