using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP323_Zevakin.Новая_папка
{
    internal class Enemy
    {
        public string Name;
        public int MaxHP;
        public int CurrentHP;
        public int Attack;
        public int Defense;
        public bool IgnoreDefense = false;
        public bool HasCrit = false;
        public int CritChance = 0;
        public bool CanFreeze = false;
        public int FreezeChance = 0;

        public Enemy(int hp, int attack, int defense)
        {
            MaxHP = hp;
            CurrentHP = hp;
            Attack = attack;
            Defense = defense;
        }

        public virtual void AttackPlayer(Player player, Random rnd)
        {
            int dmg = Attack;
            if (!IgnoreDefense)
                dmg -= player.Defense;

            if (player.BlockNextAttack)
            {
                int blockPercent = rnd.Next(70, 101);
                dmg = dmg * (100 - blockPercent) / 100;
                player.BlockNextAttack = false;
                Game.OnLog?.Invoke($"🛡️ Блок уменьшил получаемый урон на {blockPercent}%!");
            }

            if (HasCrit && rnd.Next(100) < CritChance)
            {
                dmg *= 2;
                Game.OnLog?.Invoke($"💥 {Name} нанес критический удар!");
            }

            if (dmg < 1) dmg = 1;
            player.TakeDamage(dmg);

            if (CanFreeze && rnd.Next(100) < FreezeChance)
            {
                player.IsFrozen = true;
                Game.OnLog?.Invoke($"❄️ {Name} наложил заморозку! Вы пропустите следующий ход.");
            }
        }

        public bool IsAlive()
        {
            return CurrentHP > 0;
        }
    }
}