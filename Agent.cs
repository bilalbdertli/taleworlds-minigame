using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taleworlds_minigame {
    public class Agent {
        private static int _agentCount = 0;

        public static int AgentCount {
            get {
                return _agentCount;
            }
        }

        private readonly int _id;
        private int _hp;

        private int _level;
        private int _xp;

        private Map.Tile _currentLocation;

        private bool _isDead = false;

        private bool _isEnemy = false;

        public bool IsDead {
            get {
                return _isDead;
            }
        }

        public bool IsEnemy {
            get {
                return _isEnemy;
            }
        }

        public int HP {
            get {
                return _hp;
            }
            set {
                if(value <= 0) {
                    _hp = 0;
                    _isDead = true;
                    Location?.RemoveAgent(this);
                    if(IsEnemy) {
                        Game.CurrentGame.SpawnEnemy(RandomTW.Next(0, Game.CurrentGame.Map.Width-1), RandomTW.Next(0, Game.CurrentGame.Map.Height-1), Level);
                        Game.CurrentGame.SpawnEnemy(RandomTW.Next(0, Game.CurrentGame.Map.Width-1), RandomTW.Next(0, Game.CurrentGame.Map.Height-1), Level + 1);
                    }
                    Console.WriteLine("Agent " + Id + " has died");
                    if(this == Game.CurrentGame.Player) {
                        Console.WriteLine("Game Over");
                        Console.WriteLine("Restarting");
                        Game.CurrentGame = new Game(5, 5);
                    }
                } else if(value > MaxHP) {
                    _hp = MaxHP;
                } else {
                    _hp = value;
                }
            }
        }

        public int MaxHP {
            get {
                return 4 * Level;
            }
        }

        public int Level {
            get {
                return _level;
            }
            set {
                if(value >= 1) {
                    _level = value;
                }
            }
        }

        public int XP {
            get {
                return _xp;
            }
            set {
                if(value >= 0) {
                    _xp = value;
                } else if(value >= 100) {
                    _level++;
                    _xp = 0;
                }
            }
        }

        public Map.Tile Location {
            get {
                return _currentLocation;
            }

            set {
                _currentLocation?.RemoveAgent(this);
                _currentLocation = value;
                _currentLocation?.AddAgent(this);
            }
        }

        public int AP { 
            get {
                return RandomTW.Next(Level, Level * 2);
            }
        }

        public int DP {
            get {
                return RandomTW.Next(Level/2, Level);
            }
        }

        public int Id {
            get {
                return _id;
            }
        }

        public Agent(Map.Tile loc, int level = 1, bool isEnemy = false) {
            Level = level;
            HP = MaxHP;
            _id = _agentCount++;
            Location = loc;
            _isEnemy = isEnemy;
        }

        public bool Attack(int targetId) {
            if(IsDead) {
                return false;
            }
            var enemy = Location?.FindAgent(targetId);
            if(enemy != null && !enemy.IsDead) {
                var hit = new HitInfo {
                    AgentId = Id,
                    Damage = AP
                };
                if(enemy == this) {
                    Console.WriteLine("Attacking yourself");
                    bool didTakeDamage = enemy.TakeDamage(hit);
                    return true;
                } else {
                    Console.WriteLine("Agent " + Id + " Attacking enemy: Agent " + enemy.Id);
                    bool didTakeDamage = enemy.TakeDamage(hit);
                    if(!didTakeDamage) {
                        return false;
                    } else if(enemy.IsDead) {
                        XP += 20 * enemy.Level;
                    } else if(didTakeDamage) {
                        XP += 10 * enemy.Level;
                    }
                    return true;
                }
            }
            return false;
        }

        public bool TakeDamage(HitInfo damage) {
            if(IsDead) {
                return false;
            }
            if(damage.AgentId == Game.CurrentGame.Player.Id) {
                _isEnemy = true;
            }
            int damageTaken = damage.Damage - DP;
            if(damageTaken > 0) {
                Console.WriteLine("Agent " + Id + " took " + damageTaken + " damage");
                HP -= damageTaken;
                _counterAttack(damage.AgentId);
                return true;
            } else {
                Console.WriteLine("Agent " + Id + " blocked the attack");
                _counterAttack(damage.AgentId);
            }
            return false;
        }

        private void _counterAttack(int id) {
            if(IsDead || id == Id || this == Game.CurrentGame.Player) {
                return;
            }
            var enemy = Location?.FindAgent(id);
            if(enemy != null) {
                var hit = new HitInfo {
                    AgentId = Id,
                    Damage = AP
                };
                Console.WriteLine("Agent " + Id + " Counter Attacking enemy: Agent " + enemy.Id);
                bool didTakeDamage = enemy.TakeDamage(hit);
                if(didTakeDamage) {
                    XP += 10 * enemy.Level;
                }
            }
        }

        public bool Heal(int amount) {
            if(IsDead) {
                return false;
            }
            if(amount > 0) {
                HP += amount;
                return true;
            }
            return false;
        }

        public void Move(Direction dir) {
            if(IsDead) {
                return;
            }
            Game.CurrentGame.Map.MoveAgent(this, dir);
        }

        public void LookAround() {
            if(IsDead) {
                Console.WriteLine("Agent is dead");
            }
            Console.WriteLine(Location.ToString());
        }

        public void LookNorth() {
            if(IsDead) {
                Console.WriteLine("Agent is dead");
            }
            Console.WriteLine(LookInDirection(Direction.Up).ToString());
        }

        public void LookSouth() {
            if(IsDead) {
                Console.WriteLine("Agent is dead");
            }
            Console.WriteLine(LookInDirection(Direction.Down).ToString());
        }

        public void LookEast() {
            if(IsDead) {
                Console.WriteLine( "Agent is dead");
            }
            Console.WriteLine(LookInDirection(Direction.Right).ToString());
        }

        public void LookWest() {
            if(IsDead) {
                Console.WriteLine("Agent is dead");
            }
            Console.WriteLine(LookInDirection(Direction.Left).ToString());
        }

        public override string ToString() {
            return "Agent " + Id + " at " + Location.X + ", " + Location.Y + " HP: " + HP + "/" + MaxHP + " Level: " + Level + " XP: " + XP;
        }

        public class HitInfo {
            public int AgentId { get; set; }
            public int Damage { get; set; }
        }
        
        private Map.Tile LookInDirection(Direction direction) {
            if (Location == null || IsDead) {
                return null;
            }
            return Game.CurrentGame.Map.GetAdjacentTile(Location, direction);
        }


    }
}
