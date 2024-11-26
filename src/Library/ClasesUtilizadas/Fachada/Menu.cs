using System.Net.Mime;
using DefaultNamespace;
using Library.Tipos;
using Ucu.Poo.Pokemon;

namespace Library.Combate
{
    //Clase Menu:
    //La clase Menu cumple con el Principio de Responsabilidad Única (SRP) al gestionar exclusivamente 
    //las interacciones del menú para la batalla de Pokémon, y en el Principio de Abierto/Cerrado (OCP), 
    //ya que permite la extensión sin modificar la funcionalidad existente. Además, sigue el Principio de 
    //Inversión de Dependencias (DIP) al depender de abstracciones como Jugador e IMovimiento, y el Principio 
    //de Sustitución de Liskov (LSP) al permitir el uso de diferentes implementaciones de IMovimiento.


    public class Menu
    {
        public List<string> Textohistorial = new List<string>();
        private Batalla batallaActual;

        /// <summary>
        /// Constructor de la clase `Menu`.
        /// Inicializa un nuevo objeto `Menu` y una instancia de `Batalla`.
        /// </summary>
        public Menu()
        {
            batallaActual = new Batalla();
        }

        /// <summary>
        /// Une un jugador a la batalla.
        /// </summary>
        /// <param name="jugador">Nombre del jugador que se unirá.</param>
        public string UnirJugadores(string jugador)
        {
            return batallaActual.AgregarJugador(new Jugador(jugador));
        }

        /// <summary>
        /// Verifica si la batalla ha terminado.
        /// </summary>
        public bool GetBatallaT()
        {
            return batallaActual.GetBatallaTerminada();
        }

        /// <summary>
        /// Verifica si la batalla ha iniciado.
        /// </summary>
        public bool GetBatallaI()
        {
            return batallaActual.GetBatallaIniciada();
        }

        /// <summary>
        /// Obtiene el Pokémon actual del jugador rival.
        /// </summary>
        public Pokemon GetPokemonRival()
        {
            Jugador defensor = batallaActual.GetDefensor();
            return defensor.GetPokemonEnTurno();
        }

        /// <summary>
        /// Obtiene la salud del defensor.
        /// </summary>
        public double GetHpDefensor()
        {
            return batallaActual.GetHpDefensorB();
        }

        /// <summary>
        /// Obtiene la salud del atacante.
        /// </summary>
        public double GetHpAtacante()
        {
            return batallaActual.GetHpAtacanteB();
        }

        /// <summary>
        /// Obtiene el equipo del atacante.
        /// </summary>
        public List<Pokemon> GetEquipoA()
        {
            Jugador jugadorA = batallaActual.GetAtacante();
            return jugadorA.GetPokemons();
        }

        /// <summary>
        /// Obtiene el Pokémon en turno del atacante.
        /// </summary>
        public Pokemon GetPokemonActual()
        {
            return batallaActual.GetPokemonActualB();
        }

        /// <summary>
        /// Agrega un Pokémon al equipo del atacante.
        /// </summary>
        public string AgregarPokemonesA(string pokemon)
        {
            return batallaActual.AgregarPokemonBA(pokemon);
        }

        /// <summary>
        /// Propiedad para obtener al jugador A
        /// </summary>
        public Jugador JugadorA()
        {
            return batallaActual.GetAtacante();
        }

        /// <summary>
        /// Agrega un Pokémon al equipo del defensor.
        /// </summary>
        public string AgregarPokemonesD(string pokemon)
        {
            return batallaActual.AgregarPokemonBD(pokemon);
        }

        /// <summary>
        /// Propiedad para obtener al jugador D
        /// </summary>
        public Jugador JugadorD()
        {
            return batallaActual.GetDefensor();
        }

        /// <summary>
        /// Inicia el enfrentamiento de batalla.
        /// </summary>
        public string IniciarEnfrentamiento()
        {
            return batallaActual.IniciarBatalla();
        }

        /// <summary>
        /// Muestra el estado del equipo del jugador rival.
        /// </summary>
        public string MostrarEstadoRival()
        {
            if (batallaActual.GetBatallaIniciada())
            {
                Jugador defensor = batallaActual.GetDefensor();
                return defensor.MostarEstadoEquipo();
            }

            return "La batalla no ha iniciado";
        }

        /// <summary>
        /// Muestra el estado del equipo del jugador actual.
        /// </summary>
        public string MostrarEstadoEquipo()
        {
            if (batallaActual.GetBatallaIniciada())
            {
                Jugador atacante = batallaActual.GetAtacante();
                return atacante.MostarEstadoEquipo();
            }

            return "La batalla no ha iniciado";
        }

