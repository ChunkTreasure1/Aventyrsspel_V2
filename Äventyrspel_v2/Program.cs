using System;
using System.Collections.Generic;
using System.Timers;
using System.Threading;

/*-------------------------------------------------------------------------------

Author : Ivar Jönsson
Project : Äventyrsspel_V2
Purpose : The main game file

-------------------------------------------------------------------------------*/

namespace Äventyrspel_v2 {
    class Program {

        enum EGameConditions {

            eGC_PlainCity,
            eGC_Manhattan,
            eGC_LasVegas

        }

        FightSystem PlayerFightSystem = new FightSystem();
        InventorySystem PlayerInventory = new InventorySystem();
        Buildings RandomBuildings = new Buildings();

        List<Attack> Attacks = new List<Attack>();
        System.Timers.Timer SleepTimer;
        System.Timers.Timer WalkTimer;

        EGameConditions ConditionsToUse;

        string PlayerName { get; set; }
        string HungerStatus = "Full";

        bool IsSleeping = false;
        bool IsWalking = false;

        bool WalkTimerSet = false;

        int DaysAlive = 0;
        int GameHours = 0;

        int RandomBuildingDistanceMax;
        int RandomBuildingDistanceMin;

        int RandomLootAmountMax;
        int RandomLootAmountMin;

        static void Main(string[] args) {

            //Sets the colors of the console
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            //Sets the size of the console
            Console.WindowWidth = 90;
            Console.WindowHeight = 20;

            Console.Clear();

            //Create the new program object
            Program program = new Program();

            //Starts the game
            program.StartGame();
        }

        //Method that handels all the start funcitonality
        void StartGame() {

            //Show the start screen
            ShowStartScreen();
            Console.ReadKey();

            Console.Clear();
            //Bool to check if the player has confirmed it's name
            bool isConfirmed = false;

            while (!isConfirmed) {

                Console.Write("Please state your name: ");
                PlayerName = Console.ReadLine();

                Console.Clear();

                //Ask if the name the player entered was correct
                Console.WriteLine("Is " + PlayerName + " correct?");
                Console.WriteLine("[Y]/[N]");

                string answer = Console.ReadLine();

                //If the player answered YES
                if (answer == "Y" || answer == "y") {

                    //Clear the screen and continue
                    Console.Clear();
                    Console.WriteLine("Alright great!");
                    Console.WriteLine("Press ENTER to continue");

                    isConfirmed = true;
                    Console.ReadLine();

                }
                //If the player answered NO
                else if (answer == "N" || answer == "n") {

                    //Clear the screen and continue
                    Console.Clear();
                    Console.WriteLine("Aww too bad, guess you'll have to enter it again!");
                    Console.WriteLine("Press ENTER to continue");

                    Console.ReadKey();

                }
                else {

                    //Clear the screen and let the player re enter it's name
                    //as this is assuming that it's a no

                    Console.Clear();
                    Console.WriteLine("Incorrect input! Assuming no!");
                    Console.WriteLine("Press ENTER to continue");

                    Console.ReadKey();

                }

            }

            //Let's the player choose it's start location
            ChooseStartLocation();

            //Set's the game conditions based on what you entered
            SetAttacks();
            SetFoods();
            SetItems();
            SetGameConditions();

            Thread t1 = new Thread(() => {

                System.Timers.Timer GameTimer = null;
                bool timerSet = false;

                StartGameTimer();

                //Starts the games time timer
                void StartGameTimer() {

                    //Always run this loop
                    while (true) {

                        if (!timerSet) {
                            //Start a game timer that lasts for one game hour
                            GameTimer = SetGameTimer(2500);
                            timerSet = true;
                        }

                        //If GameHours is equal to or greater than 24
                        if (GameHours >= 24) {

                            //Set game hours to 0 and and add one to the days
                            GameHours = 0;
                            DaysAlive++;

                        }

                    }

                }

                //Called when the game timer has elapsed
                void GameTimerElapsed(object sender, ElapsedEventArgs e) {

                    GameHours++;
                    PlayerFightSystem.FoodValue -= 5;

                    GameTimer.Stop();
                    GameTimer.Dispose();
                    timerSet = false;

                }

                //Creates a game timer
                System.Timers.Timer SetGameTimer(int time) {

                    System.Timers.Timer aTimer = new System.Timers.Timer();
                    aTimer.Elapsed += new ElapsedEventHandler(GameTimerElapsed);
                    aTimer.Interval = time;
                    aTimer.Enabled = true;

                    return aTimer;

                }

            });
            t1.Start();

            Thread t2 = new Thread(() => {

                while (PlayerFightSystem.IsAlive) {

                    //If the player hunger value is between 90 and 100
                    if (PlayerFightSystem.FoodValue > 90 && PlayerFightSystem.FoodValue <= 100) {

                        //Set the hunger status to full
                        HungerStatus = "Full";

                    }
                    //If the player hunger value is between 50 and 100
                    else if (PlayerFightSystem.FoodValue > 50 && PlayerFightSystem.FoodValue <= 90) {

                        //Set the hunger status to hungry
                        HungerStatus = "Hungry";

                    }
                    else if (PlayerFightSystem.FoodValue > 20 && PlayerFightSystem.FoodValue <= 50) {

                        //Set the hunger status to very hungry
                        HungerStatus = "Very hungry";

                    }
                    else if (PlayerFightSystem.FoodValue > 1 && PlayerFightSystem.FoodValue <= 20) {

                        //Set the hunger status to starving
                        HungerStatus = "Starving";

                    }

                    if (PlayerFightSystem.PlayerHealth > 150) {

                        PlayerFightSystem.PlayerHealth = 150;

                    }

                }

            });
            t2.Start();

            //Show main UI
            ShowMainUI();

        }

