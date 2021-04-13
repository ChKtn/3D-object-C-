using System;
using System.Drawing;
using System.Windows.Forms;


namespace KursachGraf
{
    public partial class Form1 : Form
    {
        Object obj;
        int typeDraw=0;
        Point Start, End;
        double arg = 0;
        public void Draw(int type, double arg)
        {
            type = 1;
            pic.Image = obj.DrawingTor(pic.Width, pic.Height, type, arg);
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            obj = new Object();
            Draw(typeDraw, 0);
        }
        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            Start = e.Location;
        }
        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            End = e.Location;
            arg = ((End.X - Start.X) / 12)%180;
            arg = 10;
            Draw(typeDraw, arg);

        }
    }
}
