using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP323_Zevakin.Новая_папка
{
    internal class Player
    {
        public string Name;
        public int MaxHP;
        public int CurrentHP;
        public int Attack;
        public int Defense;
        public bool IsFrozen = false;
        public bool BlockNextAttack = false;

        public string EquippedWeapon = "Пустая рука";
        public string EquippedArmor = "Обычная одежда";
        public int BaseAttack; // базовая атака без оружия
        public int BaseDefense; // базовая защита без брони

        public Player(string name, int hp, int attack, int defense)
        {
            Name = name;
            MaxHP = hp;
            CurrentHP = hp;
            Attack = attack;
            Defense = defense;
            BaseAttack = attack;
            BaseDefense = defense;
        }

        public void Heal()
        {
            CurrentHP = MaxHP;
            Game.OnLog?.Invoke($"💚 Вы использовали лечебное зелье! HP восстановлено до {CurrentHP}");
        }

        public void EquipWeapon(string newWeapon, int atk)
        {
            // Автоматически экипируем если новое оружие лучше
            if (atk > Attack)
            {
                EquippedWeapon = newWeapon;
                Attack = atk;
                Game.OnLog?.Invoke($"⚔️ Экипировано: {newWeapon} (Атака: {atk})");
            }
            else
            {
                Game.OnLog?.Invoke($"⚔️ Найдено: {newWeapon} (Атака: {atk}), но текущее оружие лучше");
            }
        }

        public void EquipArmor(string newArmor, int def)
        {
            // Автоматически экипируем если новая броня лучше
            if (def > Defense)
            {
                EquippedArmor = newArmor;
                Defense = def;
                Game.OnLog?.Invoke($"🛡️ Экипировано: {newArmor} (Защита: {def})");
            }
            else
            {
                Game.OnLog?.Invoke($"🛡️ Найдено: {newArmor} (Защита: {def}), но текущая броня лучше");
            }
        }

        public bool TryDodge(Random rnd)
        {
            int chance = rnd.Next(100);
            if (chance < 40)
            {
                Game.OnLog?.Invoke("💨 Вы успешно увернулись от следующей атаки!");
                return true;
            }
            return false;
        }

        public void TakeDamage(int dmg)
        {
            CurrentHP -= dmg;
            if (CurrentHP < 0) CurrentHP = 0;
            Game.OnLog?.Invoke($"💔 Вы получили {dmg} урона! HP: {CurrentHP}/{MaxHP}");
        }

        public bool IsAlive()
        {
            return CurrentHP > 0;
        }
    }
}