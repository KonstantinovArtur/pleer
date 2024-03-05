using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

namespace WpfApp8
{
    /// Я не реализовал историю, повтор, перемешку и отслеживание секунд музыки.
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> Pl { get; set; }
        private DispatcherTimer timer;
        private int Index = 0;

        public MainWindow()
        {
            InitializeComponent();
            Pl = new ObservableCollection<string>();
            ListBox.ItemsSource = Pl;
            media.Volume = 0.5;
            audioSlider.Value = 5;
            media.Volume = audioSlider.Value;          
        }

        private void AddToPlaylist(string filePath)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            Pl.Add(filePath);
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBox.SelectedItem != null)
            {
                string selectedSong = ListBox.SelectedItem.ToString();
                media.Source = new Uri(selectedSong);
                Index = ListBox.SelectedIndex;
                media.Play();
            }
        }
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new OpenFileDialog();
            if (folderDialog.ShowDialog() == true)
            {
                string folderPath = System.IO.Path.GetDirectoryName(folderDialog.FileName);
                string[] audioFiles = Directory.GetFiles(folderPath, "*.mp3");
                foreach (string file in audioFiles)
                {
                    AddToPlaylist(file);
                    audioSlider.ValueChanged += audioSlider_ValueChanged;
                    media.Source = new Uri(Pl[0]);
                    media.Play();

                    timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(100);
                    timer.Tick += Timer_Tick;
                   

                }
            }
        }
 
        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            musicSlider.Maximum = media.NaturalDuration.TimeSpan.TotalSeconds;
            timer.Start();
            TimeSpan time1 = media.NaturalDuration.TimeSpan;
            TimeSpan time2= media.Position;
            qwe.Text = time1.ToString(@"mm\:ss");
            qwer.Text = time2.ToString(@"mm\:ss");
        }
        private void media_MediaEnded(object sender, EventArgs e)
        {
            Index++;
            ListBox.SelectedIndex = Index;
        }


        private void musicSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
                media.Position = TimeSpan.FromSeconds(musicSlider.Value);   
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            musicSlider.Value = media.Position.TotalSeconds;   
        }


        private void audioSlider_ValueChanged(object sender, EventArgs e)
        {
            double v = audioSlider.Value / 10;
            media.Volume = v;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            media.Pause();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            media.Play();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Index++;
            ListBox.SelectedIndex = Index;
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Index--;
            ListBox.SelectedIndex = Index;
        }
    }
}
