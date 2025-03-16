using System;
using System.IO;
using System.Data;
using System.Collections.Generic;

namespace ASTDECO
{
    public class Clasificador
    {
        public DataTable CrearClasi(string Path)
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

            using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                int numMensaje = 1;

                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    try
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
                        foreach (byte fspecByte in fspecBytes)
                        {
                            for (int bit = 7; bit >= 1; bit--) // Se ignora el bit 0
                            {
                                if (index < dataItemsList.Count && (fspecByte & (1 << bit)) != 0)
                                {
                                    row[dataItemsList[index]] = true; // Marcar Data Item como presente
                                }
                                index++;
                            }
                        }

                        // Agregar la fila a la tabla
                        dataTable.Rows.Add(row);

                        // Saltar el resto del mensaje si aún hay bytes por leer
                        long bytesRestantes = length - fspecBytes.Count;
                        if (bytesRestantes > 0 && br.BaseStream.Position + bytesRestantes <= br.BaseStream.Length)
                        {
                            br.BaseStream.Position += bytesRestantes;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error procesando mensaje {numMensaje}: {ex.Message}");
                        break; // Detener lectura si ocurre un error
                    }
                }
            }

            return dataTable;
        }
    }
}
