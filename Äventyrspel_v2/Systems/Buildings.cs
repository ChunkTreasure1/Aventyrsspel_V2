using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*-------------------------------------------------------------------------------

Author : Ivar Jönsson
Project : Äventyrsspel_V2
Purpose : Handels the random generation of buildings

-------------------------------------------------------------------------------*/

namespace Äventyrspel_v2.Systems {
    class Buildings {

        public List<Enemy> Enemies = new List<Enemy>();

        public int RandomEnemyCountMax;
        public int RandomEnemyCountMin;

        //Generates random buildings when player 
        //decides to go out and venture
        public void GenerateRandomBuilding(List<Attack> attacks, FightSystem fightSystem) {

            //Clear the list of enemies in the building at start
            Enemies.Clear();
            Random randomEnemies = new Random();

            //Generate a random amount of enemies
            int enemies = randomEnemies.Next(RandomEnemyCountMin, RandomEnemyCountMax + 1);

            //Generates the enemies
            for (int i = 0; i < enemies; i++) {

                //Called to generate an enemy
                Enemies.Add(GenerateEnemy(attacks, fightSystem, randomEnemies));

            } 

        }

        //Generates an enemy
        Enemy GenerateEnemy(List<Attack> attacks, FightSystem fightSystem, Random random) {

            List<string> names = new List<string>();

            names.Add("Damaged");
            names.Add("Ruthless");
            names.Add("Deatheater");
            names.Add("Killerman");
            names.Add("Tester");

            //Create the new enemy
            Enemy newEnemy = new Enemy();

            //Set the enemy's health based on the players level
            if (fightSystem.PlayerLevel >= 15) {

                newEnemy.Health = random.Next(80, 150 + 1);

            }
            else if (fightSystem.PlayerLevel >= 10){

                newEnemy.Health = random.Next(50, 100 + 1);

            }
            else if (fightSystem.PlayerLevel >= 1) {

                newEnemy.Health = random.Next(30, 60 + 1);

            }

            newEnemy.MaxEnemyAttacks = random.Next(2, 5 + 1);
            newEnemy.name = names[random.Next(0, 5)];

            //If the enemy has more than three attacks
            if (newEnemy.MaxEnemyAttacks > 3) {

                //Generate a random amount of xp between 70 and 110
                newEnemy.XP = random.Next(70, 110);

            }
            else {

                //Generate a random amount of xp between 30 and 70
                newEnemy.XP = random.Next(30, 70 + 1);

            }

            //Add all the attacks to the enemys attack array
            for (int i = 0; i < newEnemy.MaxEnemyAttacks; i++) {

                newEnemy.EnemyAttacks.Add(attacks[random.Next(0, attacks.Count)]);

                //Check if sniper or rocket attack is added
                if (newEnemy.EnemyAttacks[i].AttackName == "Sniper Shot" || newEnemy.EnemyAttacks[i].AttackName == "Rocket") {

                    //Check if player is below level 15
                    if (fightSystem.PlayerLevel < 15) {

                        //Remove the attack
                        newEnemy.EnemyAttacks.RemoveAt(i);
                        i--;

                    }

                }

            }

            return newEnemy;

        }

    }
}
