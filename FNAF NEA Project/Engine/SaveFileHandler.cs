using FNAF_NEA_Project.Engine.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public static class SaveFileHandler
    {
        const string DirectoryName = @"C:\FNAF_NEA";
        const string PathName = @"C:\FNAF_NEA\SaveFile.txt";

        public static bool ReadSaveData()
        {
            if (File.Exists(PathName))
            {
                string[] Data = File.ReadAllLines(PathName);

                if (Data.Length >= 4)
                {
                    SaveData.NightNum = Convert.ToInt32(Data[0]);
                    SaveData.CustomNight = Convert.ToBoolean(Data[1]);
                    SaveData.MaxMode = Convert.ToBoolean(Data[2]);
                    SaveData.MaxModeAllChallenges = Convert.ToBoolean(Data[3]);
                }
                else return false;

                return true;
            }
            else return false;
        }

        public static void WriteSaveData()
        {
            if (!Directory.Exists(DirectoryName)) Directory.CreateDirectory(DirectoryName);
            File.WriteAllText(PathName, SaveData.NightNum + "\n" + SaveData.CustomNight + "\n" + SaveData.MaxMode + "\n" + SaveData.MaxModeAllChallenges);
        }
    }
}
