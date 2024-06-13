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

        private bool isDead = false;

        public bool IsDead {
            get {
                return isDead;
            }
        }

        public int HP {
            get {
                return _hp;
            }
            set {
                if(value <= 0) {
                    _hp = 0;
                    isDead = true;
                    Location?.RemoveAgent(this);
                } else if(value > MaxHP) {
                    _hp = MaxHP;
                } else {
                    _hp = value;
                }
            }
        }

        public int MaxHP {
            get {
                return 4 * _level;
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
                return _level * 2;
            }
        }

        public int DP {
            get {
                return _level /2;
            }
        }

        public int Id {
            get {
                return _id;
            }
        }

        public Agent(Map.Tile loc, int level = 1) {
            _level = level;
            _hp = MaxHP;
            _id = _agentCount++;
            Location = loc;
        }

        public bool Attack(int targetId) {
            if(isDead) {
                return false;
            }
            var enemy = Location?.FindAgent(targetId);
            if(enemy != null) {
                if(enemy == this) {
                    Console.WriteLine("Attacking yourself: " + enemy);
                    bool didTakeDamage = enemy.TakeDamage(AP);
                    return true;
                } else {
                    Console.WriteLine("Attacking enemy: " + enemy);
                    bool didTakeDamage = enemy.TakeDamage(AP);
                    if(didTakeDamage) {
                        XP += 10 * enemy.Level;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TakeDamage(int damage) {
            if(isDead) {
                return false;
            }
            int damageTaken = damage - DP;
            if(damageTaken > 0) {
                Console.WriteLine("Agent " + Id + " took " + damageTaken + " damage");
                _hp -= damageTaken;
                return true;
            }
            return false;
        }

        public bool Heal(int amount) {
            if(isDead) {
                return false;
            }
            if(amount > 0) {
                _hp += amount;
                if(_hp > MaxHP) {
                    _hp = MaxHP;
                }
                return true;
            }
            return false;
        }

        public void Move(Direction dir) {
            if(isDead) {
                return;
            }
            Game.CurrentGame.Map.MoveAgent(this, dir);
        }

        public string LookAround() {
            if(isDead) {
                return "Agent is dead";
            }
            return Location.ToString();
        }

        public override string ToString() {
            return "Agent " + Id + " at " + Location.X + ", " + Location.Y + " HP: " + HP + "/" + MaxHP + " Level: " + Level + " XP: " + XP;
        }
    }
}
