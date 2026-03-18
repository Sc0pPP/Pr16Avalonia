using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP323_Zevakin.Новая_папка
{
    internal class MonsterFabric
    {
        static Random rnd = new Random();

        public static Enemy CreateMonster (bool isboss = false)
        {
            if (isboss)
            {
                int type = rnd.Next(maxValue: 4);
                switch (type)
                {
                    case 0: return new GoblinBoss();
                    case 1: return new SkeletBossKova();
                    case 2: return new SkeletBossPest();
                    case 3: return new MagBoss(); 
                }
            }
            else
            {
                int type = rnd.Next(maxValue: 4);
                switch (type)
                {
                    
                   
                    case 0: return new Goblin();
                    case 1: return new Skelet();
                    case 2: return new Mag();
                    case 3: return new Slime();
                }

                }
            return null;

        }
    }
}
