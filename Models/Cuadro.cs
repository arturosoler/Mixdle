namespace Mixdle.Models
{
    public class Cuadro
    {

        public char Letra { get; set; }
        public bool Marcada { get; set; }
        public bool Usada;

        public Cuadro (char letra)
        {
            Letra= letra;
            Marcada = false;
            Usada = false;
        }
        public bool Disabled() => Usada;
        public string ClaseCSS() => Usada ? "botonUsado" : Marcada ? "botonSeleccionado" : "botonActivo";
    }
}
