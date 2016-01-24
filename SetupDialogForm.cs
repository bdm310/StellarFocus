using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using ASCOM.Utilities;
using ASCOM.StellarFocus;

namespace ASCOM.StellarFocus
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private Focuser sf;
        private System.Timers.Timer StatusTimer;
        private System.Timers.Timer FormUpdateTimer;

        private Thread HomeThread;
        private Thread ConnectThread;

        private object mStatus = new object();

        private int lastConnectStatus = 0;

        //Status variables
        private int dispPosition = 0;
        private bool dispMoving = false;
        private bool dispHomeState = false;
        private double dispTemp = 0;

        #region Setup

        //Constructor
        public SetupDialogForm()
        {
            InitializeComponent();

            sf = new Focuser();
            
            // Initialise current values of user settings from the ASCOM Profile 
            ComPortSelector.Text = Focuser.comPort;
            chkTrace.Checked = Focuser.traceState;

            TempCompEnabledCheck.Checked = Focuser.TempCompEnabled;
            TempCoeff.Text = Focuser.TempCoeff.ToString();

            IdleOff.Checked = Focuser.IdleOff;
            Reverse.Checked = Focuser.Reverse;
            MaxVel.Text = Focuser.MaxVel.ToString();
            Accel.Text = Focuser.Accel.ToString();

            Home.Checked = Focuser.Home;
            HomePositive.Checked = Focuser.HomeDirection;
            InvertHome.Checked = Focuser.InvertHome;
            HomeVel.Text = Focuser.HomeVel.ToString();
            UseSwitch.Checked = Focuser.HomeUseSwitch;
            Distance.Text = Focuser.HomeDistance.ToString();
            HomePosition.Text = Focuser.HomePosition.ToString();

            if (Focuser.HomeUseSwitch) Distance.Enabled = false;
            else Distance.Enabled = true;
        }

        //Initialize before loading the form
        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            Serial sp;
            sp = new Serial();

            //Find COM ports
            string[] sports = sp.AvailableCOMPorts;

            for(uint i = 0; i <= sports.GetUpperBound(0); i++)
            {
                ComPortSelector.Items.Add(sports[i]);
            }

            //Set up status timer
            StatusTimer = new System.Timers.Timer(200);
            StatusTimer.Elapsed += new ElapsedEventHandler(GetStatus);
            StatusTimer.Enabled = true;

            //Set up form update timer
            FormUpdateTimer = new System.Timers.Timer(100);
            FormUpdateTimer.SynchronizingObject = this;
            FormUpdateTimer.Elapsed += new ElapsedEventHandler(UpdateForm);
            FormUpdateTimer.Enabled = true;
        }

        private void SetupDialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            lock (mStatus)
            {
                StatusTimer.Enabled = false;
                FormUpdateTimer.Enabled = false;
            }

            if (HomeThread != null)
            {
                sf.CancelHome = true;
                while (HomeThread.IsAlive) { };
            }

            if (ConnectThread != null)
            {
                while (ConnectThread.IsAlive) { };
            }

            sf.Connected = false;
            sf.Dispose();
        }

        #endregion

        #region Threads

        private void DoHome()
        {
            double IntervalSave = StatusTimer.Interval;

            StatusTimer.Interval = 1000;
            try
            {
                sf.FindHome();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            StatusTimer.Interval = IntervalSave;
        }

        private void TryConnect()
        {
            if (sf.Connected)
            {
                sf.Connected = false;
            }
            else
            {
                try
                {
                    sf.Connected = true;
                }
                catch (ASCOM.DriverException connectexception)
                {
                    sf.Connected = false;
                    MessageBox.Show(connectexception.Message);
                }
            }
        }

        private void GetStatus(object source, ElapsedEventArgs e)
        {
            lock (mStatus)
            {
                int ConnectStatus = sf.ConnectStatus;

                if (sf != null && ConnectStatus != 1)
                {
                    try
                    {
                        if (sf.Connected)
                        {
                            try
                            {
                                dispTemp = sf.Temperature;
                            }
                            catch (ASCOM.DriverException)
                            {
                                dispTemp = double.NaN;
                            }

                            if (sf.IsMoving) dispMoving = true;
                            else dispMoving = false;

                            dispPosition = sf.Position;

                            dispHomeState = sf.HomeState;
                        }
                    }
                    catch
                    { }
                }
            }
        }

        private void UpdateForm(object source, ElapsedEventArgs e)
        {
            int ConnectStatus = sf.ConnectStatus;

            if (ConnectStatus == 2)
            {
                Connect.Text = "Disconnect";

                if (lastConnectStatus != ConnectStatus)
                {
                    MaxVel.Text = Focuser.MaxVel.ToString();
                    Accel.Text = Focuser.Accel.ToString();
                    IdleOff.Checked = Focuser.IdleOff;

                    lastConnectStatus = ConnectStatus;
                }

                if (double.IsNaN(dispTemp)) Temp.Text = "Temperature: Sensor Error";
                else Temp.Text = "Temperature: " + dispTemp.ToString() + "°C";

                if (dispMoving) MovingLabel.Text = "Moving: Yes";
                else MovingLabel.Text = "Moving: No";

                PositionLabel.Text = "Position: " + dispPosition.ToString();

                HomeState.Text = "Home Switch: " + dispHomeState.ToString();

                TempCoeff.Text = Focuser.TempCoeff.ToString();
                TempCompEnabledCheck.CheckedChanged -= TempCompEnabledCheck_CheckedChanged;
                TempCompEnabledCheck.Checked = Focuser.TempCompEnabled;
                TempCompEnabledCheck.CheckedChanged += TempCompEnabledCheck_CheckedChanged;

                if (sf.Homing)
                {
                    SetPos.Enabled = false;
                    Halt.Enabled = false;
                    WriteSettings.Enabled = false;
                    Reverse.Enabled = false;
                    TempCompEnabledCheck.Enabled = false;
                    ZeroPos.Enabled = false;
                    if (!sf.CancelHome) FindHome.Text = "Cancel";
                    else FindHome.Text = "Halt...";
                }
                else
                {
                    FindHome.Text = "Home";
                    SetPos.Enabled = true;
                    Halt.Enabled = true;
                    WriteSettings.Enabled = true;
                    Reverse.Enabled = true;
                    TempCompEnabledCheck.Enabled = true;
                    ZeroPos.Enabled = true;
                }
                FindHome.Enabled = true;
            }
            else
            {
                if (ConnectStatus == 0) Connect.Text = "Connect";
                else Connect.Text = "Connecting...";

                Temp.Text = "Temperature: ";
                MovingLabel.Text = "Moving: ";
                PositionLabel.Text = "Position: ";
                HomeState.Text = "Home Switch: ";

                SetPos.Enabled = false;
                Halt.Enabled = false;
                WriteSettings.Enabled = false;
                TempCompEnabledCheck.Enabled = false;
                FindHome.Enabled = false;
                ZeroPos.Enabled = false;
            }
        }

        #endregion

        #region Button Handlers

        private void Connect_Click(object sender, EventArgs e)
        {
            if (ConnectThread == null || !ConnectThread.IsAlive)
            {
                ConnectThread = new Thread(new ThreadStart(TryConnect));
                ConnectThread.Start();
            }
        }

        private void Halt_Click(object sender, EventArgs e)
        {
            if(sf.Connected) sf.Halt();
        }

        private void SetPos_Click(object sender, EventArgs e)
        {
            if (sf.Connected)
            {
                Int32 Pos;
                try
                {
                    Pos = Convert.ToInt32(Position.Text);
                }
                catch
                {
                    MessageBox.Show("Invalid input.");
                    return;
                }

                try
                {
                    sf.Move(Pos);
                }
                catch (Exception seterr)
                {
                    MessageBox.Show(seterr.Message);
                }
            }
        }

        private void FindHome_Click(object sender, EventArgs e)
        {
            if (HomeThread == null || !HomeThread.IsAlive)
            {
                sf.CancelHome = false;
                HomeThread = new Thread(new ThreadStart(DoHome));
                HomeThread.Start();
            }
            else
            {
                sf.CancelHome = true;
            }
        }

        private void ZeroPos_Click(object sender, EventArgs e)
        {
            if(sf.Connected)
            {
                try
                {
                    sf.SetZero(Focuser.HomePosition);
                }
                catch(Exception innere)
                {
                    MessageBox.Show(innere.Message);
                }
            }
        }

        private void WriteSettings_Click(object sender, EventArgs e)
        {
            if (sf.Connected)
            {
                try
                {
                    sf.WriteMotorParams();
                }
                catch (Exception writeerr)
                {
                    MessageBox.Show("Could not write parameters: " + writeerr.Message);
                }
            }
        }

        #endregion

        #region Check Boxes

        private void TempCompEnabledCheck_CheckedChanged(object sender, EventArgs e)
        {
            if(sf.Connected) sf.TempComp = TempCompEnabledCheck.Checked;
        }

        private void chkTrace_CheckedChanged(object sender, EventArgs e)
        {
            Focuser.traceState = chkTrace.Checked;
        }

        private void IdleOff_CheckedChanged(object sender, EventArgs e)
        {
            Focuser.IdleOff = IdleOff.Checked;
        }

        private void InvertHome_CheckedChanged(object sender, EventArgs e)
        {
            Focuser.InvertHome = InvertHome.Checked;
        }

        private void HomePositive_CheckedChanged(object sender, EventArgs e)
        {
            Focuser.HomeDirection = HomePositive.Checked;
        }

        private void Home_CheckedChanged(object sender, EventArgs e)
        {
            Focuser.Home = Home.Checked;
        }

        private void UseSwitch_CheckedChanged(object sender, EventArgs e)
        {
            Focuser.HomeUseSwitch = UseSwitch.Checked;

            if (Focuser.HomeUseSwitch) Distance.Enabled = false;
            else Distance.Enabled = true;
        }

        private void Reverse_CheckedChanged(object sender, EventArgs e)
        {
            Focuser.Reverse = Reverse.Checked;
        }

        #endregion

        #region Text Inputs

        private void HomeVel_TextChanged(object sender, EventArgs e)
        {
            double Vel;

            if (!HomeVel.Text.Equals(""))
            {
                try
                {
                    Vel = Convert.ToDouble(HomeVel.Text);
                }
                catch
                {
                    MessageBox.Show("Invalid input.");
                    HomeVel.Text = Focuser.HomeVel.ToString();
                    return;
                }

                try
                {
                    Focuser.HomeVel = Vel;
                }
                catch (Exception innere)
                {
                    MessageBox.Show(innere.Message);
                    HomeVel.Text = Focuser.HomeVel.ToString();
                }
            }
        }

        private void Accel_TextChanged(object sender, EventArgs e)
        {
            double A;

            if (!Accel.Text.Equals(""))
            {
                try
                {
                    A = Convert.ToDouble(Accel.Text);
                }
                catch
                {
                    MessageBox.Show("Invalid input.");
                    Accel.Text = Focuser.Accel.ToString();
                    return;
                }

                try
                {
                    Focuser.Accel = A;
                }
                catch
                {
                    MessageBox.Show("Acceleration must be from 0 to 12700.");
                    Accel.Text = Focuser.Accel.ToString();
                }
            }
        }

        private void TempCoeff_TextChanged(object sender, EventArgs e)
        {
            double Coeff;

            if (!TempCoeff.Text.Equals(""))
            {
                try
                {
                    Coeff = Convert.ToDouble(TempCoeff.Text);
                }
                catch
                {
                    MessageBox.Show("Invalid input.");
                    TempCoeff.Text = Focuser.TempCoeff.ToString();
                    return;
                }

                try
                {
                    Focuser.TempCoeff = Coeff;
                }
                catch (Exception innere)
                {
                    MessageBox.Show(innere.Message);
                    TempCoeff.Text = Focuser.TempCoeff.ToString();
                }
            }
        }

        private void MaxVel_TextChanged(object sender, EventArgs e)
        {
            double Vel;

            if (!MaxVel.Text.Equals(""))
            {
                try
                {
                    Vel = Convert.ToDouble(MaxVel.Text);
                }
                catch
                {
                    MessageBox.Show("Invalid input.");
                    MaxVel.Text = Focuser.MaxVel.ToString();
                    return;
                }

                try
                {
                    Focuser.MaxVel = Vel;
                }
                catch (Exception innere)
                {
                    MessageBox.Show(innere.Message);
                    MaxVel.Text = Focuser.MaxVel.ToString();
                }
            }
        }

        private void Distance_TextChanged(object sender, EventArgs e)
        {
            int Dist;
            if (!Distance.Text.Equals(""))
            {
                try
                {
                    Dist = Convert.ToInt32(Distance.Text);
                }
                catch
                {
                    MessageBox.Show("Invalid input.");
                    Distance.Text = Focuser.HomeDistance.ToString();
                    return;
                }

                try
                {
                    Focuser.HomeDistance = Dist;
                }
                catch (Exception innere)
                {
                    MessageBox.Show(innere.Message);
                    Distance.Text = Focuser.HomeDistance.ToString();
                }
            }
        }

        private void HomePosition_TextChanged(object sender, EventArgs e)
        {
            int Pos;

            if (!HomePosition.Text.Equals(""))
            {
                try
                {
                    Pos = Convert.ToInt32(HomePosition.Text);
                }
                catch
                {
                    MessageBox.Show("Invalid input.");
                    HomePosition.Text = Focuser.HomePosition.ToString();
                    return;
                }

                try
                {
                    Focuser.HomePosition = Pos;
                }
                catch (Exception innere)
                {
                    MessageBox.Show(innere.Message);
                    HomePosition.Text = Focuser.HomePosition.ToString();
                }
            }
        }

        #endregion

        //Update the focuser COM port
        private void ComPortSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Focuser.comPort = ComPortSelector.Text;
        }
    }
}