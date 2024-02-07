using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

namespace Eidos
{
    public partial class MainWindow : Window
    {
        static Random random = new Random();
        private readonly Dictionary<int, int> Positions = new Dictionary<int, int>();
        private List<ProgressBar> ProgressBars = new List<ProgressBar>();
        private int currentCountTasks = 0;
        private List<ProgressBar> FrozenProgressBars = new List<ProgressBar>();

        private bool Order = true;
        private List<Thickness> nextPositions = new List<Thickness>();

        public MainWindow()
        {
            InitializeComponent();
            Logs.IsReadOnly = true;
            Logs.TextWrapping = TextWrapping.Wrap;

            RemoveAll.IsEnabled = false;
            StopAll.IsEnabled   = false;  
            RunAll.IsEnabled    = false;

            Positions.Add(0, 25);
            Positions.Add(1, 100);
            Positions.Add(2, 175);
            Positions.Add(3, 250);
            Positions.Add(4, 325);   
        }

        // 50% вероятность успеха или ошибки при полном заполнении ProgressBar
        private void isProgressBarFinished(object sender, RoutedEventArgs e)
        {
            ProgressBar progressBar = (ProgressBar)sender;
            foreach (var item in ProgressBars)
            {
                if (progressBar == item)
                {
                    if (item.Value == 0.0 && item.IsAnimating(ProgressBar.ValueProperty) == false)
                    {
                        if(random.Next(0, 100) <= 49)
                        {
                            Logs.Text += "\n" + DateTime.Now.ToString("HH:mm:ss") + " Задача " + item.Name + " завершилась c ошибкой!";
                        } else
                        {
                            Logs.Text += "\n" + DateTime.Now.ToString("HH:mm:ss") + " Задача " + item.Name + " завершилась c успехом!";
                        }
                    }
                }
            }
        }

        // Удаление отдельного ProgressBar при нажатии на кнопку, которая появляется после двойного нажатия на ProgressBar
        private void ClickDeleteButton(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            foreach(var item in ProgressBars)
            {
                if(item.Margin.Top == button.Margin.Top)
                {
                    Label label = new Label();
                    label.Content = item.Name.ToString();
                    MainCanvas.Children.Remove(item);

                    foreach(var children in MainCanvas.Children)
                    {
                        if(children.Margin.Top == button.Margin.Top && children.Margin.Left == 495)
                        {
                            MainCanvas.Children.Remove(children);
                            break;
                        }
                    }

                    MainCanvas.Children.Remove(button);
                    ProgressBars.Remove(item);
                    currentCountTasks--;
                    Order = false;
                    nextPositions.Add(item.Margin);
                    break;
                }
            }


            Logs.Text += "\n" + DateTime.Now.ToString("HH:mm:ss") + " Удаление завершено";
        }

        // Вывод кнопки для удаления отдельного ProgressBar и краткой информации по задаче
        private void DoubleTappedProgressBar(object sender, RoutedEventArgs e)
        {
            ProgressBar progressBar = (ProgressBar)sender;
            progressBar.Width = 450;

            Label label = new Label();
            label.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
            label.Content = progressBar.Name;
            label.Margin = new Thickness(495, progressBar.Margin.Top, 0, 0);
            MainCanvas.Children.Add(label);

            Button deleteButton = new Button();
            deleteButton.Click += ClickDeleteButton;
            deleteButton.Content = "Удалить";
            deleteButton.Width  = 161;
            deleteButton.Height = 50;
            deleteButton.Margin = new Thickness(614, progressBar.Margin.Top, 0, 0);
            MainCanvas.Children.Add(deleteButton);
            Logs.Text += "\n" + DateTime.Now.ToString("HH:mm:ss") + " Двойной клик по ProgressBar";
        }

