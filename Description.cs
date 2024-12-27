using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CIPHER_WF
{
    public partial class Description : Form
    {
        public Description()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Вы вводите текст и он переворачивается задом наперёд. Кажется крайне простым, однако, используется во многих случаях, в которых вообще требуются шифры");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Шифр Цезаря: берется ваш текст и каждая буква в нем заменяется на символ, что стоит на n шага впереди в заданном алфавите.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Виженер: Есть ваш текст, алфавит и ключевое слово. Каждая буква вашего текста двигается на то же значение вперед, на какое эта же по порядку буква в ключе по счету в алфавите");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Алфавит: вам нужно ввести алфавит. На этом алфавите будет основываться ваше сообщение и ключ, если требуется. В программе предусмотренны короткие пути: введите eng для английского алфавита или русский - для русского");
            MessageBox.Show("Текст: Тут нужно ввести текст, который будет шифроваться. Все символы в нем должны быть символами алфавита, на котором основывается ваш текст. То же касается и ключа");
            MessageBox.Show("Результат: Тут будет выводиться зашифрованный текст");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Алфавит: вам нужно ввести алфавит. На этом алфавите будет основываться ваше сообщение и ключ, если требуется. В программе предусмотренны короткие пути: введите eng для английского алфавита или русский - для русского");
            MessageBox.Show("Текст: Тут нужно ввести текст, который будет дешифроваться. Все символы в нем должны быть символами алфавита, на котором основывается ваш шифр. То же касается и ключа (Ключ должен быть тот же, что и при шифре!)");
            MessageBox.Show("Результат: Тут будет выводиться расшифрованный текст");
        }

        private void Description_Load(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Умножает каждый элемент текстового ключа на значение, указанное в поле для чисел");
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Использует продвинутые алгоритмы шифрования. Работает, прежде всего, с текстовыми документами.\n  - Чтобы зашифровать: \nImport public key  =>  Encript file\n  -  - Чтобы расшифровать:\nGet private key  =>  Decript file");
            MessageBox.Show("Также:\n Export public key - Сохраняет public key. Конечно же, только первую часть с которой можно зашифровать, но не расшифровать\nCreate asm key - создание ассиметричного ключа, что в свою очередь шифрует Aes ключ");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Заменяет символы на те, что стоят также далеко от конца алфавита, насколько текущий символ далеко от начала алфавита");

        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Заменяет символы на значения числа, по счету которым стоит в алфавите сама буква");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Есть двумерный массив - то есть квадрат. Он заполняется буквами алфавита и теперь каждая буква в зашифрованном сообщении\n будет парой чисел, которая представляет собой координаты нахождения буквы в этом квадрате");
        }
    }
}
