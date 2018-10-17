using System;
using System.Collections.Generic;
using System.Threading;

/*-------------------------------------------------------------------------------

Author : Ivar Jönsson
Project : Äventyrsspel_V2
Purpose : Handels the players inventory

-------------------------------------------------------------------------------*/

namespace Äventyrspel_v2.Systems {
    class InventorySystem {

        public List<Item> Inventory = new List<Item>();
        List<Food> Foods = new List<Food>();

        CraftingSystem PlayerCrafting = new CraftingSystem();

        public List<Item> AllItems = new List<Item>();
        public List<Food> AllFood = new List<Food>();

        public Food Pasta = new Food();
        public Food Bread = new Food();
        public Food Rice = new Food();

        public Food Stew = new Food();
        public Food Nachos = new Food();
        public Food Tacos = new Food();

        public Food Meatballs = new Food();
        public Food Potatoes = new Food();

        public Item IronBar = new Item();
        public Item WoodenStick = new Item();
        public Item StoneBrick = new Item();

        public Item Pot = new Item();
        public Item PlasticBar = new Item();
        public Item RubberCube = new Item();

        public Item Nail = new Item();

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
        public void DropItem(InventorySystem playerInventory, FightSystem fightSystem) {

            bool hasDropped = false;

            while (hasDropped) {

                ShowInventory(playerInventory, fightSystem);
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
        public void ShowInventory(InventorySystem playerInventory, FightSystem fightSystem) {

            Console.Clear();
            SetRecipes(fightSystem);
            Console.WriteLine("Your Inventory: ");

            //Go through every element in the inventory
            for (int i = 0; i < Inventory.Count; i++) {

                //Write out every item
                Console.WriteLine((i + 1) + ". " + Inventory[i].ItemName);

            }

            Console.WriteLine("");
            Console.WriteLine("Press 0 to enter crafting menu");

            string input = Console.ReadLine();

            //If the player chooses 0
            if (input == "0") {

                //Show the crafting menu
                PlayerCrafting.ShowCraftingMenu(fightSystem, playerInventory);

            }
            else {
                return;
            }

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

            Thread FoodMenu = new Thread(() => {

                while (inMenu) {

                    Console.Clear();

                    Console.WriteLine("Health: " + fightSystem.PlayerHealth + "              " + "Hunger: " + fightSystem.FoodValue + "/100");
                    Console.WriteLine("Choose a food item to eat or choose 0 to quit");

                    //Show all the food items
                    for (int i = 0; i < Foods.Count; i++) {

                        //Shows the food and it's healing power
                        Console.WriteLine((i + 1) + ". " + Foods[i].FoodName + " - " + "+" + Foods[i].HealingPower + ", +" + Foods[i].FoodPower);

                    }

                    Thread.Sleep(1500);

                }

            });
            FoodMenu.Start();

            //Get the user input
            int selection = Convert.ToInt32(Console.ReadLine());

            //If the player chooses 0
            if (selection == 0) {

                inMenu = false;
                FoodMenu.Join();
                return;

            }

            //Int and bool to handle the selection
            int j = 0;
            bool notSelected = true;

            //While the player hasn't selected an item
            while (notSelected) {

                //If selection - 1 is equal to the always increameanting j eat that item
                if ((selection - 1) == j) {

                    inMenu = false;
                    FoodMenu.Join();
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

        //Eats a food item
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

        //Sets all of the recipes values
        public void SetRecipes(FightSystem fightSystem) {

            //Add all recipes to the recipes list, do this to be able to show
            //all recipes in recipe menu, and also to be able to painlessly add
            //new attacks if needed.
            PlayerCrafting.Recipes.Add(PlayerCrafting.SpearThrowRes);
            PlayerCrafting.Recipes.Add(PlayerCrafting.GunShotRes);
            PlayerCrafting.Recipes.Add(PlayerCrafting.ShotgunShotRes);

            PlayerCrafting.Recipes.Add(PlayerCrafting.SniperShotRes);
            PlayerCrafting.Recipes.Add(PlayerCrafting.RocketRes);
            PlayerCrafting.Recipes.Add(PlayerCrafting.NailgunShotRes);

            //Spear throw
            PlayerCrafting.SpearThrowRes.name = "Spear throw";
            PlayerCrafting.SpearThrowRes.craftingItem1 = WoodenStick;
            PlayerCrafting.SpearThrowRes.craftingItem2 = IronBar;

            PlayerCrafting.SpearThrowRes.OutAttack = fightSystem.SpearThrow;
            PlayerCrafting.SpearThrowRes.CraftXP = 30;
            //Spear throw

            //Gunshot
            PlayerCrafting.GunShotRes.name = "Gunshot";
            PlayerCrafting.GunShotRes.craftingItem1 = IronBar;
            PlayerCrafting.GunShotRes.craftingItem2 = IronBar;

            PlayerCrafting.GunShotRes.OutAttack = fightSystem.GunShot;
            PlayerCrafting.GunShotRes.CraftXP = 70;
            //Gunshot

            //Shotgun shot
            PlayerCrafting.ShotgunShotRes.name = "Shotgun shot";
            PlayerCrafting.ShotgunShotRes.craftingItem1 = WoodenStick;
            PlayerCrafting.ShotgunShotRes.craftingItem2 = IronBar;

            PlayerCrafting.ShotgunShotRes.OutAttack = fightSystem.ShotgunShot;
            PlayerCrafting.ShotgunShotRes.CraftXP = 100;
            //Shotgun shot

            //Sniper shot
            PlayerCrafting.SniperShotRes.name = "Sniper shot";
            PlayerCrafting.SniperShotRes.craftingItem1 = IronBar;
            PlayerCrafting.SniperShotRes.craftingItem2 = RubberCube;

            PlayerCrafting.SniperShotRes.OutAttack = fightSystem.SniperShot;
            PlayerCrafting.SniperShotRes.CraftXP = 150;
            //Sniper shot

            //Rocket
            PlayerCrafting.RocketRes.name = "Rocket";
            PlayerCrafting.RocketRes.craftingItem1 = IronBar;
            PlayerCrafting.RocketRes.craftingItem2 = Pot;

            PlayerCrafting.RocketRes.OutAttack = fightSystem.Rocket;
            PlayerCrafting.SniperShotRes.CraftXP = 140;
            //Rocket

            //Nailgun shot
            PlayerCrafting.NailgunShotRes.name = "Nailgun shot";
            PlayerCrafting.NailgunShotRes.craftingItem1 = Nail;
            PlayerCrafting.NailgunShotRes.craftingItem2 = PlasticBar;

            PlayerCrafting.NailgunShotRes.OutAttack = fightSystem.NailgunShot;
            PlayerCrafting.SniperShotRes.CraftXP = 50;
            //Nailgun shot

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
