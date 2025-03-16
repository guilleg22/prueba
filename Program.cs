using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace ASTDECO
{
    // Clase que representa un mensaje Asterix
    public class AsterixMessage
    {
        public Dictionary<string, bool> DataItems { get; set; } = new Dictionary<string, bool>();

        public void SetDataItem(string item, bool value)
        {
            DataItems[item] = value;
        }

        public bool GetDataItem(string item)
        {
            return DataItems.ContainsKey(item) && DataItems[item];
        }
    }
    public class Clasificador
{
    public DataTable CrearClasi(string path)
    {
        DataTable dataTable = new DataTable();

        // Lista de Data Items en el orden del FSPEC
        List<string> dataItemsList = new List<string>
        {
            "010", "140", "020", "040", "070", "090", "130",
            "220", "240", "250", "161", "042", "200", "170",
            "210", "030", "080", "100", "110", "120", "230",
            "260", "055", "050", "065", "060", "SP", "RE"
        };

        // Agregar la columna del número de mensaje
        dataTable.Columns.Add("Mensaje", typeof(int));

        // Agregar columnas de Data Items como booleanos
        foreach (var item in dataItemsList)
        {
            dataTable.Columns.Add(item, typeof(bool));
        }

        // Agregar columnas adicionales para los Data Items decodificados
        dataTable.Columns.Add("SAC", typeof(string));
        dataTable.Columns.Add("SIC", typeof(string));
        dataTable.Columns.Add("RHO", typeof(string));
        dataTable.Columns.Add("THETA", typeof(string));
        dataTable.Columns.Add("SRL130", typeof(string));
        dataTable.Columns.Add("SRR130", typeof(string));
        dataTable.Columns.Add("SAM130", typeof(string));
        dataTable.Columns.Add("PRL130", typeof(string));
        dataTable.Columns.Add("PAM130", typeof(string));
        dataTable.Columns.Add("RPD130", typeof(string));
        dataTable.Columns.Add("APD130", typeof(string));
        dataTable.Columns.Add("Mode_S", typeof(string));
        dataTable.Columns.Add("MCP_STATUS", typeof(string));
        dataTable.Columns.Add("MCP_ALT", typeof(string));
        dataTable.Columns.Add("FMS_STATUS", typeof(string));
        dataTable.Columns.Add("FMS_ALT", typeof(string));
        dataTable.Columns.Add("BP_STATUS", typeof(string));
        dataTable.Columns.Add("BP", typeof(string));
        dataTable.Columns.Add("MODE_STATUS", typeof(string));
        dataTable.Columns.Add("VNAV", typeof(string));
        dataTable.Columns.Add("ALTHOLD", typeof(string));
        dataTable.Columns.Add("APP", typeof(string));
        dataTable.Columns.Add("TARGETALT_STATUS", typeof(string));
        dataTable.Columns.Add("TARGETALT_SOURCE", typeof(string));
        dataTable.Columns.Add("RA_STATUS", typeof(string));
        dataTable.Columns.Add("RA", typeof(string));
        dataTable.Columns.Add("TTA_STATUS", typeof(string));
        dataTable.Columns.Add("TTA", typeof(string));
        dataTable.Columns.Add("GS_STATUS", typeof(string));
        dataTable.Columns.Add("GS", typeof(string));
        dataTable.Columns.Add("TAR_STATUS", typeof(string));
        dataTable.Columns.Add("TAR", typeof(string));
        dataTable.Columns.Add("TAS_STATUS", typeof(string));
        dataTable.Columns.Add("TAS", typeof(string));
        dataTable.Columns.Add("HDG_STATUS", typeof(string));
        dataTable.Columns.Add("HDG", typeof(string));
        dataTable.Columns.Add("IAS_STATUS", typeof(string));
        dataTable.Columns.Add("IAS", typeof(string));
        dataTable.Columns.Add("MACH_STATUS", typeof(string));
        dataTable.Columns.Add("MACH", typeof(string));
        dataTable.Columns.Add("BAR_STATUS", typeof(string));
        dataTable.Columns.Add("BAR", typeof(string));
        dataTable.Columns.Add("IVV_STATUS", typeof(string));
        dataTable.Columns.Add("IVV", typeof(string));
        dataTable.Columns.Add("Ground_Speedkt", typeof(string));
        dataTable.Columns.Add("Heading", typeof(string));
        dataTable.Columns.Add("COM230", typeof(string));
        dataTable.Columns.Add("STAT230", typeof(string));
        dataTable.Columns.Add("SI230", typeof(string));
        dataTable.Columns.Add("MSSC230", typeof(string));
        dataTable.Columns.Add("ARC230", typeof(string));
        dataTable.Columns.Add("AIC230", typeof(string));
        dataTable.Columns.Add("B1A230", typeof(string));
        dataTable.Columns.Add("Surveillance_Capability", typeof(string));
        dataTable.Columns.Add("ACAS_generation", typeof(string));
        dataTable.Columns.Add("RTCA_version", typeof(string));

        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                int numMensaje = 1;

                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    // Verificar si quedan suficientes bytes para leer CAT + Longitud (mínimo 3 bytes)
                    if (br.BaseStream.Position + 3 > br.BaseStream.Length)
                    {
                        Console.WriteLine("Fin del archivo alcanzado antes de leer CAT y longitud.");
                        break;
                    }

                    // Leer y omitir los 3 primeros bytes (CAT + longitud)
                    br.ReadByte();
                    byte[] lengthBytes = br.ReadBytes(2);
                    if (lengthBytes.Length < 2)
                    {
                        Console.WriteLine("Error: No se pudieron leer los bytes de longitud.");
                        break;
                    }
                    int length = ((lengthBytes[0] << 8) | lengthBytes[1]) - 3; // Longitud total menos los 3 bytes ya leídos

                    // Verificar si quedan suficientes bytes para leer FSPEC
                    if (br.BaseStream.Position >= br.BaseStream.Length)
                    {
                        Console.WriteLine("No hay suficientes datos para leer FSPEC.");
                        break;
                    }

                    // Leer FSPEC con manejo de errores
                    List<byte> fspecBytes = new List<byte>();
                    byte currentByte;
                    do
                    {
                        if (br.BaseStream.Position >= br.BaseStream.Length)
                        {
                            Console.WriteLine("Intento de leer FSPEC fuera del rango del archivo.");
                            break;
                        }
                        currentByte = br.ReadByte();
                        fspecBytes.Add(currentByte);
                    } while ((currentByte & 0x01) != 0); // Si el bit 0 es 1, seguir leyendo FSPEC

                    // Crear una nueva fila
                    DataRow row = dataTable.NewRow();
                    row["Mensaje"] = numMensaje++;

                    // Inicializar todos los Data Items como false
                    foreach (var item in dataItemsList)
                    {
                        row[item] = false;
                    }

                    // Procesar FSPEC correctamente
                    int index = 0;
                    AsterixMessage message = new AsterixMessage();
                    foreach (byte fspecByte in fspecBytes)
                    {
                        for (int bit = 7; bit >= 1; bit--) // Se ignora el bit 0
                        {
                            if (index < dataItemsList.Count && (fspecByte & (1 << bit)) != 0)
                            {
                                string dataItem = dataItemsList[index];
                                row[dataItem] = true; // Marcar Data Item como presente
                                message.SetDataItem(dataItem, true);

                                // Decodificar Data Items específicos
                                switch (dataItem)
                                {
                                    case "010":
                                        List<byte> bytes010 = new List<byte>(br.ReadBytes(2));
                                        List<string> decoded010 = Item010(bytes010);
                                        row["SAC"] = decoded010[0];
                                        row["SIC"] = decoded010[1];
                                        break;
                                    case "040":
                                        List<byte> bytes040 = new List<byte>(br.ReadBytes(4));
                                        List<string> decoded040 = Item040(bytes040);
                                        row["RHO"] = decoded040[0];
                                        row["THETA"] = decoded040[1];
                                        break;
                                    case "130":
                                        int length130 = CalculateLength130(br);
                                        List<byte> bytes130 = new List<byte>(br.ReadBytes(length130 + 1));
                                        List<string> decoded130 = Item130(bytes130);
                                        row["SRL130"] = decoded130[0];
                                        row["SRR130"] = decoded130[1];
                                        row["SAM130"] = decoded130[2];
                                        row["PRL130"] = decoded130[3];
                                        row["PAM130"] = decoded130[4];
                                        row["RPD130"] = decoded130[5];
                                        row["APD130"] = decoded130[6];
                                        break;
                                    case "250":
                                        int repeticiones = br.ReadByte();
                                        int totalBytes = repeticiones * 8;
                                        List<byte> bytes250 = new List<byte>(br.ReadBytes(totalBytes));
                                        List<string> decoded250 = Item250(bytes250);
                                        row["Mode_S"] = decoded250[0];
                                        row["MCP_STATUS"] = decoded250[1];
                                        row["MCP_ALT"] = decoded250[2];
                                        row["FMS_STATUS"] = decoded250[3];
                                        row["FMS_ALT"] = decoded250[4];
                                        row["BP_STATUS"] = decoded250[5];
                                        row["BP"] = decoded250[6];
                                        row["MODE_STATUS"] = decoded250[7];
                                        row["VNAV"] = decoded250[8];
                                        row["ALTHOLD"] = decoded250[9];
                                        row["APP"] = decoded250[10];
                                        row["TARGETALT_STATUS"] = decoded250[11];
                                        row["TARGETALT_SOURCE"] = decoded250[12];
                                        row["RA_STATUS"] = decoded250[13];
                                        row["RA"] = decoded250[14];
                                        row["TTA_STATUS"] = decoded250[15];
                                        row["TTA"] = decoded250[16];
                                        row["GS_STATUS"] = decoded250[17];
                                        row["GS"] = decoded250[18];
                                        row["TAR_STATUS"] = decoded250[19];
                                        row["TAR"] = decoded250[20];
                                        row["TAS_STATUS"] = decoded250[21];
                                        row["TAS"] = decoded250[22];
                                        row["HDG_STATUS"] = decoded250[23];
                                        row["HDG"] = decoded250[24];
                                        row["IAS_STATUS"] = decoded250[25];
                                        row["IAS"] = decoded250[26];
                                        row["MACH_STATUS"] = decoded250[27];
                                        row["MACH"] = decoded250[28];
                                        row["BAR_STATUS"] = decoded250[29];
                                        row["BAR"] = decoded250[30];
                                        row["IVV_STATUS"] = decoded250[31];
                                        row["IVV"] = decoded250[32];
                                        break;
                                    case "200":
                                        List<byte> bytes200 = new List<byte>(br.ReadBytes(4));
                                        List<string> decoded200 = Item200(bytes200);
                                        row["Ground_Speedkt"] = decoded200[0];
                                        row["Heading"] = decoded200[1];
                                        break;
                                    case "230":
                                        List<byte> bytes230 = new List<byte>(br.ReadBytes(2));
                                        List<string> decoded230 = Item230(bytes230);
                                        row["COM230"] = decoded230[0];
                                        row["STAT230"] = decoded230[1];
                                        row["SI230"] = decoded230[2];
                                        row["MSSC230"] = decoded230[3];
                                        row["ARC230"] = decoded230[4];
                                        row["AIC230"] = decoded230[5];
                                        row["B1A230"] = decoded230[6];
                                        row["Surveillance_Capability"] = decoded230[7];
                                        row["ACAS_generation"] = decoded230[8];
                                        row["RTCA_version"] = decoded230[9];
                                        break;
                                }
                            }
                            index++;
                        }
                    }

                    // Agregar la fila a la tabla
                    dataTable.Rows.Add(row);
                    Console.WriteLine($"Mensaje {numMensaje - 1} procesado y añadido a la tabla.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error procesando archivo: {ex.Message}");
        }

        return dataTable;
    }

    public int CalculateLength130(BinaryReader br)
    {
        byte firstByte = br.ReadByte();
        int length130 = 0;
        for (int i = 7; i > 0; i--)
        {
            if (((firstByte >> i) & 1) == 1)
            {
                length130++;
            }
        }
        if ((firstByte & 1) != 0)
        {
            throw new Exception("Error: FX de 130 es 1 :(");
        }
        return length130;
    }

    // Métodos de decodificación copiados de Decoder048.cs
    public List<string> Item010(List<byte> bytes010)
    {
        return new List<string> { Convert.ToString(bytes010[0]), Convert.ToString(bytes010[1]) };
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

    public List<string> Item130(List<byte> bytes130)
    {
        List<string> decoded130 = new List<string>();
        byte bytePrincipal = bytes130[0];
        if ((bytePrincipal & 0b10000000) != 0)
        {
            double num = bytes130[1] * (360 / (Math.Pow(2, 13)));
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
            decoded130.Add(Convert.ToString(Math.Round(Convert.ToDouble(num) / 256, 3) + " NM"));
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

    public List<string> Item200(List<byte> bytes200)
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
                decoded230.Add("N/A");
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

        int nextBit = (firstByte & 0b00000010) >> 1;

        if (nextBit == 0)
        {
            decoded230.Add("SI-Code Capable");
        }
        else
        {
            decoded230.Add("II-Code Capable");
        }

        byte secondByte = bytes230[1];
        int MSSC = (secondByte & 0b10000000) >> 7;

        if (MSSC == 0)
        {
            decoded230.Add("No");
        }
        else
        {
            decoded230.Add("Yes");
        }

        int ARC = (secondByte & 0b01000000) >> 6;

        if (ARC == 0)
        {
            decoded230.Add("100 ft resolution");
        }
        else
        {
            decoded230.Add("25 ft resolution");
        }

        int AIC = (secondByte & 0b00100000) >> 5;

        if (AIC == 0)
        {
            decoded230.Add("No");
        }
        else
        {
            decoded230.Add("Yes");
        }

        int B1A = (secondByte & 0b00010000) >> 4;

        if (B1A == 1)
        {
            decoded230.Add("ACAS is operational");
        }
        else
        {
            decoded230.Add("ACAS has failed or is on standby");
        }

        int bit4 = (secondByte & 0b00001000) >> 3;

        if (bit4 == 1)
        {
            decoded230.Add("Capability of hybrid surveillance");
        }
        else
        {
            decoded230.Add("No hybrid surveillance capability");
        }

        int bit3 = (secondByte & 0b00000100) >> 2;

        if (bit3 == 1)
        {
            decoded230.Add("ACAS is generating TAs and RAs");
        }
        else
        {
            decoded230.Add("ACAS generation of TAs only");
        }

        int lastBits = (secondByte & 0b00000011);

        switch (lastBits)
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
                decoded230.Add("N/A");
                break;
        }

        return decoded230;
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
}
}
