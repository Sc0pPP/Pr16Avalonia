using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _3ISIP323_Zevakin.Новая_папка
{
    internal class Goblin:Enemy
    {
        
            public Goblin() : base(30, 5, 2)
            {
                Name = "Гоблин";
                HasCrit = true;
                CritChance = 20;
            }
        
    }
}
