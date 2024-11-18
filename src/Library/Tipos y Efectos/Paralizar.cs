using DefaultNamespace;

namespace Library.Tipos;

//Paralizar:
// SRP: La clase Paralizar tiene una responsabilidad clara: gestionar el efecto de la paralización en un Pokémon. 
//OCP: La clase Paralizar es fácil de extender en caso de que se desee añadir nuevas lógicas de efectos, como 
//diferentes probabilidades de paralización, sin modificar el código actual. 
//LSP:  La clase Paralizar hereda de Efecto y se puede usar en cualquier lugar donde se espere un objeto Efecto


public class Paralizar:Efecto
{
    private Random random = new Random();
    
    /// <summary>  
    /// Inicializa una nueva instancia de la clase <see cref="Paralizar"/>.  
    /// </summary>
    public Paralizar()
    {
    }
    
    /// <summary>  
    /// Determina si el Pokémon puede atacar en su turno, teniendo en cuenta el efecto de paralización.  
    /// Se genera un número aleatorio para simular la probabilidad de que el Pokémon pueda atacar.  
    /// </summary>  
    /// <param name="pokemon">El Pokémon que se está evaluando para determinar si puede atacar.</param>  
    /// <returns>Verdadero si el Pokémon puede atacar, falso si no puede.</returns>  
    private bool Jugar(Pokemon pokemon)
    {
        int numero= random.Next(1, 4);
        if (numero == 1)
        {
            Console.WriteLine($"El pokemon {pokemon.GetName()} puede atacar en este turno a pesar de estar paralizado");
            return true;
        }
        Console.WriteLine($"El pokemon {pokemon.GetName()} no puede atacar, ha sido paralizado");
        return false;
    }

    /// <summary>  
    /// Aplica el efecto de paralización al Pokémon.  
    /// Actualiza el estado del Pokémon para indicar si puede atacar o no durante su turno.
    /// 




    /// </summary>  
    /// <param name="pokemon">El Pokémon al que se le aplicará el efecto de paralización.</param>  
    public override void HacerEfecto(Pokemon pokemon)
    {
        pokemon.SetPuedeAtacar(Jugar(pokemon));
    }
}