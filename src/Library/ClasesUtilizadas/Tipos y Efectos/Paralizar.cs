using DefaultNamespace;
using Library.Tipos.Paralisis_Strategy;

namespace Library.Tipos;

//Paralizar:
// SRP: La clase Paralizar tiene una responsabilidad clara: gestionar el efecto de la paralización en un Pokémon. 
//OCP: La clase Paralizar es fácil de extender en caso de que se desee añadir nuevas lógicas de efectos, como 
//diferentes probabilidades de paralización, sin modificar el código actual. 
//LSP:  La clase Paralizar hereda de Efecto y se puede usar en cualquier lugar donde se espere un objeto Efecto


public class Paralizar:Efecto
{
    private IEfectoParalisisStrategy paralisisEfecto;
    /// <summary>  
    /// Inicializa una nueva instancia de la clase <see cref="Paralizar"/>.  
    /// </summary>
    public Paralizar()
    {
        this.paralisisEfecto = new EfectoParalisisRandom();
    }
    public void SetStrategyParalisis(IEfectoParalisisStrategy efecto)
    {
        this.paralisisEfecto = efecto;
    }
    public IEfectoParalisisStrategy GetStrategyParalisis()
    {
        return paralisisEfecto;
    }

    /// <summary>  
    /// Aplica el efecto de paralización al Pokémon.  
    /// Actualiza el estado del Pokémon para indicar si puede atacar o no durante su turno.  
    /// </summary>  
    /// <param name="pokemon">El Pokémon al que se le aplicará el efecto de paralización.</param>  
    public override string HacerEfecto(Pokemon pokemon)
    {
        bool valor = paralisisEfecto.GetValor();
        pokemon.SetPuedeAtacar(valor);
        if (valor)
        {
            return $"El pokemon {pokemon.GetName()} puede atacar en este turno a pesar de estar paralizado.";
        }

        return $"El pokemon {pokemon.GetName()} no puede atacar, ha sido paralizado.";
    }
}