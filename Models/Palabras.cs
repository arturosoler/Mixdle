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
            if (Cuadros[columna][fila].Estado==Cuadro.EstadoCuadro.Disponible) //está dentro de las letras acertadas hasta el momento
            {
                var RegexApuesta = new StringBuilder("");
                for (int iColumna = 0; iColumna < 6; iColumna++)
                    if (iColumna == columna) //es la columna de la apuesta en curso
                        RegexApuesta.Append(Cuadros[columna][fila].Letra);
                    else if (Cuadros[iColumna].Any(c => c.Estado==Cuadro.EstadoCuadro.PresentePalabra))  
                        RegexApuesta
                            .Append(Cuadros[iColumna].First(c => c.Estado == Cuadro.EstadoCuadro.PresentePalabra).Letra);
                    else RegexApuesta.Append(".");
                //Console.WriteLine(RegexApuesta);
                var apuesta = RegexApuesta.ToString();
                if (Seleccion.Except(Aciertos).Any(s => Regex.IsMatch(s, apuesta)))
                {   //Ha acertado la palabra
                    if (!apuesta.Contains('.'))
                    {   //Era la ultima letra
                        var pal = Seleccion.First(s => Regex.IsMatch(s, apuesta));
                        Aciertos.Add(pal);
                        ContadorPalabras++;
                        //resetea 
                        for (int iCol = 0; iCol < 6; iCol++)
                            for (int iFila = 0; iFila < 6; iFila++)
                            if (Cuadros[iCol][iFila].Estado!=Cuadro.EstadoCuadro.Acertado)
                            {
                                if (Cuadros[iCol][iFila].Estado == Cuadro.EstadoCuadro.PresentePalabra ||
                                        (columna == iCol && iFila == fila))
                                    Cuadros[iCol][iFila].Estado = Cuadro.EstadoCuadro.Acertado;
                                else //if (Cuadros[iCol][iFila].Estado == Cuadro.EstadoCuadro.AusentePalabra)
                                    Cuadros[iCol][iFila].Estado = Cuadro.EstadoCuadro.Disponible;
                            }
                    } 
                    else for (var iFila = 0; iFila < 6; iFila++)   //marca la fial
                        if (Cuadros[columna][iFila].Estado != Cuadro.EstadoCuadro.Acertado)
                            Cuadros[columna][iFila].Estado = iFila == fila ?
                                    Cuadro.EstadoCuadro.PresentePalabra : Cuadro.EstadoCuadro.AusentePalabra;                    
                }              
                else //No ha acertado
                {
                    Cuadros[columna][fila].Estado = Cuadro.EstadoCuadro.AusentePalabra;
                    ContadorErrores++;
                }
            }
        }
    }
}