        //Shows the start screen. Called at the start of the game
        void ShowStartScreen() {

            var startScreen = new[] {

                @"----------------------------------------------------------------------",
                @"-                                                                    -",
                @"-                                                                    -",
                @"-                                                                    -",
                @"-                         Apocalyptic World                          -",
                @"-                                                                    -",
                @"-                      A Game by ChunkTreasure                       -",
                @"-                                                                    -",
                @"-                                                                    -",
                @"-                                                                    -",
                @"-                                                                    -",
                @"-                      Press ENTER to continue                       -",
                @"-                                                                    -",
                @"----------------------------------------------------------------------",
            };

            foreach (string line in startScreen) {
                Console.WriteLine(line);
                System.Threading.Thread.Sleep(5);
            }

        }
        
        //Lets the player choose the it's start location
        void ChooseStartLocation() {

            //Clear the console
            Console.Clear();

            //Show the start locations
            Console.WriteLine("Time to choose a starting location!");
            Console.WriteLine("1 - Plain City");
            Console.WriteLine("2 - Manhattan");
            Console.WriteLine("3 - Las Vegas");

            string location = Console.ReadLine();

            //Bool to check if the player has chosen it's start location
            bool hasChosen = false;

            //While the player has not chosen a start location
            while (!hasChosen) {

                if (location == "1") {
                    //Set the game conditions to Plain City
                    ConditionsToUse = EGameConditions.eGC_PlainCity;
                    hasChosen = true;
                    break;
                }
                else if (location == "2") {
                    //Set the game conditions to Manhattan
                    ConditionsToUse = EGameConditions.eGC_Manhattan;
                    hasChosen = true;
                    break;
                }
                else if (location == "3") {
                    //Set the game conditions to Las Vegas
                    ConditionsToUse = EGameConditions.eGC_LasVegas;
                    hasChosen = true;
                    break;
                }
                else {
                    Console.WriteLine("Incorrect input!");
                    Console.WriteLine("Press ENTER to continue");
                    Console.ReadKey();

                    Console.Clear();
                }

            }

        }

