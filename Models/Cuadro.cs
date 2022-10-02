namespace Mixdle.Models
{
    public class Cuadro
    {
        public enum EstadoCuadro
        {
            Disponible,
            AusentePalabra,
            PresentePalabra,
            Acertado
        }
        public char Letra { get; set; }
        public EstadoCuadro Estado { get; set; }

        public Cuadro(char letra)
        {
            Letra = letra;
            Estado = EstadoCuadro.Disponible;
        }
        public bool Disabled() => Estado != EstadoCuadro.Disponible;
        public string ClaseCSS() => Estado switch
            {
                EstadoCuadro.Disponible=>"botonDisponible",
                EstadoCuadro.AusentePalabra=>"botonAusentePalabra",
                EstadoCuadro.PresentePalabra=>"botonPresentePalabra",
                EstadoCuadro.Acertado=>"botonAcertado"
            };
            
            
            //Usada ? "botonUsado" : Marcada ? "botonSeleccionado" : "botonActivo";
    }
}
