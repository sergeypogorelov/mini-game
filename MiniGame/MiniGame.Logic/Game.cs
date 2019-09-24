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

        public Game(int mapSize)
        {
            if (!GameMap.CheckSize(mapSize))
                throw new ArgumentOutOfRangeException();

            Map = new GameMap(mapSize);
        }

        public bool CheckIfRiddleSolved()
        {
            var indexesOfColumnsWithCards = Enumerable.Range(0, Map.Size).Where(index => index % 2 == 0).ToArray();

            var result = indexesOfColumnsWithCards.All(index => {
                var columnWithCards = Enumerable.Range(0, Map.Cells.GetLength(0)).Select(row => Map.Cells[row, index]);
                var firstCard = columnWithCards.First() as Card;
                if (firstCard == null)
                    return false;

                return columnWithCards.Skip(1).All(cell => {
                    var card = cell as Card;
                    if (card == null)
                        return false;

                    return card.Color == firstCard.Color;
                });
            });

            return result;
        }
    }
}
