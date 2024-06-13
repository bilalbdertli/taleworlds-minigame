using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taleworlds_minigame {
    public class Map {

        private int _lastId;

        private int _width;

        private int _height;

        public int Width {
            get {
                return _width;
            }
        }

        public int Height {
            get {
                return _height;
            }
        }

        public int LastId {
            get {
                return _lastId;
            }
        }

        private Agent _player;

        public Agent Player {
            get {
                return _player;
            }
        }

        List<List<Tile>> _tiles = new List<List<Tile>>();

        public static Map CurrentMap = new Map();


        Map(int width = 5, int height = 5) {
            _lastId = 0;
            for(int i = 0; i < width; i++) {
                List<Tile> row = new List<Tile>();
                for(int j = 0; j < height; j++) {
                    row.Add(new Tile(i, j));
                }
                _tiles.Add(row);
            }

            Agent agent = new Agent(10);
            agent.Location = _tiles[0][0];
            _player = agent;
        }

        public bool AssignId(Agent agent) {
            agent.Id = _lastId;
            if(agent.Id != _lastId) {
                return false;
            }
            _lastId++;
            return true;
        }

        public void MoveAgent(Agent agent, Direction direction) {
            if(direction == Direction.Left) {
                if(agent.Location.X > 0) {
                    agent.Location = _tiles[agent.Location.X - 1][agent.Location.Y];
                }
            } else if(direction == Direction.Right) {
                if(agent.Location.X < Width - 1) {
                    agent.Location = _tiles[agent.Location.X + 1][agent.Location.Y];
                }
            } else if(direction == Direction.Up) {
                if(agent.Location.Y > 0) {
                    agent.Location = _tiles[agent.Location.X][agent.Location.Y - 1];
                }
            } else if(direction == Direction.Down) {
                if(agent.Location.Y < Height - 1) {
                    agent.Location = _tiles[agent.Location.X][agent.Location.Y + 1];
                }
            }
        }





        public class Tile {
            private int _x;
            private int _y;

            private List<Agent> _agents = new List<Agent>();

            public IEnumerable<Agent> Agents {
                get {
                    return _agents;
                }
            }

            public int X {
                get {
                    return _x;
                }
            }

            public int Y {
                get {
                    return _y;
                }
            }

            public Tile(int x, int y) {
                this._x = x;
                this._y = y;
            }

            public void AddAgent(Agent agent) {
                if(agent.Location != this || _agents.FirstOrDefault(a => a.Id == agent.Id) != null) {
                    return;
                }
                _agents.Add(agent);
            }

            public void RemoveAgent(Agent agent) {
                if(agent.Location != this) {
                    return;
                }
                _agents.Remove(agent);
            }

            public bool IsOccupied() {
                return _agents.Count > 0;
            }
        }

        public enum Direction {
            Up,
            Down,
            Left,
            Right
        }
    }
}
