using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP323_Zevakin.Новая_папка
{
    internal class Mag : Enemy
    {
        public Mag() : base(25, 4, 2)
        {
            Name = "Маг";
            CanFreeze = true;
            FreezeChance = 20;
        }
    }
}