        // Добавление задачи типа A, подписка на события и запуск анимации
        private void ClickAddTaskTypeA(object sender, RoutedEventArgs e)
        {
            if (currentCountTasks < 5)
            {
                ProgressBar progressBar = new ProgressBar();
                progressBar.ShowProgressText = true;
                progressBar.Foreground = new SolidColorBrush(Colors.ForestGreen);
                progressBar.Name = "A Номер " + (currentCountTasks + 1).ToString();
                progressBar.Background = Brushes.MistyRose;
                progressBar.Width = 750;
                progressBar.Height = 50;
                if (Order == true)
                {
                    progressBar.Margin = new Thickness(25, Positions[currentCountTasks], 0, 0);
                }
                else if (Order == false)
                {
                    progressBar.Margin = nextPositions[0];
                    nextPositions.Remove(nextPositions[0]);
                }
                progressBar.ValueChanged += isProgressBarFinished;
                progressBar.DoubleTapped += DoubleTappedProgressBar;
                MainCanvas.Children.Add(progressBar);

                new Animation()
                {
                    Duration = TimeSpan.FromSeconds(new Random().Next(5, 10)),
                    IterationCount = IterationCount.Parse("1"),
                    Children =
                {
                    new KeyFrame
                    {
                        Cue = default,
                        Setters =
                        {
                            new Setter(ProgressBar.ValueProperty, 0.0)
                        }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue(1),
                        Setters =
                        {
                            new Setter(ProgressBar.ValueProperty, 100.0)
                        }
                    }
                }
                }.RunAsync(progressBar);

                ProgressBars.Add(progressBar);
                currentCountTasks++;
                Logs.Text += "\n" + DateTime.Now.ToString("HH:mm:ss") + " Добавлена задача типа A!";

                if (RemoveAll.IsEnabled == false || StopAll.IsEnabled == false || RunAll.IsEnabled == false)
                {
                    RemoveAll.IsEnabled = true;
                    StopAll.IsEnabled = true;
                    RunAll.IsEnabled = true;

                    RemoveAll.Opacity = 100;
                    StopAll.Opacity = 100;
                    RunAll.Opacity = 100;
                }
            } 
            else
            {
                Logs.Text += "\n" + DateTime.Now.ToString("HH:mm:ss") + " Превышено допустимое количество задач!";
            }
        }

        // Добавление задачи типа B, подписка на события и запуск анимации
        private void ClickAddTaskTypeB(object sender, RoutedEventArgs e)
        {
            if (currentCountTasks < 5)
            {
                ProgressBar progressBar = new ProgressBar();
                progressBar.ShowProgressText = true;
                progressBar.Foreground = new SolidColorBrush(Colors.ForestGreen);
                progressBar.Name = "B  Номер " + (currentCountTasks + 1).ToString();
                progressBar.Background = Brushes.Bisque;
                progressBar.Width = 750;
                progressBar.Height = 50;
                if (Order == true)
                {
                    progressBar.Margin = new Thickness(25, Positions[currentCountTasks], 0, 0);
                }
                else if (Order == false)
                {
                    progressBar.Margin = nextPositions[0];
                    nextPositions.Remove(nextPositions[0]);
                }
                progressBar.ValueChanged += isProgressBarFinished;
                progressBar.DoubleTapped += DoubleTappedProgressBar;
                MainCanvas.Children.Add(progressBar);

                new Animation()
                {
                    Duration = TimeSpan.FromSeconds(new Random().Next(5, 10)),
                    IterationCount = IterationCount.Parse("1"),
                    Children =
                {
                    new KeyFrame
                    {
                        Cue = default,
                        Setters =
                        {
                            new Setter(ProgressBar.ValueProperty, 0.0)
                        }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue(1),
                        Setters =
                        {
                            new Setter(ProgressBar.ValueProperty, 100.0)
                        }
                    }
                }
                }.RunAsync(progressBar);

                ProgressBars.Add(progressBar);
                currentCountTasks++;
                Logs.Text += "\n" + DateTime.Now.ToString("HH:mm:ss") + " Добавлена задача типа B!";

                if (RemoveAll.IsEnabled == false || StopAll.IsEnabled == false || RunAll.IsEnabled == false)
                {
                    RemoveAll.IsEnabled = true;
                    StopAll.IsEnabled = true;
                    RunAll.IsEnabled = true;

                    RemoveAll.Opacity = 100;
                    StopAll.Opacity = 100;
                    RunAll.Opacity = 100;
                }
            }
            else
            {
                Logs.Text += "\n" + DateTime.Now.ToString("HH:mm:ss") + " Превышено допустимое количество задач!";
            }
        }

