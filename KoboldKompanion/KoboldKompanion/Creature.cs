using KoboldKompanion.Properties;
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
    internal class Creature
    {
        static private Random rand = new Random(); //random for picking shit

        public Image currentImage = Resources.Base; //the current image to render
        List<Image> currentImages = new List<Image>();
        int imageAnimTrack = 0;

        bool imageFlipped = false;
        int direction; //left is -1, 1 is right

        private Point currentTarget; //THIS IS A TEST POINT, PLEASE REMOVE

        private Timer tmrAction = new Timer(); //random action tracker
        private Timer tmrWait = new Timer();
        private Timer tmrNewImage = new Timer(); //cycle image
        public Timer tmrTimeOut = new Timer();
        // private bool waitFlag = false; //DEPRECATED 
        public bool interactionFlag = true; //has the user interacted with the creature?



        private int fallSpeed = 0;
        public int _fallSpeed { get { return fallSpeed; } }
        private int fallRate = 2;

        public bool isFalling; //should we enter the falling animation?

        public ActionState currentAction { get; set; }


        //current path for images? 

        public enum ActionState 
        {  //Action that the creature is currently preforming
            Wander,
            Rest,
            Sleep,
        }

        public Creature()
        {

            tmrAction.Interval = 60000;//good question, how long should this be? (we will start at 1min?)
            tmrAction.Tick += TmrAction_Tick;
            tmrAction.Start();

            tmrWait.Interval = 20000; //20 seconds
            tmrWait.Tick += TmrWait_Tick;
            //tmrWait.Start();

            tmrNewImage.Interval = 100; //1/10 second
            tmrNewImage.Tick += TmrNewImage_Tick;
            tmrNewImage.Start();

            tmrTimeOut.Interval = 300000; //300000 ~about 5 minutes will be the timeout
            tmrTimeOut.Tick += TmrTimeOut_Tick;
            tmrTimeOut.Start();

            isFalling = false;

            currentAction = ActionState.Rest;
            //testing actions
        }

        private void TmrTimeOut_Tick(object sender, EventArgs e)
        {
            //set action to sleep
            currentAction= ActionState.Sleep;
            Sleep();
            tmrTimeOut.Stop();
        }

        private void TmrNewImage_Tick(object sender, EventArgs e)
        {
            if(currentImages.Count < 1)
                return;
            if(imageAnimTrack >= currentImages.Count - 1)
            {
                imageAnimTrack = 0;
            }
            else
            {
                imageAnimTrack++;
            }

            currentImage = currentImages[imageAnimTrack];

            //make sure immages are flipped
            if (direction > 0 && !imageFlipped)
            {
                currentImages.ForEach(x => x.RotateFlip(RotateFlipType.RotateNoneFlipX));
                imageFlipped = true;
                Trace.WriteLine($"Image Flipped");
            }
            else if (direction < 0 && imageFlipped)
            {
                currentImages.ForEach(x => x.RotateFlip(RotateFlipType.RotateNoneFlipX));
                imageFlipped = false;
                Trace.WriteLine($"Image UnFlipped");
            }
        }

        private void TmrWait_Tick(object sender, EventArgs e)
        {
           //waitFlag = true; Removed, for better AI
        }

        private void TmrAction_Tick(object sender, EventArgs e)
        {
            //Pick a new action for the creature to do. 
            /*
             * WANDER: the creature will pick a direction on screen and wander to its location.
             * CLIMB: the creature will climb the edge of a program, to rest atop it. (this is more to counter the falling animation
             * SLEEP: The creature will find a location on one of the corners of the screen, and fall asleep
             * REST: The creature will sit on the taskbar/window and rest
             * more actions will likely follow
             */
             currentAction = (ActionState)rand.Next(0,2); //will be more
            //currentAction = ActionState.Rest;

            //wander around, do a silly
            if (currentAction == ActionState.Wander) 
            {
                Wander();
            }
            else if(currentAction == ActionState.Rest)
            {
                Sit();
            }
            else if(currentAction == ActionState.Sleep)
            {
                Sleep();
            }
            else
            {
                SetIdleAnim(currentImages);
            }

            Trace.WriteLine(currentAction.ToString());

        }

        /// <summary>
        /// Helper method to handle falling
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="Size"></param>
        /// <returns></returns>
        public Point Fall(Point Location, Size Size, double additionalFallspeed = 0)
        {
            if(Screen.GetWorkingArea(Location).Bottom - 20 > Location.Y + Size.Height)
            {
                isFalling = true;
                fallSpeed += fallRate ;
                Location.Y += fallSpeed + additionalFallspeed > 60 ? 60 : fallSpeed; //arb pick. allowing for gravitys
            }
            else
            {
                isFalling = false;
                fallSpeed = 0;

                currentAction = ActionState.Wander; //falling will rouse the creature from its state
                Wander();
            }

            return Location;
        }

        /// <summary>
        /// helper method to move the creature
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="Size"></param>
        public Point Move(Point Location, Size Size)
        {
            if(Location.X >= currentTarget.X + 10
                || Location.X <= currentTarget.X - 10)
            {
                direction = currentTarget.X.CompareTo(Location.X);

                Location.X += direction * 5; //compare to returns -1, 0 or 1, meaning if I multiply the result, thats the direction itll go

            }
            else
            {
                Location.X = currentTarget.X; //rest for a bit, then wander

                currentImage = Resources.Base; //reset anim just in case

                if(Location.X + Size.Width > Screen.GetWorkingArea(Location).Right 
                    || Location.X - Size.Width < Screen.GetWorkingArea(Location).Left)
                {
                    Wander();
                }
                else
                {
                    currentAction = ActionState.Rest;
                    Sit();
                }

                
                /*if(waitFlag)
                {
                    Wander(); //wander
                    waitFlag = false;
                }*/ //Removed for potentially better AI
                    
            }

            return Location;

        }

        /// <summary>
        /// Will select a new point to wander to at selection of action
        /// </summary>
        public void Wander()
        {
            tmrTimeOut.Start();
            tmrAction.Interval = 60000;
            tmrNewImage.Interval = 100;
            SetWalkAnim(currentImages);

            var screens = Screen.AllScreens;
            Screen p = screens[rand.Next(0, screens.Count())];

            currentTarget = new Point(rand.Next(p.Bounds.Left, p.Bounds.Right), rand.Next(p.Bounds.Top, p.Bounds.Bottom));
            //yes I am picking Y values, I dont know what to do with them yet. 
        }

        public void Sit()
        {
            tmrAction.Interval = 60000;
            tmrNewImage.Interval = 2000;
            SetSitAnim(currentImages);
        }

        public void Sleep()
        {
            tmrNewImage.Interval = 1000; //3 seconds for each breath
            tmrAction.Interval = (int)1.8e6; //should be about 30mins?

            SetSleepAnim(currentImages);

        }

        public void SetWalkAnim(List<Image> images)
        {
            images.Clear();
            //fill images with walking animation
            images.Add(Resources.Base);
            images.Add(Resources.walk1);
            images.Add(Resources.walk2);

            if(imageFlipped)
            {
                images.ForEach(x => x.RotateFlip(RotateFlipType.RotateNoneFlipX));
            }
        }

        public void SetIdleAnim(List<Image> images)
        {
            images.Clear();
            images.Add(Resources.Base);

            if (imageFlipped)
            {
                images.ForEach(x => x.RotateFlip(RotateFlipType.RotateNoneFlipX));
            }
        }

        public void SetSitAnim(List<Image> images)
        {
            images.Clear();
            images.Add(Resources.Sit);
            images.Add(Resources.Sit);
            images.Add(Resources.Sit2);
            images.Add(Resources.Sit);
            images.Add(Resources.Sit);

            if (imageFlipped)
            {
                images.ForEach(x => x.RotateFlip(RotateFlipType.RotateNoneFlipX));
            }

        }

        public void SetSleepAnim(List<Image> images)
        {
            images.Clear();
            images.Add(Resources.sleep1);
            images.Add(Resources.sleep2);
            images.Add(Resources.sleep3);

            if (imageFlipped)
            {
                images.ForEach(x => x.RotateFlip(RotateFlipType.RotateNoneFlipX));
            }

        }


    }
}
