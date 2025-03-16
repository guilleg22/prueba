using ASTDECO;
using System.Data;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace AST_DECO
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ruta del archivo de entrada
            string filePath = @"C:\Users\test\source\repos\ASTERIX\ASTERIX\230502-est-080001_BCN_60MN_08_09.ast";

            // Ruta de salida para CSV
            string csvPath = @"C:\Users\test\source\repos\ASTERIX\ASTERIX\output.csv";

            // Instanciar clasificador
            ClasificadorAsterix clasificador = new ClasificadorAsterix();

            // Crear DataTable
            DataTable? tabla = clasificador.CrearClasis(filePath, 3); // Limitar a 3 mensajes

            if (tabla != null)
            {
                // Exportar a CSV
                ExportarACSV(tabla, csvPath);

                // Mensaje de confirmación
                Console.WriteLine($"Archivo CSV generado correctamente en: {csvPath}");
            }
            else
            {
                Console.WriteLine("Error: No se pudo crear la tabla.");
            }

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }

        // Función para exportar DataTable a CSV
        static void ExportarACSV(DataTable tabla, string ruta)
        {
            StringBuilder csvContent = new StringBuilder();

            // Escribir encabezados
            string[] columnNames = tabla.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
            csvContent.AppendLine(string.Join(";", columnNames));

            // Escribir filas
            foreach (DataRow row in tabla.Rows)
            {
                string[] fields = row.ItemArray.Select(field =>
                    field != null && (field.ToString().Contains(";") || field.ToString().Contains("\n"))
                    ? $"\"{field}\""
                    : field?.ToString() ?? string.Empty
                ).ToArray();
                csvContent.AppendLine(string.Join(";", fields));
            }

            // Escribir el contenido CSV en el archivo
            File.WriteAllText(ruta, csvContent.ToString()); 
        }
    }
}
