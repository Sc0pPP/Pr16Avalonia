using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static Action<string> OnShowImage;
        public static Action OnReturnToMenu;

        // Состояние игры
        public static Player CurrentPlayer;
        public static List<Enemy> CurrentEnemies; // *** ИЗМЕНЕНО: теперь список врагов ***
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

        // *** НОВЫЙ МЕТОД: Генерация группы врагов ***
        public static List<Enemy> GenerateEnemyGroup(bool isBoss)
        {
            List<Enemy> enemies = new List<Enemy>();
            
            if (isBoss)
            {
                // Босс всегда один
                enemies.Add(GenerateRandomEnemy(true));
            }
            else
            {
                // Шанс встретить группу врагов (30%)
                int groupChance = rnd.Next(100);
                
                if (groupChance < 30) // 30% шанс группы
                {
                    int enemyCount = rnd.Next(2, 4); // От 2 до 3 врагов
                    
                    for (int i = 0; i < enemyCount; i++)
                    {
                        enemies.Add(GenerateRandomEnemy(false));
                    }
                    
                    OnLog?.Invoke($"⚠️ Внимание! Вы встретили группу из {enemyCount} врагов!");
                }
                else
                {
                    // Обычный одиночный враг
                    enemies.Add(GenerateRandomEnemy(false));
                }
            }
            
            return enemies;
        }

        public static void OpenChest(Player player)
        {
            int roll = rnd.Next(3);
            if (roll == 0)
            {
                OnShowImage?.Invoke("potion");
                player.Heal();
                OnLog?.Invoke("🎁 Вы нашли сундук! Получено зелье лечения!");
                OnStatsUpdate?.Invoke(player.CurrentHP, player.MaxHP, CurrentTurn);
                OnEnableNextButton?.Invoke(true);
            }
            else if (roll == 1)
            {
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

        // *** ОБНОВЛЕННЫЙ МЕТОД: Начало боя с группой ***
        public static void StartBattle(Player player, List<Enemy> enemies)
        {
            CurrentEnemies = enemies;
            IsInBattle = true;
            
            // Показываем картинку первого врага (или можно показывать всех)
            string enemyImage = GetEnemyImageName(enemies[0].Name);
            OnShowImage?.Invoke(enemyImage);
            
            if (enemies.Count == 1)
            {
                OnLog?.Invoke($"\n⚔️ Вы столкнулись с {enemies[0].Name}!");
                OnLog?.Invoke($"❤️ HP врага: {enemies[0].CurrentHP}/{enemies[0].MaxHP}");
            }
            else
            {
                OnLog?.Invoke($"\n⚔️ Вы столкнулись с группой врагов:");
                foreach (var enemy in enemies)
                {
                    OnLog?.Invoke($"   • {enemy.Name} (HP: {enemy.CurrentHP}/{enemy.MaxHP})");
                }
                
                // Считаем общее HP группы
                int totalHP = enemies.Sum(e => e.CurrentHP);
                int totalMaxHP = enemies.Sum(e => e.MaxHP);
                OnLog?.Invoke($"💀 Общее HP группы: {totalHP}/{totalMaxHP}");
            }
            
            OnEnableButtons?.Invoke(true);
            OnEnableNextButton?.Invoke(false);
        }

        private static string GetEnemyImageName(string enemyName)
        {
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
                return "monster";
        }

        // *** ОБНОВЛЕННЫЙ МЕТОД: Атака группы врагов ***
        public static void PlayerAttack()
        {
            if (!IsInBattle || CurrentEnemies == null || CurrentEnemies.Count == 0 || CurrentPlayer == null)
                return;

            if (!CurrentPlayer.IsFrozen)
            {
                // Атакуем ВСЕХ живых врагов
                int totalDamage = 0;
                
                foreach (var enemy in CurrentEnemies.Where(e => e.IsAlive()))
                {
                    int dmg = CurrentPlayer.Attack - enemy.Defense;
                    if (dmg < 1) dmg = 1;
                    
                    enemy.CurrentHP -= dmg;
                    totalDamage += dmg;
                    
                    OnLog?.Invoke($"⚔️ Вы нанесли {dmg} урона {enemy.Name}!");
                    
                    if (!enemy.IsAlive())
                    {
                        OnLog?.Invoke($"💀 {enemy.Name} повержен!");
                    }
                    else
                    {
                        OnLog?.Invoke($"   HP {enemy.Name}: {enemy.CurrentHP}/{enemy.MaxHP}");
                    }
                }
                
                OnLog?.Invoke($"💥 Всего нанесено урона: {totalDamage}");
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
            if (!IsInBattle || CurrentEnemies == null || CurrentEnemies.Count == 0 || CurrentPlayer == null)
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

        // *** ОБНОВЛЕННЫЙ МЕТОД: Ход группы врагов ***
        private static void EnemyTurn()
        {
            if (CurrentEnemies == null || CurrentEnemies.Count == 0)
                return;
            
            // Каждый живой враг атакует с уроном 30% от своей атаки
            var aliveEnemies = CurrentEnemies.Where(e => e.IsAlive()).ToList();
            
            if (aliveEnemies.Count > 0)
            {
                OnLog?.Invoke("\n🔴 Ход врагов:");
                
                foreach (var enemy in aliveEnemies)
                {
                    // Враг наносит 30% от своего урона
                    int originalAttack = enemy.Attack;
                    enemy.Attack = (int)(originalAttack * 0.3);
                    
                    if (enemy.Attack < 1) enemy.Attack = 1;
                    
                    enemy.AttackPlayer(CurrentPlayer, rnd);
                    
                    // Восстанавливаем оригинальную атаку
                    enemy.Attack = originalAttack;
                }
                
                OnStatsUpdate?.Invoke(CurrentPlayer.CurrentHP, CurrentPlayer.MaxHP, CurrentTurn);
            }

            CheckBattleEnd();
        }

        // *** ОБНОВЛЕННЫЙ МЕТОД: Проверка конца боя ***
        private static void CheckBattleEnd()
        {
            if (!CurrentPlayer.IsAlive())
            {
                OnShowImage?.Invoke("death");
                OnLog?.Invoke("💀 Вы были побеждены...");
                OnLog?.Invoke($"Вы прошли {CurrentTurn} этажей.");
                IsInBattle = false;
                OnEnableButtons?.Invoke(false);
                OnEnableNextButton?.Invoke(false);
                OnGameOver?.Invoke();
                
                // Возврат в меню через 3 секунды
                System.Threading.Tasks.Task.Delay(3000).ContinueWith(_ => 
                {
                    OnReturnToMenu?.Invoke();
                });
                
                return;
            }

            // Проверяем живы ли враги
            bool allEnemiesDead = CurrentEnemies.All(e => !e.IsAlive());
            
            if (allEnemiesDead)
            {
                OnShowImage?.Invoke("victory");
                
                if (CurrentEnemies.Count == 1)
                {
                    OnLog?.Invoke($"✅ Вы победили {CurrentEnemies[0].Name}!");
                }
                else
                {
                    OnLog?.Invoke($"✅ Вы победили всю группу врагов!");
                    OnLog?.Invoke($"   Повержено: {CurrentEnemies.Count} врагов");
                }
                
                IsInBattle = false;
                OnEnableButtons?.Invoke(false);
                CurrentEnemies = null;
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
                OnShowImage?.Invoke("chest");
                OpenChest(CurrentPlayer);
            }
            else
            {
                // *** ГЕНЕРИРУЕМ ГРУППУ ВРАГОВ ***
                List<Enemy> enemies = GenerateEnemyGroup(isBossTurn);
                StartBattle(CurrentPlayer, enemies);
            }
        }

        public static void StartNewGame()
        {
            CurrentPlayer = new Player("Герой", 100, 10, 5);
            CurrentTurn = 1;
            IsInBattle = false;
            CurrentEnemies = null;

            OnShowImage?.Invoke("start");
            OnLog?.Invoke("🏰 Добро пожаловать в подземелье!");
            OnStatsUpdate?.Invoke(CurrentPlayer.CurrentHP, CurrentPlayer.MaxHP, CurrentTurn);
            OnEnableNextButton?.Invoke(true);
        }
    }
}