using MiniGame.Logic.Entities;
using MiniGame.Logic.Entities.Cells;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniGame.Logic
{
    /// <summary>
    /// Grid coordinate
    /// </summary>
    public struct Coordinate
    {
        /// <summary>
        /// Row in grid
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Column in grid
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Creates an instance of grid coordinate
        /// </summary>
        /// <param name="row">Row in grid</param>
        /// <param name="column">Column in grid</param>
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

    /// <summary>
    /// Arguments of the event which occurs in case of success swap
    /// </summary>
    public struct SuccessSwapEventArgs
    {
        public Coordinate Coordinate1 { get; set; }

        public Coordinate Coordinate2 { get; set; }
    }

    /// <summary>
    /// Map in the game
    /// </summary>
    public class GameMap
    {
        /// <summary>
        /// Min size of the map
        /// </summary>
        public static readonly int MIN_SIZE;

        /// <summary>
        /// Max size of the map
        /// </summary>
        public static readonly int MAX_SIZE;

        /// <summary>
        /// Inits some static properties
        /// </summary>
        static GameMap()
        {
            MIN_SIZE = Card.MIN_COUNT_OF_COLORS * 2 - 1;
            MAX_SIZE = Card.GetAllAvailableColors().Length * 2 - 1;
        }

        /// <summary>
        /// Checks if the specified size is within min and max
        /// </summary>
        /// <param name="size">The size to check</param>
        /// <returns></returns>
        public static bool CheckSize(int size)
        {
            return size >= MIN_SIZE && size <= MAX_SIZE && size % 2 != 0;
        }

        /// <summary>
        /// Generates all available cards
        /// </summary>
        /// <param name="size">The map size</param>
        /// <param name="randomize">Mixes the cards</param>
        /// <returns></returns>
        public static Card[] GenerateAvailableCards(int size, bool randomize = true)
        {
            if (!CheckSize(size))
                throw new ArgumentOutOfRangeException();

            var countOfColumnsWithCards = size / 2 + 1;

            var allCardColors = Card.GetAllAvailableColors();
            var availableCardColors = allCardColors.Take(countOfColumnsWithCards).ToArray();

            var cards = new List<Card>();

            foreach (var cardColor in availableCardColors)
            {
                for (var i = 0; i < size; i++)
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

        /// <summary>
        /// Occurs in case of success swap of cells on the current map
        /// </summary>
        public event Action<SuccessSwapEventArgs> SuccessSwap;

        /// <summary>
        /// Size of the current map
        /// </summary>
        public int Size { get { return Cells.GetLength(0); } }

        /// <summary>
        /// Cells of the current map
        /// </summary>
        public Cell[,] Cells { get; private set; }

        /// <summary>
        /// Creates an instance of the game map
        /// </summary>
        /// <param name="size"></param>
        public GameMap(int size)
        {
            InitCells(size);
        }

        /// <summary>
        /// Cells
        /// </summary>
        /// <param name="coordinate">The cell coordinate</param>
        /// <returns></returns>
        public Cell this[Coordinate coordinate]
        {
            get
            {
                if (!CheckIfCoordinateIsValid(coordinate))
                    throw new ArgumentOutOfRangeException();

                return Cells[coordinate.Row, coordinate.Column];
            }
            private set
            {
                if (!CheckIfCoordinateIsValid(coordinate))
                    throw new ArgumentOutOfRangeException();

                Cells[coordinate.Row, coordinate.Column] = value ?? throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Checks if the coordinate is within of the current map area
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public bool CheckIfCoordinateIsValid(Coordinate coordinate)
        {
            return  coordinate.Row >= 0 &&
                    coordinate.Row < Size &&
                    coordinate.Column >= 0 &&
                    coordinate.Column < Size;
        }

        /// <summary>
        /// Checks if the specified coordinates are vertically or horizontally close to each other
        /// </summary>
        /// <param name="coordinate1">Grid coordinate 1</param>
        /// <param name="coordinate2">Grid coordinate 2</param>
        /// <returns></returns>
        public bool CheckIfCoordinatesAreClosest(Coordinate coordinate1, Coordinate coordinate2)
        {
            if (coordinate1.Equals(coordinate2))
                return false;

            if (!CheckIfCoordinateIsValid(coordinate1) || !CheckIfCoordinateIsValid(coordinate2))
                return false;

            var dx = Math.Abs(coordinate1.Column - coordinate2.Column);
            var dy = Math.Abs(coordinate1.Row - coordinate2.Row);

            var closestHorizontally = dx == 1 && dy == 0;
            var closestVertically = dy == 1 && dx == 0;

            return (closestHorizontally || closestVertically) && closestHorizontally != closestVertically;
        }

        /// <summary>
        /// Swaps the cells specified by the coordinates
        /// </summary>
        /// <param name="coordinate1">Grid coordinate 1</param>
        /// <param name="coordinate2">Grid coordinate 2</param>
        /// <returns></returns>
        public bool SwapCells(Coordinate coordinate1, Coordinate coordinate2)
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

            SuccessSwap?.Invoke(new SuccessSwapEventArgs { Coordinate1 = coordinate1, Coordinate2 = coordinate2 });

            return true;
        }

        /// <summary>
        /// Inits cells on the map
        /// </summary>
        /// <param name="size">The map size</param>
        private void InitCells(int size)
        {
            if (!CheckSize(size))
                throw new ArgumentOutOfRangeException();

            Cells = new Cell[size, size];

            var cards = new Stack<Card>(GenerateAvailableCards(size));

            for (var row = 0; row < Cells.GetLength(0); row++)
            {
                for (var column = 0; column < Cells.GetLength(1); column++)
                {
                    if (column % 2 == 0)
                    {
                        Cells[row, column] = cards.Pop();
                    }
                    else
                    {
                        if (row % 2 == 0)
                        {
                            Cells[row, column] = new Block();
                        }
                        else
                        {
                            Cells[row, column] = new EmptyCell();
                        }
                    }
                }
            }
        }
    }
}
