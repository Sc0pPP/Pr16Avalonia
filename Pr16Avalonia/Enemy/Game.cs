using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pr16Avalonia.Pages;
using WpfLikeAvaloniaNavigation;

namespace _3ISIP323_Zevakin.Новая_папка
{
    internal class Game
    {
        static Random rnd = new Random();

        // События для обновления UI
        public static Action<string> OnLog;
        public static Action<int, int, int> OnStatsUpdate;
        public static Action<bool> OnEnableButtons;
        public static Action<bool> OnEnableNextButton;
        public static Action<string, int, bool> OnShowEquipChoice;
        public static Action OnGameOver;
        public static Action<string> OnShowImage; // *** НОВОЕ: показать картинку ***

        // Состояние игры
        public static Player CurrentPlayer;
        public static Enemy CurrentEnemy;
        public static int CurrentTurn = 1;
        public static bool IsInBattle = false;
        
        // Временное хранение найденной экипировки
        private static string PendingEquipName;
        private static int PendingEquipValue;
        private static bool PendingIsWeapon;

        public static Enemy GenerateRandomEnemy(bool isBoss = false)
        {
            if (isBoss)
            {
                Enemy newMonster = MonsterFabric.CreateMonster(true);
                return newMonster;
            }
            else
            {
                Enemy newMonster = MonsterFabric.CreateMonster();
                return newMonster;
            }
            return null;
        }

        public static void OpenChest(Player player)
        {
            int roll = rnd.Next(3);
            if (roll == 0)
            {
                // *** Показываем картинку зелья ***
                OnShowImage?.Invoke("potion");
                
                player.Heal();
                OnLog?.Invoke("🎁 Вы нашли сундук! Получено зелье лечения!");
                OnStatsUpdate?.Invoke(player.CurrentHP, player.MaxHP, CurrentTurn);
                OnEnableNextButton?.Invoke(true);
            }
            else if (roll == 1)
            {
                // *** Показываем картинку меча ***
                OnShowImage?.Invoke("sword");
                
                int atk = rnd.Next(5, 16);
                string weaponName = $"Меч +{atk}";
                OnLog?.Invoke($"⚔️ Вы нашли сундук с оружием!");
                OnLog?.Invoke($"Новое: {weaponName} (Атака: {atk})");
                OnLog?.Invoke($"Текущее: {player.EquippedWeapon} (Атака: {player.Attack})");
                
                PendingEquipName = weaponName;
                PendingEquipValue = atk;
                PendingIsWeapon = true;
                OnShowEquipChoice?.Invoke(weaponName, atk, true);
            }
            else
            {
                // *** Показываем картинку брони ***
                OnShowImage?.Invoke("armor");
                
                int def = rnd.Next(1, 11);
                string armorName = $"Броня +{def}";
                OnLog?.Invoke($"🛡️ Вы нашли сундук с броней!");
                OnLog?.Invoke($"Новая: {armorName} (Защита: {def})");
                OnLog?.Invoke($"Текущая: {player.EquippedArmor} (Защита: {player.Defense})");
                
                PendingEquipName = armorName;
                PendingEquipValue = def;
                PendingIsWeapon = false;
                OnShowEquipChoice?.Invoke(armorName, def, false);
            }
        }

        public static void EquipItem()
        {
            if (PendingIsWeapon)
            {
                CurrentPlayer.EquippedWeapon = PendingEquipName;
                CurrentPlayer.Attack = PendingEquipValue;
                OnLog?.Invoke($"✅ Экипировано: {PendingEquipName}");
            }
            else
            {
                CurrentPlayer.EquippedArmor = PendingEquipName;
                CurrentPlayer.Defense = PendingEquipValue;
                OnLog?.Invoke($"✅ Экипировано: {PendingEquipName}");
            }
            
            OnStatsUpdate?.Invoke(CurrentPlayer.CurrentHP, CurrentPlayer.MaxHP, CurrentTurn);
            OnEnableNextButton?.Invoke(true);
        }

        public static void KeepOldItem()
        {
            if (PendingIsWeapon)
            {
                OnLog?.Invoke($"❌ Оставлено старое оружие: {CurrentPlayer.EquippedWeapon}");
            }
            else
            {
                OnLog?.Invoke($"❌ Оставлена старая броня: {CurrentPlayer.EquippedArmor}");
            }
            
            OnEnableNextButton?.Invoke(true);
        }

        public static void StartBattle(Player player, Enemy enemy)
        {
            CurrentEnemy = enemy;
            IsInBattle = true;
            
            // *** Показываем картинку монстра по его имени ***
            string enemyImage = GetEnemyImageName(enemy.Name);
            OnShowImage?.Invoke(enemyImage);
            
            OnLog?.Invoke($"\n⚔️ Вы столкнулись с {enemy.Name}!");
            OnLog?.Invoke($"❤️ HP врага: {enemy.CurrentHP}/{enemy.MaxHP}");
            OnEnableButtons?.Invoke(true);
            OnEnableNextButton?.Invoke(false);
        }

