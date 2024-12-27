using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public class Room
    {
        private bool OnCams;
        private char TempGroup;
        private string Name;
        private int Num;

        public Room()
        {
            TempGroup = 'A';
            Num = 1;
        }

        public Room(char TempGroup, string Name)
        {
            OnCams = false;
            this.TempGroup = TempGroup;
            this.Name = Name;
        }

        public Room(char TempGroup, string Name, int Num)
        {
            OnCams = true;
            this.TempGroup = TempGroup;
            this.Num = Num;
            this.Name = Name;
        }

        public string GetName()
        {
            return Name;
        }

        public char GetTempGroup()
        {
            return TempGroup;
        }
    }
}
