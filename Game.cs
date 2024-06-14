using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taleworlds_minigame {
    public class Game {
        public static Game CurrentGame = new Game(5, 5);

        public readonly Map Map;

        private Agent _player;

        public Agent Player {
            get {
                return _player;
            }
        }

        public Game(int w, int h) {
            Map = new Map(w, h);
            SpawnPlayer(0, 0);
        }

        public Agent SpawnAgent(int x, int y, int level = 1, bool isEnemy = false) {
            return new Agent(Map.Tiles[x][y], level, isEnemy);
        }

        public void SpawnPlayer(int x, int y, int level = 1) {
            if(_player == null || _player.IsDead) {
                _player = SpawnAgent(x, y, level);
            }
        }

        public void SpawnEnemy(int x, int y, int level = 1) {
            SpawnAgent(x, y, level, true);
        }
    }
}
