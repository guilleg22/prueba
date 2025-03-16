using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Funciones
{
    public class divisorAsterix
    {
        public DataTable divisorMessajes(string AsterixPath)
        {
            //Creación DataTable
            Decoder048 decoder048 = new Decoder048();
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("NUM", typeof(string)),
                //010
                new DataColumn("SAC", typeof(string)),
                new DataColumn("SIC", typeof(string)),
                //140
                new DataColumn("Time", typeof(string)),
                //LLA
                new DataColumn("Latitud", typeof(string)),
                new DataColumn("Longitud", typeof(string)),
                new DataColumn("h", typeof(string)),
                //020
                new DataColumn("TYP020", typeof(string)),
                new DataColumn("SIM020", typeof(string)),
                new DataColumn("RDP020", typeof(string)),
                new DataColumn("SPI020", typeof(string)),
                new DataColumn("RAB020", typeof(string)),
                new DataColumn("TST020", typeof(string)),
                new DataColumn("ERR020", typeof(string)),
                new DataColumn("XPP020", typeof(string)),
                new DataColumn("ME020", typeof(string)),
                new DataColumn("MI020", typeof(string)),
                new DataColumn("FOEFRI_020", typeof(string)),
                new DataColumn("ADSB_EP020", typeof(string)),
                new DataColumn("ADSB_VAL020", typeof(string)),
                new DataColumn("SCN_EP020", typeof(string)),
                new DataColumn("SCN_VAL020", typeof(string)),
                new DataColumn("PAI_EP020", typeof(string)),
                new DataColumn("PAI_VAL020", typeof(string)),
                //040
                new DataColumn("RHO", typeof(string)),
                new DataColumn("THETA", typeof(string)),
                //070
                new DataColumn("V070", typeof(string)),
                new DataColumn("G070", typeof(string)),
                new DataColumn("L070", typeof(string)),
                new DataColumn("Mode_3A", typeof(string)),
                //090
                new DataColumn("V090", typeof(string)),
                new DataColumn("G090", typeof(string)),
                new DataColumn("Flight_Level", typeof(string)),
                //Mode C Corrected
                new DataColumn("ModeC_corrected", typeof(string)),
                //130
                new DataColumn("SRL130", typeof(string)),
                new DataColumn("SRR130", typeof(string)),
                new DataColumn("SAM130", typeof(string)),
                new DataColumn("PRL130", typeof(string)),
                new DataColumn("PAM130", typeof(string)),
                new DataColumn("RPD130", typeof(string)),
                new DataColumn("APD130", typeof(string)),
                //220
                new DataColumn("Target_address", typeof(string)),
                //240
                new DataColumn("Target_identification", typeof(string)),
                //250
                new DataColumn("Mode_S", typeof(string)),
                    // 250: 4,0: Selected vertical intention
                new DataColumn("MCP_STATUS", typeof(string)),
                new DataColumn("MCP_ALT", typeof(string)),
                new DataColumn("FMS_STATUS", typeof(string)),
                new DataColumn("FMS_ALT", typeof(string)),
                new DataColumn("BP_STATUS", typeof(string)),
                new DataColumn("BP", typeof(string)),
                new DataColumn("MODE_STATUS", typeof(string)),
                new DataColumn("VNAV", typeof(string)),
                new DataColumn("ALTHOLD", typeof(string)),
                new DataColumn("APP", typeof(string)),
                new DataColumn("TARGETALT_STATUS", typeof(string)),
                new DataColumn("TARGETALT_SOURCE", typeof(string)),
                    //250: 5,0: Track and turn report
                new DataColumn("RA_STATUS", typeof(string)),
                new DataColumn("RA", typeof(string)),
                new DataColumn("TTA_STATUS", typeof(string)),
                new DataColumn("TTA", typeof(string)),
                new DataColumn("GS_STATUS", typeof(string)),
                new DataColumn("GS", typeof(string)),
                new DataColumn("TAR_STATUS", typeof(string)),
                new DataColumn("TAR", typeof(string)),
                new DataColumn("TAS_STATUS", typeof(string)),
                new DataColumn("TAS", typeof(string)),
                    //250: 6,0: Heading and speed report
                new DataColumn("HDG_STATUS", typeof(string)),
                new DataColumn("HDG", typeof(string)),
                new DataColumn("IAS_STATUS", typeof(string)),
                new DataColumn("IAS", typeof(string)),
                new DataColumn("MACH_STATUS", typeof(string)),
                new DataColumn("MACH", typeof(string)),
                new DataColumn("BAR_STATUS", typeof(string)),
                new DataColumn("BAR", typeof(string)),
                new DataColumn("IVV_STATUS", typeof(string)),
                new DataColumn("IVV", typeof(string)),
                //161
                new DataColumn("Track_number", typeof(string)),
                //042
                new DataColumn("X_Component", typeof(string)),
                new DataColumn("Y_Component", typeof(string)),
                //200
                new DataColumn("Ground_Speedkt", typeof(string)),
                new DataColumn("Heading", typeof(string)),
                //170
                new DataColumn("CNF170", typeof(string)),
                new DataColumn("RAD170", typeof(string)),
                new DataColumn("DOU170", typeof(string)),
                new DataColumn("MAH170", typeof(string)),
                new DataColumn("CDM170", typeof(string)),
                new DataColumn("TRE170", typeof(string)),
                new DataColumn("GHO170", typeof(string)),
                new DataColumn("SUP170", typeof(string)),
                new DataColumn("TCC170", typeof(string)),
                //110
                new DataColumn("Measured_Height", typeof(string)),
                //230
                new DataColumn("COM230", typeof(string)),
                new DataColumn("STAT230", typeof(string)),
                new DataColumn("SI230", typeof(string)),
                new DataColumn("MSSC230", typeof(string)),
                new DataColumn("ARC230", typeof(string)),
                new DataColumn("AIC230", typeof(string)),
                new DataColumn("B1A230", typeof(string)),
                new DataColumn("Surveillance_Capability", typeof(string)),
                new DataColumn("ACAS_generation", typeof(string)),
                new DataColumn("RTCA_version", typeof(string))
            });

            FileStream fs = new FileStream(AsterixPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            int NUM = 1;
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                DataRow nuevaFila = dataTable.NewRow();

                byte nextByte = br.ReadByte();

                byte[] nextTwoBytes = br.ReadBytes(2);
                int intValue = BitConverter.ToInt16(nextTwoBytes, 0);
                int combinedValue = (nextTwoBytes[0] << 8) | nextTwoBytes[1];
                int lenght = combinedValue - 3;

                List<byte> completeMessage = new List<byte>(br.ReadBytes(lenght));

                //Trabajamos con el RECORD
                List<bool> fieldPresence = new List<bool>();

                int byteIndex = 0;

                while (byteIndex < completeMessage.Count)
                {
                    byte currentByte = completeMessage[byteIndex];
                    // Procesar los 7 bits más significativos del byte actual
                    for (int j = 7; j > 0; j--)
                    {
                        fieldPresence.Add(((currentByte >> j) & 1) == 1);
                    }
                    if ((currentByte & 1) == 0)
                        break;
                    byteIndex++;
                }

                // Remover los bytes procesados
                completeMessage.RemoveRange(0, byteIndex + 1);

                while (fieldPresence.Count <= 28) { fieldPresence.Add(false); }

                // Partición de DataItems

                nuevaFila["NUM"] = Convert.ToString(NUM);

                if (fieldPresence[0] == true) // 010: Data Source Identifier
                {
                    List<byte> bytes010 = completeMessage.GetRange(0, 2);
                    List<string> decoded010 = decoder048.Item010(bytes010);
                    nuevaFila["SAC"] = decoded010[0];
                    nuevaFila["SIC"] = decoded010[1];
                    completeMessage.RemoveRange(0, 2);
                }
                else
                {
                    nuevaFila["SIC"] = "N/A";
                    nuevaFila["SAC"] = "N/A";
                    completeMessage.RemoveRange(0, 2);
                }

                // 140: Time of Day
                if (fieldPresence[1] == true)
                {
                    List<byte> bytes140 = completeMessage.GetRange(0, 3);
                    string decoded140 = decoder048.Item140(bytes140);
                    nuevaFila["Time"] = decoded140;
                    completeMessage.RemoveRange(0, 3);
                }
                else
                {
                    nuevaFila["Time"] = "N/A";
                }

                // 020: Target report descriptor
                if (fieldPresence[2] == true)
                {
                    List<byte> bytes020 = new List<byte>();
                    int byteIndex2 = 0;
                    while (byteIndex2 < completeMessage.Count)
                    {
                        byte currentByte = completeMessage[byteIndex2];
                        bytes020.Add(currentByte);
                        if ((currentByte & 1) == 0)
                        {
                            break;
                        }
                        byteIndex2++;
                    }
                    List<string> decoded020 = decoder048.Item020(bytes020);
                    nuevaFila["TYP020"] = decoded020[0];
                    nuevaFila["SIM020"] = decoded020[1];
                    nuevaFila["RDP020"] = decoded020[2];
                    nuevaFila["SPI020"] = decoded020[3];
                    nuevaFila["RAB020"] = decoded020[4];
                    nuevaFila["TST020"] = decoded020[5];
                    nuevaFila["ERR020"] = decoded020[6];
                    nuevaFila["XPP020"] = decoded020[7];
                    nuevaFila["ME020"] = decoded020[8];
                    nuevaFila["MI020"] = decoded020[9];
                    nuevaFila["FOEFRI_020"] = decoded020[10];
                    nuevaFila["ADSB_EP020"] = decoded020[11];
                    nuevaFila["ADSB_VAL020"] = decoded020[12];
                    nuevaFila["SCN_EP020"] = decoded020[13];
                    nuevaFila["SCN_VAL020"] = decoded020[14];
                    nuevaFila["PAI_EP020"] = decoded020[15];
                    nuevaFila["PAI_VAL020"] = decoded020[16];
                    completeMessage.RemoveRange(0, byteIndex2 + 1);
                }
                else
                {
                    nuevaFila["TYP020"] = "N/A";
                    nuevaFila["SIM020"] = "N/A";
                    nuevaFila["RDP020"] = "N/A";
                    nuevaFila["SPI020"] = "N/A";
                    nuevaFila["RAB020"] = "N/A";
                    nuevaFila["TST020"] = "N/A";
                    nuevaFila["ERR020"] = "N/A";
                    nuevaFila["XPP020"] = "N/A";
                    nuevaFila["ME020"] = "N/A";
                    nuevaFila["MI020"] = "N/A";
                    nuevaFila["FOEFRI_020"] = "N/A";
                    nuevaFila["ADSB_EP020"] = "N/A";
                    nuevaFila["ADSB_VAL020"] = "N/A";
                    nuevaFila["SCN_EP020"] = "N/A";
                    nuevaFila["SCN_VAL020"] = "N/A";
                    nuevaFila["PAI_EP020"] = "N/A";
                    nuevaFila["PAI_VAL020"] = "N/A";
                }

                // 040: Measured position in Slant Polar Co-ordinates
                if (fieldPresence[3] == true)
                {
                    List<byte> bytes040 = completeMessage.GetRange(0, 4);
                    List<string> decoded040 = decoder048.Item040(bytes040);
                    nuevaFila["RHO"] = decoded040[0];
                    nuevaFila["THETA"] = decoded040[1];
                    completeMessage.RemoveRange(0, 4);
                }
                else
                {
                    nuevaFila["RHO"] = "N/A";
                    nuevaFila["THETA"] = "N/A";
                }

                // 070: Mode-3/A Code in Octal representation 
                if (fieldPresence[4] == true)
                {
                    List<byte> bytes070 = completeMessage.GetRange(0, 2);
                    List<string> decoded070 = decoder048.Item070(bytes070);
                    nuevaFila["V070"] = decoded070[0];
                    nuevaFila["G070"] = decoded070[1];
                    nuevaFila["L070"] = decoded070[2];
                    nuevaFila["Mode_3A"] = decoded070[3];
                    completeMessage.RemoveRange(0, 2);
                }
                else
                {
                    nuevaFila["V070"] = "N/A";
                    nuevaFila["G070"] = "N/A";
                    nuevaFila["L070"] = "N/A";
                    nuevaFila["Mode_3A"] = "N/A";
                }

                // 090: Flight Level in Binary Representation
                if (fieldPresence[5] == true)
                {
                    List<byte> bytes090 = completeMessage.GetRange(0, 2);
                    List<string> decoded090 = decoder048.Item090(bytes090);
                    nuevaFila["V090"] = decoded090[0];
                    nuevaFila["G090"] = decoded090[1];
                    nuevaFila["Flight_Level"] = decoded090[2];
                    completeMessage.RemoveRange(0, 2);
                }
                else
                {
                    nuevaFila["V090"] = "N/A";
                    nuevaFila["G090"] = "N/A";
                    nuevaFila["Flight_Level"] = "N/A";
                }

                // 130: Radar plot Characteristics
                if (fieldPresence[6] == true)
                {
                    byte firstByte = completeMessage[0];
                    int lenght130 = 0;
                    for (int i = 7; i > 0; i--)
                    {
                        if (((firstByte >> i) & 1) == 1)
                        {
                            lenght130++;
                        }
                    }
                    if ((firstByte & 1) != 0)
                    {
                        throw new Exception("Error: FX de 130 es 1 :(");
                    }
                    List<byte> bytes130 = completeMessage.GetRange(0, lenght130 + 1);
                    List<string> decoded130 = decoder048.Item130(bytes130);
                    nuevaFila["SRL130"] = decoded130[0];
                    nuevaFila["SRR130"] = decoded130[1];
                    nuevaFila["SAM130"] = decoded130[2];
                    nuevaFila["PRL130"] = decoded130[3];
                    nuevaFila["PAM130"] = decoded130[4];
                    nuevaFila["RPD130"] = decoded130[5];
                    nuevaFila["APD130"] = decoded130[6];
                    completeMessage.RemoveRange(0, lenght130 + 1);
                }
                else
                {
                    nuevaFila["SRL130"] = "N/A";
                    nuevaFila["SRR130"] = "N/A";
                    nuevaFila["SAM130"] = "N/A";
                    nuevaFila["PRL130"] = "N/A";
                    nuevaFila["PAM130"] = "N/A";
                    nuevaFila["RPD130"] = "N/A";
                    nuevaFila["APD130"] = "N/A";
                }

                // 220: Aircraft Address
                if (fieldPresence[7] == true)
                {
                    List<byte> bytes220 = completeMessage.GetRange(0, 3);
                    string decoded220 = decoder048.Item220(bytes220);
                    nuevaFila["Target_address"] = decoded220;
                    completeMessage.RemoveRange(0, 3);
                }
                else
                {
                    nuevaFila["Target_address"] = "N/A";
                }

                // 240: Aircraft Identification
                if (fieldPresence[8] == true)
                {
                    List<byte> bytes240 = completeMessage.GetRange(0, 6);
                    string decoded240 = decoder048.Item240(bytes240);
                    nuevaFila["Target_identification"] = decoded240;
                    completeMessage.RemoveRange(0, 6);
                }
                else
                {
                    nuevaFila["Target_identification"] = "N/A";
                }

                // 250: BDS Register Data / Mode S MB Data
                if (fieldPresence[9] == true)
                {
                    int repeticiones = completeMessage[0];
                    int totalBytes = repeticiones * 8 + 1;
                    List<byte> bytes250 = completeMessage.GetRange(0, totalBytes);
                    List<string> decoded250 = decoder048.Item250(bytes250);
                    nuevaFila["Mode_S"] = decoded250[0];
                    nuevaFila["MCP_STATUS"] = decoded250[1];
                    nuevaFila["MCP_ALT"] = decoded250[2];
                    nuevaFila["FMS_STATUS"] = decoded250[3];
                    nuevaFila["FMS_ALT"] = decoded250[4];
                    nuevaFila["BP_STATUS"] = decoded250[5];
                    nuevaFila["BP"] = decoded250[6];
                    nuevaFila["MODE_STATUS"] = decoded250[7];
                    nuevaFila["VNAV"] = decoded250[8];
                    nuevaFila["ALTHOLD"] = decoded250[9];
                    nuevaFila["APP"] = decoded250[10];
                    nuevaFila["TARGETALT_STATUS"] = decoded250[11];
                    nuevaFila["TARGETALT_SOURCE"] = decoded250[12];
                    nuevaFila["RA_STATUS"] = decoded250[13];
                    nuevaFila["RA"] = decoded250[14];
                    nuevaFila["TTA_STATUS"] = decoded250[15];
                    nuevaFila["TTA"] = decoded250[16];
                    nuevaFila["GS_STATUS"] = decoded250[17];
                    nuevaFila["GS"] = decoded250[18];
                    nuevaFila["TAR_STATUS"] = decoded250[19];
                    nuevaFila["TAR"] = decoded250[20];
                    nuevaFila["TAS_STATUS"] = decoded250[21];
                    nuevaFila["TAS"] = decoded250[22];
                    nuevaFila["HDG_STATUS"] = decoded250[23];
                    nuevaFila["HDG"] = decoded250[24];
                    nuevaFila["IAS_STATUS"] = decoded250[25];
                    nuevaFila["IAS"] = decoded250[26];
                    nuevaFila["MACH_STATUS"] = decoded250[27];
                    nuevaFila["MACH"] = decoded250[28];
                    nuevaFila["BAR_STATUS"] = decoded250[29];
                    nuevaFila["BAR"] = decoded250[30];
                    nuevaFila["IVV_STATUS"] = decoded250[31];
                    nuevaFila["IVV"] = decoded250[32];
                    completeMessage.RemoveRange(0, totalBytes);
                }
                else
                {
                    nuevaFila["Mode_S"] = "N/A";
                    nuevaFila["MCP_STATUS"] = "N/A";
                    nuevaFila["MCP_ALT"] = "N/A";
                    nuevaFila["FMS_STATUS"] = "N/A";
                    nuevaFila["FMS_ALT"] = "N/A";
                    nuevaFila["BP_STATUS"] = "N/A";
                    nuevaFila["BP"] = "N/A";
                    nuevaFila["MODE_STATUS"] = "N/A";
                    nuevaFila["VNAV"] = "N/A";
                    nuevaFila["ALTHOLD"] = "N/A";
                    nuevaFila["APP"] = "N/A";
                    nuevaFila["TARGETALT_STATUS"] = "N/A";
                    nuevaFila["TARGETALT_SOURCE"] = "N/A";
                    nuevaFila["RA_STATUS"] = "N/A";
                    nuevaFila["RA"] = "N/A";
                    nuevaFila["TTA_STATUS"] = "N/A";
                    nuevaFila["TTA"] = "N/A";
                    nuevaFila["GS_STATUS"] = "N/A";
                    nuevaFila["GS"] = "N/A";
                    nuevaFila["TAR_STATUS"] = "N/A";
                    nuevaFila["TAR"] = "N/A";
                    nuevaFila["TAS_STATUS"] = "N/A";
                    nuevaFila["TAS"] = "N/A";
                    nuevaFila["HDG_STATUS"] = "N/A";
                    nuevaFila["HDG"] = "N/A";
                    nuevaFila["IAS_STATUS"] = "N/A";
                    nuevaFila["IAS"] = "N/A";
                    nuevaFila["MACH_STATUS"] = "N/A";
                    nuevaFila["MACH"] = "N/A";
                    nuevaFila["BAR_STATUS"] = "N/A";
                    nuevaFila["BAR"] = "N/A";
                    nuevaFila["IVV_STATUS"] = "N/A";
                    nuevaFila["IVV"] = "N/A";
                }

                // 161: Track/Plot Number
                if (fieldPresence[10] == true)
                {
                    List<byte> bytes161 = completeMessage.GetRange(0, 2);
                    string decoded161 = decoder048.Item161(bytes161);
                    nuevaFila["Track_number"] = decoded161;
                    completeMessage.RemoveRange(0, 2);
                }
                else
                {
                    nuevaFila["Track_number"] = "N/A";
                }

                // 042: Calculated position in Cartesian Co-ordinates
                if (fieldPresence[11] == true)
                {
                    List<byte> bytes042 = completeMessage.GetRange(0, 4);
                    List<string> decoded042 = decoder048.Item042(bytes042);
                    nuevaFila["X_Component"] = decoded042[0];
                    nuevaFila["Y_Component"] = decoded042[1];
                    completeMessage.RemoveRange(0, 4);
                }
                else
                {
                    nuevaFila["X_Component"] = "N/A";
                    nuevaFila["Y_Component"] = "N/A";
                }

                // 200: Calculated Track Velocity in Polar
                if (fieldPresence[12] == true)
                {
                    List<byte> bytes200 = completeMessage.GetRange(0, 4);
                    List<string> decoded200 = decoder048.Item200(bytes200);
                    nuevaFila["Ground_Speedkt"] = decoded200[0];
                    nuevaFila["Heading"] = decoded200[1];
                    completeMessage.RemoveRange(0, 4);
                }
                else
                {
                    nuevaFila["Ground_Speedkt"] = "N/A";
                    nuevaFila["Heading"] = "N/A";
                }

                // 170: Track Status
                if (fieldPresence[13] == true)
                {
                    List<byte> bytes170 = new List<byte>();
                    int index = 0;
                    bool continuarLeyendo = true;
                    while (continuarLeyendo && index < completeMessage.Count)
                    {
                        byte currentByte = completeMessage[index];
                        bytes170.Add(currentByte);
                        if ((currentByte & 1) == 0)
                        {
                            continuarLeyendo = false;
                        }
                        index++;
                    }
                    List<string> decoded170 = decoder048.Item170(bytes170);
                    nuevaFila["CNF170"] = decoded170[0];
                    nuevaFila["RAD170"] = decoded170[1];
                    nuevaFila["DOU170"] = decoded170[2];
                    nuevaFila["MAH170"] = decoded170[3];
                    nuevaFila["CDM170"] = decoded170[4];
                    nuevaFila["TRE170"] = decoded170[5];
                    nuevaFila["GHO170"] = decoded170[6];
                    nuevaFila["SUP170"] = decoded170[7];
                    nuevaFila["TCC170"] = decoded170[8];
                    completeMessage.RemoveRange(0, bytes170.Count);
                }
                else
                {
                    nuevaFila["CNF170"] = "N/A";
                    nuevaFila["RAD170"] = "N/A";
                    nuevaFila["DOU170"] = "N/A";
                    nuevaFila["MAH170"] = "N/A";
                    nuevaFila["CDM170"] = "N/A";
                    nuevaFila["TRE170"] = "N/A";
                    nuevaFila["GHO170"] = "N/A";
                    nuevaFila["SUP170"] = "N/A";
                    nuevaFila["TCC170"] = "N/A";
                }

                // 210: Track Quality
                if (fieldPresence[14] == true) { completeMessage.RemoveRange(0, 2); }

                // 030: Warning/Error Conditions/Target Classification
                if (fieldPresence[15] == true) 
                {
                    List<byte> bytes030 = new List<byte>();

                    int byteIndex2 = 0;

                    while (byteIndex2 < completeMessage.Count)
                    {
                        byte currentByte = completeMessage[byteIndex2];
                        bytes030.Add(currentByte);

                        if ((currentByte & 1) == 0)
                        {
                            break;
                        }
                        byteIndex2++;
                    }
                    completeMessage.RemoveRange(0, byteIndex2 + 1);
                }

                // 080: Mode-3/A Code Confidence Indicator
                if (fieldPresence[16] == true) { completeMessage.RemoveRange(0, 2); }

                // 100: Mode-C Code and Confidence Indicator
                if (fieldPresence[17] == true) { completeMessage.RemoveRange(0, 4); }

                // 110: Height Measured by a 3D Radar
                if (fieldPresence[18] == true) 
                {
                    List<byte> bytes110 = completeMessage.GetRange(0, 2);
                    string decoded110 = decoder048.Item110(bytes110);
                    nuevaFila["Measured_Height"] = decoded110;
                    completeMessage.RemoveRange(0, 2);
                }
                else
                {
                    nuevaFila["Measured_Height"] = "N/A";
                }

                // 120: Radial Doppler Speed
                if (fieldPresence[19] == true)
                {
                    int bytesIndex = 1;
                    byte bytePrimario = completeMessage[0];
                    if ((bytePrimario & 0b10000000) != 0) { bytesIndex += 2; }
                    if ((bytePrimario & 0b01000000) != 0)
                    {
                        byte byteREP = completeMessage[bytesIndex];
                        bytesIndex += (1 + byteREP * 6);
                    }
                    completeMessage.RemoveRange(0, bytesIndex);
                }

                // 230: Communication / ACAS Capability and Flight Status
                if (fieldPresence[20] == true) 
                {
                    List<byte> bytes230 = completeMessage.GetRange(0, 2);
                    List<string> decoded230 = decoder048.Item230(bytes230);
                    nuevaFila["COM230"] = decoded230[0];
                    nuevaFila["STAT230"] = decoded230[1];
                    nuevaFila["SI230"] = decoded230[2];
                    nuevaFila["MSSC230"] = decoded230[3];
                    nuevaFila["ARC230"] = decoded230[4];
                    nuevaFila["AIC230"] = decoded230[5];
                    nuevaFila["B1A230"] = decoded230[6];
                    nuevaFila["Surveillance_Capability"] = decoded230[7];
                    nuevaFila["ACAS_generation"] = decoded230[8];
                    nuevaFila["RTCA_version"] = decoded230[9];
                    completeMessage.RemoveRange(0, 2);
                }
                else
                {
                    nuevaFila["COM230"] = "N/A";
                    nuevaFila["STAT230"] = "N/A";
                    nuevaFila["SI230"] = "N/A";
                    nuevaFila["MSSC230"] = "N/A";
                    nuevaFila["ARC230"] = "N/A";
                    nuevaFila["AIC230"] = "N/A";
                    nuevaFila["B1A230"] = "N/A";
                    nuevaFila["Surveillance_Capability"] = "N/A";
                    nuevaFila["ACAS_generation"] = "N/A";
                    nuevaFila["RTCA_version"] = "N/A";
                }

                // 260: ACAS Resolution Advisory Report
                if (fieldPresence[21] == true) { completeMessage.RemoveRange(0, 7); }

                // 055: Mode 1 Code in Octal Representation
                if (fieldPresence[22] == true) { completeMessage.RemoveRange(0, 1); }

                // 050: Mode 2 Code in Octal Representation
                if (fieldPresence[23] == true) { completeMessage.RemoveRange(0, 2); }

                // 065: Mode 1 Code Confidence Indicator
                if (fieldPresence[24] == true) { completeMessage.RemoveRange(0, 1); }

                // 060: Mode 1 Code Confidence Indicator
                if (fieldPresence[25] == true) { completeMessage.RemoveRange(0, 2); }

                if (completeMessage.Count != 0)
                {
                    Console.WriteLine("Error: completeMessage no está vacío al finalizar.");
                }

                //Mode C corrected
                Position position = new Position();
                try 
                { 
                    string FL1 = nuevaFila["Flight_Level"].ToString(); 
                    string Pressure = nuevaFila["BP"].ToString();
                    nuevaFila["ModeC_corrected"] = position.CorrectedFL(FL1, Pressure);
                }
                catch { nuevaFila["ModeC_corrected"] = "N/A"; }

                // Transformación de coordenadas a Geodésicas
                List<string> coordinatesSpherical = new List<string>();
                coordinatesSpherical.Add(nuevaFila["RHO"].ToString());
                coordinatesSpherical.Add(nuevaFila["THETA"].ToString());
                double FL = 0;
                try { FL = Convert.ToDouble(nuevaFila["Flight_Level"]) * 100 * 0.3048; }
                catch { } //////////////////Falta excepción
                coordinatesSpherical.Add(FL.ToString());
                List<string> LLA = decoder048.GeodesicCoor(coordinatesSpherical);
                nuevaFila["Latitud"] = LLA[0];
                nuevaFila["Longitud"] = LLA[1];
                nuevaFila["h"] = LLA[2];

                dataTable.Rows.Add(nuevaFila);
                NUM++;
            }
            if (fs.Position == fs.Length)
            {
                Console.WriteLine("Archivo leído completamente.");
            }
            else
            {
                throw new Exception("Error: No se ha leído todo el archivo.");
            }
            return dataTable;
        }
    }
}
