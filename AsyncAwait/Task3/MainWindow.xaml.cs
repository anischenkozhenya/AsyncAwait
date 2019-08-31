using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
//Создайте WPF приложение, разместите в окне TextBox и две кнопки.При нажатии на первую
//кнопку в TextBox выводится сообщение «Подключен к базе данных» при этом в обработчике
//установите задержку в 3-5 сек для имитации подключения к БД, также данная кнопка запускает
//таймер, который с периодичностью в несколько секунд выводит в TextBox сообщение «Данные
//получены». При нажатии на вторую кнопку по аналогии с первой отключаемся от базы(с
//задержкой), выводим сообщение и останавливаем таймер.
namespace Task3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        private void MyTick(object sender, EventArgs e)
        {
            TextboxMain.Text += "Данные получены\n";
        }
        static string ConnectMethod()
        {
            Thread.Sleep(5000);
            return "Подключен к базе данных";
        }
        static string Disconnect()
        {
            return "Отключен от базы данных";
        }

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(MyTick);
        }

        private async void ConnectToDataBase_Click(object sender, RoutedEventArgs e)
        {
            Task<string> task = Task.Factory.StartNew(new Func<string>(ConnectMethod));
            TextboxMain.Text += await task + "\n";
            timer.Start();
        }

        private async void ConnectToDataBase_Copy_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            Task<string> task = new Task<string>(Disconnect);
            task.Start();
            TextboxMain.Text = TextboxMain.Text + await task + "\n";
        }

    }
}
