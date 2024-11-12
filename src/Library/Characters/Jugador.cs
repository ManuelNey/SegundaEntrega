using System.Text;
using DefaultNamespace;
using Library.Tipos;
using Ucu.Poo.Pokemon;

namespace Library.Combate;
//Clase Jugador:
//SRP se aplica al gestionar la información del jugador, como su nombre, equipo e inventario.
//Expert está presente, ya que la clase maneja sus propios datos y toma decisiones basadas en ellos.

/// <summary>
/// La clase Jugador representa a un jugador que posee un equipo de Pokémon y un inventario de items.
/// </summary>
/// <remarks>
/// Se aplican los principios de SRP (Responsabilidad Única) al gestionar la información del jugador y sus interacciones con el equipo y los items.
/// El principio Expert también se sigue, ya que la clase toma decisiones relacionadas con el estado del jugador y su equipo.
/// </remarks>
public class Jugador
{
    private string name;
    private List<Pokemon> listaPokemons;
    private Pokemon pokemonEnTurno;
    private bool teamIsAlive;
    private InventarioItems inventarioJugador;

    /// <summary>
    /// Constructor que inicializa un nuevo jugador con un nombre.
    /// </summary>
    /// <param name="name">El nombre del jugador.</param>
    public Jugador(string name)
    {
        this.name = name;
        this.listaPokemons = new List<Pokemon>();
        this.teamIsAlive = true;
        this.inventarioJugador = new InventarioItems();
    }
    /// <summary>
    /// Obtiene si el Pokémon en turno puede atacar.
    /// </summary>
    /// <returns><c>true</c> si el Pokémon puede atacar, <c>false</c> en caso contrario.</returns>
    public bool GetPokemonEnTurnoAtaca()
    {
        return pokemonEnTurno.GetPuedeAtacar();
    }
    /// <summary>
    /// Aplica el efecto del Pokémon en turno sobre otro Pokémon.
    /// </summary>
    /// <param name="pokemon">El Pokémon al que se le aplicará el efecto.</param>
    public void HacerEfectoPokemonEnTurno(Pokemon pokemon)
    {
        pokemonEnTurno.HacerEfectoPokemon(pokemon);
    }
    
    /// <summary>
    /// Obtiene el efecto que tiene el Pokémon en turno.
    /// </summary>
    /// <returns>El efecto actual sobre el Pokémon en turno.</returns>
    public Efecto GetEfectoPokemonTurno()
    {
        return pokemonEnTurno.GetEfecto();
    }
    
    /// <summary>
    /// Recibe un ataque hacia el Pokémon en turno.
    /// </summary>
    /// <param name="ataque">El movimiento de ataque que se va a recibir.</param>
    public string PokemonAtacado(IMovimientoAtaque ataque)
    {
       return pokemonEnTurno.RecibirAtaque(ataque);
    }

    /// <summary>
    /// Obtiene la cantidad de Pokémon que tiene el jugador.
    /// </summary>
    /// <returns>La cantidad de Pokémon en el equipo del jugador.</returns>
    public int GetCantpokemon()
    {
        return listaPokemons.Count;
    }

    /// <summary>
    /// Obtiene el nombre del Pokémon en turno.
    /// </summary>
    /// <returns>El nombre del Pokémon en turno.</returns>
    public string GetNamePokemonTurno()
    {
        return pokemonEnTurno.GetName();
    }

    /// <summary>
    /// Verifica si el Pokémon en turno está vivo.
    /// </summary>
    /// <returns><c>true</c> si el Pokémon está vivo, <c>false</c> en caso contrario.</returns>
    public bool PokemonEnTurnoAlive()
    {
        return pokemonEnTurno.GetIsAlive();
    }
    
    /// <summary>
    /// Obtiene el nombre del jugador.
    /// </summary>
    /// <returns>El nombre del jugador.</returns>
    public string GetName()
    {
        return this.name;
    }

    /// <summary>
    /// Verifica si el equipo del jugador está vivo.
    /// </summary>
    /// <returns><c>true</c> si el equipo está vivo, <c>false</c> en caso contrario.</returns>
    public bool TeamIsAlive()
    {
        return this.teamIsAlive;
    }
   
    /// <summary>
    /// Obtiene la lista de Pokémon del jugador.
    /// </summary>
    /// <returns>La lista de Pokémon del jugador.</returns>
    public List<Pokemon> GetPokemons()
    {
        return listaPokemons;
    }
    

