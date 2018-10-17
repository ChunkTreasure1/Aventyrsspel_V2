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
        public void GenerateRandomBuilding(List<Attack> attacks) {

            //Clear the list of enemies in the building at start
            Enemies.Clear();
            Random randomEnemies = new Random();

            //Generate a random amount of enemies
            int enemies = randomEnemies.Next(RandomEnemyCountMin, RandomEnemyCountMax);

            //Generates the enemies
            for (int i = 0; i < enemies; i++) {

                //Called to generate an enemy
                Enemies.Add(GenerateEnemy(attacks));

            }

        }

        //Generates an enemy
        Enemy GenerateEnemy(List<Attack> attacks) {

            List<string> names = new List<string>();

            names.Add("Damaged");
            names.Add("Ruthless");
            names.Add("Deatheater");
            names.Add("Killerman");
            names.Add("Tester");

            Random random = new Random();

            //Create the new enemy
            Enemy newEnemy = new Enemy();

            newEnemy.Healh = random.Next(80, 150);
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

            }

            return newEnemy;

        }

    }
}
