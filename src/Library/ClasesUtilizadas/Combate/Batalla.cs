using System.ComponentModel.Design;
using DefaultNamespace;
using Library.Tipos;
using Ucu.Poo.Pokemon;

namespace Library.Combate
{
    //Clase Batalla:
    //La clase Batalla cumple con el principio de Responsabilidad Única (SRP) porque se encarga 
    //exclusivamente de la lógica relacionada con el manejo de una batalla entre dos jugadores. 
    //Esto incluye iniciar y terminar la batalla, avanzar los turnos, y determinar el estado de 
    //la batalla (si ha terminado o no).
    //Esto hace que también cumpla con Expert al gestionar únicamente la lógica de la batalla y
    //ser experta en ello.
    public class Batalla
    {
        private bool Turnos { get; set; }
        private List<Jugador> JugadoresEnEspera { get; set; }
        private Jugador JugadorAtacante { get; set; }
        private Jugador JugadorDefensor { get; set; }
        private bool BatallaTerminada { get; set; }
        public bool BatallaIniciada { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase Batalla, estableciendo los valores iniciales.
        /// </summary>
        public Batalla()
        {
            this.Turnos = true;
            this.BatallaTerminada = false;
            this.BatallaIniciada = false;
        }

        /// <summary>
        /// Recibe un ataque y lo aplica al Pokémon defensor.
        /// </summary>
        /// <param name="ataque">El movimiento de ataque que se aplica al defensor.</param>

        public string RecibirAtaqueB(IMovimientoAtaque ataque)
        {
            return JugadorDefensor.PokemonAtacado(ataque);
        }


        /// <summary>
        /// Agrega un jugador a la batalla, asignándolo como atacante o defensor.
        /// </summary>
        /// <param name="jugador">El jugador que se va a agregar a la batalla.</param>
        public string AgregarJugador(Jugador jugador)
        {
            if (JugadorDefensor == null && JugadorAtacante == null)
            {
                // Asigna aleatoriamente al jugador como defensor o atacante si ambos están vacíos.
                if (new Random().Next(1, 3) == 1)
                {
                    JugadorDefensor = jugador;
                    return $"{JugadorDefensor.GetName()} te va tocar esperar, empieza tu oponente";
                }
                JugadorAtacante = jugador;
                return $"{JugadorAtacante.GetName()} tu empezaras el combate";
            }

            if (JugadorDefensor == null)
            {
                JugadorDefensor = jugador;
                return $"{JugadorDefensor.GetName()} te va tocar esperar, empieza tu oponente";
            }
            JugadorAtacante = jugador;
            return $"{JugadorAtacante.GetName()} tu empezaras el combate";
        }

        /// <summary>
        /// Obtiene el jugador atacante actual.
        /// </summary>
        /// <returns>El jugador que está atacando en la batalla.</returns>
        public Jugador GetAtacante()
        {
            return JugadorAtacante;
        }

        /// <summary>
        /// Agrega un Pokémon al equipo del jugador atacante.
        /// </summary>
        /// <param name="pokemon">El nombre del Pokémon que se agrega al equipo.</param>
        public string AgregarPokemonBA(string pokemon)
        {
            if (!BatallaIniciada)
            {
                return JugadorAtacante.AgregarAlEquipo(pokemon);
            }

            return "La batalla ya ha iniciado";
        }
        
        /// <summary>
        /// Agrega un Pokémon al equipo del jugador defensor.
        /// </summary>
        /// <param name="pokemon">El nombre del Pokémon que se agrega al equipo.</param>
        public string AgregarPokemonBD(string pokemon)
        {
            if (!BatallaIniciada)
            {
                return JugadorDefensor.AgregarAlEquipo(pokemon);
            }

            return "La batalla ya ha iniciado";
        }

        /// <summary>
        /// Obtiene el Pokémon actual del jugador atacante.
        /// </summary>
        /// <returns>El Pokémon en turno del jugador atacante.</returns>
        public Pokemon GetPokemonActualB()
        {
            return JugadorAtacante.GetPokemonEnTurno();
        }
        
        /// <summary>
        /// Obtiene el valor de vida del Pokémon defensor en turno.
        /// </summary>
        /// <returns>El valor de vida del Pokémon defensor.</returns>
        public double GetHpDefensorB()
        {
            return JugadorDefensor.HpPokemonEnTurno();
        }

        /// <summary>
        /// Obtiene el valor de vida del Pokémon atacante en turno.
        /// </summary>
        /// <returns>El valor de vida del Pokémon atacante.</returns>
        public double GetHpAtacanteB()
        {
            return JugadorAtacante.HpPokemonEnTurno();
        }

        /// <summary>
        /// Obtiene el jugador defensor actual.
        /// </summary>
        /// <returns>El jugador que está defendiendo en la batalla.</returns>
        public Jugador GetDefensor()
        {
            return JugadorDefensor;
        }

        /// <summary>
        /// Obtiene el estado de la batalla, indicando si está terminada o no.
        /// </summary>
        /// <returns>El estado de la batalla (terminada o no).</returns>
        public bool GetBatallaTerminada()
        {
            return BatallaTerminada;
        }
        /// <summary>
        /// Obtiene el estado de la batalla, indicando si ha sido iniciada.
        /// </summary>
        /// <returns>El estado de la batalla (iniciada o no).</returns>
        public bool GetBatallaIniciada()
        {
            return BatallaIniciada;
        }

        public List<Pokemon> GetTeamPokemonA()
        {
            return JugadorAtacante.GetPokemons();
        }
        /// <summary>
        /// Inicia la batalla si ambos jugadores tienen Pokémon en sus equipos y la batalla no ha comenzado.
        /// </summary>
        public string IniciarBatalla()
        {
            string texto ="..........\n";
            // Verifica si ambos jugadores tienen equipos y la batalla no ha comenzado
            if (!BatallaIniciada && JugadorAtacante.GetCantpokemon() > 0 && JugadorDefensor.GetCantpokemon()> 0 && JugadorAtacante != null && JugadorDefensor != null)
            {
                BatallaIniciada = true;
                return $"La batalla ha iniciado, comienza el jugador {JugadorAtacante.GetName()}";
            }
            return ($"La batalla ya ha comenzado o uno de los jugadores no tiene Pokémon.");
        }

        /// <summary>
        /// Finaliza la batalla si alguno de los jugadores ha perdido todos sus Pokémon.
        /// </summary>
        public string TerminarBatalla()
        {
            string texto = "";
            // Revisa si alguno de los equipos ha perdido, si ya ha perdido cambia el bool
            if (!JugadorAtacante.TeamIsAlive())
            {
                texto += $"¡Ha ganado el jugador {JugadorDefensor.GetName()}!" + " \nLa batalla ha terminado";
                BatallaTerminada = true;
            }
            else if (!JugadorDefensor.TeamIsAlive())
            {
                texto += $"¡Ha ganado el jugador {JugadorAtacante.GetName()}!" + " \nLa batalla ha terminado";
                BatallaTerminada = true;
            }
            return texto;
        }

        /// <summary>
        /// Verifica si el Pokémon defensor está debilitado y cambia al siguiente Pokémon si es necesario.
        /// </summary>
        private string VerificarPokemonDefensorDebilitado()
        {
            if (!JugadorDefensor.PokemonEnTurnoAlive())
            {
                foreach (var pokemon in JugadorDefensor.GetPokemons())
                {
                    if (pokemon.GetIsAlive())
                    {
                        Pokemon pokemonDebilitado = JugadorDefensor.GetPokemonEnTurno();
                        JugadorDefensor.CambiarPokemon(pokemon);
                        return $"{pokemonDebilitado.GetName()} ha sido debilitado y cambiado por {JugadorDefensor.GetNamePokemonTurno()} automáticamente";
                    }
                }
                return ($"A {JugadorDefensor.GetName()} no le quedan más Pokémon en condiciones de combatir.") +  TerminarBatalla();
            }
            return "";
        }

        /// <summary>
        /// Avanza al siguiente turno de la batalla, alternando entre los jugadores y verificando si alguno de los equipos ha perdido.
        /// </summary>
        public string AvanzarTurno()
        {
            string texto = "";
            texto += VerificarPokemonDefensorDebilitado()+ "\n";

            if (BatallaTerminada)
            {
                return "La batalla ha terminado.";
            }

            if (JugadorDefensor.GetEfectoPokemonTurno() != null)
            {
                Pokemon pokemon = JugadorDefensor.GetPokemonEnTurno();
                texto += JugadorDefensor.HacerEfectoPokemonEnTurno(pokemon); 
            }

            CambiarTurno();

            if (!JugadorAtacante.GetPokemonEnTurnoAtaca())
            {
                texto += $"{JugadorAtacante.GetName()} no puede atacar este turno.\n";
                AvanzarTurno();
            }
            else
            {
                JugadorDefensor.ActualizarEstadoEquipo();
                JugadorAtacante.ActualizarEstadoEquipo();
            }
            if (!JugadorAtacante.TeamIsAlive() || !JugadorDefensor.TeamIsAlive())
            {
                TerminarBatalla();
                texto += TerminarBatalla();
                return texto;
            }
            else
            {
                texto += $"Es el turno de {JugadorAtacante.GetName()} con el Pokémon {JugadorAtacante.GetNamePokemonTurno()}.";
            }

            return texto;
        }

        /// <summary>
        /// Cambia el turno entre el jugador atacante y el defensor. El atacante es el defensor y viceversa
        /// </summary>
        private string CambiarTurno()
        {
            Jugador temporal = JugadorAtacante;
            JugadorAtacante = JugadorDefensor;
            JugadorDefensor = temporal;
            Turnos = !Turnos;
            return $"Ahora es turno de {JugadorAtacante.GetName()} \n";
        }

        public string Rendirme()
        {
            if (BatallaTerminada)
            {
                return "La batalla ya terminó";
            }
            if (!BatallaIniciada)
            {
                return "La batalla no inició aún";
            }

            return this.JugadorAtacante.Rendirme()+this.TerminarBatalla();
        }
        
    }
}
