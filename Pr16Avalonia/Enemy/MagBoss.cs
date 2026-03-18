using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP323_Zevakin.Новая_папка
{
    internal class MagBoss : Mag
    {
        public MagBoss()
        {
            Name = "Архимаг C++";
            MaxHP = (int)(25 * 1.8);
            CurrentHP = MaxHP;
            Attack = (int)(4 * 1.6);
            Defense = (int)(2 * 1.1);
            FreezeChance += 10;
        }
    }

}
