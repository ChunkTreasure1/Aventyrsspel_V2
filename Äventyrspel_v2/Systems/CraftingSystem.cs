﻿using System;
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

        public List<Recipe> Recipes = new List<Recipe>();

        public Recipe SpearThrowRes = new Recipe();
        public Recipe GunShotRes = new Recipe();
        public Recipe ShotgunShotRes = new Recipe();

        public Recipe SniperShotRes = new Recipe();
        public Recipe RocketRes = new Recipe();
        public Recipe NailgunShotRes = new Recipe();

        //Crafts a new attack from two items
        public void Craft(InventorySystem playerInventory, int attackToCraft) {

        }

        //Shows the crafting menu
        public void ShowCraftingMenu(FightSystem fightSystem, InventorySystem playerInventory) {

            bool inMenu = true;

            while (inMenu) {

                //Tell the player it's attack amount
                Console.Clear();
                Console.WriteLine("You have: " + fightSystem.Attacks.Count + " attacks!");
                Console.WriteLine("Choose an Attack to craft or choose 0 to exit!");

                //Print out all the recipes and it's needed items
                for (int i = 0; i < Recipes.Count; i++) {

                    Console.WriteLine((i + 1) + ". " + Recipes[i].name + " - " + "Items needed: " + "1. " +
                        Recipes[i].craftingItem1.ItemName + " and " + "1. " + Recipes[i].craftingItem2.ItemName);
                }

                //Get the player input
                int selection = Convert.ToInt32(Console.ReadLine());

                //If the player chooses to exit
                if (selection == 0) {

                    inMenu = false;
                    return;

                }

                //Int and bool to handle the selection
                int j = 0;
                bool selected = false;

                //While the player hasn't selected a recipe to craft
                while (!selected) {

                    //If the selection - 1 is equal to the always incrementing j, craft that item
                    if ((selection - 1) == j) {

                        Craft(playerInventory, selection - 1);
                        selected = true;
                    }

                    //If j minus 1 is equal to the number of elements in the recipes list
                    if (j - 1 == Recipes.Count) {
                        
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
}
