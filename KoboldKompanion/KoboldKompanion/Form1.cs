using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace KoboldKompanion
{
    public partial class CharacterBack : Form
    {
        //handle mouse drag
        private bool mouseDown = false;
        private Point lastLocation;
        private bool isDrag = false;
        
        Timer tmrAnimation = new Timer();

        Creature creature;

        //momentum tries: 
        const int maxVel = 100;
        const int minVel = -100;
        double velY = 0;
        double velX = 0;
        Point cursorPos;
        Point prevCursorPos;

        //heart attempt
        petHeart heart;

        public CharacterBack()
        { 
            InitializeComponent();


            MouseDown += CharacterBack_MouseDown; //when the mouse clicks
            MouseMove += CharacterBack_MouseMove;
            MouseUp += CharacterBack_MouseUp;

        }

        private void CharacterBack_Load(object sender, EventArgs e)
        {
            creature = new Creature();

            tmrAnimation.Enabled = true;
            tmrAnimation.Interval = 20;
            tmrAnimation.Start();
            tmrAnimation.Tick += TmrAnimation_Tick;
            imgCharacter.Image = creature.currentImage;
        }

        private void TmrAnimation_Tick(object sender, EventArgs e)
        {
            //drive the animation, picking each sprite depending on the action
            //for now, it does nothing
            //Trace.WriteLine($"Creature Location: ({Location.X},{Location.Y})");
            imgCharacter.Image = creature.currentImage;

            if(creature.interactionFlag)
            {
                creature.tmrTimeOut.Stop();
                creature.tmrTimeOut.Start();
                creature.interactionFlag = false; //reset the timer
            }

            if (!mouseDown)
            {
                //handle falling
                if (Location.Y + Size.Height < Screen.GetWorkingArea(Location).Bottom - 20) //Probs gonna need to error check this more
                {
                    Location = creature.Fall(this.Location, this.Size);
                    if (velX != 0 ||
                        velY != 0)
                    {
                        Location = new Point((int)(Location.X + velX), (int)(Location.Y + velY));
                    }
                    
                }
                else if (creature.isFalling)
                {
                    creature.Fall(this.Location, this.Size); //make sure to finish up the fall animation
                    //the creature has hit the ground, remove all velocity
                    velY = 0;
                    velX = 0.0;
                }

                //Handle wandering
                if (!creature.isFalling)
                {
                    Location = new Point(Location.X, Screen.GetWorkingArea(Location).Bottom - Size.Height); //fix position if broke

                    if (creature.currentAction == Creature.ActionState.Wander)
                    {
                        Location = creature.Move(Location, Size); //make sure to pass in the location and size of the form
                    }

                    if(Location.X > Screen.GetWorkingArea(Location).Right + Size.Width * 2) //if creature is off screen, bring him back
                    {
                        Location = new Point(Screen.GetWorkingArea(Location).Right + Size.Width, Location.Y); 
                    }
                    else if (Location.X < Screen.GetWorkingArea(Location).Left - Size.Width * 2) 
                    {
                        Location = new Point(Screen.GetWorkingArea(Location).Left - Size.Width, Location.Y);
                    }
                }

                //handle resting??
                if(creature.currentAction == Creature.ActionState.Rest && !creature.isFalling)
                {
                   //resting code
                }
            }


            //velocity decay, will happen as long as velocity is not equal to zero
            velX += (0.0).CompareTo(velX) * 0.5;
            velY += (0.0).CompareTo(velY) * 0.5;


        }

        private void CharacterBack_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown= false;
            isDrag = false;
            //reset position if wrong
            //need to add different rules if resting
            if (Location.Y + Size.Height > Screen.GetWorkingArea(Location).Bottom + 50)
            {
                Location = new Point(Location.X, Screen.GetWorkingArea(Location).Bottom - Size.Height);
            }
        }

        private void CharacterBack_MouseMove(object sender, MouseEventArgs e)
        {
            cursorPos = Cursor.Position;

            if (mouseDown && cursorPos != prevCursorPos) //dont do anything unless we are dragging
            {
                this.Location = new Point((this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                Update();

                //velocity calcs? v=d/t
                //accumulate velocity if the difference is significant
                velX = (cursorPos.X - prevCursorPos.X) / 0.5; //50ns per loop, scaled up 
                velY = (cursorPos.Y - prevCursorPos.Y) / 0.5;

                prevCursorPos = cursorPos;
                isDrag = true;
            }
        }

        private void CharacterBack_MouseDown(object sender, MouseEventArgs e)
        {
            //start to drag
            mouseDown = true;
            lastLocation = e.Location;
            cursorPos = Cursor.Position;
            prevCursorPos= cursorPos;
            isDrag = false;
            creature.interactionFlag = true;
        }

        private void imgCharacter_MouseClick(object sender, MouseEventArgs e)
        {
            Trace.WriteLine(isDrag);
            //pet! will summon a heart emoji
            if (isDrag)
                return;
            heart = new petHeart(Location);

            heart.Show();
           
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //probably play a little animation, then leave

            this.Close();
        }

        
    }
}
