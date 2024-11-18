using DefaultNamespace;

namespace Library.Tipos;
//Efecto:
//Cumple con SRP: La clase Efecto tiene la responsabilidad de representar efectos que se aplican sobre un 
//Pokemon.
//OCP: La clase Efecto está abierta a la extensión, pero cerrada a la modificación. Si se crean nuevas 
//subclases que representan tipos específicos de efectos, no es necesario modificar la clase Efecto para 
//agregarlos. Esto se logra al definir HacerEfecto como un método virtual que puede ser sobrecargado en 
//clases derivadas.
//LSP: Cualquier subclase de Efecto puede ser sustituida por una instancia de Efecto, y el comportamiento 
//general del sistema no se verá afectado.
//Polimorfismo: Gracias al uso de HacerEfecto como método virtual, las subclases pueden sobrescribirlo 
//para proporcionar su propia implementación. Esto permite aplicar el polimorfismo y gestionar diferentes 
//tipos de efectos de manera uniforme.


public class Efecto
{
    /// <summary>  
    /// Inicializa una nueva instancia de la clase <see cref="Efecto"/>.  
    /// El constructor es protegido para permitir que solo las subclases lo utilicen.  
    /// </summary>  
    protected Efecto()
    {
        
    }

    /// <summary>  
    /// Método virtual que aplica el efecto a un Pokémon.  
    /// Este método puede ser sobrescrito por las subclases para proporcionar una implementación específica.  
    /// </summary>  
    /// <param name="pokemon">El Pokémon al que se le aplicará el efecto.</param>  
    public virtual string HacerEfecto(Pokemon pokemon)
    {
        return "";
    }

    /// <summary>  
    /// Crea una copia de un efecto específico definido por el tipo proporcionado.  
    /// Utiliza reflexión para instanciar el tipo dinámicamente.  
    /// </summary>  
    /// <param name="tipoEfecto">El tipo del efecto que se desea copiar.</param>  
    /// <returns>Una nueva instancia del efecto del tipo especificado.</returns>  
    public static Efecto CrearCopia(Type tipoEfecto)
    {
        return (Efecto)Activator.CreateInstance(tipoEfecto);
    }
}