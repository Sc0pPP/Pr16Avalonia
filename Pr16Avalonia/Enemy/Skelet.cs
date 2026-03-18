using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP323_Zevakin.Новая_папка
{
    internal class Skelet : Enemy
    {
        public Skelet() : base(40, 6, 3)
        {
            Name = "Скелет";
            IgnoreDefense = true;
        }
    }

}
