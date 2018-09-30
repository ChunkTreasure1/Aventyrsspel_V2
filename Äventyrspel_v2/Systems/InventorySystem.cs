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
        List<Food> Foods = new List<Food>();

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

        //Add food item
        public void AddFood(Food food) {

            Foods.Add(food);

        }

        //Access the food menu
        public void AccessFoodMenu(int playerHealth) {

            //Tell the player it's health and show the food in the food inventory
            Console.Clear();

            bool inMenu = true;
            while (inMenu) {

                Console.WriteLine("Player health: " + playerHealth);
                Console.WriteLine("Choose a food item to eat or choose 0 to quit");

                //Show all the food items
                for (int i = 0; i < Foods.Count; i++) {

                    //Shows the food and it's healing power
                    Console.WriteLine((i + 1) + ". " + Foods[i].FoodName + " - " + "+" + Foods[i].HealingPower);

                }

                //Get the user input
                int selection = Convert.ToInt32(Console.ReadLine());

                //If the player chooses 0
                if (selection == 0) {

                    inMenu = false;
                    return;

                }

                //Int and bool to handle the selection
                int j = 0;
                bool notSelected = true;

                //While the player hasn't selected an item
                while (notSelected) {

                    //If selection - 1 is equal to the always increameanting j eat that item
                    if ((selection - 1) == j) {

                        Foods[j].Eat();
                        notSelected = false;
                    }

                    //If j is equal to the number of elements in the foods list
                    if (j - 1 == Foods.Count) {
                        //Set j to zero
                        j = 0;
                    }
                    else {
                        //Otherwise increment it
                        j++;
                    }

                }

            }

        }

    }

    class Food {

        public int HealingPower;
        public int FoodPower;
        public string FoodName;

        public void Eat() {

        }

    }
}
