using Microsoft.AspNetCore.Components.Forms;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Mixdle.Models
{
    public partial class Palabras
    {
        public List<string> Seleccion{get;set; }=new List<string>();
        public List<string> Aciertos { get; set; } = new List<string>();
        public Cuadro[][] Cuadros { get; set; } = new Cuadro[6][] //Cuadros [Columns][Fila]
        {
            new Cuadro[6], new Cuadro[6],new Cuadro[6],new Cuadro[6],new Cuadro[6],new Cuadro[6]
        };

        public int ContadorErrores { get; set; } = 0;
        public int ContadorPalabras { get; set; } = 0;
        public Palabras()
        {
            var rnd=new Random(int.Parse(DateTime.Today.ToString("yyyyMMdd")));
            string palabra;
            while (Seleccion.Count < 6)
                if (!Seleccion.Contains((palabra = Todas[rnd.Next(Todas.Count)])))
                {
                    Seleccion.Add(palabra);
                    var col = 0;
                    while (col < 6)
                    {
                        var columna = Cuadros[col];
                        bool colocada = false;
                        int libre;
                        while (!colocada)
                        {
                            if (colocada=columna[libre=rnd.Next(6)]==null)
                            {
                                columna[libre] = new Cuadro(palabra.ToCharArray()[col]);
                                col++;
                            }
                        }
                    }
                }
        }

        public void Click(int columna,int fila)
        {
            Console.WriteLine($"c: {columna} f:{fila}");
            if (Cuadros[columna][fila].Usada)
                return;
            if (Cuadros[columna][fila].Marcada)
            {
                Cuadros[columna][fila].Marcada = false;
            }
            else
            {
                var RegexApuesta = new StringBuilder("");
                for (int iColumna = 0; iColumna < 6; iColumna++)
                    if (iColumna == columna) //es la columna de la apuesta en curso
                    {
                        RegexApuesta.Append(Cuadros[columna][fila].Letra);
                        if (Cuadros[iColumna].Any(c => c.Marcada))
                            Cuadros[iColumna].First(c => c.Marcada).Marcada = false;
                    }
                        
                    else if (Cuadros[iColumna].Any(c => c.Marcada))  
                        RegexApuesta
                            .Append(Cuadros[iColumna].First(c => c.Marcada).Letra);
                    else RegexApuesta.Append(".");
                var apuesta = RegexApuesta.ToString();
                if (Seleccion.Except(Aciertos).Any(s => Regex.IsMatch(s, apuesta)))
                {
                    Cuadros[columna][fila].Marcada = true;
                    if (!apuesta.Contains('.'))
                    {
                        var pal = Seleccion.First(s => Regex.IsMatch(s, apuesta));
                        Aciertos.Add(pal);
                        ContadorPalabras++;
                        //resetea 
                        for (int iCol = 0; iCol < 6; iCol++)
                        {
                            var c = Cuadros[iCol].First(c => c.Marcada);
                            c.Marcada = false;
                            c.Usada = true;
                        }
                    }
                }
                else ContadorErrores++;
            }
        }
    }
}
