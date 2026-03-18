using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP323_Zevakin.Новая_папка
{
    internal class GoblinBoss : Goblin
    {
        public GoblinBoss()
        {
            Name = "ВВГ Гоблин-Босс";
            MaxHP = 30 * 2;
            CurrentHP = MaxHP;
            Attack = (int)(5 * 1.5);
            Defense = (int)(2 * 1.2);
            CritChance += 10;
        }
    }

}
