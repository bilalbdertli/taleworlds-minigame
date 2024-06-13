using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taleworlds_minigame {
    public class Map {

        private readonly int _width;

        private readonly int _height;

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

        private readonly List<List<Tile>> _tiles = new List<List<Tile>>();

        public IReadOnlyList<IReadOnlyList<Tile>> Tiles {
            get {
                return _tiles;
            }
        }


        public Map(int width, int height) {
            _width = width;
            _height = height;
            for(int i = 0; i < width; i++) {
                var row = new List<Tile>();
                for(int j = 0; j < height; j++) {
                    row.Add(new Tile(i, j));
                }
                _tiles.Add(row);
            }
        }

        public void MoveAgent(Agent agent, Direction direction) {
            if(agent.Location == null) {
                return;
            }
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
            private readonly int _x;
            private readonly int _y;

            private readonly List<Agent> _agents = new List<Agent>();

            public IReadOnlyList<Agent> Agents {
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

            public int NumberOfAgents {
                get {
                    return _agents.Count;
                }
            }

            public Tile(int x, int y) {
                _x = x;
                _y = y;
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
                bool didRemoved = _agents.Remove(agent);
                if(didRemoved) {
                    agent.Location = null;
                }
            }

            public Agent FindAgent(int id) {
                return _agents.FirstOrDefault(a => a.Id == id);
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