        // *** Определяем какую картинку показать для монстра ***
        private static string GetEnemyImageName(string enemyName)
        {
            // Можно сделать по-умному, определяя тип монстра
            if (enemyName.Contains("Слизь") || enemyName.Contains("слизь"))
                return "slime";
            else if (enemyName.Contains("Гоблин") || enemyName.Contains("гоблин"))
                return "goblin";
            else if (enemyName.Contains("Скелет") || enemyName.Contains("скелет"))
                return "skeleton";
            else if (enemyName.Contains("Орк") || enemyName.Contains("орк"))
                return "orc";
            else if (enemyName.Contains("Дракон") || enemyName.Contains("дракон"))
                return "dragon";
            else if (enemyName.Contains("Босс") || enemyName.Contains("босс"))
                return "boss";
            else
                return "monster"; // картинка по умолчанию
        }

        public static void PlayerAttack()
        {
            if (!IsInBattle || CurrentEnemy == null || CurrentPlayer == null)
                return;

            if (!CurrentPlayer.IsFrozen)
            {
                int dmg = CurrentPlayer.Attack - CurrentEnemy.Defense;
                if (dmg < 1) dmg = 1;
                CurrentEnemy.CurrentHP -= dmg;
                OnLog?.Invoke($"⚔️ Вы нанесли {dmg} урона {CurrentEnemy.Name}!");
                OnLog?.Invoke($"❤️ HP врага: {CurrentEnemy.CurrentHP}/{CurrentEnemy.MaxHP}");
            }
            else
            {
                OnLog?.Invoke("❄️ Вы пропускаете ход из-за заморозки!");
                CurrentPlayer.IsFrozen = false;
            }

            EnemyTurn();
        }

        public static void PlayerDefend()
        {
            if (!IsInBattle || CurrentEnemy == null || CurrentPlayer == null)
                return;

            if (!CurrentPlayer.IsFrozen)
            {
                if (!CurrentPlayer.TryDodge(rnd))
                {
                    CurrentPlayer.BlockNextAttack = true;
                    OnLog?.Invoke("🛡️ Уклонение не удалось, блок активирован!");
                }
                else
                {
                    OnLog?.Invoke("💨 Успешное уклонение!");
                }
            }
            else
            {
                OnLog?.Invoke("❄️ Вы пропускаете ход из-за заморозки!");
                CurrentPlayer.IsFrozen = false;
            }

            EnemyTurn();
        }

        private static void EnemyTurn()
        {
            if (CurrentEnemy.IsAlive())
            {
                CurrentEnemy.AttackPlayer(CurrentPlayer, rnd);
                OnStatsUpdate?.Invoke(CurrentPlayer.CurrentHP, CurrentPlayer.MaxHP, CurrentTurn);
            }

            CheckBattleEnd();
        }

        private static void CheckBattleEnd()
        {
            if (!CurrentPlayer.IsAlive())
            {
                // *** Показываем картинку смерти ***
                OnShowImage?.Invoke("death");
                
                OnLog?.Invoke("💀 Вы были побеждены...");
                IsInBattle = false;
                OnEnableButtons?.Invoke(false);
                OnEnableNextButton?.Invoke(false);
                OnGameOver?.Invoke();
                
                return;
            }

            if (!CurrentEnemy.IsAlive())
            {
                // *** Показываем картинку победы ***
                OnShowImage?.Invoke("victory");
                
                OnLog?.Invoke($"✅ Вы победили {CurrentEnemy.Name}!");
                IsInBattle = false;
                OnEnableButtons?.Invoke(false);
                CurrentEnemy = null;
                OnEnableNextButton?.Invoke(true);
            }
        }

        public static void NextTurn()
        {
            OnEnableNextButton?.Invoke(false);
            
            CurrentTurn++;
            OnLog?.Invoke($"\n=== Этаж {CurrentTurn} ===");
            OnStatsUpdate?.Invoke(CurrentPlayer.CurrentHP, CurrentPlayer.MaxHP, CurrentTurn);

            bool isBossTurn = (CurrentTurn % 10 == 0);
            bool chestEvent = rnd.Next(2) == 0;

            if (chestEvent && !isBossTurn)
            {
                // *** Показываем картинку сундука ***
                OnShowImage?.Invoke("chest");
                OpenChest(CurrentPlayer);
            }
            else
            {
                Enemy enemy = GenerateRandomEnemy(isBossTurn);
                StartBattle(CurrentPlayer, enemy);
            }
        }

        public static void StartNewGame()
        {
            CurrentPlayer = new Player("Герой", 100, 10, 5);
            CurrentTurn = 1;
            IsInBattle = false;
            CurrentEnemy = null;

            // *** Показываем картинку начала игры ***
            OnShowImage?.Invoke("start");

            OnLog?.Invoke("🏰 Добро пожаловать в подземелье!");
            OnStatsUpdate?.Invoke(CurrentPlayer.CurrentHP, CurrentPlayer.MaxHP, CurrentTurn);
            OnEnableNextButton?.Invoke(true);
        }
    }
}