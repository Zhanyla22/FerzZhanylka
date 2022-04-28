using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FerzZhanylka
{
    public partial class Form1 : Form
    {
        private int N; // количество ферзей
                       // Проверка, бьются ли 2 ферзя, возвращает false если не бьются
        void AddToListBox(int[] M, int N)
        {
            // добавить к listBox1 строку
            string s = "";
            for (int i = 1; i <= N; i++)
                s = s + M[i].ToString() + "," + i.ToString() + "-";
            listBox1.Items.Add(s);
        }

        bool IsStrike(int x1, int y1, int x2, int y2)
        {
            // 1. Горизонталь, вертикаль
            // Ферзи бьют друг друга, если соответствующие 2 параметра совпадают
            if ((x1 == x2) || (y1 == y2)) return true;

            // 2. Главная диагональ
            int tx, ty; // дополнительные переменные

            // 2.1. Влево-вверх
            tx = x1 - 1; ty = y1 - 1;
            while ((tx >= 1) && (ty >= 1))
            {
                if ((tx == x2) && (ty == y2)) return true;
                tx--; ty--;
            }

            // 2.2. Вправо-вниз
            tx = x1 + 1; ty = y1 + 1;
            while ((tx <= N) && (ty <= N))
            {
                if ((tx == x2) && (ty == y2)) return true;
                tx++; ty++;
            }

            // 3. Дополнительная диагональ
            // 3.1. Вправо-вверх
            tx = x1 + 1; ty = y1 - 1;
            while ((tx <= N) && (ty >= 1))
            {
                if ((tx == x2) && (ty == y2)) return true;
                tx++; ty--;
            }

            // 3.2. Влево-вниз
            tx = x1 - 1; ty = y1 + 1;
            while ((tx >= 1) && (ty <= N))
            {
                if ((tx == x2) && (ty == y2)) return true;
                tx--; ty++;
            }
            return false;
        }
        // Проверка, накладывается ли последний элемент M[p]
        // с элементами M[1], M[2], ..., M[p-1].
        // Данная функция использует функцию IsStrike()
        bool Strike(int[] M, int p)
        {
            int px, py, x, y;
            int i;
            px = M[p];
            py = p;

            for (i = 1; i <= p - 1; i++)
            {
                x = M[i];
                y = i;
                if (IsStrike(x, y, px, py))
                    return true;
            }
            return false;
        }
        // метод, отображающий в dataGridView1 строку s,
        // N - количество ферзей
        // принимаем, что dataGridView1 уже инициализирован в размер N*N
        void ShowDataGridView(string s, int N)
        {
            int i;
            int j;
            string xs, ys;
            int x, y;

            // сначала очистить dataGridView1
            for (i = 0; i < N; i++)
                for (j = 0; j < N; j++)
                    dataGridView1.Rows[i].Cells[j].Value = "";

            j = 0; // смещение
            for (i = 0; i < N; i++)
            {
                // сформировать xs
                xs = "";
                while (s[j] != ',')
                {
                    xs = xs + s[j].ToString();
                    j++;
                } // xs - число x в виде строки

                // прокрутить смещение
                j++;

                // сформировать ys
                ys = "";
                while (s[j] != '-')
                {
                    ys = ys + s[j].ToString();
                    j++;
                }

                // прокрутить смещение
                j++;

                // перевести xs->x, ys->y
                x = Convert.ToInt32(xs);
                y = Convert.ToInt32(ys);

                // обозначить позицию (x, y) ферзя
                dataGridView1.Rows[y - 1].Cells[x - 1].Value = "X";
            }
        }
        // начальная инициализация dataGridView1
        // N - количество ферзей
        void InitDataGridView(int N)
        {
            int i;
            dataGridView1.Columns.Clear();

            // сформировать dataGridView1 - добавить столбцы
            for (i = 1; i <= N; i++)
            {
                dataGridView1.Columns.Add("i" + i.ToString(), i.ToString());

                // ширина столбца в пикселах
                dataGridView1.Columns[i - 1].Width = 30;
            }

            // добавить строки
            dataGridView1.Rows.Add(N);

            // установить номер в каждой строке
            for (i = 1; i <= N; i++)
                dataGridView1.Rows[i - 1].HeaderCell.Value = i.ToString();

            // забирает последнюю строку, чтобы нельзя было добавлять строки в режиме выполнения
            dataGridView1.AllowUserToAddRows = false;

            // выравнивание по центру во всех строках
            dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            


        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            if (listBox1.Items.Count <= 0) return;

            int num;
            string s;
            num = listBox1.SelectedIndex;
            s = listBox1.Items[num].ToString();
            ShowDataGridView(s, N);
        }

        private void listBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (listBox1.Items.Count <= 0) return;

            int num;
            string s;
            num = listBox1.SelectedIndex;
            s = listBox1.Items[num].ToString();
            ShowDataGridView(s, N);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            const int MaxN = 20;
            int[] M = new int[MaxN]; // массив, формирующий циклические вложения
            int p; // номер размещаемого ферзя
            int k; // количество вариантов размещений

            // взять число ферзей
            N = Convert.ToInt32(textBox1.Text);

            // Инициализировать dataGridView1
            InitDataGridView(N);

            // очистить listBox1
            listBox1.Items.Clear();

            // АЛГОРИТМ ФОРМИРОВАНИЯ РАЗМЕЩЕНИЯ
            // начальные настройки
            p = 1;
            M[p] = 0;
            M[0] = 0;
            k = 0;

            // цикл поиска размещений
            while (p > 0) // если p==0, то выход из цикла
            {
                M[p] = M[p] + 1;
                if (p == N) // последний элемент
                {
                    if (M[p] > N)
                    {
                        while (M[p] > N) p--; // перемотка назад
                    }
                    else
                    {
                        if (!Strike(M, p))
                        {
                            // зафиксировать размещение
                            AddToListBox(M, N);
                            k++;
                            p--;
                        }
                    }
                }
                else // не последний элемент
                {
                    if (M[p] > N)
                    {
                        while (M[p] > N) p--; // перемотать обратно
                    }
                    else
                    {
                        if (!Strike(M, p)) // Если M[p] не накладывается с предыдущими M[1],M[2], ..., M[p-1]
                        {
                            p++; // перейти на размещение следующего ферзя
                            M[p] = 0;
                        }
                    }
                }
            }

            // вывести количество вариантов размещения
            if (k > 0)
            {
                listBox1.SelectedIndex = 0;
                listBox1_SelectedIndexChanged(sender, e);
                label2.Text = "Number of placements = " + k.ToString();
            }
        }

    }
}