    /// <summary>
    /// Agrega un Pokémon al equipo del jugador.
    /// </summary>
    /// <param name="nombre">El nombre del Pokémon que se quiere agregar al equipo.</param>
    public string AgregarAlEquipo(string nombre)
    {
        
        if (listaPokemons.Count < 6)
        {
            Pokemon pokemonencontrado = Pokedex.EntregarPokemon(nombre);
            if (pokemonencontrado != null)
            {
                
                listaPokemons.Add(pokemonencontrado);
                if (listaPokemons.Count == 1)
                {
                    pokemonEnTurno = pokemonencontrado;
                }
                //Console.Writeline($"Se añadió el pokemon {pokemonencontrado.GetName()} a tu equipo, ¿vas a seguir añadiendo más?")
                return $"Se añadió el pokemon {pokemonencontrado.GetName()} a tu equipo, ¿vas a seguir añadiendo más?";
            }
        }
        else
        {
            //Console.Writeline("Ya tienes 6 Pokemons!")
            return "Ya tienes 6 Pokemons!";
        }

        return null;
    }
    
    /// <summary>
    /// Actualiza el estado del equipo, verificando si al menos un Pokémon está vivo.
    /// </summary>
    public void ActualizarEstadoEquipo()
    {
        bool equipoestado = listaPokemons.Any(pokemon => pokemon.GetIsAlive());
        this.teamIsAlive = equipoestado;
    }

    /// <summary>
    /// Cambia el Pokémon en turno por otro del equipo.
    /// </summary>
    /// <param name="pokemon">El Pokémon que se desea poner en turno.</param>
    public void CambiarPokemon(Pokemon pokemon)
    {
        if (listaPokemons.Contains(pokemon))
        {
            int indicepokemonAremplazar = listaPokemons.IndexOf(pokemon);
            Pokemon pokemonAnterior = listaPokemons[0];
            listaPokemons[0] = pokemon;
            listaPokemons[indicepokemonAremplazar] = pokemonAnterior;
            pokemonEnTurno = pokemon;
        }
    }

    /// <summary>
    /// Obtiene el Pokémon en turno.
    /// </summary>
    /// <returns>El Pokémon que está en turno.</returns>
    public Pokemon GetPokemonEnTurno()
    {
        return pokemonEnTurno;
    }

    /// <summary>
    /// Obtiene los puntos de vida actuales del Pokémon en turno.
    /// </summary>
    /// <returns>La cantidad de vida actual del Pokémon en turno.</returns>
    public double HpPokemonEnTurno()
    {
        return pokemonEnTurno.GetVidaActual();
    }

    /// <summary>
    /// Muestra el estado de todos los Pokémon del equipo del jugador.
    /// </summary>
    public string MostarEstadoEquipo()
    {
        //Console.WriteLine($"El estado del equipo de {name} es:");
        foreach (Pokemon pokemon in listaPokemons)
        {
            if (pokemon.GetIsAlive())
            {
                //Console.WriteLine($"{pokemon.GetName()} {pokemon.GetVidaActual()}/{pokemon.GetVidaTotal()}");
                return $"El estado del equipo de {{name}} es:" +
                       $"{pokemon.GetName()} {pokemon.GetVidaActual()}/{pokemon.GetVidaTotal()}";
            }
            else
            {
                //Console.WriteLine($"{pokemon.GetName()} ha muerto");
                return $"El estado del equipo de {{name}} es:" +
                       $"{pokemon.GetName()} ha muerto";
            }
        }

        return null;
    }

    /// <summary>
    /// Usa un item del inventario sobre un Pokémon.
    /// </summary>
    /// <param name="item">El nombre del item a usar.</param>
    /// <param name="pokemon">El Pokémon sobre el que se va a usar el item.</param>
    public string UsarItem(string item, Pokemon pokemon)
    {
        if (listaPokemons.Contains(pokemon))
        {
            int IndicePokemonAEfectuar = listaPokemons.IndexOf(pokemon);
            Pokemon PokemonAEfectuar = listaPokemons[IndicePokemonAEfectuar];
            return inventarioJugador.UsarItem(item, PokemonAEfectuar);
        }

        return null;
    }

    /// <summary>
    /// Muestra todos los items disponibles en el inventario del jugador.
    /// </summary>
    public List<string> Mostrar_items() //Este método llama al mostrar items de InventarioItems para mostrar los items disponibles que tiene el jugador
    {
       return inventarioJugador.MostrarItems();
    }
}