using MiniGame.Logic.Entities;
using MiniGame.Logic.Entities.Cells;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniGame.Logic
{
    public struct Coordinate
    {
        public int Row { get; set; }

        public int Column { get; set; }

        public Coordinate(int row = 0, int column = 0)
        {
            Row = row;
            Column = column;
        }

        public override int GetHashCode()
        {
            return Row.GetHashCode() ^ Column.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Coordinate))
                return false;

            var coordinate = (Coordinate) obj;
            return Row == coordinate.Row && Column == coordinate.Column;
        }
    }

    public class GameMap
    {
        public static readonly int MIN_SIZE;

        public static readonly int MAX_SIZE;

        static GameMap()
        {
            MIN_SIZE = Card.MIN_COUNT_OF_COLORS * 2 - 1;
            MAX_SIZE = Card.GetAllAvailableColors().Length * 2 - 1;
        }

        public static Card[] GenerateAvailableCards(int gameMapSize, bool randomize = true)
        {
            if (gameMapSize < MIN_SIZE)
                throw new ArgumentOutOfRangeException();

            if (gameMapSize > MAX_SIZE)
                throw new ArgumentOutOfRangeException();

            if (gameMapSize % 2 == 0)
                throw new ArgumentException();

            var countOfColumnsWithCards = gameMapSize / 2 + 1;

            var allCardColors = Card.GetAllAvailableColors();
            var availableCardColors = allCardColors.Take(countOfColumnsWithCards).ToArray();

            var cards = new List<Card>();

            foreach (var cardColor in availableCardColors)
            {
                for (var i = 0; i < gameMapSize; i++)
                {
                    cards.Add(new Card(cardColor));
                }
            }

            if (randomize)
            {
                var random = new Random();
                return cards.OrderBy(i => random.Next()).ToArray();
            }

            return cards.ToArray();
        }

        public int Size { get { return _cells.GetLength(0); } }

        public GameMap(int size)
        {
            InitCells(size);
        }

        public Cell this[Coordinate coordinate]
        {
            get
            {
                if (!CheckIfCoordinateIsValid(coordinate))
                    throw new ArgumentOutOfRangeException();

                return _cells[coordinate.Row, coordinate.Column];
            }
            private set
            {
                if (!CheckIfCoordinateIsValid(coordinate))
                    throw new ArgumentOutOfRangeException();

                _cells[coordinate.Row, coordinate.Column] = value ?? throw new ArgumentNullException();
            }
        }

        public bool CheckIfCoordinateIsValid(Coordinate coordinate)
        {
            return  coordinate.Row >= 0 &&
                    coordinate.Row < Size &&
                    coordinate.Column >= 0 &&
                    coordinate.Column < Size;
        }

        public bool CheckIfCoordinatesAreClosest(Coordinate coordinate1, Coordinate coordinate2)
        {
            if (coordinate1.Equals(coordinate2))
                return false;

            if (!CheckIfCoordinateIsValid(coordinate1) || !CheckIfCoordinateIsValid(coordinate2))
                return false;

            var closestHorizontally = Math.Abs(coordinate1.Column - coordinate2.Column) == 1;
            var closestVertically = Math.Abs(coordinate1.Row - coordinate2.Row) == 1;

            return (closestHorizontally || closestVertically) && closestHorizontally != closestVertically;
        }

        public bool Swap(Coordinate coordinate1, Coordinate coordinate2)
        {
            if (coordinate1.Equals(coordinate2))
                return false;

            if (!CheckIfCoordinateIsValid(coordinate1) || !CheckIfCoordinateIsValid(coordinate2))
                return false;

            if (!CheckIfCoordinatesAreClosest(coordinate1, coordinate2))
                return false;

            if (!this[coordinate1].CanSwap(this[coordinate2]))
                return false;

            var temp = this[coordinate1];
            this[coordinate1] = this[coordinate2];
            this[coordinate2] = temp;

            return true;
        }

        private Cell[,] _cells;

        private void InitCells(int size)
        {
            if (size < MIN_SIZE)
                throw new ArgumentOutOfRangeException();

            if (size > MAX_SIZE)
                throw new ArgumentOutOfRangeException();

            if (size % 2 == 0)
                throw new ArgumentException();

            _cells = new Cell[size, size];

            var cards = new Stack<Card>(GenerateAvailableCards(size));

            for (var row = 0; row < _cells.GetLength(0); row++)
            {
                for (var column = 0; column < _cells.GetLength(1); column++)
                {
                    if (column % 2 == 0)
                    {
                        _cells[row, column] = cards.Pop();
                    }
                    else
                    {
                        if (row % 2 == 0)
                        {
                            _cells[row, column] = new Block();
                        }
                        else
                        {
                            _cells[row, column] = new EmptyCell();
                        }
                    }
                }
            }
        }
    }
}
