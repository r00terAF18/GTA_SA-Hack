using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;

namespace GTA_SA_Hack
{
    public partial class Form1 : Form
    {

        /*
         "gta-sa.exe"+007FC7D8
          Offset = 540
        */

        // get proccess name
        static string procName = "gta-sa";

        // playerAddress & Offsets
        static int playerAddress = 0x007FC7D8;
        static int playerHealthOffset = 0x540;
        static int moneyPointer = 0x530;
        static int moneyAddress = 0x00179FA0;

        // initilize Values
        VAMemory vam;
        int baseAddress;
        int playerBaseAddress;
        int playerHealthAddress;
        int moneyBaseAddress;
        int money;

        bool gameOn;

        public Form1()
        {
            InitializeComponent();
        }

        private void checkGame()
        {
           Process[] proces = Process.GetProcessesByName(procName);
            if (proces.Length > 0)
            {
                gameOn = true;
            }
            else
            {
                gameOn = false;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                checkGame();
                if (gameOn)
                {
                    // start memory object and calculate values
                    vam = new VAMemory(procName); // initilize a Memory reader object
                    baseAddress = (int)Process.GetProcessesByName(procName)[0].MainModule.BaseAddress; // get the Base Address, which we use to get all the other addresses
                    playerBaseAddress = vam.ReadInt32((IntPtr)baseAddress + playerAddress); // the player base address is basically the starting point
                    playerHealthAddress = playerBaseAddress + playerHealthOffset; // the player health address is calculated by adding the health offset to the player Base Address

                    //check if any checkbox is checked
                    if (godMode.Checked == true)
                    {
                        vam.WriteFloat((IntPtr)playerHealthAddress, 100); // write to the memory the value 100, so in another words 'unlimited health'
                    }
                }
                else
                {

                }
            }
            catch (Exception)
            {
                throw;
            }
            GC.Collect();
        }

        private void BtnAddMoney_Click(object sender, EventArgs e)
        {
            if (gameOn)
            {
                vam = new VAMemory(procName);
                baseAddress = (int)Process.GetProcessesByName(procName)[0].MainModule.BaseAddress;
                moneyBaseAddress = vam.ReadInt32((IntPtr)baseAddress + moneyAddress);
                money = moneyBaseAddress + moneyPointer;
                vam.WriteInt32((IntPtr)money, 900000); // this just overwrites the old value, it does not add up
            }
            else
            {

            }
        }
    }
}
