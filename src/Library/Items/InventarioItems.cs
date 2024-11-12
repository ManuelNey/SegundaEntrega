using DefaultNamespace;

namespace Library.Combate;
//Clase InventarioItems:
//Cumple con alta chesion, ya que tiene una responsabilidad única, que es gestionar los ítems del jugador (registrar, 
//mostrar y usar ítems). La clase es coherente, ya que solo maneja los aspectos relacionados con el inventario de ítems, 
//sin involucrarse en otras responsabilidades. Tambien cumple con SRP por la misma razon.
//El método UsarItem utiliza polimorfismo al llamar a AplicarEfecto de la clase Item, que es un método polimórfico. Esto 
//permite que, dependiendo del tipo de ítem, se ejecute el comportamiento específico sin tener que modificar la lógica en 
//la clase InventarioItems.
//Cumple con OCP, ya que la clase está abierta a la extensión (por ejemplo, se pueden agregar nuevos tipos de ítems como 
//nuevas clases que heredan de Item), pero está cerrada a la modificación, ya que no es necesario modificar InventarioItems 
//para agregar nuevos ítems.
//Cumple con LSP: Cualquier clase que herede de Item puede ser usada en lugar de un Item sin afectar el comportamiento del 
//sistema. Si se agrega un nuevo tipo de ítem, se puede manejar dentro de InventarioItems sin problemas.


public class InventarioItems
{
    private Dictionary<String, Item> items;
    private Superpocion superpocion;
    private Revivir revivir;
    private Curatotal curatotal;
    
    /// <summary>
    /// Constructor que inicializa el inventario con una lista de ítems predefinidos.
    /// </summary>
    public InventarioItems()
    {
        items = new Dictionary<String, Item> //Crea un diccionario en el que registra cada item y cuanta cantidad hay de cada uno
        {
            { "Superpocion",  superpocion = new Superpocion(4) },
            { "Revivir", revivir = new Revivir(1) },
            { "Curatotal", curatotal = new Curatotal(2) }
        };
    }

    /// <summary>
    /// Muestra en consola los ítems disponibles en el inventario y su cantidad.
    /// </summary>
    public List<string> MostrarItems() //Imprime en pantalla cuales items y cuantos de cada uno le queda al jugador 
    {
        List<string> listaitems = new List<string>();
        foreach (var item in items)
        {
            listaitems.Add($"{item.Key}: {item.Value.Cantidad} disponibles");
        }

        return listaitems;
    }

    /// <summary>
    /// Utiliza un ítem del inventario para aplicar su efecto sobre el Pokémon.
    /// </summary>
    /// <param name="item">El nombre del ítem a usar.</param>
    /// <param name="pokemon">El Pokémon al que se le aplicará el efecto del ítem.</param>
    public string UsarItem(string item, Pokemon pokemon) //Busca el item que le pasaste, llama al AplicarEfecto para que haga su efecto y baja en 1 su cantidad
    {
        if (items.ContainsKey(item) && items[item].Cantidad > 0)
        {
            if (item == "Superpocion") //Si escribiste Superpocion, llamará al curar del revivir
            {
                superpocion.AplicarEfecto(pokemon);
                items[item].Cantidad--;
            }
            if (item == "Revivir") //Si escribiste Revivir, llamará al revivir del jugador
            {
                revivir.AplicarEfecto(pokemon);
                items[item].Cantidad--;
            }
            if (item == "Curatotal")//Si escribiste Curatotal, llamará al CurarEstado del jugador
            {
                curatotal.AplicarEfecto(pokemon);
                items[item].Cantidad--;
            }
            else
            {
                //Console.WriteLine("Seleccione una opcion correcta por favor, 'SuperPocion' para usar una superposión, 'Revivir' para usar un revivir o 'CuraTotal' para usar un curatotal");
                return "Seleccione una opcion correcta por favor, 'SuperPocion' para usar una superposión, 'Revivir' para usar un revivir o 'CuraTotal' para usar un curatotal";
            }
        }
        return "Ítem no disponible o cantidad insuficiente.";
    }
}