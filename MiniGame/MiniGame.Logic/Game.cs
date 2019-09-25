using MiniGame.Logic.Entities;
using MiniGame.Logic.Entities.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGame.Logic
{
    public class Game
    {
        public GameMap Map { get; private set; }

        public Card[] GoalCards { get; private set; }

        public Game(int mapSize)
        {
            InitMap(mapSize);
            InitGoalCards();
        }

        public bool CheckIfRiddleSolved()
        {
            var indexesOfColumnsWithCards = Enumerable.Range(0, Map.Size).Where(index => index % 2 == 0).ToArray();

            var result = indexesOfColumnsWithCards.All(index => {
                var goalCard = GoalCards[index / 2];
                var columnWithCards = Enumerable.Range(0, Map.Cells.GetLength(0)).Select(row => Map.Cells[row, index]).ToArray();

                return columnWithCards.All(cell => {
                    var card = cell as Card;
                    if (card == null)
                        return false;

                    return card.Color == goalCard.Color;
                });
            });

            return result;
        }

        private void InitMap(int mapSize)
        {
            if (!GameMap.CheckSize(mapSize))
                throw new ArgumentOutOfRangeException();

            Map = new GameMap(mapSize);
        }

        private void InitGoalCards()
        {
            GoalCards = GenerateGoalCards();
        }

        private Card[] GenerateGoalCards()
        {
            var random = new Random();
            var countOfCardsToGenerate = Map.Size / 2 + 1;

            return Card.GetAllAvailableColors()
                .OrderBy(color => random.Next())
                .Take(countOfCardsToGenerate)
                .Select(color => new Card(color))
                .ToArray();
        }
    }
}
