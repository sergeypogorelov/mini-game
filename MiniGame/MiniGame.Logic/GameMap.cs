using MiniGame.Logic.Entities;
using MiniGame.Logic.Entities.Cells;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniGame.Logic
{
    public class GameMap
    {
        public const int MIN_SIZE = 3;

        public static readonly int MAX_SIZE;

        static GameMap()
        {
            var allCardColors = Card.GetAllAvailableColors();
            MAX_SIZE = allCardColors.Length * 2 - 1;
        }

        public int Size { get { return _cells.GetLength(0); } }

        public GameMap(int size)
        {
            InitCells(size);
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

            for (var i = 0; i < _cells.GetLength(0); i++)
            {
                for (var j = 0; j < _cells.GetLength(1); j++)
                {
                    if (j % 2 == 0)
                    {
                        _cells[i, j] = cards.Pop();
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            _cells[i, j] = new Block();
                        }
                        else
                        {
                            _cells[i, j] = new EmptyCell();
                        }
                    }
                }
            }
        }

        private Card[] GenerateAvailableCards(int gameMapSize)
        {
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

            var random = new Random();
            return cards.OrderBy(i => random.Next()).ToArray();
        }
    }
}
