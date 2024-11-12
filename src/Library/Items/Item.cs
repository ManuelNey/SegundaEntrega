using DefaultNamespace;

namespace Library.Combate;

//Clase Item
//Tiene alta cohesion debido a que tiene la unica responsabilidad de representar un item (su cantidad y 
//el efecto que puede aplicar sobre un objeto del tipo Pokemon) y no mezcla funcionalidades ajenas. Por ende 
//al tener una unica reponsabilidad, tambien cumple con SRP.
//Cumple con Polimorfismo, ya que el Metodo AplicarEfecto tiene una implementacion diferente segun el tipo 
//de Item utilizado.
//Cumple con OCP debido a que esta clase está abierta para la extensión pero cerrada para la modificación. 
//Si se quieren agregar nuevos tipos de ítems, se pueden crear nuevas subclases de Item sin necesidad de modificar 
//la clase Item.
//Cumple con LSP, ya que Item es una clase abstracta, cualquier subclase de Item (como CuraTotal o SuperPocion) puede
//sustituir una instancia de Item sin romper el comportamiento esperado del sistema.

public abstract class Item
{

    /// <summary>
    /// Obtiene o establece la cantidad de este ítem disponible.
    /// </summary>
    /// <value>La cantidad de ítems disponibles.</value>
    public int Cantidad { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Item"/> con la cantidad especificada.
    /// </summary>
    /// <param name="cantidad">La cantidad inicial del ítem.</param>
    protected Item(int cantidad)
    {
        this.Cantidad = cantidad;
    }

    /// <summary>
    /// Aplica el efecto del ítem sobre un Pokémon especificado.
    /// Este método es abstracto y debe ser implementado en clases derivadas para aplicar el efecto específico de cada tipo de ítem.
    /// </summary>
    /// <param name="pokemon">El Pokémon al que se le aplicará el efecto del ítem.</param>
    public abstract string AplicarEfecto(Pokemon pokemon);
}