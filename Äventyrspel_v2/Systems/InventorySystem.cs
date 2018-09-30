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

        List<Item> Inventory = new List<Item>();
        List<Food> Foods = new List<Food>();

        public List<Item> AllItems = new List<Item>();
        public List<Food> AllFood = new List<Food>();

        public Food Pasta = new Food();
        public Food Bread = new Food();
        public Food Rice = new Food();

        public Food Stew = new Food();

        public Item IronBar = new Item();
        public Item WoodenStick = new Item();
        public Item StoneBrick = new Item();

        int MaxInventorySize = 10;

        //Adds an item to the inventory
        public void PickupItem(Item item) {

            //If the players inventory isn't full
            if (Inventory.Count < MaxInventorySize) {

                //Add the item
                Inventory.Add(item);

                Console.WriteLine("Item '" + item.ItemName + "' added to inventory!");

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
                Console.WriteLine((i + 1) + ". " + Inventory[i].ItemName);

            }

            Console.ReadKey();

        }

        //Add food item
        public void AddFood(Food food) {

            Foods.Add(food);

            Console.WriteLine("Food '" + food.FoodName + "' added to food storage!");

        }

        //Access the food menu
        public void AccessFoodMenu(FightSystem fightSystem) {

            //Tell the player it's health and show the food in the food inventory
            Console.Clear();

            bool inMenu = true;
            while (inMenu) {

                Console.Clear();
                Console.WriteLine("Player health: " + fightSystem.PlayerHealth);
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

                        Eat(selection - 1, fightSystem);
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

        public void Eat(int foodToEat, FightSystem fightSystem) {

            Console.Clear();

            //Adds the values to the player
            fightSystem.PlayerHealth += Foods[foodToEat].HealingPower;
            fightSystem.FoodValue += Foods[foodToEat].FoodPower;

            Console.WriteLine("You ate: " + Foods[foodToEat].FoodName + "!");
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();

            //Removes the food from the array
            Foods.RemoveAt(foodToEat);

        }

    }

    class Item {

        public string ItemName;

    }

    class Food {

        public int HealingPower;
        public int FoodPower;
        public string FoodName;

    }
}
