using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taleworlds_minigame {
    public class Agent {
        int _id;
        int _hp;

        int _maxHp;

        int _level;
        int _xp;

        private Map.Tile _currentLocation;

        public Map.Tile Location {
            get {
                return _currentLocation;
            }

            set {
                if(_currentLocation != null) {
                    _currentLocation.RemoveAgent(this);
                }
                _currentLocation = value;
                _currentLocation.AddAgent(this);
            }
        }

        List<int> _potions = new List<int>();



        public int Id {
            get {
                return _id;
            }
            set {
                if(_id == null && Map.CurrentMap.LastId == value) {
                    _id = value;
                }
            }
        }

        public Agent(int maxHp) {
            _maxHp = maxHp;
            _hp = maxHp;

            Map.CurrentMap.AssignId(this);
        }

        public bool Attack(int targetId) {
            
        }

        public bool TakeDamage(int damage) {

        }
    }
}