        // Фильтр задач по типу A
        private void ShowOnlyTypeA(object sender, RoutedEventArgs e)
        {
            foreach (var item in ProgressBars)
            {
                if (item.Name[0] == 'A')
                {
                    item.Opacity = 100;
                }
                else if (item.Name[0] == 'B')
                {
                    item.Opacity = 0;
                }
            }
            Logs.Text += "\n" + DateTime.Now.ToString("HH:mm:ss") + " Фильтр по типу A!";
        }

        // Фильтр задач по типу A
        private void ShowOnlyTypeB(object sender, RoutedEventArgs e)
        {
            foreach (var item in ProgressBars)
            {
                if (item.Name[0] == 'B')
                {
                    item.Opacity = 100;
                }
                else if (item.Name[0] == 'A')
                {
                    item.Opacity = 0;
                }
            }
            Logs.Text += "\n" + DateTime.Now.ToString("HH:mm:ss") + " Фильтр по типу B!";
        }

        // Удаление всех задач
        private void RemoveAllTasks(object sender, RoutedEventArgs e)
        {
            foreach (var item in ProgressBars)
            {
                MainCanvas.Children.Remove(item);
            }
            ProgressBars.Clear();
            currentCountTasks = 0;
            Logs.Text += "\n" + DateTime.Now.ToString("HH:mm:ss") + " Все задачи удалены!";

            RemoveAll.Opacity = 50;
            StopAll.Opacity   = 50;
            RunAll.Opacity    = 50;

            RemoveAll.IsEnabled = false;
            StopAll.IsEnabled   = false;
            RunAll.IsEnabled    = false;

            Order = true;
        }

        // "Заморозка" выполнения всех задач
        private void StopAllTasks(object sender, RoutedEventArgs e)
        {
            foreach (var item in ProgressBars)
            {
                ProgressBar progressBar = new ProgressBar();
                progressBar.ShowProgressText = true;
                progressBar.Foreground = new SolidColorBrush(Colors.ForestGreen);
                progressBar.Name = item.Name;
                progressBar.Background = Brushes.Orange;
                progressBar.Width = 750;
                progressBar.Height = 50;
                progressBar.Value = item.Value;
                progressBar.Margin = item.Margin;

                progressBar.ValueChanged += isProgressBarFinished;
                progressBar.DoubleTapped += DoubleTappedProgressBar;

                FrozenProgressBars.Add(progressBar);
            }

            foreach (var item in ProgressBars)
            {
                MainCanvas.Children.Remove(item);
            }
            ProgressBars.Clear();

            foreach(var item in FrozenProgressBars)
            {
                MainCanvas.Children.Add(item);
                ProgressBars.Add(item);
            }
            Logs.Text += "\n" + DateTime.Now.ToString("HH:mm:ss") + " Все задачи остановлены!";
        }

        // Возобновление работы всех задач
        private void RunAllTasks(object sender, RoutedEventArgs e)
        {
            foreach (var item in ProgressBars)
            {
                if (item.Name[0] == 'A')
                {
                    item.Background = Brushes.MistyRose;
                } else
                {
                    item.Background = Brushes.Bisque;
                }

                new Animation()
                {
                    Duration = TimeSpan.FromSeconds(new Random().Next(7, 10)),
                    IterationCount = IterationCount.Parse("1"),
                    Children =
                {
                    new KeyFrame
                    {
                        Cue = default,
                        Setters =
                        {
                            new Setter(ProgressBar.ValueProperty, item.Value)
                        }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue(1),
                        Setters =
                        {
                            new Setter(ProgressBar.ValueProperty, 100.0)
                        }
                    }
                }
                }.RunAsync(item);
            }

            Logs.Text += "\n" + DateTime.Now.ToString("HH:mm:ss") + " Все задачи запущены!";

            foreach (var item in ProgressBars)
            {
                item.Value = 0.0;
            }
        }
    }
}