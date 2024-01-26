using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Amazing_Ninja
{
    class introoo
    {
        public int X, Y;
        public Bitmap img;
    }
    class NINJA
    {
        public int X, Y;
        public int Flag_State;
        public List<Bitmap> Imgs_Run = new List<Bitmap>();
        public int icurr_Run; // 1 for run
        public List<Bitmap> Imgs_Hit = new List<Bitmap>();
        public int icurr_Hit; // 2 for hit
        public List<Bitmap> Imgs_Jump = new List<Bitmap>();
        public int icurr_Jumb; // 3 for jump
        public List<Bitmap> Imgs_Land = new List<Bitmap>();
        public int icurr_Land; // 4 for landing
        public List<Bitmap> Imgs_Drawn = new List<Bitmap>();
        public int icurr_Drawn;// 5 for drawn
        public List<Bitmap> imgs_Death = new List<Bitmap>();
        public int icurr_Death; // 6 for death
    }

    class Slave
    {
        public int X, Y;
        public bool Alive;
        public List<Bitmap> Imgs_Run = new List<Bitmap>();
        public int icurr_Run;
    }
    class Enemy
    {
        public int X, Y;
        public bool Alive;
        public List<Bitmap> Imgs_pose = new List<Bitmap>();
        public int icurr_pose; // 1 for pose
        public List<Bitmap> Imgs_Kill = new List<Bitmap>();
        public int icurr_Hit; // 2 for hit
        public List<Bitmap> Imgs_Death = new List<Bitmap>();
        public int icurr_Death; // 3 for death 
        public List<Bitmap> Imgs_Kill_Revers = new List<Bitmap>();
        public int icurr_Rev_kill;
        public int Flag_State;
    }
    class block
    {
        public int X, Y, W, H;
        public Color clr;
    }
    public partial class Form1 : Form
    {
        Bitmap OFF; Bitmap imgg;  int Ct_Double_Press;
        Random RR = new Random();  int Color_Block; 
        int which_width; int Enemy_Or_Slave;
        int which_height;int Range_X;
        int Ct_Timer = 0; Color Cl; int Ct_Score = 0; int initiator = 1;
        bool Flag_Start_Game, GAME_START; int Block_Landed_On;
        int Flag_Up_Jump;
        Timer T = new Timer();
        List<block> Lblocks = new List<block>();
        block PnB;
        List<NINJA> LNinja = new List<NINJA> ();
        NINJA PnN;
        List<Enemy> LEnemies = new List<Enemy>();
        Enemy PnE;
        List<Slave> LSlaves = new List<Slave>();
        Slave PnS;
        List<introoo> LIntro = new List<introoo> ();
        introoo Pnintro;
        bool GAME_OVER; int Catch_Game_Over_Block;
        int[] Width_Block = new int[] {200 ,300, 400, 500 };
        int[] Height_Block = new int[] { 200, 225, 250, 275 };
        string Score = ""; int Score_Adaptation = 43; // when doing the divide and see that it has more digits than the previos one 
        // therefore make it -= 33
        Color[] colors = new Color[]
                        {
                            Color.Green,
                            Color.Red, Color.Blue,Color.Yellow,
                            Color.YellowGreen,Color.Brown,
                            //Color.FromArgb(106,241,192,255),
                            //Color.FromArgb(200, 111, 125, 255),
                            //Color.Brown,
                            //Color.FromArgb(185, 105, 225, 255),
                            //Color.FromArgb(108, 194, 254, 255),
                            //Color.Red,
                        };

        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Amazing Ninja";
            Paint += Form1_Paint;
            Load += Form1_Load;
            T.Interval = 50;
            T.Tick += T_Tick;
            KeyDown += Form1_KeyDown;
            MouseDown += Form1_MouseDown;
            this.DoubleBuffered = true;

            //this.FormBorderStyle = FormBorderStyle.None;
            //this.ClientSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            T.Start();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X >= LIntro[1].X && e.X <= LIntro[1].X + LIntro[1].img.Width
                && e.Y>= LIntro[1].Y && e.Y <= LIntro[1].Y + LIntro[1].img.Height)
           {
                GAME_START = true;
           }
        }
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {

                case Keys.Space: // for jumb
                    if (GAME_START)
                    {
                        if (LNinja[0].Flag_State != 4)
                        {
                            Ct_Double_Press++;
                            if (Ct_Double_Press == 2 && LNinja[0].Flag_State == 3)
                            {
                                LNinja[0].icurr_Jumb = 0;
                                LNinja[0].Y -= 3;
                                Flag_Up_Jump = 0;
                                // once he lands there fore Ct_Double_Press will be zero
                            }
                            Flag_Start_Game = true;
                            LNinja[0].Flag_State = 3;
                        }
                    }
                    break;
                case Keys.Enter: // for killing
                    if (GAME_START)
                    {
                        Flag_Start_Game = true;
                        if (LNinja[0].Flag_State == 1) 
                        {
                            LNinja[0].Flag_State = 2;
                            Check_Killing_Enemy();
                        }
                    }
                    break;
                default:

                    break;
            }
        }

        void Check_Killing_Enemy()
        {
            for (int i = 0; i < LEnemies.Count; i++)
            {
                if (LEnemies[i].X + 146 - 70
                    <= LNinja[0].X + LNinja[0].Imgs_Run[LNinja[0].icurr_Run].Width
                    && LNinja[0].Y == LEnemies[i].Y)
                {
                    LEnemies[i].Flag_State = 3;
                }
            }
        }
        void Check_Enemy_Killing()
        {
            for (int i = 0; i < LEnemies.Count; i++)
            {
                if (LNinja[0].X + 20 >= LEnemies[i].X && LNinja[0].Y == LEnemies[i].Y)
                {
                    if (LNinja[0].X + 165 >= LEnemies[i].X + LEnemies[i].Imgs_pose[0].Width)
                    {
                        LEnemies[i].Flag_State = 4;
                        LEnemies[i].X = LNinja[0].X - 75;
                       // LEnemies[i].Flag_State = 2;
                        LNinja[0].Flag_State = 6;
                    }
                    else
                    {
                        LEnemies[i].Flag_State = 2; // hit
                        LNinja[0].Flag_State = 6;   // death 
                    }
                }
            }
        }
        private void T_Tick(object sender, EventArgs e)
        {
            if(!GAME_OVER)
            {
                this.Text = "" + this.ClientSize.Height;
                if (GAME_START)
                {
                    if (Flag_Start_Game)
                    {
                        Ct_Timer++;
                        for (int i = 0; i < Lblocks.Count; i++)
                        {
                            Lblocks[i].X -= 6;
                            if (Lblocks[i].X + Lblocks[i].W < 0)
                                Lblocks.RemoveAt(i);
                        }
                        if (Ct_Timer % 100 == 0 || Ct_Timer == 40) 
                        {
                            PnB = new block();
                            PnB.X = this.ClientSize.Width;
                            Color_Block = RR.Next(0, 6);
                            PnB.clr = colors[Color_Block];
                            which_width = RR.Next(0, 4);
                            PnB.W = Width_Block[which_width];
                            which_height = RR.Next(0, 4);
                            PnB.Y = this.ClientSize.Height - Height_Block[which_height];
                            if (Ct_Timer == 40) 
                                PnB.W = Width_Block[0];
                            if (which_width == 0)
                            {

                            }
                            else if (which_width == 1&&Ct_Timer!=40)
                            {
                                
                                PnE = new Enemy();
                                for (int i = 1; i <= 1; i++) // for static pose
                                {
                                    imgg = new Bitmap("EA" + i + ".png");
                                    Cl = imgg.GetPixel(0, 0);
                                    imgg.MakeTransparent(Cl);
                                    PnE.Imgs_pose.Add(imgg);
                                }
                                for (int i = 2; i <= 7; i++) // for hitting 
                                {
                                    imgg = new Bitmap("EA" + i + ".png");
                                    Cl = imgg.GetPixel(0, 0);
                                    imgg.MakeTransparent(Cl);
                                    PnE.Imgs_Kill.Add(imgg);
                                }
                                for (int i = 2; i <= 7; i++) // for reverse killing
                                {
                                    imgg = new Bitmap("EAR" + i + ".png");
                                    Cl = imgg.GetPixel(0, 0);
                                    imgg.MakeTransparent(Cl);
                                    PnE.Imgs_Kill_Revers.Add(imgg);
                                }
                                for (int i = 1; i <= 6; i++) // for death
                                {
                                    imgg = new Bitmap("ED" + i + ".png");
                                    Cl = imgg.GetPixel(0, 0);
                                    imgg.MakeTransparent(Cl);
                                    PnE.Imgs_Death.Add(imgg);
                                }
                                Range_X = RR.Next(PnB.X + 80, PnB.X + PnB.W - 100);
                                PnE.X = Range_X;
                                PnE.Flag_State = 1;
                                PnE.Y = PnB.Y - imgg.Height;
                                LEnemies.Add(PnE);
                            }
                            else if (which_width == 2 && Ct_Timer != 40) 
                            {
                                Enemy_Or_Slave = RR.Next(0, 2);
                                if (Enemy_Or_Slave == 0)
                                {
                                    PnS = new Slave();
                                    for (int i = 1; i <= 15; i++)
                                    {
                                        imgg = new Bitmap("slave" + i + ".png");
                                        Cl = imgg.GetPixel(0, 0);
                                        imgg.MakeTransparent(Cl);
                                        PnS.Imgs_Run.Add(imgg);
                                    }
                                    Range_X = RR.Next(PnB.X + 30, PnB.X + PnB.W - 130);
                                    PnS.X = Range_X;
                                    PnS.Y = PnB.Y - imgg.Height;
                                    LSlaves.Add(PnS);
                                }
                                else
                                {
                                    PnE = new Enemy();
                                    for (int i = 1; i <= 1; i++) // for static pose
                                    {
                                        imgg = new Bitmap("EA" + i + ".png");
                                        Cl = imgg.GetPixel(0, 0);
                                        imgg.MakeTransparent(Cl);
                                        PnE.Imgs_pose.Add(imgg);
                                    }
                                    for (int i = 2; i <= 7; i++) // for hitting 
                                    {
                                        imgg = new Bitmap("EA" + i + ".png");
                                        Cl = imgg.GetPixel(0, 0);
                                        imgg.MakeTransparent(Cl);
                                        PnE.Imgs_Kill.Add(imgg);
                                    }
                                    for (int i = 2; i <= 7; i++) // for reverse killing
                                    {
                                        imgg = new Bitmap("EAR" + i + ".png");
                                        Cl = imgg.GetPixel(0, 0);
                                        imgg.MakeTransparent(Cl);
                                        PnE.Imgs_Kill_Revers.Add(imgg);
                                    }
                                    for (int i = 1; i <= 6; i++) // for death
                                    {
                                        imgg = new Bitmap("ED" + i + ".png");
                                        Cl = imgg.GetPixel(0, 0);
                                        imgg.MakeTransparent(Cl);
                                        PnE.Imgs_Death.Add(imgg);
                                    }
                                    Range_X = RR.Next(PnB.X + 80, PnB.X + PnB.W - 100);
                                    PnE.X = Range_X;
                                    PnE.Flag_State = 1;
                                    PnE.Y = PnB.Y - imgg.Height;
                                    LEnemies.Add(PnE);
                                }
                            }
                            else if (which_width == 3 && Ct_Timer != 40) 
                            {
                                Enemy_Or_Slave = RR.Next(0, 2);
                                if (Enemy_Or_Slave == 0)
                                {
                                    PnS = new Slave();
                                    for (int i = 1; i <= 15; i++)
                                    {
                                        imgg = new Bitmap("slave" + i + ".png");
                                        Cl = imgg.GetPixel(0, 0);
                                        imgg.MakeTransparent(Cl);
                                        PnS.Imgs_Run.Add(imgg);
                                    }
                                    Range_X = RR.Next(PnB.X + 30, PnB.X + PnB.W - 130);
                                    PnS.X = Range_X;
                                    PnS.Y = PnB.Y - imgg.Height;
                                    LSlaves.Add(PnS);
                                }
                                else
                                {
                                    PnE = new Enemy();
                                    for (int i = 1; i <= 1; i++) // for static pose
                                    {
                                        imgg = new Bitmap("EA" + i + ".png");
                                        Cl = imgg.GetPixel(0, 0);
                                        imgg.MakeTransparent(Cl);
                                        PnE.Imgs_pose.Add(imgg);
                                    }
                                    for (int i = 2; i <= 7; i++) // for hitting 
                                    {
                                        imgg = new Bitmap("EA" + i + ".png");
                                        Cl = imgg.GetPixel(0, 0);
                                        imgg.MakeTransparent(Cl);
                                        PnE.Imgs_Kill.Add(imgg);
                                    }
                                    for (int i = 2; i <= 7; i++) // for reverse killing
                                    {
                                        imgg = new Bitmap("EAR" + i + ".png");
                                        Cl = imgg.GetPixel(0, 0);
                                        imgg.MakeTransparent(Cl);
                                        PnE.Imgs_Kill_Revers.Add(imgg);
                                    }
                                    for (int i = 1; i <= 6; i++) // for death
                                    {
                                        imgg = new Bitmap("ED" + i + ".png");
                                        Cl = imgg.GetPixel(0, 0);
                                        imgg.MakeTransparent(Cl);
                                        PnE.Imgs_Death.Add(imgg);
                                    }
                                    Range_X = RR.Next(PnB.X + 80, PnB.X + PnB.W - 100);
                                    PnE.X = Range_X;
                                    PnE.Flag_State = 1;
                                    PnE.Y = PnB.Y - imgg.Height;
                                    LEnemies.Add(PnE);
                                }
                            }
                            
                           
                            PnB.H = this.ClientSize.Height - PnB.Y;
                            Lblocks.Add(PnB);
                        }
                    }
                    if (LNinja[0].Flag_State == 1) // run
                    {
                        LNinja[0].icurr_Run++;
                        LNinja[0].icurr_Run = LNinja[0].icurr_Run % 15;
                    }
                    else if (LNinja[0].Flag_State == 2) // hit
                    {
                        LNinja[0].icurr_Hit++;
                        if (LNinja[0].icurr_Hit == 5)
                        {
                            LNinja[0].icurr_Hit = 0;
                            LNinja[0].Flag_State = 1;
                        }
                        LNinja[0].icurr_Run = LNinja[0].icurr_Run % 4;
                    }
                    else if (LNinja[0].Flag_State == 3) // jump
                    {
                        if (LNinja[0].icurr_Jumb == 0 && Flag_Up_Jump != 3)
                            Flag_Up_Jump = 1;
                        else if (LNinja[0].icurr_Jumb % 17 == 0 && Flag_Up_Jump != 3)
                            Flag_Up_Jump = 2;
                        else if (LNinja[0].icurr_Jumb % 20 == 0)
                            Flag_Up_Jump = 3;
                        if (Flag_Up_Jump == 1)
                        {
                            LNinja[0].Y -= 10;
                        }
                        else if (Flag_Up_Jump == 2)
                        {
                            LNinja[0].Y -= 2;
                        }
                        else if (Flag_Up_Jump == 3)
                        {
                            LNinja[0].Y += 8;
                            for (int i = 0; i < Lblocks.Count; i++)
                            {
                                if (LNinja[0].X + 64 >= Lblocks[i].X && LNinja[0].X < Lblocks[i].X + Lblocks[i].W
                                    && LNinja[0].Y + LNinja[0].Imgs_Jump[LNinja[0].icurr_Jumb].Height + 13
                                    >= Lblocks[i].Y)
                                {
                                    Flag_Up_Jump = 0;
                                    Block_Landed_On = i;
                                    LNinja[0].Flag_State = 4;
                                    break;
                                }
                            }
                        }                
                        LNinja[0].icurr_Jumb++;
                        LNinja[0].icurr_Jumb %= 21;
                    }
                    else if (LNinja[0].Flag_State == 4) // landing
                    {
                        LNinja[0].icurr_Land++;
                        if (LNinja[0].icurr_Land == 4)
                        {
                            LNinja[0].Flag_State = 1;
                            Flag_Up_Jump = 0;
                            LNinja[0].icurr_Jumb = 0;
                            LNinja[0].icurr_Land = 0;
                            LNinja[0].Y = Lblocks[Block_Landed_On].Y - LNinja[0].Imgs_Run[0].Height;
                            Ct_Double_Press = 0;
                        }

                    }
                    else if (LNinja[0].Flag_State == 5) // drawn
                    {
                        LNinja[0].icurr_Drawn++;
                        LNinja[0].icurr_Drawn %= 20;
                        LNinja[0].Y += 8;
                        for (int i = 0; i < Lblocks.Count; i++)
                        {
                            if (LNinja[0].Y >= Lblocks[i].Y && LNinja[0].X + 128 >= Lblocks[i].X
                                && LNinja[0].X < Lblocks[i].X + Lblocks[i].W)
                            {
                                Catch_Game_Over_Block = i;
                                GAME_OVER = true;
                                break;
                            }
                        }
                        if (LNinja[0].Y > this.ClientSize.Height)
                            GAME_OVER = true;
                        for (int i = 0; i < Lblocks.Count; i++)
                        {
                            if (LNinja[0].X + 64 >= Lblocks[i].X && LNinja[0].X + 64 < Lblocks[i].X + Lblocks[i].W
                                && LNinja[0].Y + LNinja[0].Imgs_Jump[LNinja[0].icurr_Jumb].Height + 14
                                >= Lblocks[i].Y)
                            {
                                Flag_Up_Jump = 0;
                                Block_Landed_On = i;
                                LNinja[0].Flag_State = 4;
                                //LNinja[0].Y = Lblocks[i].Y - 128; // this is for drowing bug
                                break;
                            }
                        }
                    }
                    else if (LNinja[0].Flag_State == 6) 
                    {
                        LNinja[0].icurr_Death++;
                        if (LNinja[0].icurr_Death == 4) 
                            GAME_OVER=true;
                    }
                    for (int i = 0; i < LEnemies.Count; i++)
                    {
                        if (LEnemies[i].Flag_State == 3) 
                        {
                            LEnemies[i].icurr_Death++;
                            LEnemies[i].X += 3;
                            if (LEnemies[i].icurr_Death == 6)
                            {
                                LEnemies.RemoveAt(i);
                                Ct_Score++;
                            }
                        }
                        else if (LEnemies[i].Flag_State == 2) 
                        {
                            LEnemies[i].icurr_Hit++;
                            if (LEnemies[i].icurr_Hit == 7) 
                                GAME_OVER =true;
                        }
                        else if (LEnemies[i].Flag_State==4)
                        {
                            LEnemies[i].icurr_Rev_kill++;
                            if (LEnemies[i].icurr_Rev_kill == 7)
                                GAME_OVER = true;
                        }
                    }
                    for(int i = 0;i<LEnemies.Count;i++)
                    {
                        LEnemies[i].X -= 6;
                    }
                    for (int i = 0; i < LSlaves.Count; i++)
                    {
                        LSlaves[i].icurr_Run++;
                        LSlaves[i].icurr_Run %= 15;
                        LSlaves[i].X -= 6;
                    }
                    for(int i = 0; i < LSlaves.Count;i++)
                    {
                        if (LSlaves[i].X + LSlaves[i].Imgs_Run[LSlaves[i].icurr_Run].Width < 0)
                            LSlaves.RemoveAt(i);
                    }
                    if (LNinja[0].Flag_State != 3 && LNinja[0].Flag_State != 4) // here is the gravity 
                    {
                        for (int i = 0; i < Lblocks.Count; i++)
                        {
                            if (LNinja[0].X + 62 > Lblocks[i].X) 
                            {
                                if (LNinja[0].X + 62 <= Lblocks[i].X + Lblocks[i].W)
                                {
                                   
                                }
                                else
                                {
                                    LNinja[0].Flag_State = 5; // drawn
                                    break;
                                }
                            } 
                        }
                    }
                    if (!GAME_OVER)
                    {
                        Check_Slave_Hit();
                        Check_Enemy_Killing();
                    }
                    if (LNinja[0].Flag_State == 1)
                    {
                        for (int i = 0; i < Lblocks.Count; i++) 
                        {
                            if (LNinja[0].X+30 >= Lblocks[i].X && LNinja[0].X < Lblocks[i].X + Lblocks[i].W)
                            {
                                LNinja[0].Y = Lblocks[i].Y - 128;
                            }
                        }
                    }
                }

                Invalidate();
            }
        }

        void Check_Slave_Hit()
        {
            for (int i = 0; i < LSlaves.Count; i++) 
            {
                if (LSlaves[i].X + LSlaves[i].Imgs_Run[LSlaves[i].icurr_Run].Width - 50 
                    <= LNinja[0].X + LNinja[0].Imgs_Run[LNinja[0].icurr_Run].Width
                    && LNinja[0].Y == LSlaves[i].Y) 
                {
                    if (LNinja[0].X + 70 > LSlaves[i].X + LSlaves[i].Imgs_Run[LSlaves[i].icurr_Run].Width) 
                    {

                    }
                    else
                    {
                        LNinja[0].Flag_State= 6;
                        break;
                    }
                }
            }
            for(int i = 0; i < Lblocks.Count;i++)
            { 
                if (LNinja[0].Y >= Lblocks[i].Y && LNinja[0].X + 128 >= Lblocks[i].X
                    && LNinja[0].X < Lblocks[i].X + Lblocks[i].W) 
                {
                    GAME_OVER = true;
                    break;
                }
            }
            if (LNinja[0].Y > this.ClientSize.Height)
                GAME_OVER = true;
        }
        void Create_Blocks()
        {
            for (int i = 1; i <= 4; i++) 
            {
                Pnintro = new introoo();
                imgg = new Bitmap("Intro" + i + ".jpg");
                
                Pnintro.img = imgg;
                if (i == 1) 
                {
                    Pnintro.X = 550 - 30;
                    Pnintro.Y = 100;
                }
                if (i == 2) 
                {
                    Pnintro.X = 650;
                    Pnintro.Y = 550;
                }
                if (i == 3) 
                {
                    Pnintro.X = LNinja[0].X+LNinja[0].Imgs_Run[0].Width+6;
                    Pnintro.Y = LNinja[0].Y ;
                }
                if (i == 4) 
                {
                    Pnintro.X = LNinja[0].X + 10;
                    Pnintro.Y = 200;
                }
                LIntro.Add(Pnintro);
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //DrawDub(e.Graphics);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OFF = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            CreateInitial();
            Create_Blocks();
           
        }
        void CreateInitial()
        {
            PnB = new block();
            PnB.clr = Color.Red;
            PnB.W = this.ClientSize.Width;
            PnB.Y = this.ClientSize.Height - 220;
            PnB.X = 0;
            PnB.H = 220;
            Lblocks.Add(PnB);
            // creation of the Ninja
            PnN = new NINJA();
            PnN.X = 150; PnN.Flag_State = 1;
            for (int i = -1; i <= 13; i++)  // run for loop
            {   
                imgg = new Bitmap(i+".jpg");
                Cl = imgg.GetPixel(0, 0);
                imgg.MakeTransparent(Cl);
                PnN.Imgs_Run.Add(imgg);
            }

            for (int i = 1; i <= 5; i++)   // Hit for loop
            {
                imgg = new Bitmap("Hit"+i + ".jpg");
                Cl = imgg.GetPixel(0, 0);
                imgg.MakeTransparent(Cl);
                PnN.Imgs_Hit.Add(imgg);
            }

            for (int i = 1; i <= 21; i++)  // jump for loop
            { 
                imgg = new Bitmap("J"+i + ".png");
                Cl = imgg.GetPixel(0, 0);
                imgg.MakeTransparent(Cl);
                PnN.Imgs_Jump.Add(imgg);
            }

            for (int i = 0; i < 4; i++)  // land for loop
            {
                imgg = new Bitmap("L" + i + ".png");
                Cl = imgg.GetPixel(0, 0);
                imgg.MakeTransparent(Cl);
                PnN.Imgs_Land.Add(imgg);
            }

            for (int i = 1; i < 21; i++) // drawn for loop
            {
                imgg = new Bitmap("J" + i + ".png");
                Cl = imgg.GetPixel(0, 0);
                imgg.MakeTransparent(Cl);
                PnN.Imgs_Drawn.Add(imgg);
            }

            for (int i = 1; i <= 4; i++)  // death for loop
            {
                imgg = new Bitmap("PD" + i + ".png");
                Cl = imgg.GetPixel(0, 0);
                imgg.MakeTransparent(Cl);
                PnN.imgs_Death.Add(imgg);
            }
            
            PnN.Y = Lblocks[0].Y - PnN.Imgs_Run[0].Height;
            LNinja.Add(PnN);

            // drawing of the slave for testing 
            //PnS = new Slave();
            //for (int i = 1; i <= 15; i++)
            //{
            //    imgg = new Bitmap("slave" + i + ".png");
            //    Cl = imgg.GetPixel(0, 0);
            //    imgg.MakeTransparent(Cl);
            //    PnS.Imgs_Run.Add(imgg);
            //}
            //PnS.X = 1000;
            //PnS.Y = Lblocks[0].Y - PnN.Imgs_Run[0].Height;
            //LSlaves.Add(PnS);

            // drawing of the enemy for testing 
            /*
            PnE = new Enemy();
            for (int i = 1; i <= 1; i++) // for static pose
            {
                imgg = new Bitmap("EA" + i + ".png");
                Cl = imgg.GetPixel(0, 0);
                imgg.MakeTransparent(Cl);
                PnE.Imgs_pose.Add(imgg);
            }
            for (int i = 2; i <= 7; i++) // for hitting 
            {
                imgg = new Bitmap("EA" + i + ".png");
                Cl = imgg.GetPixel(0, 0);
                imgg.MakeTransparent(Cl);
                PnE.Imgs_Kill.Add(imgg);
            }
            for (int i = 2; i <= 7; i++)              // for reverse hitting 
            {
                imgg = new Bitmap("EA" + i + ".png");
                Cl = imgg.GetPixel(0, 0);
                imgg.MakeTransparent(Cl);
                PnE.Imgs_Kill_Revers.Add(imgg);
            }

            for (int i = 1; i <= 6; i++) // for death
            {
                imgg = new Bitmap("ED" + i + ".png");
                Cl = imgg.GetPixel(0, 0);
                imgg.MakeTransparent(Cl);
                PnE.Imgs_Death.Add(imgg);
            }
            PnE.X = 1000; PnE.Flag_State = 1;
            PnE.Y = Lblocks[0].Y - PnN.Imgs_Run[0].Height;
            LEnemies.Add(PnE);*/
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawScene(e.Graphics);
        }
        void DrawScene(Graphics g2)
        {
            g2.Clear(Color.White);
            if (!GAME_OVER)
            {
                if (GAME_START)
                {   
                    Font font = new Font("Arial", 50);
                    Brush brushh = Brushes.Black;
                    string jump = "Tap Space to Jump";
                    string enter = "Enter to Attack";
                    Font size = new Font("Arial", 21);
                  
                    int x= Ct_Score; initiator = 0;
                    int f = x;
                    while (x != 0) 
                    {
                        x /= 10;
                        initiator++;
                    }
                    string score = Ct_Score.ToString();
                    if (initiator == 0) initiator = 1;
                    g2.DrawString(score, font, brushh, this.ClientSize.Width - Score_Adaptation * initiator, 3);
                    if (!Flag_Start_Game) 
                        g2.DrawString(jump, size, brushh, LIntro[3].X, LIntro[3].Y - 60);

                    for (int i = 0; i < Lblocks.Count; i++)
                    {
                        Brush brsh = new SolidBrush(Lblocks[i].clr);
                        g2.FillRectangle(brsh, Lblocks[i].X, Lblocks[i].Y, Lblocks[i].W, Lblocks[i].H);
                    }
                    for (int i = 0; i < LNinja.Count; i++) // ninja drawing
                    {
                        if (LNinja[0].Flag_State == 1)
                            g2.DrawImage(LNinja[i].Imgs_Run[LNinja[i].icurr_Run], LNinja[i].X, LNinja[i].Y);
                        else if (LNinja[0].Flag_State == 2)
                            g2.DrawImage(LNinja[i].Imgs_Hit[LNinja[i].icurr_Hit], LNinja[i].X, LNinja[i].Y);
                        else if (LNinja[0].Flag_State == 3)
                            g2.DrawImage(LNinja[i].Imgs_Jump[LNinja[i].icurr_Jumb], LNinja[i].X, LNinja[i].Y);
                        else if (LNinja[0].Flag_State == 4)
                            g2.DrawImage(LNinja[i].Imgs_Land[LNinja[i].icurr_Land], LNinja[i].X, LNinja[i].Y);
                        else if (LNinja[0].Flag_State == 5)
                            g2.DrawImage(LNinja[i].Imgs_Drawn[LNinja[i].icurr_Drawn], LNinja[i].X, LNinja[i].Y);
                        else if (LNinja[0].Flag_State == 6) // death
                            g2.DrawImage(LNinja[i].imgs_Death[LNinja[i].icurr_Death], LNinja[i].X, LNinja[i].Y);
                    }
                    for (int i = 0; i < LSlaves.Count; i++) // slaves drawing
                    {
                        g2.DrawImage(LSlaves[i].Imgs_Run[LSlaves[i].icurr_Run], LSlaves[i].X, LSlaves[i].Y);
                    }
                    for(int i = 0;i<LEnemies.Count;i++)
                    {
                        if (LEnemies[i].Flag_State == 1) // pose
                            g2.DrawImage(LEnemies[i].Imgs_pose[LEnemies[i].icurr_pose], LEnemies[i].X, LEnemies[i].Y);
                        else if (LEnemies[i].Flag_State == 2) // enemy hit
                        {
                            if (LEnemies[i].icurr_Hit != 6)
                                g2.DrawImage(LEnemies[i].Imgs_Kill[LEnemies[i].icurr_Hit], LEnemies[i].X, LEnemies[i].Y);
                        }
                        else if (LEnemies[i].Flag_State == 3) // enemy hit
                            g2.DrawImage(LEnemies[i].Imgs_Death[LEnemies[i].icurr_Death], LEnemies[i].X, LEnemies[i].Y);
                        else if (LEnemies[i].Flag_State == 4)
                        {
                            if (LEnemies[i].icurr_Rev_kill != 6)
                                g2.DrawImage(LEnemies[i].Imgs_Kill_Revers[LEnemies[i].icurr_Rev_kill], LEnemies[i].X, LEnemies[i].Y);
                        }
                    }
                    if (!Flag_Start_Game)
                        g2.DrawString(enter, size, brushh, LIntro[2].X + 120, LIntro[2].Y - 28);

                }
                if (!Flag_Start_Game)
                {
                    for (int i = 0; i < LIntro.Count; i++)
                    {
                        if (!GAME_START)
                        {
                            if (i < 2)
                                g2.DrawImage(LIntro[i].img, LIntro[i].X, LIntro[i].Y);
                        }
                        else
                        {
                            if (i >= 2)
                                g2.DrawImage(LIntro[i].img, LIntro[i].X, LIntro[i].Y);
                        }
                    }
                }
            }
            else
            {
                int x = Lblocks[Catch_Game_Over_Block].X + Lblocks[Catch_Game_Over_Block].W;
                //MessageBox.Show("Xhero:" + LNinja[0].X + " Xblock" + Lblocks[Catch_Game_Over_Block].X +" wblock  " +x );
                //MessageBox.Show("GAME OVER");
                Bitmap imgg = new Bitmap("GAME OVER.jpg");
                g2.DrawImage(imgg, 480, 250);
                Font font = new Font("Arial", 50);
                Brush brushh = Brushes.Black;
     

                string a5er_haga = "your Score is : " + Ct_Score;
                g2.DrawString(a5er_haga, font, brushh, 500, 500);
                //this.Close();
            }
        }
    }
}

// solve the error enu msh by3ml landing badry
// solve the error when doing landing then pressing space key
// e3ml heta aham haga tarf l hero w hwa bynot

// law l landing da5lt fe mara mat5ushesh feh mara tanya ela b3d ama ynzl 3la lard w ygry aw ela b3d ama 
// y7sal drawn 8er keda la 