        //Sets the conditions of the game
        void SetGameConditions() {

            //If the game conditions are set to Las Vegas
            if (ConditionsToUse == EGameConditions.eGC_LasVegas) {

                //Set the vars
                RandomBuildings.RandomEnemyCountMax = 5;
                RandomBuildings.RandomEnemyCountMin = 2;

                RandomBuildingDistanceMax = 5;
                RandomBuildingDistanceMin = 2;

                RandomLootAmountMax = 5;
                RandomLootAmountMin = 1;

                //Start attacks
                PlayerFightSystem.AddAttack(PlayerFightSystem.GunShot);
                PlayerFightSystem.AddAttack(PlayerFightSystem.ShotgunShot);

            }
            else if (ConditionsToUse == EGameConditions.eGC_Manhattan) {

                //Set the vars
                RandomBuildings.RandomEnemyCountMax = 7;
                RandomBuildings.RandomEnemyCountMin = 1;

                RandomBuildingDistanceMax = 8;
                RandomBuildingDistanceMin = 4;

                RandomLootAmountMax = 2;
                RandomLootAmountMin = 1;

                //Start attacks
                PlayerFightSystem.AddAttack(PlayerFightSystem.SniperShot);
                PlayerFightSystem.AddAttack(PlayerFightSystem.ShotgunShot);

            }
            else if (ConditionsToUse == EGameConditions.eGC_PlainCity) {

                //Set the vars
                RandomBuildings.RandomEnemyCountMax = 3;
                RandomBuildings.RandomEnemyCountMin = 1;

                RandomBuildingDistanceMax = 10;
                RandomBuildingDistanceMin = 7;

                RandomLootAmountMax = 3;
                RandomLootAmountMin = 1;

                //Start attacks
                PlayerFightSystem.AddAttack(PlayerFightSystem.SniperShot);
                PlayerFightSystem.AddAttack(PlayerFightSystem.GunShot);

            }

        }

        //Shows the main UI
        void ShowMainUI() {

            if (PlayerFightSystem.IsAlive) {

                //Always runs because we should always get back here
                while (true) {

                    Console.Clear();

                    var menu = new[] {

                        @"Player healh: " + PlayerFightSystem.PlayerHealth + "             " + "Days alive: " + DaysAlive + "             " + "Hunger: " + HungerStatus,
                        @"",
                        @"What would you like to do?",
                        @"1 - Go out and venture",
                        @"2 - Sleep",
                        @"3 - Access inventory",
                        @"4 - Eat",
                        @"5 - HELP"

                    };

                    //Show the menu with a short delay between the lines to give an effect
                    foreach (string line in menu) {
                        Console.WriteLine(line);
                        System.Threading.Thread.Sleep(30);
                    }

                    //Get the players choice
                    string playerChoice = Console.ReadLine();

                    //If the player choses one
                    if (playerChoice == "1") {

                        //Let's the player go out and search for items
                        GoOut();
                    }
                    //If the player choses two
                    else if (playerChoice == "2") {

                        //Let's the player sleep
                        StartSleep();

                    }
                    //If the player choses three
                    else if (playerChoice == "3") {

                        //Shows the inventory
                        PlayerInventory.ShowInventory(PlayerInventory, PlayerFightSystem);
                    }
                    //If the player choses four
                    else if (playerChoice == "4") {

                        //Show the food menu
                        PlayerInventory.AccessFoodMenu(PlayerFightSystem);
                    }
                    //If the player choses five
                    else if (playerChoice == "5") {

                        //Show the help menu
                        ShowHelp();
                    }
                    else {

                        Console.WriteLine("Incorrect input!");
                        Console.WriteLine("Press ENTER to continue");

                        Console.ReadKey();
                    }

                }
            }
            else {
                PlayerFightSystem.ShowGameOver(DaysAlive);
                PlayerFightSystem.CauseOfDeath = "Starving";
            }
        }

        //Shows the help menu
        void ShowHelp() {

        }

