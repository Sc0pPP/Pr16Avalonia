using System;
using _3ISIP323_Zevakin.Новая_папка;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace Pr16Avalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Game.OnLog += AddLog;
        Game.OnStatsUpdate += UpdateStats;
        Game.OnEnableButtons += EnableActionButtons;
        Game.OnEnableNextButton += EnableNextButton;
        Game.OnShowEquipChoice += ShowEquipChoice;
        Game.OnGameOver += OnGameOver;
        Game.OnShowImage += ShowImage; 

    }
  // Начать игру
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            LogText.Text = "";
            StartGameButton.IsVisible = false;
            GameImage.IsVisible = true; 
            
            RestartButton.IsVisible = false;
            RestartButton.IsEnabled = false;
            
            Game.StartNewGame();
        }

        // Атака
        private void AttackButton_Click(object sender, RoutedEventArgs e)
        {
            Game.PlayerAttack();
        }

        // Защита
        private void DefendButton_Click(object sender, RoutedEventArgs e)
        {
            Game.PlayerDefend();
        }

        // Далее
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Game.NextTurn();
        }

        // Взять экипировку
        private void TakeEquipButton_Click(object sender, RoutedEventArgs e)
        {
            Game.EquipItem();
            HideEquipChoice();
        }

        // Оставить старую экипировку
        private void KeepEquipButton_Click(object sender, RoutedEventArgs e)
        {
            Game.KeepOldItem();
            HideEquipChoice();
        }

        // Перезапуск
        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            LogText.Text = "";
            AttackButton.IsEnabled = false;
            DefendButton.IsEnabled = false;
            NextButton.IsEnabled = false;
            RestartButton.IsVisible = false;
            Game.StartNewGame();
        }

        // *** ПОКАЗАТЬ КАРТИНКУ ***
        private void ShowImage(string imageName)
        {
            Dispatcher.UIThread.Post(() =>
            {
                try
                {
                    string imagePath = $"C:/Users//Scop/RiderProjects/Pr16Avalonia/Pr16Avalonia/Assets/Images/{imageName}.jpg";
                    
                    
                    var bitmap = new Bitmap(imagePath);
                    GameImage.Source = bitmap;
                    GameImage.IsVisible = true;
                }
                catch (Exception ex)
                {
                    // Если картинка не найдена - просто скрываем Image
                    GameImage.IsVisible = false;
                    System.Diagnostics.Debug.WriteLine($"Не удалось загрузить картинку: {imageName}, ошибка: {ex.Message}");
                }
            });
        }

        // Добавить сообщение в лог
        private void AddLog(string message)
        {
            Dispatcher.UIThread.Post(() =>
            {
                LogText.Text += message + "\n";
                
                var parent = LogText.Parent as Control;
                while (parent != null && !(parent is ScrollViewer))
                {
                    parent = parent.Parent as Control;
                }
                
                if (parent is ScrollViewer scrollViewer)
                {
                    scrollViewer.ScrollToEnd();
                }
            });
        }

        // Обновить статистику
        private void UpdateStats(int hp, int maxHp, int floor)
        {
            Dispatcher.UIThread.Post(() =>
            {
                HealthText.Text = $"{hp}/{maxHp}";
                FloorText.Text = floor.ToString();
            });
        }

        // Включить/выключить кнопки действий (атака/защита)
        private void EnableActionButtons(bool enable)
        {
            Dispatcher.UIThread.Post(() =>
            {
                AttackButton.IsEnabled = enable;
                DefendButton.IsEnabled = enable;
            });
        }

        // Включить/выключить кнопку "Далее"
        private void EnableNextButton(bool enable)
        {
            Dispatcher.UIThread.Post(() =>
            {
                NextButton.IsEnabled = enable;
            });
        }

        // Показать выбор экипировки
        private void ShowEquipChoice(string name, int value, bool isWeapon)
        {
            Dispatcher.UIThread.Post(() =>
            {
                BattleButtons.IsVisible = false;
                EquipButtons.IsVisible = true;
            });
        }

        // Скрыть выбор экипировки
        private void HideEquipChoice()
        {
            BattleButtons.IsVisible = true;
            EquipButtons.IsVisible = false;
        }

        // Конец игры
        private void OnGameOver()
        {
            Dispatcher.UIThread.Post(() =>
            {
                AttackButton.IsEnabled = false;
                DefendButton.IsEnabled = false;
                NextButton.IsEnabled = false;
                
                RestartButton.IsVisible = true;
                RestartButton.IsEnabled = true;
            });
        }
    }

    

