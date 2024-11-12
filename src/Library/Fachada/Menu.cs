using System.Text;
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
        private StringBuilder textosaenviar;

        public Menu()
        {
            batallaActual = new Batalla();
            textosaenviar = new StringBuilder();
        }

        public void UnirJugadores(string jugador)
        {
            textosaenviar.Append(batallaActual.AgregarJugador(new Jugador(jugador)));
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
            textosaenviar.Append(batallaActual.AgregarPokemonBA(pokemon)); 
        }

        public void AgregarPokemonesD(string pokemon)
        {
            textosaenviar.Append(batallaActual.AgregarPokemonBD(pokemon));
        }

        public void IniciarEnfrentamiento()
        {
            textosaenviar.Append(batallaActual.IniciarBatalla());
        }

        public void MostrarEstadoRival()
        {
            if (batallaActual.GetBatallaIniciada())
            {
                // Muestra el estado del defensor, no del atacante
                Jugador defensor = batallaActual.GetDefensor();
                textosaenviar.Append(defensor.MostarEstadoEquipo());
            }
        }

        public void MostrarEstadoEquipo()
        {
            if (batallaActual.GetBatallaIniciada())
            {
                // Muestra el estado del atacante
                Jugador atacante = batallaActual.GetAtacante();
                textosaenviar.Append(atacante.MostarEstadoEquipo());
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
                        textosaenviar.Append($"El Pokémon {pokemonElegido.GetName()} ha entrado en combate y {pokemon.GetName()} ha sido guardado en su pokebola");
                        textosaenviar.Append(batallaActual.AvanzarTurno());
                    }
                    else
                    {
                        textosaenviar.Append($"El Pokémon {pokemonElegido.GetName()} está debilitado y no puede entrar en combate");
                    }
                }
                else
                {
                    textosaenviar.Append("No tienes ese pokemon");
                }
            }
        }

        public void MostrarAtaquesDisponibles()
        {
            if (batallaActual.GetBatallaIniciada())
            {
                
                Jugador jugadorAtacante = batallaActual.GetAtacante();
                Pokemon pokemon = jugadorAtacante.GetPokemonEnTurno();
                textosaenviar.Append($"El Pokémon {pokemon.GetName()} tiene los siguientes movimientos:");
                foreach (IMovimiento movimiento in pokemon.GetListaMovimientos())
                {
                    if (movimiento is IMovimientoEspecial especial && especial.GetUsadoAnteriormente())
                    {
                        textosaenviar.Append($"{movimiento.GetName()}(especial) no puede ser usado en este turno");
                    }
                    else
                    {
                        if (movimiento is IMovimientoEspecial movimientoEspecial)
                        {
                            textosaenviar.Append(movimientoEspecial.GetName());
                            textosaenviar.Append("(especial)");
                        }
                        else
                        {
                            textosaenviar.Append(movimiento.GetName());
                        }
                    }
                }
            }
        }
        

        public void UsarMovimientos(int numDeMovimiento)
        {
            if (batallaActual.GetBatallaTerminada())
            {
                textosaenviar.Append("La batalla ya ha terminado.");
                return;
            }

            if (!batallaActual.GetBatallaIniciada())
            {   
                textosaenviar.Append("La batalla no ha iniciado");
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
                    textosaenviar.Append($"El movimiento {movimiento.GetName()} es especial y ya fue usado anteriormente. Elija otro movimiento.");
                }
                else
                {
                    textosaenviar.Append(pokemonActual.UsarMovimiento(movimiento));
                    textosaenviar.Append($"{pokemonActual.GetName()} ha usado {movimiento.GetName()}.");
                    if (movimiento is IMovimientoAtaque movimientoAtaque)
                    {
                        Random random = new Random();
                        int numeroAleatorio = random.Next(1, 101); //Numero aleatorio para saber si acierto 
                        if (numeroAleatorio <= movimientoAtaque.GetPrecision())
                        {
                            textosaenviar.Append($"{pokemonActual.GetName()} ha acertado su ataque");
                            textosaenviar.Append(batallaActual.RecibirAtaqueB(movimientoAtaque));
                        }
                        else
                        {
                            textosaenviar.Append($"El ataque {movimientoAtaque.GetName()} ha fallado");
                        }

                        if (movimientoAtaque is IMovimientoEspecial movimientoEspecial)
                        {
                            movimientoEspecial.UsadoAnteriormente(true);
                        }
                    }
                    textosaenviar.Append(batallaActual.AvanzarTurno());
                }
            }
            else
            { 
                textosaenviar.Append("Movimiento inválido. Por favor, seleccione un movimiento entre 1 y 4, o uno que pueda usarse en este turno.");
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
                    textosaenviar.Append($"{i}. {pokemon.GetName()}");
                }
            }
            
        }
        public void MostrarItemsDisponibles() //Llama al método de jugador para mostrar los items
        {
            if (batallaActual.GetBatallaIniciada())
            {
                Jugador jugador = batallaActual.GetAtacante();
                foreach (string item in jugador.Mostrar_items())
                {
                    textosaenviar.Append(item);
                }
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
                    textosaenviar.Append(jugadorAtacante.UsarItem(item, pokemonElegido));
                    textosaenviar.Append(batallaActual.AvanzarTurno());
                }
                else
                {
                    textosaenviar.Append("Seleccione el pokemon correctamente");
                }
            }
        }
    }
}