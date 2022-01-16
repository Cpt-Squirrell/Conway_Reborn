using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Conway_Reborn
{
    class Cell
    {
        Gameboard _gameboard;
        public Vector2 Position;

        public Cell(Gameboard gameboard, Vector2 position, Cell[][] array)
        {
            if (!(array is Cell[][]))
                throw new System.Exception();
            _gameboard = gameboard;
            Position = position;
        }

        public Vector2[] getSurroundingPositions()
        {
            return _gameboard.getSurroundings(Position);
        }

        public Cell[] getAliveNeighbours()
        {
            List<Cell> neighbours = new List<Cell>();
            foreach (Vector2 grid in _gameboard.getSurroundings(Position))
                if (_gameboard.getCell(grid) is Cell)
                    neighbours.Add(_gameboard.getCell(grid));
            return neighbours.ToArray();
        }

        public Cell[] getNeighbours()
        {
            Cell[] neighbours = new Cell[8];
            Vector2[] surroundings = getSurroundingPositions();
            for (int index = 0; index < 8; index++)
                neighbours[index] = _gameboard.getCell(surroundings[index]);
            return neighbours;
        }

        public bool isDying()
        {
            if (getAliveNeighbours().Length < 3 || getAliveNeighbours().Length > 4)
                return true;
            return false;
        }
    }
}