        /// <summary>
        /// Cambia el Pokémon en turno del jugador atacante.
        /// </summary>
        public string CambiarPokemon(int numeroDePokemon)
        {
            if (batallaActual.GetBatallaIniciada())
            {
                Jugador jugadorAtacante = batallaActual.GetAtacante();
                List<Pokemon> pokemons = jugadorAtacante.GetPokemons();

                if (numeroDePokemon > 0 && numeroDePokemon < pokemons.Count)
                {
                    Pokemon pokemonElegido = pokemons[numeroDePokemon];

                    if (pokemonElegido.GetIsAlive())
                    {
                        Pokemon pokemon = jugadorAtacante.GetPokemonEnTurno();
                        jugadorAtacante.CambiarPokemon(pokemonElegido);
                        string texto =
                            $"El Pokémon {pokemonElegido.GetName()} ha entrado en combate y {pokemon.GetName()} ha sido guardado en su pokebola";
                        texto += batallaActual.AvanzarTurno();
                        return texto;
                    }

                    return $"El Pokémon {pokemonElegido.GetName()} está debilitado y no puede entrar en combate";
                }

                if (numeroDePokemon == 0)
                {
                    return ("No puede cambiar al pokemon que ya está atacando");
                }

                return "No tienes ese pokemon";
            }

            return "La batalla no ha iniciado";
        }

        /// <summary>
        /// Muestra los movimientos disponibles del Pokémon en turno.
        /// </summary>
        public string MostrarAtaquesDisponibles()
        {
            if (batallaActual.GetBatallaIniciada())
            {

                Jugador jugadorAtacante = batallaActual.GetAtacante();
                Pokemon pokemon = jugadorAtacante.GetPokemonEnTurno();
                string texto =
                    $"El Pokémon {pokemon.GetName()} tiene los siguientes movimientos y sus numeros para ser utilizados son los siguientes:\n";
                List<IMovimiento> movimientos = pokemon.GetListaMovimientos();
                for (int i = 0; i < movimientos.Count; i++)
                {

                    if (movimientos[i] is IMovimientoEspecial especial && especial.GetUsadoAnteriormente())
                    {
                        texto += $"{i + 1}.{movimientos[i].GetName()}(especial) no puede ser usado en este turno\n";
                    }

                    else if (movimientos[i] is IMovimientoEspecial movimientoEspecial)
                    {
                        texto += $"{i + 1}.{movimientoEspecial.GetName()} (especial)\n";
                    }
                    else
                    {
                        texto += $"{i + 1}.{movimientos[i].GetName()}\n";
                    }
                }

                texto += "Para usarlos, ingresa el número correspondiente a cada movimiento.\n";
                return texto;
            }

            return "La batalla no ha iniciado";
        }

        /// <summary>
        /// Usa un movimiento del Pokémon en turno.
        /// </summary>
        public string UsarMovimientos(int numDeMovimiento)
        {
            if (batallaActual.GetBatallaTerminada())
            {
                return "La batalla ya ha terminado.";
            }

            if (!batallaActual.GetBatallaIniciada())
            {
                return "La batalla no ha iniciado";
            }

            string texto = "";
            string TextoHistorial = "";
            Jugador jugador = batallaActual.GetAtacante();
            Pokemon pokemonActual = jugador.GetPokemonEnTurno();
            List<IMovimiento> movimientos = pokemonActual.GetListaMovimientos();

            if (numDeMovimiento > 0 && numDeMovimiento <= movimientos.Count)
            {
                IMovimiento movimiento = movimientos[numDeMovimiento - 1];

                if (movimiento is IMovimientoEspecial especial && especial.GetUsadoAnteriormente())
                {
                    texto +=
                        $"El movimiento {movimiento.GetName()} es especial y ya fue usado anteriormente. Elija otro movimiento.";
                    TextoHistorial +=
                        $"{batallaActual.GetAtacante().GetName()} ha tratado de usar el {movimiento.GetName()} con {pokemonActual} pero no pudo porque ya lo uso";
                }
                else
                {
                    string accion = pokemonActual.UsarMovimiento(movimiento);
                    texto += accion;
                    TextoHistorial += accion;

                    if (movimiento is IMovimientoAtaque movimientoAtaque)
                    {
                        texto += ($"{pokemonActual.GetName()} ha usado {movimiento.GetName()}.");
                        TextoHistorial +=
                            $"{batallaActual.GetAtacante().GetName()} ha usado {pokemonActual.GetName()} que ha usado {movimiento.GetName()}.";
                        Random random = new Random();
                        int numeroAleatorio = random.Next(1, 101);
                        if (numeroAleatorio <= movimientoAtaque.GetPrecision())
                        {
                            texto += "Y ha acertado.";
                            texto += $"{batallaActual.RecibirAtaqueB(movimientoAtaque)}\n";
                            TextoHistorial += "Y ha hacertado. ";
                            TextoHistorial+= $"{batallaActual.RecibirAtaqueB(movimientoAtaque)}\n";
                            
                        }
                        else
                        {
                            texto += ("Y ha fallado.\n");
                            TextoHistorial += "Y ha fallado.\n";
                        }

                        if (movimientoAtaque is IMovimientoEspecial movimientoEspecial)
                        {
                            movimientoEspecial.UsadoAnteriormente(true);
                        }
                    }
                    Textohistorial.Add(TextoHistorial);
                    texto += batallaActual.AvanzarTurno();
                }
                
                return texto;
            }

            return
                "Movimiento inválido. Por favor, seleccione un movimiento entre 1 y 4, o uno que pueda usarse en este turno.";
        }

