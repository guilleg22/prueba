using MultiCAT6.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funciones
{
    internal class Decoder048
    {

        public List<string> Item130(List<byte> bytes130)
        {
            List<string> decoded130 = new List<string>();
            byte bytePrincipal = bytes130[0];
            if ((bytePrincipal & 0b10000000) != 0)
            {
                double num = bytes130[1] * (360/(Math.Pow(2,13)));
                decoded130.Add(Convert.ToString(Math.Round(num, 3)) + " dg");
                bytes130.RemoveAt(1);
            }
            else { decoded130.Add("N/A"); }
            if ((bytePrincipal & 0b01000000) != 0)
            {
                decoded130.Add(Convert.ToString(bytes130[1]));
                bytes130.RemoveAt(1);
            }
            else { decoded130.Add("N/A"); }
            if ((bytePrincipal & 0b00100000) != 0)
            {
                int num = bytes130[1];
                if ((num & 0b10000000) != 0) { num = num - 0b100000000; }
                decoded130.Add(Convert.ToString(num) + " dBm");
                bytes130.RemoveAt(1);
            }
            else { decoded130.Add("N/A"); }
            if ((bytePrincipal & 0b00010000) != 0)
            {
                double num = bytes130[1] * (360 / (Math.Pow(2, 13)));
                decoded130.Add(Convert.ToString(Math.Round(num, 3)) + " dg");
                bytes130.RemoveAt(1);
            }
            else { decoded130.Add("N/A"); }
            if ((bytePrincipal & 0b00001000) != 0)
            {
                int num = bytes130[1];
                if ((num & 0b10000000) != 0) { num = num - 0b100000000; }
                decoded130.Add(Convert.ToString(num) + " dBm");
                bytes130.RemoveAt(1);
            }
            else { decoded130.Add("N/A"); }
            if ((bytePrincipal & 0b00000100) != 0)
            {
                int num = bytes130[1];
                if ((num & 0b10000000) != 0) { num = num - 0b100000000; }
                decoded130.Add(Convert.ToString(Math.Round(Convert.ToDouble(num)/256, 3) + " NM"));
                bytes130.RemoveAt(1);
            }
            else { decoded130.Add("N/A"); }
            if ((bytePrincipal & 0b00000010) != 0)
            {
                int num = bytes130[1];
                if ((num & 0b10000000) != 0) { num = num - 0b100000000; }
                double num2 = Convert.ToDouble(num) * (360 / Math.Pow(2, 14));
                decoded130.Add(Convert.ToString(Math.Round(num2, 3)) + " dg");
                bytes130.RemoveAt(1);
            }
            else { decoded130.Add("N/A"); }
            return decoded130;
        }

        public List<string> Item020(List<byte> bytes020)
        {
            // Hay 17 strings
            List<string> Report = new List<string>();

            byte firstOctet = bytes020[0];

            byte TYP = (byte)(firstOctet & 0b11100000);

            switch (TYP)
            {
                case 0b00000000:
                    Report.Add("No detection");
                    break;
                case 0b00100000:
                    Report.Add("Single PSR detection");
                    break;
                case 0b01000000:
                    Report.Add("Single SSR detection");
                    break;
                case 0b01100000:
                    Report.Add("SSR+PSR detection");
                    break;
                case 0b10000000:
                    Report.Add("Single ModeS All-Call");
                    break;
                case 0b10100000:
                    Report.Add("Single ModeS Roll-Call");
                    break;
                case 0b11000000:
                    Report.Add("ModeS All-Call+PSR");
                    break;
                case 0b11100000:
                    Report.Add("ModeS Roll-Call + PSR");
                    break;
            }

            byte SIM = (byte)(firstOctet & 0b00010000);

            switch (SIM)
            {
                case 0b0000:
                    Report.Add("Actual target report");
                    break;
                case 0b0001:
                    Report.Add("Simulated target report");
                    break;
            }

            byte RDP = (byte)(firstOctet & 0b00001000);

            switch (RDP)
            {
                case 0b00000:
                    Report.Add("Rerport form RDP Chain 1");
                    break;
                case 0b00001:
                    Report.Add("Report from RDP Chain 2");
                    break;
            }

            byte SPI = (byte)(firstOctet & 0b00000100);

            switch (SPI)
            {
                case 0b000000:
                    Report.Add("Absence of SPI");
                    break;
                case 0b000001:
                    Report.Add("Special Position Ientification");
                    break;
            }

            byte RAB = (byte)(firstOctet & 0b00000010);

            switch (RAB)
            {
                case 0b0000000:
                    Report.Add("Report from aircraft transponder");
                    break;
                case 0b0000001:
                    Report.Add("Report from field monitor(fixed transponder");
                    break;
            }

            byte FX = (byte)(firstOctet & 0b00000001);

            switch (FX)
            {
                case 0b00000000:
                    for (int i = 0; i <= 12; i++)
                    {
                        Report.Add("N/A");
                    }
                        break;
                case 0b00000001:
                    byte SecondOctet = bytes020[1];

                    byte TST = (byte)(SecondOctet & 0b10000000);

                    switch (TST)
                    {
                        case 0b0:
                            Report.Add("Real target report");
                            break;
                        case 0b1:
                            Report.Add("Test target report");
                            break;
                    }

                    byte ERR = (byte)(SecondOctet & 0b01000000);

                    switch (ERR)
                    {
                        case 0b00:
                            Report.Add("No Extended Range");
                            break;
                        case 0b01:
                            Report.Add("Extended Range present");
                            break;
                    }

                    byte XPP = (byte)(SecondOctet & 0b00100000);

                    switch (XPP)
                    {
                        case 0b000:
                            Report.Add("No X-Pulse Present");
                            break;
                        case 0b001:
                            Report.Add("X-Pulse Present");
                            break;
                    }




                    byte ME = (byte)(SecondOctet & 0b00010000);

                    switch (ME)
                    {
                        case 0b0000:
                            Report.Add("No military emergency");
                            break;
                        case 0b0001:
                            Report.Add("Military emergency");
                            break;
                    }

                    byte MI = (byte)(SecondOctet & 0b00001000);

                    switch (MI)
                    {
                        case 0b00000:
                            Report.Add("No military identification");
                            break;
                        case 0b00001:
                            Report.Add("Military identification");
                            break;
                    }

                    byte FOE = (byte)(SecondOctet & 0b00000110);

                    switch (FOE)
                    {
                        case 0b0000000:
                            Report.Add("No Mode4 interrogation");
                            break;
                        case 0b0000001:
                            Report.Add("Friendy target");
                            break;
                        case 0b0000010:
                            Report.Add("Unknown Target");
                            break;
                        case 0b0000011:
                            Report.Add("No reply");
                            break;
                    }


                    byte FX2 = (byte)(SecondOctet & 0b00000001);

                    switch (FX2)
                    {
                        case 0b00000000:
                            for (int i = 0; i <= 6; i++)
                            {
                                Report.Add("N/A");
                            }
                            break;
                        case 0b00000001:
                            byte ThirdOctet = bytes020[2];

                            byte ADSB_EP = (byte)(ThirdOctet & 0b10000000);

                            switch (ADSB_EP)
                            {
                                case 0b0:
                                    Report.Add("ADSB not populated");
                                    break;
                                case 0b1:
                                    Report.Add("ADSB populated");
                                    break;
                            }

                            byte ADSB_VAL = (byte)(ThirdOctet & 0b01000000);

                            switch (ADSB_VAL)
                            {
                                case 0b00:
                                    Report.Add("ADSB not available");
                                    break;
                                case 0b01:
                                    Report.Add("ADSB available");
                                    break;
                            }

                            byte SCN_EP = (byte)(ThirdOctet & 0b00100000);

                            switch (SCN_EP)
                            {
                                case 0b000:
                                    Report.Add("SCN not populated");
                                    break;
                                case 0b001:
                                    Report.Add("SCN populated");
                                    break;
                            }

                            byte SCN_VAL = (byte)(ThirdOctet & 0b00010000);

                            switch (SCN_VAL)
                            {
                                case 0b0000:
                                    Report.Add("SCN not available");
                                    break;
                                case 0b0001:
                                    Report.Add("SCN available");
                                    break;
                            }

                            byte PAI_EP = (byte)(ThirdOctet & 0b00001000);

                            switch (PAI_EP)
                            {
                                case 0b00000:
                                    Report.Add("PAI not populated");
                                    break;
                                case 0b00001:
                                    Report.Add("PAI populated");
                                    break;
                            }

                            byte PAI_VAL = (byte)(ThirdOctet & 0b00000100);

                            switch (PAI_VAL)
                            {
                                case 0b000000:
                                    Report.Add("PAI not available");
                                    break;
                                case 0b000001:
                                    Report.Add("PAI available");
                                    break;
                            }

                            byte SPARE = (byte)(ThirdOctet & 0b00000010);

                            switch (SPARE)
                            {
                                default:
                                    break;
                            }
                            break;
                    }
                    break;
            }
            return Report;
        }

        public List<string> Item040(List<byte> bytes040)
        {
            List<string> PolarCoordinates = new List<string>();

            int rangeval = (ushort)((bytes040[0] << 8) | bytes040[1]);
            double Range = Convert.ToDouble(rangeval) / 256;
            PolarCoordinates.Add(Convert.ToString(Math.Round(Range, 6)));

            int Angleval = (ushort)((bytes040[2] << 8) | bytes040[3]);
            double Angle = Angleval * 360 / (Math.Pow(2, 16));
            PolarCoordinates.Add(Convert.ToString(Math.Round(Angle, 6)));

            return PolarCoordinates;
        }

        public List<string> Item070(List<byte> bytes070)
        {
            List<string> Mode3Code = new List<string>();

            byte firstOctet = bytes070[0];

            byte V = (byte)(firstOctet & 0b10000000);

            switch (V)
            {
                case 0b00000000:
                    Mode3Code.Add("Code validated");
                    break;
                case 0b10000000:
                    Mode3Code.Add("Code not validated");
                    break;
            }
            byte G = (byte)(firstOctet & 0b01000000);

            switch (G)
            {
                case 0b00000000:
                    Mode3Code.Add("default");
                    break;
                case 0b01000000:
                    Mode3Code.Add("Garbled");
                    break;
            }
            byte L = (byte)(firstOctet & 0b00100000);

            switch (L)
            {
                case 0b00000000:
                    Mode3Code.Add("Mode-3/A code derived from the reply of the transponder");
                    break;

                case 0b00100000:
                    Mode3Code.Add("Mode - 3 / A code not extracted during the lastscan");
                    break;
            }

            // Combinamos los dos bytes en un ushort
            ushort combined = (ushort)((bytes070[0] << 8) | bytes070[1]);

            // Obtenemos los últimos 12 bits
            ushort last12Bits = (ushort)(combined & 0x0FFF); // 0x0FFF es la máscara para los últimos 12 bits

            // Convertimos a octal (base 8)
            string octalRepresentation = Convert.ToString(last12Bits, 8).PadLeft(4, '0');
            Mode3Code.Add(octalRepresentation);
            return Mode3Code;
        }

        public List<string> Item042(List<byte> bytes042)
        {
            List<string> CartesianCoordinates = new List<string>();

            int Xvalue = (short)((bytes042[0] << 8) | bytes042[1]);
            double X = Convert.ToDouble(Xvalue) / 128;
            CartesianCoordinates.Add(Convert.ToString(X));

            int Yvalue = (short)((bytes042[2] << 8) | bytes042[3]);
            double Y = Convert.ToDouble(Yvalue) / 128;
            CartesianCoordinates.Add(Convert.ToString(Y));
            return CartesianCoordinates;
        }

        public string Item110(List<byte> bytes110)
        {
            //1 string
            ushort combined = (ushort)((bytes110[0] << 8) | bytes110[1]);
            ushort last14Bits = (ushort)(combined & 0x3FFF);
            string Hight3D = Convert.ToString(last14Bits, 8);
            return Hight3D;
        }

        public string Item220(List<byte> bytes220)
        {
            int aircraftAddress = (bytes220[0] << 16) | (bytes220[1] << 8) | bytes220[2];
            string aircraftAdd = aircraftAddress.ToString("X6").PadLeft(4, '0');
            return aircraftAdd;
        }

        public List<string> Item250(List<byte> bytes250)
        {
            int REP = bytes250[0];
            List<List<byte>> mensaje = new List<List<byte>>();
            List<string> decoded250 = new List<string>();
            List<string> menDecoded = new List<string>();
            bytes250.RemoveAt(0);
            for (int ind = 0; ind < REP; ind++)
            {
                mensaje.Add(bytes250.GetRange(0, 8));
                bytes250.RemoveRange(0, 8);
            }
            StringBuilder sr = new StringBuilder();
            bool executed40 = false;
            bool executed50 = false;
            bool executed60 = false;
            List<string> mess40 = new List<string>();
            List<string> mess50 = new List<string>();
            List<string> mess60 = new List<string>();
            foreach (List<byte> bytes_ModeS in mensaje)
            {
                byte id = bytes_ModeS[bytes_ModeS.Count - 1];
                int id1 = id >> 4;
                int id2 = id & 0b00001111;
                sr.AppendLine("BDS: " + Convert.ToString(id1) + "," + Convert.ToString(id2));
                bytes_ModeS.RemoveAt(bytes_ModeS.Count - 1);

                if (!executed40 && id1 == 4 && id2 == 0)
                {
                    mess40.AddRange(modeS40(bytes_ModeS));
                    executed40 = true;
                    continue;
                }
                if (!executed50 && id1 == 5 && id2 == 0)
                {
                    mess50.AddRange(modeS50(bytes_ModeS));
                    executed50 = true;
                    continue;
                }
                if (!executed60 && id1 == 6 && id2 == 0)
                {
                    mess60.AddRange(modeS60(bytes_ModeS));
                    executed60 = true;
                    continue;
                }
            }
            if (mess40.Count == 0) { mess40.AddRange(Enumerable.Repeat("N/A", 12)); }
            if (mess50.Count == 0) { mess50.AddRange(Enumerable.Repeat("N/A", 10)); }
            if (mess60.Count == 0) { mess60.AddRange(Enumerable.Repeat("N/A", 10)); }
            menDecoded.AddRange(mess40);
            menDecoded.AddRange(mess50);
            menDecoded.AddRange(mess60);

            decoded250.Add(sr.ToString().TrimEnd('\r', '\n'));
            decoded250.AddRange(menDecoded);
            return decoded250;
        }

        public List<string> modeS40(List<byte> bytes_ModeS)
        {
            List<string> decoded40S = new List<string>();
            ulong data = 0;
            for (int i = 0; i < 7; i++)
            {
                data = (data << 8) | bytes_ModeS[i]; 
            }

            // Decodificar MCP_STATUS (bit 1)
            bool MCP_STATUS = ((data >> 55) & 0b1) == 1;
            decoded40S.Add(MCP_STATUS ? "1" : "0");

            // Decodificar MCP_ALT (bits 2-13)
            int MCP_ALT = (int)((data >> 43) & 0xFFF); 
            MCP_ALT *= 16; 
            decoded40S.Add(MCP_ALT.ToString());

            // Decodificar FMS_STATUS (bit 14)
            bool FMS_STATUS = ((data >> 42) & 0b1) == 1;
            decoded40S.Add(FMS_STATUS ? "1" : "0");

            // Decodificar FMS_ALT (bits 15-26)
            int FMS_ALT = (int)((data >> 30) & 0xFFF); 
            FMS_ALT *= 16; 
            decoded40S.Add(FMS_ALT.ToString());

            // Decodificar BP_STATUS (bit 27)
            bool BP_STATUS = ((data >> 29) & 0b1) == 1;
            decoded40S.Add(BP_STATUS ? "1" : "0");

            // Decodificar BP (bits 28-39)
            int BP = (int)((data >> 17) & 0xFFF);
            double BP_value = BP * 0.1 + 800.0;
                                                
            if (BP_value < 800 || BP_value > 1209.5)
            {
                BP_STATUS = false; // Invalidar BP_STATUS
            }
            decoded40S.Add(BP_value.ToString("F1"));

            // Decodificar MODE_STATUS (bit 48)
            int MODE_STATUS = (int)((data >> 8) & 0b1); 
            decoded40S.Add(MODE_STATUS == 1 ? "Mode information deliberately provided" : "No mode information provided");

            // Decodificar VNAV (bit 49)
            bool VNAV = ((data >> 7) & 0b1) == 1;
            decoded40S.Add(VNAV ? "1" : "0");

            // Decodificar ALTHOLD (bit 50)
            bool ALTHOLD = ((data >> 6) & 0b1) == 1;
            decoded40S.Add(ALTHOLD ? "1" : "0");

            // Decodificar APP (bit 51)
            bool APP = ((data >> 5) & 0b1) == 1;
            decoded40S.Add(APP ? "1" : "0");

            // Decodificar TARGETALT_STATUS (bit 54)
            bool TARGETALT_STATUS = ((data >> 2) & 0b1) == 1;
            decoded40S.Add(TARGETALT_STATUS ? "1" : "0");

            // Decodificar TARGETALT_SOURCE (bits 55-56)
            int TARGETALT_SOURCE = (int)(data & 0b11); 
            string targetAltSourceString;
            switch (TARGETALT_SOURCE)
            {
                case 0b00:
                    targetAltSourceString = "Unknown";
                    break;
                case 0b01:
                    targetAltSourceString = "Aircraft altitude";
                    break;
                case 0b10:
                    targetAltSourceString = "FCU/MCP selected altitude";
                    break;
                case 0b11:
                    targetAltSourceString = "FMS selected altitude";
                    break;
                default:
                    targetAltSourceString = "Unknown";
                    break;
            }
            decoded40S.Add(targetAltSourceString);

            return decoded40S;
        }

        public List<string> modeS50(List<byte> bytes_ModeS)
        {

            List<string> decoded50S = new List<string>();

            ulong data = 0;
            for (int i = 0; i < 7; i++)
            {
                data = (data << 8) | bytes_ModeS[i]; 
            }

            // Decodificar el bit 1
            bool bit1 = ((data >> 55) & 0b1) == 1;
            decoded50S.Add(bit1 ? "1" : "0");

            // Decodificar ROLL ANGLE (bits 2-11)
            bool rollSign = ((data >> 54) & 0b1) == 1; 
            int rollAngleBits = (int)((data >> 45) & 0x1FF); 
            double rollAngle = rollAngleBits * (45.0 / 256.0);
            if (rollSign) { rollAngle = rollAngle - 90; }
            decoded50S.Add(Math.Round(rollAngle, 3).ToString());

            // Decodificar el bit 12
            bool bit12 = ((data >> 44) & 0b1) == 1;
            decoded50S.Add(bit12 ? "1" : "0");

            // Decodificar TRUE TRACK ANGLE (bits 13-23)
            bool trackSign = ((data >> 43) & 0b1) == 1; 
            int trackAngleBits = (int)((data >> 33) & 0x3FF);
            double trueTrackAngle = trackAngleBits * (90.0 / 512.0);
            if (trackSign) { trueTrackAngle = trueTrackAngle - 180; }
            decoded50S.Add(Math.Round(trueTrackAngle, 3).ToString());

            // Decodificar el bit 24
            bool bit24 = ((data >> 32) & 0b1) == 1;
            decoded50S.Add(bit24 ? "1" : "0");

            // Decodificar GROUND SPEED (bits 25-34)
            int groundSpeedBits = (int)((data >> 22) & 0x3FF); 
            double groundSpeed = groundSpeedBits * (1024.0 / 512.0); 
            decoded50S.Add(Math.Round(groundSpeed).ToString());

            // Decodificar el bit 35
            bool bit35 = ((data >> 21) & 0b1) == 1;
            decoded50S.Add(bit35 ? "1" : "0");

            // Decodificar TRACK ANGLE RATE (bits 36-45)
            bool trackRateSign = ((data >> 20) & 0b1) == 1;
            int trackRateBits = (int)((data >> 11) & 0x1FF); 
            double trackAngleRate = trackRateBits * (8.0 / 256.0);
            if (trackRateSign) { trackAngleRate = trackAngleRate - 16; }
            decoded50S.Add(Math.Round(trackAngleRate, 3).ToString());

            // Decodificar el bit 46
            bool bit46 = ((data >> 10) & 0b1) == 1;
            decoded50S.Add(bit46 ? "1" : "0");

            // Decodificar TRUE AIRSPEED (bits 47-56)
            int trueAirspeedBits = (int)(data & 0x3FF); 
            double trueAirspeed = trueAirspeedBits * 2.0; 
            decoded50S.Add(Math.Round(trueAirspeed).ToString());

            return decoded50S;
        }

        public List<string> modeS60(List<byte> bytes_ModeS)
        {

            List<string> decodedS60 = new List<string>();

            ulong data = 0;
            for (int i = 0; i < 7; i++)
            {
                data = (data << 8) | bytes_ModeS[i]; 
            }

            bool bit1 = ((data >> 55) & 0b1) == 1;
            decodedS60.Add(bit1 ? "1" : "0");

            // Decodificar MAGNETIC HEADING 
            bool headingSign = ((data >> 54) & 0b1) == 1; 
            int magneticHeadingBits = (int)((data >> 44) & 0x3FF);
            double magneticHeading = magneticHeadingBits * (90.0 / 512.0); 
            if (headingSign) { magneticHeading = magneticHeading - 180; }
            decodedS60.Add(Math.Round(magneticHeading, 6).ToString());

            // Decodificar el bit 13
            bool bit13 = ((data >> 43) & 0b1) == 1;
            decodedS60.Add(bit13 ? "1" : "0");

            // Decodificar INDICATED AIRSPEED 
            int indicatedAirspeedBits = (int)((data >> 33) & 0x3FF); 
            int indicatedAirspeed = indicatedAirspeedBits; 
            decodedS60.Add(indicatedAirspeed.ToString());

            // Decodificar el bit 24
            bool bit24 = ((data >> 32) & 0b1) == 1;
            decodedS60.Add(bit24 ? "1" : "0");

            // Decodificar MACH (bits 25-34)
            int machBits = (int)((data >> 22) & 0x3FF); 
            double mach = machBits * (2.048 / 512.0); 
            decodedS60.Add(Math.Round(mach, 3).ToString());

            // Decodificar el bit 35
            bool bit35 = ((data >> 21) & 0b1) == 1;
            decodedS60.Add(bit35 ? "1" : "0");

            // Decodificar BAROMETRIC ALTITUDE RATE (bits 36-45)
            bool barometricRateSign = ((data >> 20) & 0b1) == 1; 
            int barometricRateBits = (int)((data >> 11) & 0x1FF);
            double barometricAltitudeRate = barometricRateBits * (8192.0 / 256.0); 
            if (barometricRateSign) { barometricAltitudeRate = barometricAltitudeRate - 16384; }
            decodedS60.Add(Math.Round(barometricAltitudeRate).ToString());

            // Decodificar el bit 46
            bool bit46 = ((data >> 10) & 0b1) == 1;
            decodedS60.Add(bit46 ? "1" : "0");

            // Decodificar INERTIAL VERTICAL VELOCITY (bits 47-56)
            bool inertialVerticalVelocitySign = ((data >> 9) & 0b1) == 1; 
            int inertialVerticalVelocityBits = (int)(data & 0x1FF); 
            double inertialVerticalVelocity = inertialVerticalVelocityBits * (8192.0 / 256.0);
            if (inertialVerticalVelocitySign) { inertialVerticalVelocity = inertialVerticalVelocity - 16384; }
            decodedS60.Add(Math.Round(inertialVerticalVelocity).ToString());

            return decodedS60;
        }

        public List<string> Item010(List<byte> bytes010) //[int SIC, int SAC]
        {
            List<string> decoded010 = new List<string>
            {
                Convert.ToString(bytes010[0]),
                Convert.ToString(bytes010[1])
            };
            return decoded010;
        }

        public List<string> Item090(List<byte> bytes090) //[str V, str G, int FL]

        {
            List<string> decoded090 = new List<string>();
            byte mascaraV = 0b10000000;
            byte mascaraG = 0b01000000;

            switch ((mascaraV & bytes090[0]) == 0)
            {
                case true:
                    string VT = "Code validated";
                    decoded090.Add(VT);
                    break;
                case false:
                    string VF = "Code not validated";
                    decoded090.Add(VF);
                    break;
            }

            switch ((mascaraG & bytes090[0]) == 0)
            {
                case true:
                    string GT = "Default";
                    decoded090.Add(GT);
                    break;
                case false:
                    string GF = "Garbled code";
                    decoded090.Add(GF);
                    break;
            }
            ;
            int last_14_bits = ((bytes090[0] & 0b00111111) << 8) | bytes090[1];

            if ((last_14_bits & 0x2000) != 0) 
            {
                last_14_bits = last_14_bits - 0x4000; // Restamos 2^14 para obtener el valor negativo correcto
            }
            double FL = last_14_bits * 1.0 / 4;
            decoded090.Add(Convert.ToString(FL));

            return decoded090;
        }

        public string Item140(List<byte> bytes140) // [str TimeOfDay]
        {
            int FirstOctet = bytes140[0] << 16;
            int SecondOctet = bytes140[1] << 8;
            int OriginValue = FirstOctet | SecondOctet | Convert.ToInt32(bytes140[2]);
            double ScaledValue = OriginValue * Math.Pow(2, -7);
            if (ScaledValue > 86400)
            {
                string TimeOfDay = "N/A";
                return TimeOfDay;
            }
            else
            {
                double segundos = ScaledValue;// por ejemplo, 1 hora, 1 segundo y 7.8125 ms

                // Convierte a TimeSpan, considerando hasta 7 decimales
                TimeSpan tiempo = TimeSpan.FromSeconds(segundos);

                // Obtén cada componente del tiempo
                int horas = tiempo.Hours;
                int minutos = tiempo.Minutes;
                int segundosEnteros = tiempo.Seconds;

                // Calcular milisegundos (hasta 3 decimales) en función del resto fraccionario de segundos
                int milisegundos = (int)(tiempo.Milliseconds);

                // Formato hh:mm:ss:ms con 3 dígitos para milisegundos
                string TimeOfDay = $"{horas:D2}:{minutos:D2}:{segundosEnteros:D2}:{milisegundos:D3}";

                return TimeOfDay;
            }

        }

        public string Item161(List<byte> bytes161) // [int TrackNumber]
        {
            int TrackNumber = (bytes161[0] << 8) | bytes161[1];
            return Convert.ToString(TrackNumber);
        }

        public List<string> Item170(List<byte> bytes170) // [str CNF, str RAD, str DOU, str MAH, str CDM, str TRE, str GHO, str SUP, str TCC]
        {
            List<string> decoded170 = new List<string>();
            byte mascaraCNF = 0b10000000;
            byte mascaraRAD = 0b01100000;
            byte mascaraDOU = 0b00010000;
            byte mascaraMAH = 0b00001000;
            byte mascaraCDM = 0b00000110;
            byte mascaraFX1 = 0b00000001;

            switch ((bytes170[0] & mascaraCNF) == 0)
            {
                case true:
                    string CNF = "Confirmed Track";
                    decoded170.Add(CNF);
                    break;

                case false:
                    string CNFf = "Tentative Track";
                    decoded170.Add(CNFf);
                    break;
            }

            switch ((bytes170[0] & mascaraRAD) >> 4)
            {
                case 0:
                    string RAD = "Combined Track";
                    decoded170.Add(RAD);
                    break;

                case 1:
                    string RAD1 = "PSR Track";
                    decoded170.Add(RAD1);
                    break;

                case 2:
                    string RAD2 = "SSR/Mode S Track";
                    decoded170.Add(RAD2);
                    break;

                case 3:
                    string RAD3 = "N/A";
                    decoded170.Add(RAD3);
                    break;
            }

            switch ((bytes170[0] & mascaraDOU) == 0)
            {
                case true:
                    string DOU = "Normal confidence";
                    decoded170.Add(DOU);
                    break;

                case false:
                    string DOUf = "Low confidence in plot to track association";
                    decoded170.Add(DOUf);
                    break;
            }

            switch ((bytes170[0] & mascaraMAH) == 0)
            {
                case true:
                    string MAH = "No horizontal man. sensed";
                    decoded170.Add(MAH);
                    break;

                case false:
                    string MAHf = "Horizontal man. sensed";
                    decoded170.Add(MAHf);
                    break;
            }

            switch ((bytes170[0] & mascaraCDM) >> 1)
            {
                case 0:
                    string CDM = "Maintaining";
                    decoded170.Add(CDM);
                    break;

                case 1:
                    string CDM1 = "Climbing";
                    decoded170.Add(CDM1);
                    break;

                case 2:
                    string CDM2 = "Descending";
                    decoded170.Add(CDM2);
                    break;

                case 3:
                    string CDM3 = "Unknown";
                    decoded170.Add(CDM3);
                    break;
            }
            int dec = 0;
            try
            {

                switch ((bytes170[0] & mascaraFX1) == 0)
                {
                    case true:
                        dec = 0;
                        break;

                    case false:
                        dec = 1;
                        break;
                }

                if (dec != 0)
                {
                    byte mascaraTRE = 0b10000000;
                    byte mascaraGHO = 0b01000000;
                    byte mascaraSUP = 0b00100000;
                    byte mascaraTCC = 0b00010000;

                    switch ((bytes170[1] & mascaraTRE) == 0)
                    {
                        case true:
                            string TRE = "Track still alive";
                            decoded170.Add(TRE);
                            break;

                        case false:
                            string TREf = "End of track lifetime";
                            decoded170.Add(TREf);
                            break;
                    }

                    switch ((bytes170[1] & mascaraGHO) == 0)
                    {
                        case true:
                            string GHO = "True target track";
                            decoded170.Add(GHO);
                            break;

                        case false:
                            string GHOf = "Ghost target track";
                            decoded170.Add(GHOf);
                            break;
                    }


                    switch ((bytes170[1] & mascaraSUP) == 0)
                    {
                        case true:
                            string SUP = "no";
                            decoded170.Add(SUP);
                            break;

                        case false:
                            string SUPf = "yes";
                            decoded170.Add(SUPf);
                            break;
                    }

                    switch ((bytes170[1] & mascaraTCC) == 0)
                    {
                        case true:
                            string TCC = "Tracking performed in so-called 'Radar Plane', i.e. neither slant range correction nor stereographical projection was applied.";
                            decoded170.Add(TCC);
                            break;

                        case false:
                            string TCCf = "Slant range correction and a suitable projection technique are used to track in a 2D.reference plane, tangential to the earth model at the Radar Site co-ordinates.";
                            decoded170.Add(TCCf);
                            break;
                    }

                }

                else
                {
                    string NoData = "N/A";
                    int i = 0;
                    while (i < 5)
                    {
                        decoded170.Add(NoData);
                        i = i + 1;
                    }
                }
            }
            catch
            {
                string NoData = "N/A";
                decoded170.Add(NoData);
                decoded170.Add(NoData);
                decoded170.Add(NoData);
                decoded170.Add(NoData);
                return decoded170;

            }
            return decoded170;
        }

        public List<string> Item200(List<byte> bytes200) // [int GroundSpeed, int Heading]
        {
            List<string> decoded200 = new List<string>();
            double OriginValueGS = (bytes200[0] << 8) | bytes200[1];
            double OriginValueH = (bytes200[2] << 8) | bytes200[3];
            double ScaledValueGS = OriginValueGS * Math.Pow(2, -14);
            if (ScaledValueGS > 2)
            {
                ScaledValueGS = 2;
            }
            double ScaledValueH = OriginValueH * 360 * Math.Pow(2, -16);
            decoded200.Add(Convert.ToString(Convert.ToSingle(Convert.ToDouble(3600) * ScaledValueGS)));
            decoded200.Add(Convert.ToString(Convert.ToSingle(ScaledValueH)));
            return decoded200;
        }

        public List<string> Item230(List<byte> bytes230)
        {
            List<string> decoded230 = new List<string>();

            byte firstByte = bytes230[0];
            int COM = (firstByte & 0b11100000) >> 5;
            switch (COM)
            {
                case 0:
                    decoded230.Add("No communications capability (surveillance only)");
                    break;
                case 1:
                    decoded230.Add("Comm. A and Comm. B capability");
                    break;
                case 2:
                    decoded230.Add("Comm. A, Comm. B and Uplink ELM");
                    break;
                case 3:
                    decoded230.Add("Comm. A, Comm. B, Uplink ELM and Downlink ELM");
                    break;
                case 4:
                    decoded230.Add("Level 5 Transponder capability");
                    break;
                default:
                    decoded230.Add("N/a");
                    break;
            }

            int nextThreeBits = (firstByte & 0b00011100) >> 2;

            switch (nextThreeBits)
            {
                case 0:
                    decoded230.Add("No alert, no SPI, aircraft airborne");
                    break;
                case 1:
                    decoded230.Add("No alert, no SPI, aircraft on ground");
                    break;
                case 2:
                    decoded230.Add("Alert, no SPI, aircraft airborne");
                    break;
                case 3:
                    decoded230.Add("Alert, no SPI, aircraft on ground");
                    break;
                case 4:
                    decoded230.Add("Alert, SPI, aircraft airborne or on ground");
                    break;
                case 5:
                    decoded230.Add("No alert, SPI, aircraft airborne or on ground");
                    break;
                case 6:
                    decoded230.Add("Not assigned");
                    break;
                case 7:
                    decoded230.Add("Unknown");
                    break;
                default:
                    decoded230.Add("N/A");
                    break;
            }

            int nextBit = (firstByte & 0b00000010) >> 1; ;

            if (nextBit == 0)
            {
                decoded230.Add("SI-Code Capable");
            }
            else
            {
                decoded230.Add("II-Code Capable");
            }

            byte SecondByte = bytes230[1];
            int MSSC = (SecondByte & 0b10000000) >> 7;
            
            if (MSSC == 0)
            {
                decoded230.Add("No");
            }
            else
            {
                decoded230.Add("Yes");
            }

            int ARC = (SecondByte & 0b01000000) >> 6;

            if (ARC == 0)
            {
                decoded230.Add("100 ft resolution");
            }
            else
            {
                decoded230.Add("25 ft resolution");
            }

            int AIC = (SecondByte & 0b00100000) >> 5;

            if (AIC == 0)
            {
                decoded230.Add("No");
            }
            else
            {
                decoded230.Add("Yes");
            }

            int B1A = (SecondByte & 0b00010000) >> 4;

            if (B1A == 1)
            {
                decoded230.Add("ACAS is operational");
            }
            else
            {
                decoded230.Add("ACAS has failed or is on standby");
            }

            int bit4 = (SecondByte & 0b00001000) >> 3;

            if (bit4 == 1)
            {
                decoded230.Add("Capability of hybrid surveillance");
            }
            else
            {
                decoded230.Add("no hybrid surveillance capability");
            }

            int bit3 = (SecondByte & 0b00000100) >> 2;

            if (bit3 == 1)
            {
                decoded230.Add("ACAS is generating TAs and RAs");
            }
            else
            {
                decoded230.Add("ACAS generation of TAs only");
            }
            int LastBits = (SecondByte & 0b00000011);
            
            switch (LastBits)
            {
                case 0:
                    decoded230.Add("RTCA DO-185");
                    break;
                case 1:
                    decoded230.Add("RTCA DO-185A");
                    break;
                case 2:
                    decoded230.Add("RTCA DO-185B");
                    break;
                case 3:
                    decoded230.Add("RTCA DO-185B");
                    break;
                case 4:
                    decoded230.Add("Reserved for future versions");
                    break;
                default:
                    decoded230.Add("N/a");
                    break;
            }

            return decoded230;
        }

        public string Item240(List<byte> bytes240) // [str ch1, str ch2, str ch3, str ch4, str ch5, str ch6, str ch7, str ch8]
        {
            List<string> decoded240 = new List<string>();

            // We take the 6 octets and convert them to 8 6 bits numbers

            int D1 = (bytes240[0] & 0b11111100) >> 2;
            int D2 = ((bytes240[0] & 0b00000011) << 4) | ((bytes240[1] & 0b11110000) >> 4);
            int D3 = ((bytes240[1] & 0b00001111) << 2) | ((bytes240[2] & 0b11000000) >> 6);
            int D4 = (bytes240[2] & 0b00111111);
            int D5 = (bytes240[3] & 0b11111100) >> 2;
            int D6 = ((bytes240[3] & 0b00000011) << 4) | ((bytes240[4] & 0b11110000) >> 4);
            int D7 = ((bytes240[4] & 0b00001111) << 2) | ((bytes240[5] & 0b11000000) >> 6);
            int D8 = (bytes240[5] & 0b00111111);

            List<int> DataList = new List<int>
            {
                D1,
                D2,
                D3,
                D4,
                D5,
                D6,
                D7,
                D8
            };

            int i = 0;
            while (i < 8)
            {
                switch (DataList[i])
                {


                    case 1:
                        string A = "A";
                        decoded240.Add(A);
                        break;

                    case 2:
                        string B = "B";
                        decoded240.Add(B);
                        break;

                    case 3:
                        string C = "C";
                        decoded240.Add(C);
                        break;

                    case 4:
                        string D = "D";
                        decoded240.Add(D);
                        break;

                    case 5:
                        string E = "E";
                        decoded240.Add(E);
                        break;

                    case 6:
                        string F = "F";
                        decoded240.Add(F);
                        break;

                    case 7:
                        string G = "G";
                        decoded240.Add(G);
                        break;

                    case 8:
                        string H = "H";
                        decoded240.Add(H);
                        break;

                    case 9:
                        string I = "I";
                        decoded240.Add(I);
                        break;

                    case 10:
                        string J = "J";
                        decoded240.Add(J);
                        break;

                    case 11:
                        string K = "K";
                        decoded240.Add(K);
                        break;

                    case 12:
                        string L = "L";
                        decoded240.Add(L);
                        break;

                    case 13:
                        string M = "M";
                        decoded240.Add(M);
                        break;

                    case 14:
                        string N = "N";
                        decoded240.Add(N);
                        break;

                    case 15:
                        string O = "O";
                        decoded240.Add(O);
                        break;

                    case 16:
                        string P = "P";
                        decoded240.Add(P);
                        break;


                    case 17:
                        string Q = "Q";
                        decoded240.Add(Q);
                        break;

                    case 18:
                        string R = "R";
                        decoded240.Add(R);
                        break;

                    case 19:
                        string S = "S";
                        decoded240.Add(S);
                        break;

                    case 20:
                        string T = "T";
                        decoded240.Add(T);
                        break;

                    case 21:
                        string U = "U";
                        decoded240.Add(U);
                        break;

                    case 22:
                        string V = "V";
                        decoded240.Add(V);
                        break;

                    case 23:
                        string W = "W";
                        decoded240.Add(W);
                        break;

                    case 24:
                        string X = "X";
                        decoded240.Add(X);
                        break;

                    case 25:
                        string Y = "Y";
                        decoded240.Add(Y);
                        break;

                    case 26:
                        string Z = "Z";
                        decoded240.Add(Z);
                        break;

                    case 32:
                        string Space = " ";
                        decoded240.Add(Space);
                        break;

                    case 48:
                        string zero = "0";
                        decoded240.Add(zero);
                        break;

                    case 49:
                        string one = "1";
                        decoded240.Add(one);
                        break;

                    case 50:
                        string two = "2";
                        decoded240.Add(two);
                        break;

                    case 51:
                        string three = "3";
                        decoded240.Add(three);
                        break;

                    case 52:
                        string four = "4";
                        decoded240.Add(four);
                        break;

                    case 53:
                        string five = "5";
                        decoded240.Add(five);
                        break;

                    case 54:
                        string six = "6";
                        decoded240.Add(six);
                        break;

                    case 55:
                        string seven = "7";
                        decoded240.Add(seven);
                        break;

                    case 56:
                        string eight = "8";
                        decoded240.Add(eight);
                        break;

                    case 57:
                        string nine = "9";
                        decoded240.Add(nine);
                        break;

                    default:
                        //string NoData = "";
                        //decoded240.Add(NoData);
                        break;

                }

                i = i + 1;
            }
            string decoded240a = string.Join("", decoded240);
            return decoded240a;

        }

        public List<string> GeodesicCoor(List<string> RadarSphericalCoor)
        {
            double Rho = Convert.ToDouble(RadarSphericalCoor[0]) * 1852;
            double Theta = Convert.ToDouble(RadarSphericalCoor[1]) * (Math.PI / 180);
            double Altura = Convert.ToDouble(RadarSphericalCoor[2]);
            if(Altura < 0)
            {
                Altura = 0;
            }

            GeoUtils geoUtils = new GeoUtils();
            // Coordenadas de la antena:
            double height = 25.25 + 2.007;
            string line = "41:18:02.5284N 02:06:07.4095E";
            CoordinatesWGS84 radarCoordinates = GeoUtils.LatLonStringBoth2Radians(line, height);

            double Elevation = GeoUtils.CalculateElevation(radarCoordinates, 6371000, Rho, Altura);
            CoordinatesPolar coordinatesSpherical = new CoordinatesPolar(Rho, Theta, Elevation);
            CoordinatesXYZ cartesianCoordinates = GeoUtils.change_radar_spherical2radar_cartesian(coordinatesSpherical);
            CoordinatesXYZ geocentricCoordinates = geoUtils.change_radar_cartesian2geocentric(radarCoordinates, cartesianCoordinates);
            CoordinatesWGS84 geodesicCoordinates = geoUtils.change_geocentric2geodesic(geocentricCoordinates);
            List<string> geodesicCoor = new List<string>();
            geodesicCoor.Add((geodesicCoordinates.Lat * 180 / Math.PI).ToString());
            geodesicCoor.Add((geodesicCoordinates.Lon * 180 / Math.PI).ToString());
            geodesicCoor.Add(geodesicCoordinates.Height.ToString());
            return geodesicCoor;
        }
    }
}


