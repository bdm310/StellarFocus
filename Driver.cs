//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Focuser driver for StellarFocus
//
// Description:	Driver for StellarFocus
//
// Implements:	ASCOM Focuser interface version: 1.0
// Author:		(BDM) Brian Moyer (bdm310@gmail.com)
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 07/10/2015	BDM	6.0.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Threading;

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;

namespace ASCOM.StellarFocus
{
    //
    // DeviceID is ASCOM.StellarFocus.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.StellarFocus.Focuser
    // The ClassInterface/None attribute prevents an empty interface called
    // _StellarFocus from being created and used as the [default] interface
    //

    /// <summary>
    /// ASCOM Focuser Driver for StellarFocus.
    /// </summary>
    [Guid("b89794d0-b655-4acc-afd1-260399f528b4")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : IFocuserV2
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.StellarFocus.Focuser";
        // TODO Change the descriptive string for your driver then remove this line
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "Focuser Driver for Stellar Focus.";

        internal static string comPortProfileName = "COM Port"; // Constants used for Profile persistence
        internal static string comPortDefault = "COM1";
        internal static string traceStateProfileName = "Trace Level";
        internal static string traceStateDefault = "false";

        internal static string TempCoeffProfileName = "Temperature Coeff";
        internal static string TempCoeffDefault = "0";
        internal static string TempCompEnabledProfileName = "Temperature Comp";
        internal static string TempCompEnabledDefault = "false";

        internal static string MaxVelProfileName = "Maximum Velocity";
        internal static string MaxVelDefault = "500";
        internal static string AccelProfileName = "Acceleration";
        internal static string AccelDefault = "500";
        internal static string IdleOffProfileName = "Idle Off";
        internal static string IdleOffDefault = "true";
        internal static string ReverseProfileName = "Reverse";
        internal static string ReverseDefault = "false";

        internal static string HomeProfileName = "Home";
        internal static string HomeDefault = "false";
        internal static string InvertHomeProfileName = "Invert Home";
        internal static string InvertHomeDefault = "false";
        internal static string HomeDirectionProfileName = "Home Direction";
        internal static string HomeDirectionDefault = "false";
        internal static string HomeVelProfileName = "Home Vel";
        internal static string HomeVelDefault = "250";
        internal static string HomeUseSwitchProfileName = "true";
        internal static string HomeUseSwitchDefault = "true";
        internal static string HomeDistanceProfileName = "Home Distance";
        internal static string HomeDistanceDefault = "0";
        internal static string HomePositionProfileName = "Home Position";
        internal static string HomePositionDefault = "0";

        private const int PositionMax = 65535;
        private const int IncrementMax = 65535;
        private const int MaxVelMax = 2000;
        private const double VelScale = 1;
        private const int AccelMax = 12700;
        private const double AccelScale = 0.001;
        private const double TempCoeffMax = 20479;
        private const double TempCoeffMin = -20480;
        private const double TempCoeffScale = 1.6;
        private const double TempScale = 0.1;
        private const int TempErrorValue = -4097;

        internal static string comPort; // Variables to hold the currrent device configuration
        internal static bool traceState;

        private static double pTempCoeff;
        internal static double TempCoeff
        {
            set
            {
                if(value > TempCoeffMax || value < TempCoeffMin) throw new ASCOM.DriverException("Temperature coefficient must be between " + 
                    TempCoeffMin.ToString() + " and " + TempCoeffMax.ToString() + ".");
                else pTempCoeff = value;
            }
            get
            {
                return pTempCoeff;
            }
        }
        internal static bool TempCompEnabled;

        private static double pMaxVel;
        internal static double MaxVel
        {
            set
            {
                if (value > MaxVelMax || value <= 0) throw new ASCOM.DriverException("Maximum velocity must be between " + 
                    (1 / VelScale).ToString() + " and " + (MaxVelMax / VelScale).ToString() + ".");
                else pMaxVel = value;
            }
            get
            {
                return pMaxVel;
            }
        }

        private static double pAccel;
        internal static double Accel
        {
            set
            {
                if (value > AccelMax || value <= 0) throw new ASCOM.DriverException("Maximum acceleration must be between " +
                     (1 / AccelScale).ToString() + " and " + (AccelMax / AccelScale).ToString() + ".");
                else pAccel = value;
            }
            get
            {
                return pAccel;
            }
        }
        internal static bool IdleOff;
        internal static bool Reverse;

        internal static bool Home;
        internal static bool InvertHome;
        internal static bool HomeDirection;
        internal bool CancelHome;
        internal bool Homing;

        //0-Disconnected 1-Connecting 2-Connected
        internal int ConnectStatus;

        private static double pHomeVel;
        internal static double HomeVel
        {
            set
            {
                if (value > MaxVelMax || value <= 0) throw new ASCOM.DriverException("Home velocity must be between " +
                    (1 / VelScale).ToString() + " and " + (MaxVelMax / VelScale).ToString() + ".");
                else pHomeVel = value;
            }
            get
            {
                return pHomeVel;
            }
        }
        internal static bool HomeUseSwitch;
        private static int pHomeDistance;
        internal static int HomeDistance
        {
            set
            {
                if (value > IncrementMax || value < 0) throw new ASCOM.DriverException("Home distance must be between 0 and " +
                    IncrementMax.ToString() + ".");
                else pHomeDistance = value;
            }
            get
            {
                return pHomeDistance;
            }
        }
        private static int pHomePosition;
        internal static int HomePosition
        {
            set
            {
                if (value > IncrementMax || value < 0) throw new ASCOM.DriverException("Home position must be between 0 and " +
                    PositionMax.ToString() + ".");
                else pHomePosition = value;
            }
            get
            {
                return pHomePosition;
            }
        }

        private ASCOM.Utilities.Serial serialPort;
        private object serLock = new object();

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        private TraceLogger tl;

        /// <summary>
        /// Initializes a new instance of the <see cref="StellarFocus"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Focuser()
        {
            ReadProfile(); // Read device configuration from the ASCOM Profile store

            tl = new TraceLogger("", "StellarFocus");
            tl.Enabled = traceState;
            tl.LogMessage("Focuser", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro utilities object
            serialPort = new ASCOM.Utilities.Serial();

            ConnectStatus = 0;
            Homing = false;

            tl.LogMessage("Focuser", "Completed initialisation");
        }


        //
        // PUBLIC COM INTERFACE IFocuserV2 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// </summary>
        public void SetupDialog()
        {
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show("Already connected, be careful with configuration changes.");

            using (SetupDialogForm F = new SetupDialogForm())
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
                else
                {
                    ReadProfile();
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            lock (serLock)
            {
                if (serialPort.Connected)
                {
                    try
                    {
                        serialPort.Connected = false;
                    }
                    catch { }
                }
                serialPort.Dispose();
            }
            serLock = null;

            // Clean up the tracelogger and util objects
            tl.Enabled = false;
            tl.Dispose();
            utilities.Dispose();
            astroUtilities.Dispose();
        }

        public bool Connected
        {
            get
            {
                tl.LogMessage("Connected Get", IsConnected.ToString());
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected Set", value.ToString());
                if (value == IsConnected)
                    return;

                if (value)
                {
                    tl.LogMessage("Connected Set", "Connecting to port " + comPort);
                    ConnectStatus = 1;

                    lock (serLock)
                    {
                        //Set up serial port options and try opening the port
                        serialPort.PortName = comPort;
                        serialPort.Speed = SerialSpeed.ps115200;
                        serialPort.Parity = SerialParity.None;
                        serialPort.DataBits = 8;
                        serialPort.StopBits = SerialStopBits.One;
                        serialPort.Handshake = SerialHandshake.None;
                        serialPort.ReceiveTimeoutMs = 2000;

                        try
                        {
                            serialPort.Connected = true;
                        }
                        catch (Exception e)
                        {
                            ConnectStatus = 0;
                            throw new ASCOM.DriverException("Could not open COM port: " + comPort, e);
                        }
                    }

                    //Get the controller's configuration
                    byte[] recData, serData;
                    serData = new byte[1];

                    serData[0] = 0x05;

                    connectedState = true;

                    try
                    {
                        recData = SendCommand(serData, 6);
                    }
                    catch
                    {
                        lock (serLock)
                        {
                            connectedState = false;
                            if (serialPort.Connected)
                            {
                                try
                                {
                                    serialPort.Connected = false;
                                }
                                catch { }
                            }
                        }

                        ConnectStatus = 0;
                        throw;
                    }

                    byte[] intBytes;
                    intBytes = new byte[2];

                    //Extract the current temp coefficient
                    intBytes[0] = recData[1];
                    intBytes[1] = recData[2];
                    if (!BitConverter.IsLittleEndian) Array.Reverse(intBytes);

                    Int16 CurrTempCoeff;

                    CurrTempCoeff = BitConverter.ToInt16(intBytes, 0);

                    //Decide if temp comp is enabled and set the driver's coefficient if it is
                    TempCompEnabled = false;
                    if (CurrTempCoeff != 0)
                    {
                        TempCoeff = Convert.ToDouble(CurrTempCoeff) / TempCoeffScale;
                        TempCompEnabled = true;
                    }

                    //Extract the motor parameters
                    IdleOff = false;
                    if(recData[3] != 0) IdleOff = true;

                    Accel = Convert.ToDouble(recData[4]) / AccelScale;

                    intBytes[0] = recData[5];
                    intBytes[1] = recData[6];
                    if (!BitConverter.IsLittleEndian) Array.Reverse(intBytes);

                    MaxVel = Convert.ToDouble(BitConverter.ToUInt16(intBytes, 0)) / VelScale;

                    if (Home) FindHome();

                    ConnectStatus = 2;
                }
                else
                {
                    connectedState = false;
                    tl.LogMessage("Connected Set", "Disconnecting from port " + comPort);

                    lock (serLock)
                    {
                        if (serialPort.Connected) serialPort.Connected = false;
                    }

                    ConnectStatus = 0;
                }
            }
        }

        public string Description
        {
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                tl.LogMessage("InterfaceVersion Get", "2");
                return Convert.ToInt16("2");
            }
        }

        public string Name
        {
            get
            {
                string name = "StellarFocus Driver";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region IFocuser Implementation

        public double StepSize
        {
            get
            {
                tl.LogMessage("StepSize Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("StepSize", false);
            }
        }

        public int MaxIncrement
        {
            get
            {
                tl.LogMessage("MaxIncrement Get", PositionMax.ToString());
                return IncrementMax; // Maximum change in one move
            }
        }

        public int MaxStep
        {
            get
            {
                tl.LogMessage("MaxStep Get", PositionMax.ToString());
                return PositionMax; // Maximum extent of the focuser (0 - PositionMax)
            }
        }

        public bool Absolute
        {
            get
            {
                tl.LogMessage("Absolute Get", true.ToString());
                return true; // This is an absolute focuser
            }
        }

        public void Halt()
        {
            tl.LogMessage("Halt", "Halting focuser movement.");

            //Send halt command
            byte[] serData;
            serData = new byte[1];

            serData[0] = 0x03;

            SendCommand(serData, 0);
        }

        public bool IsMoving
        {
            get
            {
                //Get position and motion status
                byte[] recData, serData;
                serData = new byte[1];

                serData[0] = 0x0B;

                recData = SendCommand(serData, 1);

                tl.LogMessage("IsMoving Get", Convert.ToBoolean(recData[1]).ToString());
                return Convert.ToBoolean(recData[1]);
            }
        }

        public bool Link
        {
            get
            {
                tl.LogMessage("Link Get", this.Connected.ToString());
                return this.Connected; // Direct function to the connected method, the Link method is just here for backwards compatibility
            }
            set
            {
                tl.LogMessage("Link Set", value.ToString());
                this.Connected = value; // Direct function to the connected method, the Link method is just here for backwards compatibility
            }
        }

        public void Move(int Position)
        {
            tl.LogMessage("Move", Position.ToString());

            if (TempCompEnabled) throw new ASCOM.InvalidOperationException("Attempted focuser move while temperature compensation is active.");

            if(Position > PositionMax || Position < 0) throw new ASCOM.DriverException("Position must be between 0 and " + PositionMax.ToString() + ".");

            byte[] recData, serData, posBytes;
            serData = new byte[3];

            //Reverse if necessary
            if (Reverse) Position = MaxStep - Position;

            //Shift the position we send so it's centered around focuserSteps/2
            Position = Position - (PositionMax + 1) / 2;
            posBytes = BitConverter.GetBytes(Convert.ToInt16(Position));
            if (!BitConverter.IsLittleEndian) Array.Reverse(posBytes);

            //First byte is length, command in upper and lower nibble respectively
            serData[0] = Convert.ToByte(16 * (serData.Length - 1) + 0x02);

            Buffer.BlockCopy(posBytes, 0, serData, 1, posBytes.Length);

            //Send the command and get the response
            recData = SendCommand(serData, 2);

            //Validate the received data
            bool failed = false;
            for (int i = 1; i < serData.Length; i++)
            {
                if (recData[i] != serData[i]) failed = true;
            }
            
            if (failed)
            {
                string fmtSent = ByteArrayString(serData);
                string fmtData = ByteArrayString(recData);

                throw new ASCOM.DriverException("Error setting position. " +
                "\n" + fmtSent + " - Expected" +
                "\n" + fmtData + " - Received");
            }
        }

        public int Position
        {
            get
            {
                //Request position
                byte[] recData, serData;
                serData = new byte[1];

                serData[0] = 0x01;

                recData = SendCommand(serData, 2);

                byte[] posBytes = new byte[2];

                //Extract the position bytes
                posBytes[0] = recData[1];
                posBytes[1] = recData[2];
                if (!BitConverter.IsLittleEndian) Array.Reverse(posBytes);

                int pos = Convert.ToInt32(BitConverter.ToInt16(posBytes, 0)) + (PositionMax + 1) / 2;

                //Reverse if necessary
                if (Reverse) pos = MaxStep - pos;

                // Return the focuser position, centered around the midway point in our range
                return pos;
            }
        }

        public bool TempComp
        {
            get
            {
                tl.LogMessage("TempComp Get", TempCompEnabled.ToString());
                return TempCompEnabled;
            }
            set
            {
                tl.LogMessage("TempComp Set", value.ToString());

                byte[] recData, serData, tempBytes;
                serData = new byte[3];

                //Calculate the coefficient to send out
                if (value) tempBytes = BitConverter.GetBytes(Convert.ToInt16(TempCoeff * TempCoeffScale));
                else tempBytes = BitConverter.GetBytes(Convert.ToInt16(0));
                if (!BitConverter.IsLittleEndian) Array.Reverse(tempBytes);

                //First byte is length, command in upper and lower nibble respectively
                serData[0] = Convert.ToByte(16 * (serData.Length - 1) + 0x04);

                Buffer.BlockCopy(tempBytes, 0, serData, 1, tempBytes.Length);

                //Send the command and get the response
                recData = SendCommand(serData, 2);

                //Validate the received data
                bool failed = false;
                for (int i = 1; i < serData.Length; i++)
                {
                    if (recData[i] != serData[i]) failed = true;
                }

                if (failed)
                {
                    TempCompEnabled = true;
                    if (recData[1] == 0 && recData[2] == 0) TempCompEnabled = false;

                    string fmtSent = ByteArrayString(serData);
                    string fmtData = ByteArrayString(recData);

                    throw new ASCOM.DriverException("Error setting temperature compensation coefficient. " +
                    "\n" + fmtSent + " - Expected" +
                    "\n" + fmtData + " - Received");
                }

                TempCompEnabled = value;
            }
        }

        public bool TempCompAvailable
        {
            get
            {
                tl.LogMessage("TempCompAvailable Get", true.ToString());
                return true;
            }
        }

        public double Temperature
        {
            get
            {
                byte[] recData, serData;
                serData = new byte[1];

                serData[0] = 0x0A;

                //Get data from the controller
                recData = SendCommand(serData, 2);

                short TempInt;
                double Temp;
                byte[] tempBytes = new byte[2];

                //Extract the integer temp value
                tempBytes[0] = recData[1];
                tempBytes[1] = recData[2];
                if (!BitConverter.IsLittleEndian) Array.Reverse(tempBytes);

                TempInt = BitConverter.ToInt16(tempBytes, 0);

                //Check for a disconnected sensor and scale the temp if not
                if (TempInt == TempErrorValue) throw new ASCOM.DriverException("Temperature sensor read error.");
                else Temp = Convert.ToDouble(TempInt) * TempScale;

                tl.LogMessage("Temperature Get", Temp.ToString());
                return Temp;
            }
        }

        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Focuser";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Writes motor parameters to EEPROM
        /// </summary>
        internal void WriteMotorParams()
        {
            byte[] serData, recData;
            serData = new byte[5];

            //Convert values to bytes
            byte[] intBytes = BitConverter.GetBytes(Convert.ToUInt16(MaxVel * VelScale));
            if (!BitConverter.IsLittleEndian) Array.Reverse(intBytes);

            Buffer.BlockCopy(intBytes, 0, serData, 1, intBytes.Length);

            serData[3] = Convert.ToByte(Accel * AccelScale);
            if (serData[3] == 0) serData[3] = 1;

            serData[4] = 0;
            if (IdleOff) serData[4] = 1;

            //First byte is length, command in upper and lower nibble respectively
            serData[0] = Convert.ToByte(16 * (serData.Length - 1) + 0x06);

            //Send everything over
            recData = SendCommand(serData, 4);

            //Validate the received data
            bool failed = false;
            for (int i = 1; i < serData.Length; i++)
            {
                if (recData[i] != serData[i]) failed = true;
            }

            if (failed)
            {
                string fmtSent = ByteArrayString(serData);
                string fmtData = ByteArrayString(recData);

                throw new ASCOM.DriverException("Error setting motor parameters. " +
                "\n" + fmtSent + " - Expected" +
                "\n" + fmtData + " - Received");
            }
        }

        /// <summary>
        /// Handles sending a command and receiving the response
        /// </summary>
        private byte[] SendCommand(byte[] serData, int expectBytes)
        {
            //Just give up if we don't even think we're connected
            if (connectedState == false) throw new ASCOM.NotConnectedException("Not connected.");

            //Try to take the serial port
            lock (serLock)
            {
                //If the port is not already open, release our mutex and error out
                if (!serialPort.Connected)
                {
                    throw new ASCOM.NotConnectedException("Not connected");
                }

                //Try to send/receive and pass any exceptions upstream
                int recBytes = 1;
                int tryCount = 0;
                const int retries = 2;
                byte cmdByte = new byte();
                byte[] response = null;

                while (true)
                {
                    tryCount++;

                    //Try to send the command and data
                    try
                    {
                        serialPort.ClearBuffers();
                        serialPort.TransmitBinary(serData);
                    }
                    catch (Exception e)
                    {
                        throw new ASCOM.DriverException("Serial port error while sending command.", e);
                    }

                    //Lot what we sent if logging is on
                    if (traceState)
                    {
                        string fmtSent = ByteArrayString(serData);

                        tl.LogMessage("SendCommand", "Sent: " + fmtSent);
                    }

                    //Try to receive the command byte of the response
                    try
                    {
                        cmdByte = serialPort.ReceiveByte();
                    }
                    catch (Exception e)
                    {
                        if (traceState) tl.LogMessage("SendCommand", "Error: Receive command byte timed out.");

                        if (tryCount < retries) continue;
                        if (traceState) tl.LogMessage("SendCommand", "Error: Giving up.");

                        throw new ASCOM.DriverException("Error receiving response command.", e);
                    }

                    //Figure out what command was returned and how many data bytes to expect
                    int responseBytes = (byte)((cmdByte >> 4) & 0x0F);

                    response = new byte[responseBytes + 1];
                    response[0] = cmdByte;

                    cmdByte = (byte)(cmdByte & 0x0F);

                    //Try to receive data bytes
                    while (recBytes < responseBytes + 1)
                    {
                        try
                        {
                            response[recBytes] = serialPort.ReceiveByte();
                            recBytes++;
                        }
                        catch (Exception e)
                        {
                            string fmtData;
                            if (response != null)
                            {
                                fmtData = ByteArrayString(response);
                            }
                            else
                            {
                                fmtData = "Nothing";
                            }

                            throw new ASCOM.DriverException("Error receiving response data, received response: " + fmtData, e);
                        }
                    }

                    //Log what we received if logging is on
                    if (traceState)
                    {
                        string fmtData = "";
                        for (int i = 0; i < response.Length; i++)
                        {
                            fmtData = fmtData + response[i].ToString("X2") + " ";
                        }

                        tl.LogMessage("SendCommand", "Recd: " + fmtData);
                    }

                    //Received a command timeout response
                    if (cmdByte == 0x0)
                    {
                        if (traceState) tl.LogMessage("SendCommand", "Error: Received command timed out.");

                        if (tryCount < retries) continue;
                        if (traceState) tl.LogMessage("SendCommand", "Error: Giving up");

                        string fmtSent = ByteArrayString(serData);
                        string fmtData = ByteArrayString(response);

                        throw new ASCOM.DriverException("Command timed out," +
                            "\n" + fmtSent + " - Sent" +
                            "\n" + fmtData + " - Received");
                    }

                    //Received an unknown command response
                    if (cmdByte == 0xF)
                    {
                        if (traceState) tl.LogMessage("SendCommand", "Error: Received unknown command.");

                        if (tryCount < retries) continue;
                        if (traceState) tl.LogMessage("SendCommand", "Error: Giving up");

                        throw new ASCOM.DriverException("Controller did not recognize command.");
                    }

                    //Validate command byte
                    if (cmdByte != (serData[0] & 0x0F))
                    {
                        if (traceState) tl.LogMessage("SendCommand", "Error: Received unmatched command.");

                        if (tryCount < retries) continue;
                        if (traceState) tl.LogMessage("SendCommand", "Error: Giving up");

                        string fmtSent = ByteArrayString(serData);
                        string fmtData = ByteArrayString(response);

                        throw new ASCOM.DriverException("Unmatched command in response. " +
                            "\n" + fmtSent + " - Sent" + 
                            "\n" + fmtData + " - Received");
                    }

                    //Validate response length
                    if (responseBytes != expectBytes)
                    {
                        if (traceState) tl.LogMessage("SendCommand", "Error: Received incorrect number of bytes.");

                        if (tryCount < retries) continue;
                        if (traceState) tl.LogMessage("SendCommand", "Error: Giving up");

                        string fmtSent = ByteArrayString(serData);
                        string fmtData = ByteArrayString(response);

                        throw new ASCOM.DriverException("Received incorrect number of bytes. " +
                            "\n" + fmtSent + " - Sent" +
                            "\n" + fmtData + " - Received");
                    }

                    return response;
                }
            }
        }

        /// <summary>
        /// Format a byte array as a string of space separated hex values
        /// </summary>
        private string ByteArrayString(byte[] Data)
        {
            string fmtData = "";
            for (int i = 0; i < Data.Length; i++)
            {
                fmtData += Data[i].ToString("X2");
                if (i < Data.Length) fmtData += " ";
            }

            return fmtData;
        }

        /// <summary>
        /// Sets a temporary velocity limit
        /// </summary>
        private void SetTempVelLimit(double velLimit)
        {
            byte[] recData, serData, velBytes;
            serData = new byte[3];

            //Convert vel to bytes
            velBytes = BitConverter.GetBytes(Convert.ToUInt16(velLimit * VelScale));
            if (!BitConverter.IsLittleEndian) Array.Reverse(velBytes);

            //First byte is length, command in upper and lower nibble respectively
            serData[0] = Convert.ToByte(16 * (serData.Length - 1) + 0x09);

            Buffer.BlockCopy(velBytes, 0, serData, 1, velBytes.Length);

            recData = SendCommand(serData, 2);

            //Validate the received data
            bool failed = false;
            for (int i = 1; i < serData.Length; i++)
            {
                if (recData[i] != serData[i]) failed = true;
            }
            
            if (failed)
            {
                string fmtSent = ByteArrayString(serData);
                string fmtData = ByteArrayString(recData);

                throw new ASCOM.DriverException("Error setting temporary velocity limit. " +
                "\n" + fmtSent + " - Expected" +
                "\n" + fmtData + " - Received");
            }
        }

        /// <summary>
        /// Zeros the position
        /// </summary>
        internal void SetZero(int Position)
        {
            byte[] recData, serData, posBytes;
            serData = new byte[3];

            //Reverse if necessary
            if (Reverse) Position = MaxStep - Position;

            //Shift the position we send so it's centered around focuserSteps/2
            Position = Position - (PositionMax + 1) / 2;
            posBytes = BitConverter.GetBytes(Convert.ToInt16(Position));
            if (!BitConverter.IsLittleEndian) Array.Reverse(posBytes);

            //First byte is length, command in upper and lower nibble respectively
            serData[0] = Convert.ToByte(16 * (serData.Length - 1) + 0x07);

            Buffer.BlockCopy(posBytes, 0, serData, 1, posBytes.Length);

            //Send the command and get the response
            recData = SendCommand(serData, 2);

            //Validate the received data
            bool failed = false;
            for (int i = 1; i < serData.Length; i++)
            {
                if (recData[i] != serData[i]) failed = true;
            }

            if (failed)
            {
                string fmtSent = ByteArrayString(serData);
                string fmtData = ByteArrayString(recData);

                throw new ASCOM.DriverException("Error setting zero position. " +
                "\n" + fmtSent + " - Expected" +
                "\n" + fmtData + " - Received");
            }
        }

        /// <summary>
        /// Homes the focuser
        /// </summary>
        internal void FindHome()
        {
            int state = 0;

            Homing = true;

            try
            {
                //Convert vel to bytes
                byte[] serData = BitConverter.GetBytes(Convert.ToUInt16(HomeVel * VelScale));
                if (!BitConverter.IsLittleEndian) Array.Reverse(serData);

                SetTempVelLimit(HomeVel);

                if (HomeUseSwitch && HomeState) state = 2;
                else state = 0;

                if (!HomeUseSwitch)
                {
                    if (HomeDirection) SetZero(MaxStep - HomeDistance);
                    else SetZero(HomeDistance);
                }

                while (state != 10)
                {
                    if (CancelHome)
                    {
                        Halt();
                        while (IsMoving) ;
                        break;
                    }

                    switch (state)
                    {
                        //Start jogging in the home direction and wait for the switch to trip
                        case 0:
                            if (HomeDirection)
                            {
                                if (HomeUseSwitch) SetZero(0);
                                Move(MaxStep);
                            }
                            else
                            {
                                if (HomeUseSwitch) SetZero(MaxStep);
                                Move(0);
                            }

                            if (IsMoving) state = 1;
                            break;
                        case 1:
                            bool moving = IsMoving;

                            if (!moving && HomeUseSwitch)
                            {
                                throw new ASCOM.DriverException("Error while homing: Switch state unchanged.");
                            }

                            if ((HomeState && HomeUseSwitch) || (!moving && !HomeUseSwitch))
                            {
                                Halt();
                                while (IsMoving) ;
                                SetTempVelLimit(HomeVel / 2);
                                if (HomeUseSwitch) state = 2;
                                else
                                {
                                    SetZero(HomePosition);
                                    state = 10;
                                }
                            }
                            break;
                        //Switch tripped after initial jog
                        case 2:
                            if (HomeDirection)
                            {
                                SetZero(MaxStep);
                                Move(0);
                            }
                            else
                            {
                                SetZero(0);
                                Move(MaxStep);
                            }

                            if (IsMoving) state = 3;
                            break;
                        case 3:
                            if (!IsMoving)
                            {
                                throw new ASCOM.DriverException("Error while homing: Switch state unchanged.");
                            }

                            if (!HomeState)
                            {
                                Halt();
                                while (IsMoving) ;

                                if (HomeDirection)  SetZero(0);
                                else SetZero(MaxStep);

                                state = 4;
                            }
                            break;
                        //Step slowly until tripping again
                        case 4:
                            if (HomeDirection)
                            {
                                Move(Position + 1);
                            }
                            else
                            {
                                Move(Position - 1);
                            }

                            while (IsMoving) ;

                            if (HomeState) state = 5;
                            break;
                        case 5:
                            SetZero(HomePosition);
                            state = 10;
                            break;
                    }
                }

                SetTempVelLimit(MaxVel);
            }
            catch
            {
                Homing = false;
                CancelHome = false;
                throw;
            }

            Homing = false;
            CancelHome = false;
        }

        /// <summary>
        /// Return the home switch status
        /// </summary>
        internal bool HomeState
        {
            get
            {
                bool state;
                byte[] serData;
                serData = new byte[1];

                serData[0] = 0x08;
                
                //Request state
                serData = SendCommand(serData, 1);

                state = Convert.ToBoolean(serData[1]);

                //Invert it if necessary
                if (InvertHome) state = !state;

                return state;
            }
        }

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                //If we think we're connected, verify that we really are
                if (connectedState)
                {
                    try
                    {
                        byte[] serData;
                        serData = new byte[1];

                        serData[0] = 0x01;

                        SendCommand(serData, 2);
                    }
                    catch
                    {
                        connectedState = false;
                    }
                }
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";

                traceState = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(driverID, comPortProfileName, string.Empty, comPortDefault);

                TempCoeff = Convert.ToInt16(driverProfile.GetValue(driverID, TempCoeffProfileName, string.Empty, TempCoeffDefault));
                TempCompEnabled = Convert.ToBoolean(driverProfile.GetValue(driverID, TempCompEnabledProfileName, string.Empty, TempCompEnabledDefault));

                MaxVel = Convert.ToDouble(driverProfile.GetValue(driverID, MaxVelProfileName, string.Empty, MaxVelDefault));
                Accel = Convert.ToDouble(driverProfile.GetValue(driverID, AccelProfileName, string.Empty, AccelDefault));
                IdleOff = Convert.ToBoolean(driverProfile.GetValue(driverID, IdleOffProfileName, string.Empty, IdleOffDefault));
                Reverse = Convert.ToBoolean(driverProfile.GetValue(driverID, ReverseProfileName, string.Empty, ReverseDefault));

                Home = Convert.ToBoolean(driverProfile.GetValue(driverID, HomeProfileName, string.Empty, HomeDefault));
                InvertHome = Convert.ToBoolean(driverProfile.GetValue(driverID, InvertHomeProfileName, string.Empty, InvertHomeDefault));
                HomeDirection = Convert.ToBoolean(driverProfile.GetValue(driverID, HomeDirectionProfileName, string.Empty, HomeDirectionDefault));
                HomeVel = Convert.ToDouble(driverProfile.GetValue(driverID, HomeVelProfileName, string.Empty, HomeVelDefault));
                HomeUseSwitch = Convert.ToBoolean(driverProfile.GetValue(driverID, HomeUseSwitchProfileName, string.Empty, HomeUseSwitchDefault));
                HomeDistance = Convert.ToInt32(driverProfile.GetValue(driverID, HomeDistanceProfileName, string.Empty, HomeDistanceDefault));
                HomePosition = Convert.ToInt32(driverProfile.GetValue(driverID, HomePositionProfileName, string.Empty, HomePositionDefault));
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";

                driverProfile.WriteValue(driverID, traceStateProfileName, traceState.ToString());
                driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString());

                driverProfile.WriteValue(driverID, TempCoeffProfileName, TempCoeff.ToString());
                driverProfile.WriteValue(driverID, TempCompEnabledProfileName, TempCompEnabled.ToString());

                driverProfile.WriteValue(driverID, MaxVelProfileName, MaxVel.ToString());
                driverProfile.WriteValue(driverID, AccelProfileName, Accel.ToString());
                driverProfile.WriteValue(driverID, IdleOffProfileName, IdleOff.ToString());
                driverProfile.WriteValue(driverID, ReverseProfileName, Reverse.ToString());

                driverProfile.WriteValue(driverID, HomeProfileName, Home.ToString());
                driverProfile.WriteValue(driverID, InvertHomeProfileName, InvertHome.ToString());
                driverProfile.WriteValue(driverID, HomeDirectionProfileName, HomeDirection.ToString());
                driverProfile.WriteValue(driverID, HomeVelProfileName, HomeVel.ToString());
                driverProfile.WriteValue(driverID, HomeUseSwitchProfileName, HomeUseSwitch.ToString());
                driverProfile.WriteValue(driverID, HomeDistanceProfileName, HomeDistance.ToString());
                driverProfile.WriteValue(driverID, HomePositionProfileName, HomePosition.ToString());
            }
        }

        #endregion

    }
}