        /// <summary>
        /// Muestra el número de Pokémon del equipo atacante.
        /// </summary>
        public string MostrarNumPokemon()
        {
            string texto = "";
            if (batallaActual.GetBatallaIniciada())
            {
                Jugador jugador = batallaActual.GetAtacante();
                List<Pokemon> listaPokemons = jugador.GetPokemons();
                texto += $"{listaPokemons[0].GetName()} está en turno\n ";
                for (int i = 1; i < listaPokemons.Count; i++)
                {
                    Pokemon pokemon = listaPokemons[i];
                    texto += $"el número {i} es {pokemon.GetName()} \n";
                }
            }

            return texto;
        }

        /// <summary>
        /// Muestra el número de Pokémon del equipo atacante.
        /// </summary>
        public string MostrarCatalogo()
        {
            return Pokedex.MostrarCatalogo();
        }

        /// <summary>
        /// Muestra los ítems disponibles en el inventario del jugador atacante.
        /// </summary>
        public string MostrarItemsDisponibles()
        {
            if (batallaActual.GetBatallaIniciada())
            {
                Jugador jugador = batallaActual.GetAtacante();
                return jugador.MostrarItems();
            }

            return "";
        }

        /// <summary>
        /// Usa un ítem específico en el Pokémon indicado.
        /// </summary>

        public string UsarItem(string item, int numeroDePokemon)
        {
            string texto = "";

            if (!batallaActual.GetBatallaIniciada())
            {
                return "La batalla no ha iniciado";
            }

            Jugador jugadorAtacante = batallaActual.GetAtacante();

            if (!jugadorAtacante.ItemInInventory(item))
            {
                return "Item no disponible o cantidad insuficiente, porfavor revisar inventario";
            }

            List<Pokemon> pokemons = jugadorAtacante.GetPokemons();

            if (numeroDePokemon < 0 || numeroDePokemon >= pokemons.Count)
            {
                return "Seleccione el Pokémon correctamente";
            }

            Pokemon pokemonElegido = pokemons[numeroDePokemon];

            try
            {
                texto += jugadorAtacante.UsarItem(item, pokemonElegido); //intenta utilizar el item
                texto += batallaActual.AvanzarTurno();
            }
            catch (ReviveException e) //Si no se pudo se catchea si es por no poder revivirlo
            {
                return "No se puede revivir a un Pokémon que no está debilitado.";
            }
            catch (CureException e) //Se revisa si es porque no se puede curar
            {
                return "No se puede curar a un Pokémon debilitado.";
            }
            catch (OverflowException e) //Si no se pudo se catchea si es por no poder revivirlo
            {
                return "No deberías de curar a un pokemon que ya tiene toda su vida.";
            }
            catch (NullReferenceException e) //Se revisa si es porque no se puede curar
            {
                return "El pokemon no está bajo ningún efecto, no hay porque usar un curatotal";
            }

            return texto;
        }

        public string GetNamePokemonA()
        {
            return JugadorA().GetNamePokemonTurno();
        }

        public string GetNamePokemonD()
        {
            return JugadorD().GetNamePokemonTurno();
        }

        public string Rendirse()
        {
            return this.batallaActual.Rendirme();
        }

        public List<string> EnviarLista()
        {
            return Textohistorial;
        }
        
    }
}