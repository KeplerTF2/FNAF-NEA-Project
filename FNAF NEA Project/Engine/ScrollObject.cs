using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NEA_Project.Engine;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public class ScrollObject
    {
        private static Dictionary<string, ScrollObject> List = new Dictionary<string, ScrollObject>();
        private string ID;

        // Range of the input
        private int MinRange;
        private int MaxRange;

        // Domain of the output
        private int MinOut;
        private int MaxOut;

        public bool ScrollCapped = true;
        public bool InvertedScroll = false;

        public ScrollObject(string iD, int minRange, int maxRange, int minOut, int maxOut)
        {
            ID = iD;
            MinRange = minRange;
            MaxRange = maxRange;
            MinOut = minOut;
            MaxOut = maxOut;

            List.Add(ID, this);
        }

        public ScrollObject(string iD, int minRange, int maxRange, int minOut, int maxOut, bool scrollCapped)
        {
            ID = iD;
            MinRange = minRange;
            MaxRange = maxRange;
            MinOut = minOut;
            MaxOut = maxOut;
            ScrollCapped = scrollCapped;

            List.Add(ID, this);
        }

        public ScrollObject(string iD, int minRange, int maxRange, int minOut, int maxOut, bool scrollCapped, bool invertedScroll)
        {
            ID = iD;
            MinRange = minRange;
            MaxRange = maxRange;
            MinOut = minOut;
            MaxOut = maxOut;
            ScrollCapped = scrollCapped;
            InvertedScroll = invertedScroll;

            List.Add(ID, this);
        }

        // Returns the list of all scrolling objects
        public static Dictionary<string, ScrollObject> GetList()
        {
            return List;
        }

        // Sets the range of input scroll
        public void SetRange(int min, int max)
        {
            MinRange = min; MaxRange = max;
        }

        // Returns the range of input scroll
        public (int, int) GetRange()
        {
            return (MinRange, MaxRange);
        }

        // Sets the range of output scroll
        public void SetOut(int min, int max)
        {
            MinOut = min; MaxOut = max;
        }

        // Returns the range of output scroll
        public (int, int) GetOut()
        {
            return (MinOut, MaxOut);
        }

        // Changes the ID of the scroll object
        public void SetID(string iD)
        {
            List.Remove(ID);
            ID = iD;
            List.Add(ID, this);
        }

        // Return the ID of scroll object
        public string GetID()
        {
            return ID;
        }

        // Returns the output scroll amount
        public float GetScrollAmount()
        {
            // Declares variables
            float x = ((float)Mouse.GetState().X / (float)GlobalCamera.WindowSize.X) * (float)GlobalCamera.Size.X;
            float ScrollAmount = x;

            // Inverts scroll if needed
            if (InvertedScroll) ScrollAmount = (float)GlobalCamera.Size.X - ScrollAmount;

            // Calculations
            ScrollAmount -= MinRange;
            ScrollAmount /= MaxRange - MinRange;
            ScrollAmount *= MaxOut - MinOut;
            ScrollAmount += MinOut;

            // Caps the scroll if needed
            if (ScrollCapped)
            {
                ScrollAmount = Math.Min(MaxOut, ScrollAmount);
                ScrollAmount = Math.Max(MinOut, ScrollAmount);
            }

            return ScrollAmount;
        }
    }
}
