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
        const string DICT_NAME = @"C:\FNAF_NEA";
        const string PATH_NAME = @"C:\FNAF_NEA\SaveFile.txt";

        public static bool ReadSaveData()
        {
            if (File.Exists(PATH_NAME))
            {
                string[] Data = File.ReadAllLines(PATH_NAME);

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
            if (!Directory.Exists(DICT_NAME)) Directory.CreateDirectory(DICT_NAME);
            File.WriteAllText(PATH_NAME, SaveData.NightNum + "\n" + SaveData.CustomNight + "\n" + SaveData.MaxMode + "\n" + SaveData.MaxModeAllChallenges);
        }
    }
}
