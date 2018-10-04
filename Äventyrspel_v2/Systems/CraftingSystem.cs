using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Äventyrspel_v2.Systems {

    struct Recipe {

        public string name;
        public Item craftingItem1;
        public Item craftingItem2;

        public Attack OutAttack;

    }

    class CraftingSystem {

        Recipe SpearThrowRes = new Recipe();
        Recipe GunShotRes = new Recipe();
        Recipe ShotgunShotRes = new Recipe();

        Recipe SniperShotRes = new Recipe();
        Recipe RocketRes = new Recipe();
        Recipe NailgunShotRes = new Recipe();

        //Crafts a new attack from two items
        public void Craft (InventorySystem playerInventory, Item item1, Item item2) {

        }

        public void SetRecipes() {

        }

    }
}
