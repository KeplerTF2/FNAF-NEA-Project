using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public static class NightSettings
    {
        public static Dictionary<Animatronics, int> CustomAI = new Dictionary<Animatronics, int>();

        public static Dictionary<Animatronics, int> GetAIS(int NightNum)
        {
            Dictionary<Animatronics, int> AnimDict = new Dictionary<Animatronics, int>();

            switch (NightNum)
            {
                case 1:
                    AnimDict.Add(Animatronics.Freddy, 0);
                    AnimDict.Add(Animatronics.Bonnie, 2);
                    AnimDict.Add(Animatronics.Chica, 2);
                    AnimDict.Add(Animatronics.Foxy, 0);
                    AnimDict.Add(Animatronics.GoldenFreddy, 0);
                    AnimDict.Add(Animatronics.Helpy, 0);
                    break;
                case 2:
                    AnimDict.Add(Animatronics.Freddy, 0);
                    AnimDict.Add(Animatronics.Bonnie, 4);
                    AnimDict.Add(Animatronics.Chica, 4);
                    AnimDict.Add(Animatronics.Foxy, 2);
                    AnimDict.Add(Animatronics.GoldenFreddy, 0);
                    AnimDict.Add(Animatronics.Helpy, 0);
                    break;
                case 3:
                    AnimDict.Add(Animatronics.Freddy, 0);
                    AnimDict.Add(Animatronics.Bonnie, 6);
                    AnimDict.Add(Animatronics.Chica, 6);
                    AnimDict.Add(Animatronics.Foxy, 4);
                    AnimDict.Add(Animatronics.GoldenFreddy, 3);
                    AnimDict.Add(Animatronics.Helpy, 0);
                    break;
                case 4:
                    AnimDict.Add(Animatronics.Freddy, 0);
                    AnimDict.Add(Animatronics.Bonnie, 8);
                    AnimDict.Add(Animatronics.Chica, 8);
                    AnimDict.Add(Animatronics.Foxy, 6);
                    AnimDict.Add(Animatronics.GoldenFreddy, 5);
                    AnimDict.Add(Animatronics.Helpy, 3);
                    break;
                case 5:
                    AnimDict.Add(Animatronics.Freddy, 4);
                    AnimDict.Add(Animatronics.Bonnie, 10);
                    AnimDict.Add(Animatronics.Chica, 10);
                    AnimDict.Add(Animatronics.Foxy, 8);
                    AnimDict.Add(Animatronics.GoldenFreddy, 8);
                    AnimDict.Add(Animatronics.Helpy, 5);
                    break;
                case 6:
                    AnimDict.Add(Animatronics.Freddy, 8);
                    AnimDict.Add(Animatronics.Bonnie, 12);
                    AnimDict.Add(Animatronics.Chica, 12);
                    AnimDict.Add(Animatronics.Foxy, 10);
                    AnimDict.Add(Animatronics.GoldenFreddy, 10);
                    AnimDict.Add(Animatronics.Helpy, 8);
                    break;
                // Secrets for anyone who modifies the save file
                case 8:
                    AnimDict.Add(Animatronics.Freddy, 22);
                    AnimDict.Add(Animatronics.Bonnie, 22);
                    AnimDict.Add(Animatronics.Chica, 22);
                    AnimDict.Add(Animatronics.Foxy, 22);
                    AnimDict.Add(Animatronics.GoldenFreddy, 25);
                    AnimDict.Add(Animatronics.Helpy, 25);
                    break;
                case 83:
                    AnimDict.Add(Animatronics.Freddy, 0);
                    AnimDict.Add(Animatronics.Bonnie, 0);
                    AnimDict.Add(Animatronics.Chica, 0);
                    AnimDict.Add(Animatronics.Foxy, 0);
                    AnimDict.Add(Animatronics.GoldenFreddy, 50);
                    AnimDict.Add(Animatronics.Helpy, 0);
                    break;
                case 87:
                    AnimDict.Add(Animatronics.Freddy, 30);
                    AnimDict.Add(Animatronics.Bonnie, 0);
                    AnimDict.Add(Animatronics.Chica, 0);
                    AnimDict.Add(Animatronics.Foxy, 0);
                    AnimDict.Add(Animatronics.GoldenFreddy, 0);
                    AnimDict.Add(Animatronics.Helpy, 0);
                    break;
                case 0:
                    AnimDict.Add(Animatronics.Freddy, 0);
                    AnimDict.Add(Animatronics.Bonnie, 0);
                    AnimDict.Add(Animatronics.Chica, 0);
                    AnimDict.Add(Animatronics.Foxy, 0);
                    AnimDict.Add(Animatronics.GoldenFreddy, 0);
                    AnimDict.Add(Animatronics.Helpy, 50);
                    break;
                // default
                default:
                    AnimDict.Add(Animatronics.Freddy, 0);
                    AnimDict.Add(Animatronics.Bonnie, 0);
                    AnimDict.Add(Animatronics.Chica, 0);
                    AnimDict.Add(Animatronics.Foxy, 0);
                    AnimDict.Add(Animatronics.GoldenFreddy, 0);
                    AnimDict.Add(Animatronics.Helpy, 0);
                    break;
            }
            return AnimDict;
        }

    }
}
