using DefaultNamespace;
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
        private Batalla batallaActual;

        public Menu()
        {
            batallaActual = new Batalla();
        }

        public void UnirJugadores(string jugador)
        {
            batallaActual.AgregarJugador(new Jugador(jugador));
        }

        public bool GetBatallaT()
        {
            return batallaActual.GetBatallaTerminada();
        }
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

        public double GetHpDefensor()
        {
            return batallaActual.GetHpDefensorB(); 
        }
        public double GetHpAtacante()
        {
            return batallaActual.GetHpAtacanteB(); 
        }

        public List<Pokemon> GetEquipoA()
        {
            Jugador jugadorA = batallaActual.GetAtacante();
            return jugadorA.GetPokemons();
        }
        public Pokemon GetPokemonActual()
        {
            return batallaActual.GetPokemonActualB();
        }

        public void AgregarPokemonesA(string pokemon)
        {
            batallaActual.AgregarPokemonBA(pokemon); 
        }

        public void AgregarPokemonesD(string pokemon)
        {
            batallaActual.AgregarPokemonBD(pokemon);
        }

        public void IniciarEnfrentamiento()
        {
            batallaActual.IniciarBatalla();
        }

        public void MostrarEstadoRival()
        {
            if (batallaActual.GetBatallaIniciada())
            {
                // Muestra el estado del defensor, no del atacante
                Jugador defensor = batallaActual.GetDefensor();
                defensor.MostarEstadoEquipo();
            }
        }

        public void MostrarEstadoEquipo()
        {
            if (batallaActual.GetBatallaIniciada())
            {
                // Muestra el estado del atacante
                Jugador atacante = batallaActual.GetAtacante();
                atacante.MostarEstadoEquipo();
            }
        }

        public void CambiarPokemon(int numeroDePokemon)
        {
            if (batallaActual.GetBatallaIniciada())
            {
                Jugador jugadorAtacante = batallaActual.GetAtacante();
                List<Pokemon> pokemons = jugadorAtacante.GetPokemons();
            
                if (numeroDePokemon >= 0 && numeroDePokemon < pokemons.Count )
                {
                    Pokemon pokemonElegido = pokemons[numeroDePokemon];
                
                    if (pokemonElegido.GetIsAlive())
                    {
                        Pokemon pokemon = jugadorAtacante.GetPokemonEnTurno();
                        jugadorAtacante.CambiarPokemon(pokemonElegido);
                        ManejadorMensajes.Instance.LogMessage($"El Pokémon {pokemonElegido.GetName()} ha entrado en combate y {pokemon.GetName()} ha sido guardado en su pokebola");
                        batallaActual.AvanzarTurno();
                    }
                    else
                    {
                        ManejadorMensajes.Instance.LogMessage($"El Pokémon {pokemonElegido.GetName()} está debilitado y no puede entrar en combate");
                    }
                }
                else
                {
                    ManejadorMensajes.Instance.LogMessage("No tienes ese pokemon");
                }
            }
        }

        public void MostrarAtaquesDisponibles()
        {
            if (batallaActual.GetBatallaIniciada())
            {

                Jugador jugadorAtacante = batallaActual.GetAtacante();
                Pokemon pokemon = jugadorAtacante.GetPokemonEnTurno();
                Console.WriteLine($"El Pokémon {pokemon.GetName()} tiene los siguientes movimientos:");
                foreach (IMovimiento movimiento in pokemon.GetListaMovimientos())
                {
                    if (movimiento is IMovimientoEspecial especial && especial.GetUsadoAnteriormente())
                    {
                        ManejadorMensajes.Instance.LogMessage($"{movimiento.GetName()}(especial) no puede ser usado en este turno");
                    }
                    else
                    {
                        if (movimiento is IMovimientoEspecial movimientoEspecial)
                        {
                            ManejadorMensajes.Instance.LogMessage($"{movimientoEspecial.GetName()} (especial)");
                        }
                        else
                        {
                            ManejadorMensajes.Instance.LogMessage(movimiento.GetName());
                        }
                    }
                }
            }
            else
            {
                ManejadorMensajes.Instance.LogMessage("La batalla no ha iniciado");
            }
        }
        

        public void UsarMovimientos(int numDeMovimiento)
        {
            if (batallaActual.GetBatallaTerminada())
            {
                ManejadorMensajes.Instance.LogMessage("La batalla ya ha terminado.");
                return;
            }

            if (!batallaActual.GetBatallaIniciada())
            {   
                ManejadorMensajes.Instance.LogMessage("La batalla no ha iniciado");
                return;
            }
            Jugador jugador = batallaActual.GetAtacante();
            Pokemon pokemonActual = jugador.GetPokemonEnTurno();
            List<IMovimiento> movimientos = pokemonActual.GetListaMovimientos();
            if (numDeMovimiento > 0 && numDeMovimiento <= movimientos.Count)
            {
                IMovimiento movimiento = movimientos[numDeMovimiento - 1]; //acomodo por la lista

                if (movimiento is IMovimientoEspecial especial && especial.GetUsadoAnteriormente())
                {
                    ManejadorMensajes.Instance.LogMessage($"El movimiento {movimiento.GetName()} es especial y ya fue usado anteriormente. Elija otro movimiento.");
                }
                else
                {
                    pokemonActual.UsarMovimiento(movimiento);
                    Console.WriteLine($"{pokemonActual.GetName()} ha usado {movimiento.GetName()}.");
                    if (movimiento is IMovimientoAtaque movimientoAtaque)
                    {
                        Random random = new Random();
                        int numeroAleatorio = random.Next(1, 101); //Numero aleatorio para saber si acierto 
                        if (numeroAleatorio <= movimientoAtaque.GetPrecision())
                        {
                            batallaActual.RecibirAtaqueB(movimientoAtaque);
                            ManejadorMensajes.Instance.LogMessage($"{pokemonActual.GetName()} ha acertado su ataque");
                        }
                        else
                        {
                            ManejadorMensajes.Instance.LogMessage($"El ataque {movimientoAtaque.GetName()} ha fallado");
                        }

                        if (movimientoAtaque is IMovimientoEspecial movimientoEspecial)
                        {
                            movimientoEspecial.UsadoAnteriormente(true);
                        }
                    }
                    batallaActual.AvanzarTurno();
                }
            }
            else
            {
                ManejadorMensajes.Instance.LogMessage("Movimiento inválido. Por favor, seleccione un movimiento entre 1 y 4, o uno que pueda usarse en este turno.");
            }
        }
        public void MostrarNumPokemon()
        {
            if (batallaActual.GetBatallaIniciada())
            {
                Jugador jugador = batallaActual.GetAtacante();
                List<Pokemon> listaPokemons = jugador.GetPokemons();
                for (int i = 0; i < listaPokemons.Count; i++)
                {
                    Pokemon pokemon = listaPokemons[i];
                    ManejadorMensajes.Instance.LogMessage($"{i}. {pokemon.GetName()}");
                }
            }
            
        }
        public void MostrarItemsDisponibles() //Llama al método de jugador para mostrar los items
        {
            if (batallaActual.GetBatallaIniciada())
            {
                Jugador jugador = batallaActual.GetAtacante();
                jugador.Mostrar_items();
            }
        }

        public void UsarItem(string item, int numeroDePokemon) //Este método utiliza el item que le pases por string
        {
            if (batallaActual.GetBatallaIniciada())
            {
                Jugador jugadorAtacante = batallaActual.GetAtacante();
                List<Pokemon> pokemons = jugadorAtacante.GetPokemons();

                if (numeroDePokemon >= 0 && numeroDePokemon < pokemons.Count)
                {
                    Pokemon pokemonElegido = pokemons[numeroDePokemon];
                    jugadorAtacante.UsarItem(item, pokemonElegido);
                    batallaActual.AvanzarTurno();
                }
                else
                {
                    ManejadorMensajes.Instance.LogMessage("Seleccione el pokemon correctamente");
                }
            }
        }
    }
}