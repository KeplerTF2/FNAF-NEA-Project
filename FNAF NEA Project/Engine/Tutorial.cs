using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FNAF_NEA_Project.Engine
{
    public class Tutorial: IMonogame
    {
        private RectItem Rect;
        private TextItem Text;
        private string[] Information;
        private int CurrentPage = 0;
        private SpriteItem ArrowSprite;
        private Button NextButton;

        public Tutorial()
        {
            NextButton = new Button(new Rectangle(568 + 64, 152 + 96, 64, 32));
            NextButton.MouseReleased += Continue;

            MonogameIManager.AddObject(this);
        }

        public void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(Rect);
            DrawManager.EnqueueItem(Text);
            DrawManager.EnqueueItem(ArrowSprite);
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            Rect = new RectItem(640, 192);
            Rect.dp.Pos = new Vector2(64, 96);
            Rect.dp.Colour = new Color(0f, 0f, 0f, 0.67f);
            Rect.ZIndex = 10;

            Text = new TextItem("PixelFont", "test");
            Text.dp.Pos = new Vector2(64 + 8, 96 + 8);
            Text.dp.Scale = new Vector2(0.25f);
            Text.ZIndex = 10;

            ArrowSprite = new SpriteItem("NextButton");
            ArrowSprite.dp.Pos = new Vector2(568 + 64, 152 + 96);
            ArrowSprite.dp.Scale = new Vector2(4);
            ArrowSprite.ZIndex = 10;

            LoadTutorial(Game1.GetOfficeScene().NightNum);
        }

        public void Update(GameTime gameTime)
        {
        }

        private void LoadTutorial(int NightNum)
        {
            switch (NightNum)
            {
                case 1:
                    Information = new string[] {
                        "Welcome to your first night.\nIn the office, you have 2\ndoors and a light beacon in\nthe hallway to defend\nyourself.",
                        "You can use the cameras to\nsee where the animatronics\nare. You can bring them up\neither with the mouse or by\npressing S.",
                        "The temperature in each room\nwill slowly rise. Keep the\nrooms cool, as animatronics\nmove faster in hot rooms.",
                        "Everything you do uses power.\nIn Cam 05, there is a power\ngenerator you can use to gain\nsome of it back.",
                        "There are 2 threats tonight:\nBonnie the Blue Bunny, and\nChica the Yellow Chicken",
                        "Bonnie will make his way to\nCam 06. When he leaves Cam 06,\nclose the left door until you\nhear banging.",
                        "Chica will make her way to\nCam 07. When she leaves\nCam 07, close the right door\nuntil you hear banging.",
                        "That's everything for tonight.\nGood luck!"
                    };
                    break;
                case 2:
                    Information = new string[] {
                        "There's a new threat tonight:\nFoxy the Red Fox.",
                        "He will make his way to\nCam 04.",
                        "Once he leaves Cam 04, flash\nthe hallway light at him.",
                        "After that, he will run away."
                    };
                    break;
                case 3:
                    Information = new string[] {
                        "Golden Freddy will sometimes\nappear in Cam 05. When he\ndoes, you will hear him laugh.",
                        "He is there to sabotage your\npower generator! Repair it\nwhen he does this. Otherwise\nhe will cost you a lot of\npower.",
                        "After that, he will go away\nfor a bit."
                    };
                    break;
                case 4:
                    Information = new string[] {
                        "Helpy will sometimes run\nacross your office, making a\nhigh pitch laugh when he does.",
                        "If he gets to the other side\nof the office, he will kill\nyou!",
                        "Click on his nose to send him\naway."
                    };
                    break;
                case 5:
                    Information = new string[] {
                        "Tonight, there is a new and\nfinal threat:\nFreddy the Brown Bear.",
                        "Freddy can attack either door\nor the hallway. Deal with him\nlike you would Bonnie, Chica\nor Foxy.",
                        "Good luck."
                    };
                    break;
                default:
                    EndTutorial();
                    break;
            }

            // If still in tutorial, set next page
            if (Game1.GetOfficeScene().InTutorial)
            {
                Text.Text = Information[CurrentPage];
            }
        }

        private void Continue()
        {
            CurrentPage++;

            if (CurrentPage >= Information.Length) EndTutorial();
            else Text.Text = Information[CurrentPage];
        }

        private void EndTutorial()
        {
            Rect.Visible = false;
            Text.Visible = false;
            ArrowSprite.Visible = false;
            NextButton.SetActive(false);
            Game1.GetOfficeScene().InTutorial = false;
        }
    }
}
