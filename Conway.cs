using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace Conway_Reborn
{
    public class Conway : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Gameboard _gameboard;
        private List<Cell> _aliveCells;
        private Random _random;

        private Vector2 _cameraPosition;
        private Stopwatch _deltaTime = new Stopwatch();

        public Conway()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _aliveCells = new List<Cell>();
            _gameboard = new Gameboard(GraphicsDevice, _aliveCells);
            _random = new Random();

            _gameboard.addCell(new Vector2(20, 20));
            _gameboard.addCell(new Vector2(20, 21));
            _gameboard.addCell(new Vector2(20, 22));
            _gameboard.addCell(new Vector2(21, 20));
            _gameboard.addCell(new Vector2(21, 21));
            _gameboard.addCell(new Vector2(21, 22));
            _gameboard.addCell(new Vector2(22, 20));
            _gameboard.addCell(new Vector2(22, 21));
            _gameboard.addCell(new Vector2(22, 22));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                int _randomX;
                int _randomY;
                for (int index = 0; index < 1000; index++)
                {
                    _randomX = _random.Next(300, 400);
                    _randomY = _random.Next(300, 400);
                    if (_gameboard.getCell(new Vector2(_randomX, _randomY)) is null)
                        _gameboard.addCell(new Vector2(_randomX, _randomY));
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                _cameraPosition.X += 10;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                _cameraPosition.Y -= 10;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                _cameraPosition.X -= 10;
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                _cameraPosition.Y += 10;

            //Check simulation if enough time has passed
            _deltaTime.Start(); //Make sure timer is started
            if (_deltaTime.Elapsed > TimeSpan.FromMilliseconds(1000))
            { _deltaTime.Reset(); //Reset the delta

                //Resurrecting and killing of Cells \/

                //TODO: Make resurrecting faster by
                //not checking the same dead cells over and over
                //Resolve bug where it affects cells not within search radius
                Cell[] aliveCellsBuffer = _aliveCells.ToArray();
                foreach (Cell cell in aliveCellsBuffer)
                    foreach (Vector2 subCell in cell.getSurroundingPositions())
                        if (_gameboard.getCell(subCell) is null)
                        {
                            int alive = 0;
                            foreach (Vector2 position in _gameboard.getSurroundings(subCell))
                                if (_gameboard.getCell(position) is Cell)
                                    alive++;
                            if (alive == 3) //If this position has exactly three neighbours then resurrect
                                _gameboard.addCell(subCell);
                        }

                Cell[] aliveCellsArray = _aliveCells.ToArray();
                foreach (Cell cell in aliveCellsArray)
                    if (cell.isDying())
                        _gameboard.removeCell(cell.Position);
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            foreach(Cell cell in _aliveCells)
                _spriteBatch.Draw(_gameboard.CellTexture, cell.Position * _gameboard.CellScale + _cameraPosition, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}