        //Called when the player wants to sleep
        void StartSleep() {

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            //Set sleeping to true to let the while loop happen
            IsSleeping = true;
            Console.Clear();
            Console.WriteLine("You have decided to sleep");

            //Create a new random and get a random time to sleep
            Random randomSleep = new Random();
            int randomSleepTime = randomSleep.Next(3, 24 + 1);

            //Create the timer for how long the while loop should run
            SleepTimer = SetSleepTimer(randomSleepTime);
            while (IsSleeping) {

                Console.SetCursorPosition(0, 5);
                for (int i = 0; i < 3; i++) {

                    Console.Write(".");
                    Thread.Sleep(200);

                }

            }

            Console.Clear();
            //Add the time slept to the game hours var
            GameHours += randomSleepTime;
            Console.WriteLine("You slept for " + randomSleepTime + " hours.");
            PlayerFightSystem.PlayerHealth += randomSleepTime;
            Console.WriteLine("Player health: " + PlayerFightSystem.PlayerHealth);
            Console.WriteLine("Press ENTER to continue");

            Console.ReadKey();

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

        }

        //Called when the player wants to go out and venture
        void GoOut() {

            bool isOut = true;

            //While the player is out walking
            while (isOut) {

                IsWalking = true;

                //While the player is walking and not in a building
                while (IsWalking) {

                    Console.Clear();
                    Console.Write("Walking");

                    //Gets a random distance to walk before entering a new building
                    Random randomDisance = new Random();
                    int randomHouseDist = randomDisance.Next(RandomBuildingDistanceMin, RandomBuildingDistanceMax + 1);

                    //If the walk timer is'nt set and the player is still walking
                    if (!WalkTimerSet && IsWalking) {

                        //Set the walk timer
                        WalkTimer = SetWalkTimer(randomHouseDist);
                        WalkTimerSet = true;
                    }
                    //Show the dots
                    for (int i = 0; i < 3; i++) {

                        Console.Write(".");
                        Thread.Sleep(200);
                    }

                }

                //Generate a new building
                RandomBuildings.GenerateRandomBuilding(Attacks);

                //Attack the enemies
                for (int i = 0; i < RandomBuildings.Enemies.Count; i++) {
                    Console.Clear();

                    Console.WriteLine("You've entered a building and met an enemy named '" + RandomBuildings.Enemies[i].name + "'!");
                    Console.WriteLine("Press ENTER to attack!");
                    Console.ReadKey();

                    //Attacks the enemy
                    PlayerFightSystem.Attack(RandomBuildings.Enemies[i], DaysAlive);
                    Console.Clear();
                    GetRandomFoodAndLoot();

                    Console.WriteLine("Press ENTER to continue");
                    Console.ReadKey();

                    //Runs while the player hasn't decided wether to eat or not
                    bool hasDecided = false;
                    while (!hasDecided) {

                        Console.Clear();

                        Console.WriteLine("Would you like heal?");
                        Console.WriteLine("[Y]/[N]");

                        //Get the players input and check what it was
                        string healInput = Console.ReadLine();
                        //If the player chooses yes
                        if (healInput == "Y" || healInput == "y") {

                            //Access the food menu
                            PlayerInventory.AccessFoodMenu(PlayerFightSystem);
                            hasDecided = true;

                        }
                        //If the player chooses no
                        else if (healInput == "N" || healInput == "n") {
                            hasDecided = true;
                        }
                    }
                }

                bool hasChosen = false;
                while (!hasChosen) {

                    Console.WriteLine("Do you want to continue walking?");
                    Console.WriteLine("[Y]/[N]");
                    string input = Console.ReadLine();

                    //If the player chooses yes
                    if (input == "Y" || input == "y") {
                        isOut = true;
                        hasChosen = true;
                    }
                    //If the player chooses no
                    else if (input == "N" || input == "n") {
                        isOut = false;
                        hasChosen = true;
                    }
                    //If it's an incorrect input
                    else {

                        Console.WriteLine("Incorrect input!");

                    }

                }

            }

        }

