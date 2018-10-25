using System;
using System.Media;
using System.Collections.Generic;
using System.IO;

/*-------------------------------------------------------------------------------

Author : Ivar Jönsson
Project : Äventyrsspel_V2
Purpose : Handels the games fighting system

-------------------------------------------------------------------------------*/

namespace Äventyrspel_v2.Systems {
    class FightSystem {

        public List<Attack> Attacks = new List<Attack>();

        int EnemiesKilled = 0;
        public int PlayerHealth = 100;
        public int FoodValue = 100;

        public bool IsAlive = true;
        public string CauseOfDeath;
        public int PlayerXP = 0;

        public int PlayerLevel = 1;

        public Attack GunShot = new Attack();
        public Attack ShotgunShot = new Attack();
        public Attack SniperShot = new Attack();

        public Attack Rocket = new Attack();
        public Attack SpearThrow = new Attack();
        public Attack NailgunShot = new Attack();

        //Called when the player should attack an enemy
        public void Attack(Enemy enemyToAttack, int DaysAlive) {

            //Resets the attacks uses
            ResetUses();

            //While the player and the enemy is still alive
            while (IsAlive && enemyToAttack.IsAlive) {

                //Get the players and the enemys attack
                Attack attackToUse = GetAttack(enemyToAttack);
                Attack enemyAttack = enemyToAttack.GetEnemyAttack();

                //If the players attack speed is less than the enemys attack speed
                if (attackToUse.AttackSpeed < enemyAttack.AttackSpeed) {

                    //Damage the player
                    PlayerHealth -= enemyAttack.AttackDamage;

                    //If both the player and the enemy is still alive
                    if (PlayerHealth > 0 && enemyToAttack.Health > 0) {

                        //Damage the enemy
                        enemyToAttack.Health -= attackToUse.AttackDamage;

                        //Show the enemys and the players health
                        Console.Clear();
                        Console.WriteLine("Attack Successful!");
                        ShowHealth(enemyToAttack);

                        Console.WriteLine("Press ENTER to attack again");
                        Console.ReadKey();

                    }
                    //If one of them is not alive
                    else {

                        //Check who it is that is dead and end the loop
                        if (PlayerHealth <= 0) {
                            IsAlive = false;
                            break;
                        }
                        else if (enemyToAttack.Health <= 0) {
                            enemyToAttack.IsAlive = false;
                            break;
                        }

                    }
                }
                //If the players attack speed is greater than the enemys
                else if (attackToUse.AttackSpeed > enemyAttack.AttackSpeed) {

                    //Damage the enemy
                    enemyToAttack.Health -= attackToUse.AttackDamage;

                    //If the player and the enemy is still alive
                    if (enemyToAttack.Health > 0 && PlayerHealth > 0) {

                        PlayerHealth -= enemyAttack.AttackDamage;

                        //Show the players and the enemys health
                        Console.Clear();
                        Console.WriteLine("Attack Successful!");
                        ShowHealth(enemyToAttack);

                        Console.WriteLine("Press ENTER to attack again");
                        Console.ReadKey();

                    }
                    //If on of them is dead
                    else {

                        //Check who it is that is dead
                        if (PlayerHealth <= 0) {
                            IsAlive = false;
                            break;
                        }
                        else if (enemyToAttack.Health <= 0) {
                            enemyToAttack.IsAlive = false;
                            break;
                        }

                    }

                }
                //If the attack speeds are the same
                else if (attackToUse.AttackSpeed == enemyAttack.AttackSpeed) {

                    //Tell the player that the attacks have cancelled out
                    Console.Clear();
                    Console.WriteLine("The attack speed is the same! The attacks have cancelled out!");
                    ShowHealth(enemyToAttack);

                    Console.WriteLine("Press ENTER to attack again");
                    Console.ReadKey();

                }

                if (PlayerHealth <= 0) {
                    IsAlive = false;
                }
                else if (enemyToAttack.Health <= 0) {
                    enemyToAttack.IsAlive = false;
                }

            }

            //Check if the player is alive, otherwise the player 
            //died and need to rerun the game
            if (IsAlive) {

                //Tell the player that the enemy died
                Console.Clear();
                PlayerXP += enemyToAttack.XP;
                Console.WriteLine("Enemy died!");
                Console.WriteLine("Your health: " + PlayerHealth);
                Console.WriteLine("Your XP: " + PlayerXP);
                Console.WriteLine("Press ENTER to continue");
                EnemiesKilled++;
                Console.ReadKey();

            }
            else {

                //Tell the player that it died and that it's game over
                Console.Clear();
                Console.WriteLine("You died :(");
                Console.WriteLine("Press ENTER to continue");
                Console.ReadKey();

                CauseOfDeath = "Killed by enemy '" + enemyToAttack.name + "'";
                //Shows the game over screen
                ShowGameOver(DaysAlive);

            }

        }

