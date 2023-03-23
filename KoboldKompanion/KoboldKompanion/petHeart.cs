using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KoboldKompanion
{
    public partial class petHeart : Form
    {
        Timer anim = new Timer();
        static Random rand = new Random();
        double osc = 0;
        Point loca;

        public petHeart(Point Location)
        {
            InitializeComponent();

            //save location to spawn at
            loca = Location;
            //set up animations
            anim.Interval = 20;
            anim.Tick += Anim_Tick;
            anim.Start();
            Size = new Size(32, 32);

        }

        private void Anim_Tick(object sender, EventArgs e)
        {
            osc += 0.1;
            //make sure to oscillate! 
            Location = new Point((int)(Location.X + Math.Sin(osc)*3), Location.Y - 5);

            if(Location.Y + Size.Height < 0)
            {
                //close as soon as it goes off screen
                Close();
            }
        }

        private void petHeart_Load(object sender, EventArgs e)
        {
            //set location
            Location = loca;
        }
    }
}