        //Gets random loot and food
        void GetRandomFoodAndLoot() {

            Random randomItems = new Random();

            int randomItemAmount = randomItems.Next(RandomLootAmountMin, RandomLootAmountMax);
            int randomFoodAmount = randomItems.Next(1, 2);

            for (int i = 0; i < (randomItemAmount - randomFoodAmount); i++) {

                PlayerInventory.PickupItem(PlayerInventory.AllItems[randomItems.Next(0, PlayerInventory.AllItems.Count - 1)]);

            }

            for (int i = 0; i < randomFoodAmount; i++) {

                PlayerInventory.AddFood(PlayerInventory.AllFood[randomItems.Next(0, PlayerInventory.AllFood.Count - 1)]);

            }

        }

        //Creates a sleep timer
        System.Timers.Timer SetSleepTimer(int time) {
            //Create a timer
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(SleepTimerElapsed);
            aTimer.Interval = time * 500;
            aTimer.Enabled = true;

            return aTimer;
        }

        //Create a walk timer
        System.Timers.Timer SetWalkTimer(int time) {

            //Create a timer
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(WalkTimerElapsed);
            aTimer.Interval = time * 500;
            aTimer.Enabled = true;

            return aTimer;

        }

        //Called when the sleep timer has elapsed
        void SleepTimerElapsed(Object source, ElapsedEventArgs args) {
            IsSleeping = false;
            SleepTimer.Stop();
            SleepTimer.Dispose();
        }

        //Called when the walk timer has elapsed
        void WalkTimerElapsed(Object source, ElapsedEventArgs args) {
            IsWalking = false;
            WalkTimer.Stop();
            WalkTimer.Dispose();
            WalkTimerSet = false;
        }

        //Sets up the attacks with the correct values
        void SetAttacks() {

            //Gunshot
            PlayerFightSystem.GunShot.AttackName = "Gunshot";
            PlayerFightSystem.GunShot.AttackDamage = 10;
            PlayerFightSystem.GunShot.AttackSpeed = 7;
            //Gunshot

            //Shotgun shot
            Random randomDamage = new Random();

            PlayerFightSystem.ShotgunShot.AttackName = "Shotgun Shot";
            PlayerFightSystem.ShotgunShot.AttackDamage = randomDamage.Next(10, 50);
            PlayerFightSystem.ShotgunShot.AttackSpeed = 5;
            //Shotgun shot

            //Sniper shot
            PlayerFightSystem.SniperShot.AttackName = "Sniper Shot";
            PlayerFightSystem.SniperShot.AttackDamage = 70;
            PlayerFightSystem.SniperShot.AttackSpeed = 2;
            //Sniper shot

            //Rocket
            PlayerFightSystem.Rocket.AttackName = "Rocket";
            PlayerFightSystem.Rocket.AttackDamage = 60;
            PlayerFightSystem.Rocket.AttackSpeed = 1;
            //Rocket

            //Spear throw
            PlayerFightSystem.SpearThrow.AttackName = "Spear throw";
            PlayerFightSystem.SpearThrow.AttackDamage = 7;
            PlayerFightSystem.SpearThrow.AttackSpeed = 3;
            //Spear throw

            //Nailgun Shot
            PlayerFightSystem.NailgunShot.AttackName = "Nailgun shot";
            PlayerFightSystem.NailgunShot.AttackDamage = 10;
            PlayerFightSystem.NailgunShot.AttackSpeed = 9;
            //Nailgun shot

            //Add the attacks to the Attacks list for enemy generation
            Attacks.Add(PlayerFightSystem.GunShot);
            Attacks.Add(PlayerFightSystem.ShotgunShot);
            Attacks.Add(PlayerFightSystem.SniperShot);

            Attacks.Add(PlayerFightSystem.Rocket);
            Attacks.Add(PlayerFightSystem.SpearThrow);
            Attacks.Add(PlayerFightSystem.NailgunShot);

        }

