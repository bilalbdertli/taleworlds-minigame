﻿using System;
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
        public Tile GetAdjacentTile(Tile currentTile, Direction direction) {
            int newX = currentTile.X;
            int newY = currentTile.Y;

            switch (direction) {
                case Direction.Left:
                    newX -= 1;
                    break;
                case Direction.Right:
                    newX += 1;
                    break;
                case Direction.Up:
                    newY -= 1;
                    break;
                case Direction.Down:
                    newY += 1;
                    break;
            }

            // Check bounds
            if (newX >= 0 && newX < Width && newY >= 0 && newY < Height) {
                return _tiles[newX][newY];
            }
            return null; // Out of bounds, cannot move to there 
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

            private void EnemyAttack() {
               foreach(var agent in _agents) {
                    if(agent.IsEnemy) {
                        agent.Attack(Game.CurrentGame.Player.Id);
                    }
                }
            }

            public void AddAgent(Agent agent) {
                if(agent.Location != this || _agents.FirstOrDefault(a => a.Id == agent.Id) != null) {
                    return;
                }
                _agents.Add(agent);
                EnemyAttack();
            }

            public void RemoveAgent(Agent agent) {
                if(agent.Location != this) {
                    return;
                }
                EnemyAttack();
                bool didRemoved = _agents.Remove(agent);
                if(didRemoved) {
                    agent.Location = null;
                }
            }

            public Agent FindAgent(int id) {
                return _agents.FirstOrDefault(a => a.Id == id);
            }

            public override string ToString() {
                return "Tile at (" + _x + ", " + _y + ") with " + NumberOfAgents + " agents: " + string.Join("\n", _agents);
            }
        }
    }
}
