using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public static class NightSettings
    {
        public static Dictionary<Animatronics, int> GetAIS(int NightNum)
        {
            Dictionary<Animatronics, int> AnimDict = new Dictionary<Animatronics, int>();

            switch (NightNum)
            {
                case 1:
                    AnimDict.Add(Animatronics.Freddy, 0);
                    AnimDict.Add(Animatronics.Bonnie, 1);
                    AnimDict.Add(Animatronics.Chica, 2);
                    AnimDict.Add(Animatronics.Foxy, 0);
                    AnimDict.Add(Animatronics.GoldenFreddy, 0);
                    AnimDict.Add(Animatronics.Helpy, 0);
                    break;
                case 2:
                    AnimDict.Add(Animatronics.Freddy, 0);
                    AnimDict.Add(Animatronics.Bonnie, 3);
                    AnimDict.Add(Animatronics.Chica, 3);
                    AnimDict.Add(Animatronics.Foxy, 1);
                    AnimDict.Add(Animatronics.GoldenFreddy, 0);
                    AnimDict.Add(Animatronics.Helpy, 0);
                    break;
                case 3:
                    AnimDict.Add(Animatronics.Freddy, 0);
                    AnimDict.Add(Animatronics.Bonnie, 4);
                    AnimDict.Add(Animatronics.Chica, 3);
                    AnimDict.Add(Animatronics.Foxy, 2);
                    AnimDict.Add(Animatronics.GoldenFreddy, 1);
                    AnimDict.Add(Animatronics.Helpy, 0);
                    break;
                case 4:
                    AnimDict.Add(Animatronics.Freddy, 0);
                    AnimDict.Add(Animatronics.Bonnie, 5);
                    AnimDict.Add(Animatronics.Chica, 5);
                    AnimDict.Add(Animatronics.Foxy, 4);
                    AnimDict.Add(Animatronics.GoldenFreddy, 3);
                    AnimDict.Add(Animatronics.Helpy, 1);
                    break;
                case 5:
                    AnimDict.Add(Animatronics.Freddy, 3);
                    AnimDict.Add(Animatronics.Bonnie, 7);
                    AnimDict.Add(Animatronics.Chica, 7);
                    AnimDict.Add(Animatronics.Foxy, 6);
                    AnimDict.Add(Animatronics.GoldenFreddy, 5);
                    AnimDict.Add(Animatronics.Helpy, 5);
                    break;
                case 6:
                    AnimDict.Add(Animatronics.Freddy, 6);
                    AnimDict.Add(Animatronics.Bonnie, 9);
                    AnimDict.Add(Animatronics.Chica, 9);
                    AnimDict.Add(Animatronics.Foxy, 8);
                    AnimDict.Add(Animatronics.GoldenFreddy, 8);
                    AnimDict.Add(Animatronics.Helpy, 7);
                    break;
                case 8:
                    AnimDict.Add(Animatronics.Freddy, 0);
                    AnimDict.Add(Animatronics.Bonnie, 0);
                    AnimDict.Add(Animatronics.Chica, 0);
                    AnimDict.Add(Animatronics.Foxy, 0);
                    AnimDict.Add(Animatronics.GoldenFreddy, 0);
                    AnimDict.Add(Animatronics.Helpy, 0);
                    break;
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
