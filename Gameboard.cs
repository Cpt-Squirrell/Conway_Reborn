using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Conway_Reborn
{
    class Gameboard
    {
        private Texture2D _cellTexture;
        public Texture2D CellTexture { get {return _cellTexture; } }
        private GraphicsDevice _graphicsDevice;
        private int _cellScale;
        public int CellScale { get {return _cellScale; } }
        private List<Cell> _aliveCells;
        private Cell[][] _board;
            public void addCell(Vector2 position)
            {
                //TODO: Make _board able to expand if required
                //Prevent out of bounds exception
                //Try Catch try-again ?
                //Also center negative values to < half sizes
                _board[(int)position.X][(int)position.Y]
                    = new Cell(this, position, _board);
                
                _aliveCells.Add(_board[(int)position.X][(int)position.Y]);
            }
            public void removeCell(Vector2 position)
            {
                _aliveCells.Remove(_board[(int)position.X][(int)position.Y]);
                _board[(int)position.X][(int)position.Y]
                    = null;
            }
            public Cell getCell(Vector2 position)
            {
                try {
                return _board[(int)position.X][(int)position.Y];
                } catch (System.IndexOutOfRangeException) { return null; }
            }

        public Gameboard(GraphicsDevice graphicsDevice, List<Cell> aliveCells)
        {
            _graphicsDevice = graphicsDevice;
            _cellScale = 8;
            _aliveCells = aliveCells;
            _board = new Cell[512][];
            for (int index = 0; index < _board.Length; index++)
                _board[index] = new Cell[512];
            refreshTextures();
        }

        public Vector2[] getSurroundings(Vector2 position)
        {
            Vector2[] surrounding = new Vector2[]
            {
                new Vector2(position.X - 1, position.Y - 1),
                new Vector2(position.X + 0, position.Y - 1),
                new Vector2(position.X + 1, position.Y - 1),
                new Vector2(position.X - 1, position.Y + 0),
                new Vector2(position.X + 1, position.Y + 0),
                new Vector2(position.X - 1, position.Y + 1),
                new Vector2(position.X + 0, position.Y + 1),
                new Vector2(position.X + 1, position.Y + 1)
            };
            return surrounding;
        }

        private void refreshTextures() //Make new textures using the scales
        {
            _cellTexture = new Texture2D(_graphicsDevice, _cellScale, _cellScale); //Declare texture
            Color[] textureColor = new Color[_cellScale * _cellScale]; //New Color[] to fill texture with
            for (int _index = 0; _index < textureColor.Length; _index++) //Populate _textureColor
                textureColor[_index] = Color.Black;
            for (int _index = 0; _index < _cellScale * _cellScale; _index++) //Fill texture with _textureColor
                _cellTexture.SetData<Color>(textureColor);
        }
    }
}