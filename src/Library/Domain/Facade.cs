using Library.Combate;

namespace Ucu.Poo.DiscordBot.Domain;

/// <summary>
/// Esta clase recibe las acciones y devuelve los resultados que permiten
/// implementar las historias de usuario. Otras clases que implementan el bot
/// usan esta <see cref="Facade"/> pero no conocen el resto de las clases del
/// dominio. Esta clase es un singleton.
/// </summary>
public class Facade
{
    private static Facade? _instance;

    // Este constructor privado impide que otras clases puedan crear instancias
    // de esta.
    private Facade()
    {
        this.WaitingList = new WaitingList();
        this.BattlesList = new BattlesList();
        this.Menu = new Menu();
    }

    /// <summary>
    /// Obtiene la única instancia de la clase <see cref="Facade"/>.
    /// </summary>
    public static Facade Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Facade();
            }

            return _instance;
        }
    }

    /// <summary>
    /// Inicializa este singleton. Es necesario solo en los tests.
    /// </summary>
    public static void Reset()
    {
        _instance = null;
    }
    
    private WaitingList WaitingList { get; }
    
    private BattlesList BattlesList { get; }
    public Menu Menu { get; private set; }

    /// <summary>
    /// Agrega un jugador a la lista de espera.
    /// </summary>
    /// <param name="displayName">El nombre del jugador.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public string AddTrainerToWaitingList(string displayName)
    {
        if (this.WaitingList.AddTrainer(displayName))
        {
            return $"{displayName} agregado a la lista de espera";
        }
        return $"{displayName} ya está en la lista de espera";
    }

    /// <summary>
    /// Remueve un jugador de la lista de espera.
    /// </summary>
    /// <param name="displayName">El jugador a remover.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public string RemoveTrainerFromWaitingList(string displayName)
    {
        if (this.WaitingList.RemoveTrainer(displayName))
        {
            return $"{displayName} removido de la lista de espera";
        }
        else
        {
            return $"{displayName} no está en la lista de espera";
        }
    }

    /// <summary>
    /// Obtiene la lista de jugadores esperando.
    /// </summary>
    /// <returns>Un mensaje con el resultado.</returns>
    public string GetAllTrainersWaiting()
    {
        if (this.WaitingList.Count == 0)
        {
            return "No hay nadie esperando";
        }

        string result = "Esperan: ";
        foreach (Trainer trainer in this.WaitingList.GetAllWaiting())
        {
            result = result + trainer.DisplayName + "; ";
        }
        
        return result;
    }

    /// <summary>
    /// Determina si un jugador está esperando para jugar.
    /// </summary>
    /// <param name="displayName">El jugador.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public string TrainerIsWaiting(string displayName)
    {
        Trainer? trainer = this.WaitingList.FindTrainerByDisplayName(displayName);
        if (trainer == null)
        {
            return $"{displayName} no está esperando";
        }
        
        return $"{displayName} está esperando";
    }


    private string CreateBattle(string playerDisplayName, string opponentDisplayName)
    {
        // Aunque playerDisplayName y opponentDisplayName no estén en la lista
        // esperando para jugar los removemos igual para evitar preguntar si
        // están para luego removerlos.
        this.WaitingList.RemoveTrainer(playerDisplayName);
        this.WaitingList.RemoveTrainer(opponentDisplayName);
        
        BattlesList.AddBattle(playerDisplayName, opponentDisplayName);
        return $"Comienza {playerDisplayName} vs {opponentDisplayName}";
    }

    /// <summary>
    /// Crea una batalla entre dos jugadores.
    /// </summary>
    /// <param name="playerDisplayName">El primer jugador.</param>
    /// <param name="opponentDisplayName">El oponente.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public string StartBattle(string playerDisplayName, string? opponentDisplayName)
    {
        Menu = new Menu();
        //Une a los jugadores a la partida de manera aleatoria
        string result = this.Menu.UnirJugadores(playerDisplayName);
        result += "\n" + this.Menu.UnirJugadores(opponentDisplayName);
        // El símbolo ? luego de Trainer indica que la variable opponent puede
        // referenciar una instancia de Trainer o ser null.
        Trainer? opponent;

        if (!OpponentProvided() && !SomebodyIsWaiting())
        {
            return "No hay nadie esperando";
        }

        if (!OpponentProvided()) // && SomebodyIsWaiting
        {
            opponent = this.WaitingList.GetAnyoneWaiting();

            // El símbolo ! luego de opponent indica que sabemos que esa
            // variable no es null. Estamos seguros porque SomebodyIsWaiting
            // retorna true si y solo si hay usuarios esperando y en tal caso
            // GetAnyoneWaiting nunca retorna null.
            return this.CreateBattle(playerDisplayName, opponent!.DisplayName);
        }

        // El símbolo ! luego de opponentDisplayName indica que sabemos que esa
        // variable no es null. Estamos seguros porque OpponentProvided hubiera
        // retorna false antes y no habríamos llegado hasta aquí.
        opponent = this.WaitingList.FindTrainerByDisplayName(opponentDisplayName!);

        if (!OpponentFound())
        {
            return $"{opponentDisplayName} no está esperando";
        }

        return this.CreateBattle(playerDisplayName, opponent!.DisplayName) + "\n" + result;

        // Funciones locales a continuación para mejorar la legibilidad

        bool OpponentProvided()
        {
            return !string.IsNullOrEmpty(opponentDisplayName);
        }

        bool SomebodyIsWaiting()
        {
            return this.WaitingList.Count != 0;
        }

        bool OpponentFound()
        {
            return opponent != null;
        }
    }
    public string InitializeBattle()
    {
        return this.Menu.IniciarEnfrentamiento();
    }

    public string UsePokemonMove(int moveIndex)
    {
        return this.Menu.UsarMovimientos(moveIndex);
    }

    public string ShowAvailableMoves()
    {
        return this.Menu.MostrarAtaquesDisponibles();
    }

    public string ChangePokemon(int pokemonIndex)
    {
        return this.Menu.CambiarPokemon(pokemonIndex);
    }

    public string ShowPlayerStatus()
    {
        return this.Menu.MostrarEstadoEquipo();
    }

    public string ShowOpponentStatus()
    {
        return this.Menu.MostrarEstadoRival();
    }

    public bool IsBattleOngoing()
    {
        return this.Menu.GetBatallaI() && !this.Menu.GetBatallaT();
    }
    public string AddPokemosA(string pokemon)
    {
        return this.Menu.AgregarPokemonesA(pokemon);
    }
    public string JugadorA()
    {
        return Menu.JugadorA().GetName();
    }
    public string AddPokemosD(string pokemon)
    {
        return this.Menu.AgregarPokemonesD(pokemon);
    }
    public string JugadorD()
    {
        return Menu.JugadorD().GetName();
    }
    
    public string ShowPokemonNum()
    {
        return this.Menu.MostrarNumPokemon();
    }

    public string ShowAviableItems()
    {
        return this.Menu.MostrarItemsDisponibles();
    }

    public string ShowCatolog()
    {
        return Menu.MostrarCatalogo();
    }

    public string UseItem(string item, int numeroPokemon)
    {
        return this.Menu.UsarItem(item, numeroPokemon);
    }
    public string ShowAtualPokemonA()
    {
        return Menu.GetNamePokemonA();
    }
    
    public string ShowAtualPokemonD()
    {
        return Menu.GetNamePokemonD();
    }

}