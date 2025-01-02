using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public class CNCharacter : IMonogame
    {
        private int Difficulty = 0;
        private Vector2 Pos;
        private SpriteItem BGSprite;
        private SpriteItem AnimatronicSprite;
        private SpriteItem ArrowSprite;
        private TextItem DifficultyText = new TextItem("DefaultFont", "0");
        private Animatronics Animatronic;
        private Button LeftButton;
        private Button RightButton;

        private static Dictionary<Animatronics, int> AIDict = new Dictionary<Animatronics, int>();

        public CNCharacter(Animatronics Animatronic, Vector2 Pos)
        {
            this.Animatronic = Animatronic;
            this.Pos = Pos;

            AIDict.Add(Animatronic, 0);

            LeftButton = new Button(new Rectangle((int)Pos.X + 16, (int)Pos.Y + 208, 32, 32));
            RightButton = new Button(new Rectangle((int)Pos.X + 144, (int)Pos.Y + 208, 32, 32));
            LeftButton.MousePressed += LowerDifficulty;
            RightButton.MousePressed += RaiseDifficulty;

            MonogameIManager.AddObject(this);
        }

        public void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(BGSprite);
            DrawManager.EnqueueItem(AnimatronicSprite);
            DrawManager.EnqueueItem(ArrowSprite);
            DrawManager.EnqueueItem(DifficultyText);
        }

        public void Initialize() { }

        public void LoadContent()
        {
            // Sprites
            BGSprite = new SpriteItem("CNBox");
            BGSprite.dp.Pos = Pos;
            BGSprite.dp.Scale = new Vector2(4);

            AnimatronicSprite = new SpriteItem("CustomNight/" + Animatronic);
            AnimatronicSprite.dp.Pos = Pos;
            AnimatronicSprite.dp.Scale = new Vector2(4);

            ArrowSprite = new SpriteItem("CNArrows");
            ArrowSprite.dp.Pos = Pos;
            ArrowSprite.dp.Scale = new Vector2(4);

            DifficultyText.dp.Pos = Pos + new Vector2(88, 200);
            DifficultyText.dp.Scale = new Vector2(0.5f);
        }

        public void Update(GameTime gameTime)
        {

        }

        private void LowerDifficulty()
        {
            Difficulty--;
            if (Difficulty < 0) Difficulty = 20;

            AIDict[Animatronic] = Difficulty;
            DifficultyText.Text = Difficulty.ToString();
        }

        private void RaiseDifficulty()
        {
            Difficulty++;
            if (Difficulty > 20) Difficulty = 0;

            AIDict[Animatronic] = Difficulty;
            DifficultyText.Text = Difficulty.ToString();
        }

        public static Dictionary<Animatronics, int> GetAIDict()
        {
            return AIDict;
        }

        public static void ClearAIDict()
        {
            AIDict = new Dictionary<Animatronics, int>();
        }
    }
}