        //Shows the attack list and lets you choose which to use
        Attack GetAttack(Enemy enemyToAttack) {

            bool hasChosen = false;

            while (!hasChosen) {

                //Clears the console
                Console.Clear();

                ShowHealth(enemyToAttack);

                for (int i = 0; i < Attacks.Count; i++) {

                    Console.Write((i + 1) + " - " + Attacks[i].AttackName + " - " + "Damage: ");

                    Print.PrintColorText(Attacks[i].AttackDamage.ToString(), ConsoleColor.Red);
                    Console.Write(" Uses: ");

                    Print.PrintColorText(Attacks[i].Uses.ToString() + "\n", ConsoleColor.Green);

                }
                //Get the number to get the attack
                int selection = Convert.ToInt32(Console.ReadLine());

                //Return the attack that has been selected with the entered number above
                int j = 0;
                bool notSelected = true;

                //While the player hasn't selected an attack
                while (notSelected) {

                    //If selection - 1 is equal to the always incrementing j, return that attack
                    if ((selection - 1) == j) {

                        //Check if j is less than the amount of elements in Attacks
                        if (j < Attacks.Count) {

                            //If the attack has uses left
                            if (Attacks[j].Uses > 0) {

                                //Set not selected to false and return the attack
                                Attacks[j].Uses -= 1;
                                notSelected = false;
                                return Attacks[j];

                            }
                            else {

                                Console.WriteLine("You don't have enough uses left!");
                                break;

                            }

                        }
                        else {
                            //Set selected to false to re run the menu
                            notSelected = false;
                        }

                    }

                    //If j - 1 is equal to the amount of elements in the list
                    if (j - 1 == Attacks.Count) {

                        //Set j to 0
                        j = 0;

                    }
                    else {
                        //Otherwise increment j
                        j++;
                    }

                }

            }

            return new Attack();

        }

        //Prints out the health of the player and the enemy
        void ShowHealth(Enemy enemyToAttack) {

            Console.Write("Enemy health: ");

            Print.PrintColorText(enemyToAttack.Health.ToString() + "\n", ConsoleColor.Red);
            Console.Write("Player health: ");

            Print.PrintColorText(PlayerHealth.ToString() + "\n", ConsoleColor.Green);

        }

        //Shows the game over screen
        public void ShowGameOver(int DaysAlive) {

            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\Music/game_over.wav";
            player.Play();

            Console.Clear();

            var gameOverScreen = new[] {

                @"----------------------------------------------------------------------",
                @"-                                                                    -",
                @"-                                                                    -",
                @"-                                                                    -",
                @"-                                                                    -",
                @"-                                                                    -",
                @"-                            Game Over                               -",
                @"-                                                                    -",
                @"-                                                                    -",
                @"-                                                                    -",
                @"-                                                                    -",
                @"-                      Press ENTER to continue                       -",
                @"-                                                                    -",
                @"----------------------------------------------------------------------",
            };

            //Writes out the game over screen
            foreach (string line in gameOverScreen) {

                Print.PrintMiddle(line, true, 0, 0);
                System.Threading.Thread.Sleep(30);
            }

            ShowStats(DaysAlive);

            Print.PrintMiddle("Press ENTER to restart the game", true, 0, 0);
            Console.ReadKey();

            // Starts a new instance of the program itself
            System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.FriendlyName);

            // Closes the current process
            Environment.Exit(0);

        }

        //Shows the players stats on death
        void ShowStats(int DaysAlive) {

            Print.PrintMiddle("Days stayed alive: ", false, 0, 4);
            Print.PrintColorText(DaysAlive.ToString() + "\n", ConsoleColor.DarkRed);

            Print.PrintMiddle("Player XP: ", false,  0, 12);
            Print.PrintColorText(PlayerXP.ToString() + "\n", ConsoleColor.DarkRed);

            Print.PrintMiddle("Player level: ", false, 0, 8);
            Print.PrintColorText(PlayerLevel.ToString() + "\n", ConsoleColor.DarkRed);

            Print.PrintMiddle("Enemies killed: ", false, 0, 6);
            Print.PrintColorText(EnemiesKilled.ToString() + "\n", ConsoleColor.DarkRed);

            Print.PrintMiddle("Cause of death: ", false, 0, 6);
            Print.PrintColorText(CauseOfDeath + "\n", ConsoleColor.DarkRed);

            Print.PrintMiddle("Number of attacks crafted: ", false, 0, -4);
            Print.PrintColorText((Attacks.Count - 2).ToString() + "\n", ConsoleColor.DarkRed);

            Console.WriteLine();

        }

        //Adds an attack to the attacks list
        public void AddAttack(Attack attack) {

            Attacks.Add(attack);

        }

        //Resets the attacks uses
        void ResetUses() {

            GunShot.Uses = GunShot.MaxUses;
            ShotgunShot.Uses = ShotgunShot.MaxUses;
            SniperShot.Uses = SniperShot.MaxUses;

            Rocket.Uses = Rocket.MaxUses;
            SpearThrow.Uses = SpearThrow.MaxUses;
            NailgunShot.Uses = NailgunShot.MaxUses;

        }
    }

    class Attack {

        public string AttackName;
        public int AttackSpeed;
        public int AttackDamage;

        public int MaxUses;
        public int Uses;

    }

    class Enemy {

        public string name;

        public bool IsAlive = true;
        public int Health;
        public int MaxEnemyAttacks;

        public int XP;

        public List<Attack> EnemyAttacks = new List<Attack>();

        //Gets a random attack for the enemy
        public Attack GetEnemyAttack() {

            //Returns a random attack from the list if enemy attacks
            Random random = new Random();
            return EnemyAttacks[random.Next(0, (MaxEnemyAttacks))];

        }
    }

}
