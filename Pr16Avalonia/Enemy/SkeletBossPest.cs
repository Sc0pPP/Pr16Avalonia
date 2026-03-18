using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP323_Zevakin.Новая_папка
{
    internal class SkeletBossPest : Skelet
    {
        public SkeletBossPest()
        {
            Name = "Скелет-Пестов";
            MaxHP = (int)(40 * 1.3);
            CurrentHP = MaxHP;
            Attack = (int)(6 * 1.8);
            Defense = (int)(3 * 0.6);
        }
    }

}
