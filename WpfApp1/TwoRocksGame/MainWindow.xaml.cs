using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow{
        private int _batu_kiri;
        private int _batu_kanan;
        Random rnd = new Random();

        public int batu_kiri{
            get{ return _batu_kiri;}
            set{
                _batu_kiri = value;
                LeftRocksCounter.Text = Convert.ToString(value);
                
                LeftRocksPanel.Children.Clear();

                for (int i = 0; i < value; i++){
                    Ellipse rock_visual = new Ellipse();
                    LeftRocksPanel.Children.Add(rock_visual);
                }
            }
        }
        
        public int batu_kanan{
            get{ return _batu_kanan;}
            set{
                _batu_kanan = value; 
                RightRocksCounter.Text = Convert.ToString(value);
                
                RightRocksPanel.Children.Clear();

                for (int i = 0; i < value; i++){
                    Ellipse rock_visual = new Ellipse();
                    RightRocksPanel.Children.Add(rock_visual);
                }
            }
        }
        
        public MainWindow()
        {
            InitializeComponent();
            disable_all_button();
            Dispatcher.Invoke(start_game);
        }

        public async void start_game(){
            batu_kiri = 10;
            batu_kanan = 10;
            disable_all_button();

            Notification.Text = "Selamat datang di permainan Dua Batu!";
            NotificationBorder.BorderBrush = new SolidColorBrush(Colors.DeepSkyBlue);
            
            await Task.Delay(1000);
            await Dispatcher.Invoke(async () => await computer_round());
        }

        private void computer_ambil_kiri(){
            Notification.Text = "Komputer mengambil sebuah batu dari kiri";
            NotificationBorder.BorderBrush = new SolidColorBrush(Colors.OrangeRed);
            Debug.Assert(batu_kiri > 0);
            batu_kiri -= 1;
        }
        
        private void computer_ambil_kanan(){
            Notification.Text = "Komputer mengambil sebuah batu dari kanan";
            NotificationBorder.BorderBrush = new SolidColorBrush(Colors.OrangeRed);
            Debug.Assert(_batu_kanan > 0);
            batu_kanan -= 1;
        }
        
        private void computer_ambil_keduanya(){
            Notification.Text = "Komputer mengambil kedua tumpukan batu";
            NotificationBorder.BorderBrush = new SolidColorBrush(Colors.OrangeRed);
            
            Debug.Assert(batu_kiri > 0 && batu_kanan > 0);
            batu_kiri -= 1;
            batu_kanan -= 1;
        }

        private async Task computer_round(){
            await Task.Delay(1500);

            if (batu_kiri % 2 == 0 && batu_kanan % 2 == 0){
                if (batu_kiri > 0 && batu_kanan > 0){
                    int random = rnd.Next(0, 3);

                    if (random == 0){
                        computer_ambil_kiri();
                    }else if (random == 1){
                        computer_ambil_kanan();
                    }else if (random == 2){
                        computer_ambil_keduanya();
                    }
                }else if (batu_kiri > 0){
                    computer_ambil_kiri();
                }else{
                    computer_ambil_kanan();
                }
            }
            else{
                if (batu_kiri % 2 == 1 && batu_kanan % 2 == 1){
                    computer_ambil_keduanya();
                }else if (batu_kiri % 2 == 1){
                    computer_ambil_kiri();
                }else{
                    computer_ambil_kanan();
                }
            }
            
            bool game_is_not_done_yet = !check_round(false);
            if ((batu_kiri > 0 || batu_kanan > 0) && game_is_not_done_yet)
                enable_button();
        }

        private void disable_all_button(){
            AmbilKiri.IsEnabled = false;
            AmbilDua.IsEnabled = false;
            AmbilKanan.IsEnabled = false;
        }

        private async void user_round(int left_add, int right_add){
            batu_kiri += left_add;
            batu_kanan += right_add;
            
            bool game_is_not_done_yet = !check_round(true);

            if ((batu_kiri > 0 || batu_kanan > 0) && game_is_not_done_yet)
                await computer_round();
        }


        private bool check_round(bool is_currently_user_round){
            if (batu_kiri == 0 && batu_kanan == 0){
                disable_all_button();

                if (is_currently_user_round) MessageBox.Show("Selamat! Kamu memenangkan permainan!");
                else MessageBox.Show("Yah, kamu masih belum memenangkan permainan :(");
                start_game();
                return true;
            }

            return false;
        }

        private void enable_button(){
            if (batu_kiri > 0) AmbilKiri.IsEnabled = true;
            if (batu_kanan > 0) AmbilKanan.IsEnabled = true;
            if (batu_kiri > 0 && batu_kanan > 0) AmbilDua.IsEnabled = true;
        }


        private void AmbilKiri_OnClick(object sender, RoutedEventArgs e){
            disable_all_button();
            Notification.Text = "Anda mengambil sebuah batu dari kiri";
            NotificationBorder.BorderBrush = new SolidColorBrush(Colors.Chartreuse);
            
            Dispatcher.Invoke(() => user_round(-1, 0));
        }

        private void AmbilDua_OnClick(object sender, RoutedEventArgs e){
            disable_all_button();
            Notification.Text = "Anda mengambil kedua tumpukan batu";
            NotificationBorder.BorderBrush = new SolidColorBrush(Colors.Chartreuse);
            
            Dispatcher.Invoke(() => user_round(-1, -1));
        }

        private void AmbilKanan_OnClick(object sender, RoutedEventArgs e){
            disable_all_button();
            Notification.Text = "Anda mengambil sebuah batu dari kanan";
            NotificationBorder.BorderBrush = new SolidColorBrush(Colors.Chartreuse);
            
            Dispatcher.Invoke(() => user_round(0, -1));
        }
    }
}