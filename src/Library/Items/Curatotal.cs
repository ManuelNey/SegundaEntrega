using DefaultNamespace;

namespace Library.Combate;
//Clase CuraTotal:
//Tiene alta cohesion, ya que su responsabilidad es clara: aplicar un efecto de curacion al Pokemon, 
//eilminando el efecto actual delpokemon si esta vivo.
//Cumple con polimorfismo debido a que sobreescribe el metodo Aplicar Efecto de la clase.
//Cumple con SRP debido a que tiene una sola responsabilidad: aplicar un efecto de curación a un Pokémon. 

public class Curatotal : Item
{
    /// <summary>
    /// Constructor de la clase `CuraTotal`.
    /// Inicializa un nuevo objeto `CuraTotal` con una cantidad especificada.
    /// </summary>
    /// <param name="cantidad">Cantidad de ítems disponibles.</param>
    public Curatotal(int cantidad) : base( cantidad) { }
    
    /// <summary>
    /// Aplica el efecto de curación al Pokémon indicado, eliminando todos sus efectos de estado,
    /// siempre que el Pokémon esté vivo y tenga algun efecto.
    /// </summary>
    /// <param name="pokemon">El Pokémon al que se le aplicará el efecto de curación.</param>
    public override string AplicarEfecto(Pokemon pokemon)
    {
        if (pokemon.GetIsAlive())
        {
            pokemon.EliminarEfectoActual();
            //$"{pokemon.GetName()} ha sido curado de todos los efectos de estado.")
            return $"{pokemon.GetName()} ha sido curado de todos los efectos de estado.";
        }
        if (!pokemon.GetIsAlive())
        {
            //Console.Writeline($"El pokemon {pokemon.GetName()} no se puede curar de ningun efecto debido a que esta muerto")
            return $"El pokemon {pokemon.GetName()} no se puede curar de ningun efecto debido a que esta muerto";
        }

        return null;
    }
}