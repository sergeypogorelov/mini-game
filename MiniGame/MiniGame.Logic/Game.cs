using MiniGame.Logic.Entities.Cells;
using System;
using System.Linq;

namespace MiniGame.Logic
{
    /// <summary>
    /// The game instance
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Occurs in case of the victory
        /// </summary>
        public event Action Victory;

        /// <summary>
        /// Current map
        /// </summary>
        public GameMap Map { get; private set; }

        /// <summary>
        /// Current goal cards
        /// These cards the player should match in order to win
        /// </summary>
        public Card[] GoalCards { get; private set; }

        /// <summary>
        /// Creates an instance of the game
        /// </summary>
        /// <param name="mapSize">The game map size</param>
        public Game(int mapSize)
        {
            InitMap(mapSize);
            InitGoalCards();
        }

        /// <summary>
        /// Checks if the riddle is solved
        /// In other words, if the player has matched the cards on the map with the goal cards
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Inits the game map
        /// </summary>
        /// <param name="mapSize">The map size</param>
        private void InitMap(int mapSize)
        {
            if (!GameMap.CheckSize(mapSize))
                throw new ArgumentOutOfRangeException();

            Map = new GameMap(mapSize);
            Map.SuccessSwap += Map_SuccessSwap;
        }

        /// <summary>
        /// Checks each success swap if the game is over
        /// </summary>
        /// <param name="obj"></param>
        private void Map_SuccessSwap(SuccessSwapEventArgs obj)
        {
            if (CheckIfRiddleSolved())
            {
                Victory?.Invoke();
            }
        }

        /// <summary>
        /// Inits the goal cards
        /// </summary>
        private void InitGoalCards()
        {
            GoalCards = GenerateGoalCards();
        }

        /// <summary>
        /// Generates the goal cards
        /// </summary>
        /// <returns></returns>
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
