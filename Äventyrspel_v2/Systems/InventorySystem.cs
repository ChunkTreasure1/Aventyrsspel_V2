using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*-------------------------------------------------------------------------------

Author : Ivar Jönsson
Project : Äventyrsspel_V2
Purpose : Handels the players inventory

-------------------------------------------------------------------------------*/

namespace Äventyrspel_v2 {
    class InventorySystem {

        List<string> Inventory = new List<string>();
        int MaxInventorySize = 10;

        //Adds an item to the inventory
        public void PickupItem(string item) {

            //If the players inventory isn't full
            if (Inventory.Count < MaxInventorySize) {

                //Add the item
                Inventory.Add(item);

                Console.WriteLine("Item '" + item + "' added to inventory!");
                Console.WriteLine("Pess ENTER to continue");

                Console.ReadKey();

            }

        }

        //Drops an item from the players inventory
        public void DropItem() {

            bool hasDropped = false;

            while (hasDropped) {

                ShowInventory();
                Console.WriteLine("Choose an item to drop or go back using 0");
                //Gets the players input and converts it
                int itemToDrop = Convert.ToInt32(Console.ReadLine());

                //If the player chose a wrong number
                if (itemToDrop > Inventory.Count) {

                    Console.WriteLine("You don't have that many items!");

                }
                //If the player entered a valid number
                else if (itemToDrop <= Inventory.Count) {

                    //Remove the item
                    Inventory.RemoveAt(itemToDrop - 1);
                    hasDropped = true;
                }

            }
            Console.Clear();

        }

        //Shows the players inventory
        public void ShowInventory() {

            Console.Clear();

            //Go through every element in the inventory
            for (int i = 0; i < Inventory.Count; i++) {

                //Write out every item
                Console.WriteLine((i + 1) + ". " + Inventory[i]);

            }

            Console.ReadKey();

        }

    }
}
