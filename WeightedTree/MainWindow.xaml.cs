using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WeightedTree
{
    /// <summary>
    /// Логіка взаємодії для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class TableRow
        {
            public string symbol { get; set; }
            public int frequency { get; set; }
            public string code { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        TreePoints[][] drawingPoints;
        double x1, x2, y1, y2, wP, hP;

        Tree Tree;
        List<string> str1 = new List<string>();
        List<int> str2 = new List<int>();   
        int countLevel;

        static string currDir = Environment.CurrentDirectory.ToString();
        string FilePath;
        bool emptyData = true;

        Window enterDataWindow = new Window();

        private void Form1_Load(object sender, RoutedEventArgs e)
        {
     
        }

        private void Form_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!emptyData)
            {
                TablePrint();
                TreeDrawing();
            }
        }

        //Обробка події натискання на кнопку «Завантажити дані».
        //Відкриває діалогове вікно та зчитує дані вибраного файлу
        private void DownloadFromFile_Click(object sender, RoutedEventArgs e)
        {
            str1 = new List<string>();
            str2 = new List<int>();
            Tree = new Tree();
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                FilePath = openFileDialog.FileName;
            else FilePath = currDir + @"\test1.txt";

            try
            {
                using (StreamReader sr = new StreamReader(FilePath))
                {
                    while (sr.Peek() >= 0)
                    {
                        string lineCurrent = sr.ReadLine();
                        string[] words = lineCurrent.Split(' ');
                        str1.Add(words[0]);
                        str2.Add(Convert.ToInt32(words[1]));
                    }
                }
            }
            catch
            {
                MessageBox.Show("Відбулася помилка зчитування. Буде завантажено документ \"test1.txt\" ");
                DownloadTest();            
            }
            emptyData = false;
            if (CheckData(str1))
                WorkWithTree();
            else
            {
                MessageBox.Show("Сред даданих символів знаходяться 2 одинакових символа. Побудова такого дерева неможлива. Буде завантажено документ \"test1.txt\" ");
                DownloadTest();
                WorkWithTree();
            }
        }

        //Обробка події натискання на кнопку «Зберегти дані».
        //Якщо дерево містить дані, то показує на екран діалогове вікно і зберігає,
        //якщо дерево порожнє – показує відповідне повідомлення
        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            if (!emptyData)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                    FilePath = saveFileDialog.FileName;
                else FilePath = currDir + @"\Savedtest.txt";

                using (StreamWriter sw = new StreamWriter(FilePath))
                {
                    for (int i = 0; i < str1.Count; i++)
                        sw.WriteLine(str1[i] + " " + str2[i]);
                }
            }
            else MessageBox.Show("Дерево - порожнє. Збереження не буде виконано.");
        }

        TextBox tBData;

        //Обробка події натискання на кнопку «Ввести дані».
        //Виводить на екран додаткове вікно для введення даних дерева вручну
        private void EnterData_Click(object sender, RoutedEventArgs e)
        {
            enterDataWindow = new Window();
            enterDataWindow.Title = "WeightedTree";
            enterDataWindow.Height = 200;
            enterDataWindow.Width = 500;

            tBData = new TextBox();
            tBData.Height = 80;
            tBData.Width = 300;
            tBData.HorizontalAlignment = HorizontalAlignment.Left;
            tBData.VerticalAlignment = VerticalAlignment.Bottom;
            tBData.Margin = new Thickness(20, 0, 0, 20);
            tBData.TextWrapping = TextWrapping.Wrap;

            Button b = new Button();
            b.Content = "Ok";
            b.Click += B_Click;
            b.Height = 50;
            b.Width = 150;
            b.VerticalAlignment = VerticalAlignment.Bottom;
            b.HorizontalAlignment = HorizontalAlignment.Right;
            b.Margin = new Thickness(0, 0, 10, 20);
            b.Background = Brushes.SkyBlue;

            TextBlock tB = new TextBlock() { Text = "   Введення даних виконується через пробіл, кожна пара значень складається із символу та його частоти. Після введення натисніть кнопку \"Ok\" " };
            tB.TextWrapping = TextWrapping.Wrap;
            tB.VerticalAlignment = VerticalAlignment.Top;
            tB.HorizontalAlignment = HorizontalAlignment.Center;
            tB.Width = 400;
            tB.Margin = new Thickness(0, 20, 0, 0);
            tB.Foreground = Brushes.DarkSlateBlue;

            Grid q = new Grid();
            q.Children.Add(tBData);
            q.Background = Brushes.LightBlue;
            q.Children.Add(b);
            q.Children.Add(tB);
            enterDataWindow.Content = q;
            enterDataWindow.Show();

        }
          
        private void B_Click(object sender, RoutedEventArgs e)
        {
            str1 = new List<string>();
            str2 = new List<int>();

            string data = tBData.Text;
            try
            {
                string[] words = data.Split(' ');
                for (int i = 0; i < words.Length / 2; i++)
                {
                    str1.Add(words[2 * i]);
                    str2.Add(Convert.ToInt32(words[2 * i + 1]));
                }
            }
            catch
            {
                MessageBox.Show("Відбулася помилка зчитування. Буде завантажено документ \"test1.txt\" ");

                DownloadTest();
            }
            enterDataWindow.Close();

            emptyData = false;
            Tree = new Tree();
            if (CheckData(str1))
                WorkWithTree();
            else
            {
                MessageBox.Show("Сред даданих символів знаходяться 2 одинакових символа. Побудова такого дерева неможлива. Буде завантажено документ \"test1.txt\" ");
                DownloadTest();
                WorkWithTree();
            }
        }

        //Обробка події натискання на кнопку «Додати елемент».
        //Виконує додавання нового елемента до дерева
        private void AddElement_Click(object sender, RoutedEventArgs e)
        {
            if (!emptyData)
            {
                for (int i = 0; i < str1.Count; i++)
                    if (tBNewEl.Text == str1[i])
                    {
                        MessageBox.Show(" Елемент вже міститься у дереві. \n\n Примітка: у дереві неможливе знаходження одного елемента з різною частотою!");
                        return;
                    }

                str1.Add(tBNewEl.Text);
                str2.Add(Convert.ToInt32(tBNewW.Text));
                Tree = new Tree();
                WorkWithTree();
            }
            else MessageBox.Show("  Дерево порожнє! Для початку роботи з деревом заповніть його, для цього натисніть \"Завантажити дані\" або \"" + "Ввести дані\".");
        }

        //Обробка події натискання кнопки «Видалити елемент».
        //Виконує видалення елемента з дерева за парою символ-частота
        private void DeleteElement_Click(object sender, RoutedEventArgs e)
        {
            if (!emptyData)
            {
                bool deleted = false;
                if (!emptyData && Tree.count > 2)
                {
                    for (int i = 0; i < str1.Count; i++)
                        if (tBNewEl.Text == str1[i] && Convert.ToInt32(tBNewW.Text) == str2[i])
                        {
                            str1.RemoveAt(i);
                            str2.RemoveAt(i);
                            deleted = true;
                        }

                    if (deleted)
                    {
                        Tree = new Tree();
                        WorkWithTree();
                    }
                    else MessageBox.Show(" Введений елемент не міститься у дереві. \n\n Примітка: для видалення елемента також потрібно правильно вказати його частоту!");
                }
                else MessageBox.Show(" Дерево містить менше двох елементів. Видалення не буде виконано!");
            }
            else MessageBox.Show("  Дерево порожнє! Для початку роботи з деревом заповніть його, для цього натисніть \"Завантажити дані\" або \"" + "Ввести дані\".");
        }

        //Обробка події натискання на кнопку «Декодувати».
        //Виконує декодування заданого рядка за кодом Хаффмана
        private void Decode_Click(object sender, RoutedEventArgs e)
        {
            if (!emptyData)
            {
                textBox2.Text = "";
                string co = textBox1.Text;
                string[] reply = co.Split(' ');
                for (int i = 0; i < reply.Length; i++)
                {
                    reply[i] = Tree.FindNode(reply[i]);
                    textBox2.AppendText(reply[i] + ' ');
                }
            }
            else MessageBox.Show("  Дерево порожнє! Для початку роботи з деревом заповніть його, для цього натисніть \"Завантажити дані\" або \"" + "Ввести дані\".");
        }

        //Метод для завантаження тестового файлу "test1.txt"
        public void DownloadTest()
        {
            str1 = new List<string>();
            str2 = new List<int>();
            using (StreamReader sr = new StreamReader(currDir + @"\test1.txt"))
            {
                while (sr.Peek() >= 0)
                {
                    string lineCurrent = sr.ReadLine();
                    string[] words = lineCurrent.Split(' ');
                    str1.Add(words[0]);
                    str2.Add(Convert.ToInt32(words[1]));
                }
            }
        }

        List<TableRow> table;
        //Метод для виведення даних про дерево до таблиці
        public void TablePrint()
        {
            table = new List<TableRow>();
            for (int i = 0; i < Tree.table.Count; i++)
                table.Add(new TableRow { symbol = str1[i], frequency = str2[i], code = Tree.table[str1[i]] });

            dataGrid1.ItemsSource = table;
            dataGrid1.Columns[0].Width = (dataGrid1.ActualWidth - 10.0) / 4.0;
            dataGrid1.Columns[1].Width = (dataGrid1.ActualWidth - 10.0) / 4.0;
            dataGrid1.Columns[2].Width = (dataGrid1.ActualWidth - 10.0) / 2.0;
            dataGrid1.Columns[0].Header = "Символ";
            dataGrid1.Columns[1].Header = "Частота";
            dataGrid1.Columns[2].Header = "Код";
        }

        //Метод для зображення дерева
        public void TreeDrawing()
        {
            pictureCanvas.Children.Clear();

            x1 = 10;
            x2 = pictureCanvas.ActualWidth - 30;
            y1 = 5;
            y2 = pictureCanvas.ActualHeight - 30;
            wP = x2 - x1;
            hP = y2 - y1;

            countLevel = Tree.levelCount;
            drawingPoints = Tree.Print(drawingPoints, x1, x2, y1, y2, wP, hP);
            string[][] key = Tree.k;

            for (int i = 0; i < countLevel; i++)
            {
                for (int j = 0; j < drawingPoints[i].Length; j++)
                {
                    if (drawingPoints[i][j].X != 0)
                    {
                        if (i != countLevel - 1 && drawingPoints[i + 1][2 * j].X != 0)
                        {
                            Line a = new Line();
                            a.X1 = drawingPoints[i][j].X;
                            a.X2 = drawingPoints[i + 1][2 * j].X;
                            a.Y1 = drawingPoints[i][j].Y;
                            a.Y2 = drawingPoints[i + 1][2 * j].Y;
                            a.Stroke = Brushes.DarkBlue;
                            pictureCanvas.Children.Add(a);
                        }

                        if (i != countLevel - 1 && drawingPoints[i + 1][2 * j + 1].X != 0)
                        {
                            Line a = new Line();
                            a.X1 = drawingPoints[i][j].X;
                            a.X2 = drawingPoints[i + 1][2 * j + 1].X;
                            a.Y1 = drawingPoints[i][j].Y;
                            a.Y2 = drawingPoints[i + 1][2 * j + 1].Y;
                            a.Stroke = Brushes.DarkBlue;
                            pictureCanvas.Children.Add(a);
                        }

                        Canvas myCanvers = new Canvas();
                        Canvas.SetLeft(myCanvers, drawingPoints[i][j].X - 5);
                        Canvas.SetTop(myCanvers, drawingPoints[i][j].Y - 5);
                        myCanvers.Width = 20;
                        myCanvers.Height = 20;

                        Ellipse el = new Ellipse();
                        el.Width = 20;
                        el.Height = 20;
                        el.Stroke = Brushes.Black;
                        el.Fill = Brushes.Aqua;

                        TextBlock tx = new TextBlock() { Text = key[i][j] };
                        tx.HorizontalAlignment = HorizontalAlignment.Center;
                        tx.Foreground = Brushes.BlueViolet;
                        //tx.FontSize = 3;
                        Canvas.SetLeft(tx, 5);

                        myCanvers.Children.Add(el);
                        myCanvers.Children.Add(tx);
                        pictureCanvas.Children.Add(myCanvers);

                    }
                }
            }
        }

        //Метод роботи з деревом
        public void WorkWithTree()
        {
            if (str1.Count > 1)
            {
                for (int i = 0; i < str1.Count; i++)
                    Tree.CreateQue(str1[i], str2[i]);

                Tree.CreateTree();
                TablePrint();
                Tree.levelCount = DetermeCountLevel();
                TreeDrawing();
            }
            else
            {
                MessageBox.Show("Дерево містить менше двох елементів. Буде завантежено документ \"test1.txt\" ");
                DownloadTest();
                WorkWithTree();
            }
        }

        public int DetermeCountLevel()
        {
            int length = table[0].code.Length;
            for (int i = 0; i < table.Count; i++)
                if (length < table[i].code.Length)
                    length = table[i].code.Length;

            return length + 1;
        }

        //Метод перевірки введених даних на коректність.
        public bool CheckData(List<string> s)
        {
            bool clean = true;
            string str;
            for (int i = 0; i < s.Count; i++)
            {
                str = s[i];
                for (int j = 0; j < s.Count; j++)
                {
                    if (str == s[j] && i!=j)
                        clean = false;
                }
            }
            return clean;
        }
    }
}
