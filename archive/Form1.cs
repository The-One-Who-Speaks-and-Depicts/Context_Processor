/*﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WordProcessorTrial
{
    public partial class Form1 : Form
    {
        String defaultText = "Новый анализ данных";
        String newFileName;
        Boolean contextInserted = false;
        public Form1()
        {
            InitializeComponent();
           
        }
        
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.Text = defaultText;
            
            label1.Text = "Лексема";
            label2.Text = "Количество контекстов";
            label3.Text = "Семантика";
            label4.Text = "Основание для анализа";
            label5.Text = "Анализ";
            label6.Text = "Итог";
            label7.Text = "Контекст";
            label8.Text = "Источник контекста";

            button1.Text = "Внести";
            button2.Text = "Внести";
            button3.Text = "Внести";
            button4.Text = "Внести";
            button5.Text = "Внести";
            button6.Text = "Внести в файл";
            button7.Text = "Завершить ввод контекстов";
            button8.Text = "Внести";
            button9.Text = "Внести контекст";

            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;

            richTextBox5.ReadOnly = true;

            toolStripMenuItem1.Text = "Дополнительные буквы и лигатуры";

            // подпункты для выбора кириллических символов
            toolStripMenuItem2.Text = "sѣлѡ";
            toolStripMenuItem3.Text = "Ꙉерьвь";
            toolStripMenuItem4.Text = "ѿ(лиг.)";
            toolStripMenuItem5.Text = "ҁоппа";
            toolStripMenuItem6.Text = "ꙗть";
            toolStripMenuItem7.Text = "їотированъ аꙁъ";
            toolStripMenuItem8.Text = "їотированъ ѥсть";
            toolStripMenuItem9.Text = "ѭсъ малъ";
            toolStripMenuItem10.Text = "ѭсъ вєликъ";
            toolStripMenuItem11.Text = "їотированъ ѭсъ малъ";
            toolStripMenuItem12.Text = "їотированъ ѭсъ вєликъ";
            toolStripMenuItem13.Text = "кси";
            toolStripMenuItem14.Text = "пси";
            toolStripMenuItem15.Text = "фита";
            toolStripMenuItem16.Text = "ижица";

            //подподпункты для выбора кириллических символов
            //ижица
            toolStripMenuItem17.Text = "Ѵ";
            toolStripMenuItem18.Text = "ѵ";
            //фита
            toolStripMenuItem19.Text = "Ѳ";
            toolStripMenuItem20.Text = "ѳ";
            //пси
            toolStripMenuItem21.Text = "Ѱ";
            toolStripMenuItem22.Text = "ѱ";
            //кси
            toolStripMenuItem23.Text = "Ѭ"; 
            toolStripMenuItem24.Text = "ѭ"; 
            //йотированный юс большой
            toolStripMenuItem25.Text = "Ѯ"; 
            toolStripMenuItem26.Text = "ѯ"; 
            //йотированный юс малый
            toolStripMenuItem27.Text = "Ѩ";
            toolStripMenuItem28.Text = "ѩ";
            //юс большой
            toolStripMenuItem29.Text = "Ѫ";
            toolStripMenuItem30.Text = "ѫ";
            //юс малый
            toolStripMenuItem31.Text = "Ѧ";
            toolStripMenuItem32.Text = "ѧ";
            //йотированный есть
            toolStripMenuItem33.Text = "Ѥ";
            toolStripMenuItem34.Text = "ѥ";
            //йотированный аз
            toolStripMenuItem35.Text = "Ꙗ";
            toolStripMenuItem36.Text = "ꙗ";
            //ять
            toolStripMenuItem37.Text = "Ѣ";
            toolStripMenuItem38.Text = "ѣ";
            //cоппа
            toolStripMenuItem39.Text = "Ҁ";
            toolStripMenuItem40.Text = "ҁ";
            //лигатура от
            toolStripMenuItem41.Text = "Ѿ";
            toolStripMenuItem42.Text = "ѿ";
            //гервь
            toolStripMenuItem43.Text = "Ꙉ";
            toolStripMenuItem44.Text = "Ꙉ";
            //зело
            toolStripMenuItem45.Text = "Ѕ";
            toolStripMenuItem46.Text = "ѕ";
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            richTextBox1.ReadOnly = true;
            String transferrable = "<b><i>" + label1.Text + "</i></b>: "; 
            transferrable += "<i>" + richTextBox1.Text + "</i>";
            transferrable += ".<br>\n";
            richTextBox5.Text += transferrable;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            String transferrable = "<b><i>" + label2.Text + "</i></b>: ";
            richTextBox5.Text += transferrable;
            richTextBox5.Text += numericUpDown1.Value;
            richTextBox5.Text += ".<br>\n";
            button3.Enabled = true;
        }
        

     

        private void button3_Click_1(object sender, EventArgs e)
        {
            button3.Enabled = false;
            richTextBox2.ReadOnly = true;
            String transferrable = "<b><i>" + label3.Text + "</i></b>: ";
            transferrable += richTextBox2.Text;
            transferrable += ".<br>\n";
            richTextBox5.Text += transferrable;
            button8.Enabled = true;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            button4.Enabled = false;
            richTextBox3.ReadOnly = true;
            String transferrable = "<b><i>" + label4.Text + "</i></b>: ";
            transferrable += richTextBox3.Text;
            transferrable += ".<br>\n";
            richTextBox5.Text += transferrable;
            button5.Enabled = true;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {

            button5.Enabled = false;
            richTextBox4.ReadOnly = true;
            String transferrable = "<b><i>" + label5.Text + "</i></b>: ";
            transferrable += richTextBox4.Text;
            transferrable += ".<br>\n";
            richTextBox5.Text += transferrable;
            button6.Enabled = true;
        }
       

        private void button7_Click(object sender, EventArgs e)
        {
            button8.Enabled = false;
            button7.Enabled = false;
            button9.Enabled = false;
            contextInserted = false;
            richTextBox6.ReadOnly = true;
            richTextBox7.ReadOnly = true;
            button4.Enabled = true;
            richTextBox5.Text += "</ol><br>\n";
        }

        
        
       private string fn = String.Empty;

       private void SaveDocument()
        {
            if ((fn == string.Empty) || (this.Text == newFileName)) 
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fn = saveFileDialog1.FileName;
                    this.Text = fn;
                    newFileName = this.Text;
                }
            }
             
        }
        
       

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.Text == defaultText)
            {
                this.SaveDocument();
                String fn1 = Path.GetFullPath(saveFileDialog1.FileName);
                System.IO.File.AppendAllText(fn1, richTextBox5.Text);
            }
            else
            {
                string message = "Файл уже существует. Продолжить?"; 
                string caption = "Предупреждение";                 
                MessageBoxButtons buttons = MessageBoxButtons.YesNo; 
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    String fn1 = Path.GetFullPath(saveFileDialog1.FileName);
                    System.IO.File.AppendAllText(fn1, richTextBox5.Text);
                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    this.SaveDocument();
                    String fn1 = Path.GetFullPath(saveFileDialog1.FileName);
                    System.IO.File.AppendAllText(fn1, richTextBox5.Text);
                }
            }
            
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox3.Clear();
            richTextBox4.Clear();
            richTextBox5.Clear();
            richTextBox6.Clear();
            richTextBox7.Clear();
            numericUpDown1.Value = 0;
            button6.Enabled = false;
            button1.Enabled = true;
            richTextBox1.ReadOnly = false;
            richTextBox2.ReadOnly = false;
            richTextBox3.ReadOnly = false;
            richTextBox4.ReadOnly = false;
            richTextBox6.ReadOnly = false;
            richTextBox7.ReadOnly = false;
            numericUpDown1.ReadOnly = false;
        }

        

        private void button8_Click(object sender, EventArgs e)
        {
            button8.Enabled = false;
            richTextBox7.ReadOnly = true;
            button9.Enabled = true;

        }
        // вставка зело большого
        private void ToolStripMenuItem45_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false) {
                richTextBox1.Text += "Ѕ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ѕ";
            }
        }
        // вставка зело маленького
        private void ToolStripMenuItem46_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "ѕ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "ѕ";
            }
        }
        // вставка большой герви
        private void ToolStripMenuItem43_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "Ꙉ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ꙉ";
            }
        }
        // вставка маленькой герви
        private void ToolStripMenuItem44_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "Ꙉ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ꙉ";
            }
        }
        // вставка большого ота
        private void ToolStripMenuItem41_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "Ѿ ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ѿ";
            }
        }
        // вставка маленького ота
        private void ToolStripMenuItem42_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "ѿ"; 
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "ѿ";
            }
        }
        // вставка большой соппы
        private void ToolStripMenuItem39_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "Ҁ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ҁ";
            }
        }
        // вставка маленькой соппы
        private void ToolStripMenuItem40_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "ҁ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "ҁ";
            }
        }
        // вставка большого ятя
        private void ToolStripMenuItem37_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "Ѣ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ѣ";
            }
        }
        // вставка маленького ятя
        private void ToolStripMenuItem38_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "ѣ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "ѣ";
            }
        }
        // вставка большого йотированного аза
        private void ToolStripMenuItem35_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "Ꙗ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ꙗ";
            }
        }
        // вставка маленького йотированного аза
        private void ToolStripMenuItem36_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "ꙗ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "ꙗ";
            }
        }
        // вставка большой йотированной есть
        private void ToolStripMenuItem33_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "Ѥ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ѥ";
            }
        }
        // вставка маленькой йотированной есть
        private void ToolStripMenuItem34_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "ѥ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "ѥ";
            }
        }
        // вставка  большого юса малого
        private void ToolStripMenuItem31_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "Ѧ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ѧ";
            }
        }
        // вставка маленького юса малого
        private void ToolStripMenuItem32_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "ѧ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "ѧ";
            }
        }
        // вставка большого юса большого
        private void ToolStripMenuItem29_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "Ѫ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ѫ";
            }
        }
        // вставка маленького юса большого
        private void ToolStripMenuItem30_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "ѫ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "ѫ";
            }
        }
        // вставка большого йотированного юса малого  
        private void ToolStripMenuItem27_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "Ѩ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ѩ";
            }
        }
        // вставка маленького йотированного юса малого
        private void ToolStripMenuItem28_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "ѩ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "ѩ";
            }
        }
        // вставка большого йотированного юса большого 	 
        private void ToolStripMenuItem23_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "Ѭ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ѭ";
            }
        }
        // вставка маленького йотированного юса большого
        private void ToolStripMenuItem24_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "ѭ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "ѭ";
            }
        }
        // вставка кси большой  
        private void ToolStripMenuItem25_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "Ѯ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ѯ";
            }
        }
        // вставка кси маленькой
        private void ToolStripMenuItem26_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "ѯ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "ѯ";
            }
        }
        // вставка пси большой
        private void ToolStripMenuItem21_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "Ѱ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ѱ";
            }
        }
        // вставка пси маленькой 	 
        private void ToolStripMenuItem22_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "ѱ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "ѱ";
            }
        }
        // вставка большой фиты  
        private void ToolStripMenuItem19_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "Ѳ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ѳ";
            }
        }
        // вставка маленькой фиты
        private void ToolStripMenuItem20_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "ѳ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "ѳ";
            }
        }
        // вставка большой ижицы  
        private void ToolStripMenuItem17_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "Ѵ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "Ѵ";
            }
        }
        // вставка маленькой ижицы
        private void ToolStripMenuItem18_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                richTextBox1.Text += "ѵ";
            }
            else if (richTextBox6.ReadOnly == false)
            {
                richTextBox6.Text += "ѵ";
            }
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            if (!contextInserted)
            {
                String transferrable = "<b><i>" + label7.Text + "</i></b>: ";
                transferrable += "<ol>";
                transferrable += "<li><i>" + richTextBox6.Text + "</i> ";
                transferrable += "[" + richTextBox7.Text + "]";
                transferrable += ".<br></li>\n";
                richTextBox5.Text += transferrable;
                contextInserted = true;
            }
            else
            {
                String transferrable = "<li><i>" + richTextBox6.Text + "</i> ";
                transferrable += "[" + richTextBox7.Text + "]";
                transferrable += ".<br></li>\n";
                richTextBox5.Text += transferrable;
            }
            string message = "Сохранить заданный источник?";
            string caption = "Сохранение источника";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show(message, caption, buttons);
            if (result == System.Windows.Forms.DialogResult.No)
            { 
                richTextBox7.Clear();                
                richTextBox7.ReadOnly = false;
                button8.Enabled = true;
                button9.Enabled = false;
            }
            richTextBox6.Clear();            
            button7.Enabled = true;
        }
    }
}*/