        //Sets up the foods with the correct values
        void SetFoods() {

            //Add to the all list
            PlayerInventory.AllFood.Add(PlayerInventory.Pasta);
            PlayerInventory.AllFood.Add(PlayerInventory.Bread);
            PlayerInventory.AllFood.Add(PlayerInventory.Rice);

            PlayerInventory.AllFood.Add(PlayerInventory.Stew);
            PlayerInventory.AllFood.Add(PlayerInventory.Nachos);
            PlayerInventory.AllFood.Add(PlayerInventory.Tacos);

            PlayerInventory.AllFood.Add(PlayerInventory.Meatballs);
            PlayerInventory.AllFood.Add(PlayerInventory.Potatoes);

            //Pasta
            PlayerInventory.Pasta.FoodName = "Pasta";
            PlayerInventory.Pasta.FoodPower = 20;
            PlayerInventory.Pasta.HealingPower = 10;
            //Pasta

            //Bread
            PlayerInventory.Bread.FoodName = "Bread";
            PlayerInventory.Bread.FoodPower = 15;
            PlayerInventory.Bread.HealingPower = 8;
            //Bread

            //Rice
            PlayerInventory.Rice.FoodName = "Rice";
            PlayerInventory.Rice.FoodPower = 10;
            PlayerInventory.Rice.HealingPower = 20;
            //Rice

            //Stew
            PlayerInventory.Stew.FoodName = "Stew";
            PlayerInventory.Stew.FoodPower = 25;
            PlayerInventory.Stew.HealingPower = 20;
            //Stew

            //Nachos
            PlayerInventory.Nachos.FoodName = "Nachos";
            PlayerInventory.Nachos.FoodPower = 25;
            PlayerInventory.Nachos.HealingPower = 10;
            //Nachos

            //Tacos
            PlayerInventory.Tacos.FoodName = "Tacos";
            PlayerInventory.Tacos.FoodPower = 23;
            PlayerInventory.Tacos.HealingPower = 6;
            //Tacos

            //Meatballs
            PlayerInventory.Meatballs.FoodName = "Meatballs";
            PlayerInventory.Meatballs.FoodPower = 17;
            PlayerInventory.Meatballs.HealingPower = 10;
            //Meatballs

            //Potatoes
            PlayerInventory.Potatoes.FoodName = "Potatoes";
            PlayerInventory.Potatoes.FoodPower = 5;
            PlayerInventory.Potatoes.HealingPower = 7;
            //Potatoes

        }

        //Sets up the items with the correct values
        void SetItems() {

            //All the items
            PlayerInventory.AllItems.Add(PlayerInventory.IronBar);
            PlayerInventory.AllItems.Add(PlayerInventory.WoodenStick);
            PlayerInventory.AllItems.Add(PlayerInventory.StoneBrick);

            PlayerInventory.AllItems.Add(PlayerInventory.Pot);
            PlayerInventory.AllItems.Add(PlayerInventory.PlasticBar);
            PlayerInventory.AllItems.Add(PlayerInventory.RubberCube);

            PlayerInventory.AllItems.Add(PlayerInventory.Nail);

            //Iron bar
            PlayerInventory.IronBar.ItemName = "Iron bar";
            //Iron bar

            //Wooden stick
            PlayerInventory.WoodenStick.ItemName = "Wooden stick";
            //Wooden stick

            //Stone brick
            PlayerInventory.StoneBrick.ItemName = "Stone brick";
            //Stone brick

            //Pot
            PlayerInventory.Pot.ItemName = "Pot";
            //Pot

            //Plastic bar
            PlayerInventory.PlasticBar.ItemName = "Plastic bar";
            //Plastic bar

            //Rubber cube
            PlayerInventory.RubberCube.ItemName = "Rubber cube";
            //Rubber cube

            //Nail
            PlayerInventory.Nail.ItemName = "Nail";

        }
    }
}
