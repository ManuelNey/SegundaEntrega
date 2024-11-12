using DefaultNamespace;

namespace Library.Combate;

//Clase SuperPocion:
//La clase SuperPocion tiene una responsabilidad clara y única: aplicar un efecto curativo a un Pokémon. No realiza ninguna 
//otra acción, por lo que su cohesión es altay cumple con SRP.
//La clase SuperPocion hereda de Item y sobrescribe el método AplicarEfecto de manera polimórfica. 
//Cumple con OCP: La clase SuperPocion está abierta a la extensión pero cerrada a la modificación. Si se desea agregar más 
//tipos de ítems, no es necesario modificar SuperPocion. En su lugar, se pueden crear nuevas clases que hereden de Item y 
//sobrescriban el método AplicarEfecto para aplicar efectos distintos.
//Cumple con LSP: La clase SuperPocion puede ser sustituida por cualquier otra clase que herede de Item sin alterar el 
//comportamiento esperado. 

public class Superpocion : Item
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Superpocion"/> con la cantidad especificada.
    /// </summary>
    /// <param name="cantidad">La cantidad de ítems "SuperPocion" disponibles.</param>
    public Superpocion(int cantidad) : base(cantidad) { }

    /// <summary>
    /// Aplica el efecto de curar a un Pokémon, restaurando 70 puntos de vida.
    /// </summary>
    /// <param name="pokemon">El Pokémon al que se le aplicará el efecto curativo.</param>
    public override string AplicarEfecto(Pokemon pokemon)
    {
        pokemon.Curar(70);
        return $"{pokemon.GetName()} recuperó 70 puntos de vida.";
    }
}