using System;
using System.Collections.Generic;
using DefaultNamespace;
using Library.Combate;
using Library.Tipos;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Ucu.Poo.DiscordBot.ClasesUtilizadas.Characters.Strategy_Ataque;
using Ucu.Poo.Pokemon;

namespace Program.Tests.Combate;

[TestFixture]
// [TestSubject(typeof(Menu))]
public class UnitTest
{
    /// <summary>
    /// Prueba de la clase <see cref="Menu"/>.
    /// Estos test nos permiten verificar que fragmentos del codigo anden bien detectando errores tempranamente.
    /// </summary>

    [Test]
    public void JugadorTrataDeUsarPokemonQueNoTiene()
    {
        Menu menu = new Menu();
        menu.UnirJugadores("Ash");
        menu.UnirJugadores("yo");
        menu.AgregarPokemonesA("Arbok"); // Solo Ash tiene este Pokémon.
        menu.AgregarPokemonesD("Squirtle");
        menu.IniciarEnfrentamiento();
        
        var resultado = menu.CambiarPokemon(1); // Intento de usar un Pokémon no disponible.
        
        Assert.That(resultado, Is.EqualTo("No tienes ese pokemon")); 
    }

    [Test]
    public void JugadorTrataDeCambiarAPokemonQueEstaPeleando()
    {
        Menu juego = new Menu();
        juego.UnirJugadores("Ash");
        juego.UnirJugadores("Red");
        juego.AgregarPokemonesA("Squirtle"); // Squirtle es el Pokémon actual al inicio.
        juego.AgregarPokemonesD("Charmander");
        juego.AgregarPokemonesA("Bulbasaur"); // Bulbasaur es el segundo Pokémon del equipo.
        juego.IniciarEnfrentamiento();
        
        juego.CambiarPokemon(0); // Intenta cambiar Squirtle por sí mismo.
        
        // Verifica que el Pokémon actual sigue siendo Squirtle.
        string pokemonEsperado = "Squirtle";
        string pokemonObtenido = juego.GetPokemonActual().GetName();
        Assert.That(pokemonEsperado, Is.EqualTo(pokemonObtenido));
    }

    [Test]
    public void JugadorTrataDeCambiarAPokemonDebilitado()
    {
        Menu juego = new Menu();
        juego.UnirJugadores("Ash");
        juego.UnirJugadores("Red");
        juego.AgregarPokemonesA("Pikachu");
        juego.AgregarPokemonesD("Pidgey");
        juego.AgregarPokemonesD("Bulbasaur");
        juego.IniciarEnfrentamiento();
        
        juego.UsarMovimientos(1); // Pikachu usa Rayo y Pidgey es derrotado.
        juego.CambiarPokemon(1); // Intenta cambiar a Pidgey (debilitado).
        
        // Verifica que Bulbasaur es ahora el Pokémon actual.
        string pokemonEsperado = "Bulbasaur";
        string pokemonObtenido = juego.GetPokemonActual().GetName();
        Assert.That(pokemonObtenido, Is.EqualTo(pokemonEsperado));
    }

    [Test]
    public void PokemonParalizado()
    {
        Paralizar paralizadoEsperado = new Paralizar();
        Menu juego = new Menu();
        juego.UnirJugadores("Ash");
        juego.UnirJugadores("Red");
        juego.AgregarPokemonesA("Pikachu");
        juego.AgregarPokemonesD("Bulbasaur");
        juego.IniciarEnfrentamiento();
        
        // Pikachu usa un movimiento que paraliza al rival (Bulbasaur).
        juego.UsarMovimientos(1);

        // Obtiene el Pokémon rival y su efecto después del movimiento.
        Pokemon rival = juego.GetPokemonRival();
        Efecto efectoAplicado = rival.GetEfecto();
        
        // Verifica que el tipo de efecto aplicado es del mismo tipo que 'Paralizar'.
        Assert.That(efectoAplicado.GetType(), Is.EqualTo(paralizadoEsperado.GetType()));
    }
    [Test]
    public void TrataDeUsarSuperPocionEnPokemonDebilitado()
    {
        Menu juego = new Menu();
        juego.UnirJugadores("Ash");
        juego.UnirJugadores("Red");
        juego.AgregarPokemonesA("Pikachu");
        juego.AgregarPokemonesD("Pidgey");
        juego.AgregarPokemonesA("Bulbasaur");
        juego.IniciarEnfrentamiento();
        juego.UsarMovimientos(1); //Jugador 1 usa Rayo y pidgey es debilitado
        juego.UsarItem("superpocion", 1); //Trata de curar a Pidgey
        
        //Verifica que la vida de Pidgey es 0 aun siendo curado con pocion después de ser debilitado 
        Assert.That(juego.GetPokemonActual().GetVidaActual(), Is.EqualTo(0));
    }
    

    [Test]
    public void PokemonQuemado()
    {
        Menu menu = new Menu();
        menu.UnirJugadores("yo");
        menu.UnirJugadores("diego");
        menu.AgregarPokemonesA("Charmander");
        menu.AgregarPokemonesD("Squirtle");
        menu.IniciarEnfrentamiento();
        
        menu.UsarMovimientos(2); // Charmander usa Lanzallamas (quema al rival).
        menu.UsarMovimientos(4); // Squirtle usa Protección.
        
        // Verificar que Squirtle tiene el HP esperado después de usar Protección.
        int hpEsperado = 72; 
        Assert.That(menu.GetHpDefensor(), Is.EqualTo(hpEsperado));

        // Verificar que Squirtle está quemado.
        Pokemon defensor = menu.GetPokemonRival();
        Assert.That(defensor.GetEfecto().GetType(), Is.EqualTo(typeof(Quemar)));
    }

    [Test]
    public void PokemonDormido()
    {
        Menu juego = new Menu();
        juego.UnirJugadores("Ash");
        juego.UnirJugadores("Red");
        juego.AgregarPokemonesA("Stufful");
        juego.AgregarPokemonesD("Squirtle");
        juego.IniciarEnfrentamiento();
        
        // Pikachu usa un movimiento que paraliza al rival (Bulbasaur).
        juego.UsarMovimientos(1);

        // Obtiene el Pokémon rival y su efecto después del movimiento.
        Pokemon rival = juego.GetPokemonRival();
        
        // Verifica que el tipo de efecto aplicado es del mismo tipo que 'Dormir'.
        Assert.That(rival.GetEfecto().GetType(), Is.EqualTo(typeof(Dormir)));
    }

    [Test]
    public void TratoDeAtacarSinIniciarBatalla() //Verificacion Cambio de Pokemon de Turno
    {
        Menu juego= new Menu();
        juego.UnirJugadores("Ash");
        juego.UnirJugadores("Red");
        juego.AgregarPokemonesA("Squirtle"); //Squirtle era el Pokemon en Turno al inicio porque fue agregado primero
        juego.AgregarPokemonesD("Charmander");
        
        juego.UsarMovimientos(1);
        
        // Verifica que la batalla no fue iniciada y no uso el movimiento
        Assert.That(juego.UsarMovimientos(1),Is.EqualTo("La batalla no ha iniciado"));
        
        // Verifica que la vida de Charmander esta completa
        Assert.That(juego.GetHpDefensor(),Is.EqualTo(85));
        
    }

    [Test]
    /// <summary>
    /// Este test verifica la primer historia de usuario y verifica la historia de usuario que no suma a una lista a los usuarios que no pueden combatir
    /// </summary>
    public void NoAgregoPokemons()
    {
        Menu juego = new Menu();
        juego.UnirJugadores("Don Dimadon");
        juego.UnirJugadores("Timmy Turner");
        juego.AgregarPokemonesD("Squirtle");
        juego.IniciarEnfrentamiento();
        
        // Verifica que devuelve un mensaje avisando que no tiene pokemons un jugador 
        Assert.That(juego.IniciarEnfrentamiento(),Is.EqualTo("La batalla ya ha comenzado o uno de los jugadores no tiene Pokémon."));
    }

    [Test]
    public void JugadorUsaPocionParaPokemonConVidaCompleta()
    {
        Menu juego = new Menu();
        juego.UnirJugadores("Don Dimadon");
        juego.UnirJugadores("Bellota");
        juego.AgregarPokemonesA("Charmander"); //85
        juego.UsarItem("Superpocion", 1);
        
        int vidatotalCharmander = 85;
        
        // Verifica que la superpocion no le agrego vida de mas
        Assert.That(vidatotalCharmander, Is.EqualTo(juego.GetHpAtacante()));
    }

    [Test]
    /// <summary>
    /// Este test verifica la sexta historia de usuario ya que la batalla inicia y termina
    /// El mensaje impreso en programa pasará a ser un string pasado al bot de discord
    /// </summary>
    public void PierdoBatalla()
    {
        Menu juego = new Menu();
        juego.UnirJugadores("Ash");
        juego.UnirJugadores("Red");
        juego.AgregarPokemonesA("Pikachu");
        juego.AgregarPokemonesD("Pidgey");
        juego.IniciarEnfrentamiento();
        juego.UsarMovimientos(1); //Pikachu usa royo
       
        bool batallaperdida = juego.GetBatallaI() && juego.GetBatallaT();
        
        // Verifica que la batalla fue terminada al perder un jugador
        Assert.That(batallaperdida, Is.EqualTo(true));
    }

    [Test]
    /// <summary>
    /// Este test verifica la  historia de usuario que nos permite usar varios items
    /// </summary>
    public void JugadorAgrega7Pokemons()
    {
        StringWriter consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        Menu juego = new Menu();
        juego.UnirJugadores("Ash");
        juego.UnirJugadores("Red");

        juego.AgregarPokemonesA("Squirtle");
        juego.AgregarPokemonesA("Pidgey");
        juego.AgregarPokemonesA("Larvitar");
        juego.AgregarPokemonesA("Caterpie");
        juego.AgregarPokemonesA("Charmander");
        juego.AgregarPokemonesA("Dratini");
        juego.AgregarPokemonesA("Gengar");

        List<Pokemon> listapokemonsatacante = juego.GetEquipoA();
        int numero = listapokemonsatacante.Count;
        
        // Verifica que agrego los pokemones al jugador correctamente
        Assert.That(6, Is.EqualTo(numero));

    }
    [Test]
    public void UsoDeEnvenenamiento()
    {
        Menu menu = new Menu();
        menu.UnirJugadores("quien");
        menu.UnirJugadores("yo");
        menu.AgregarPokemonesA("Arbok");
        menu.AgregarPokemonesD("Squirtle");
        menu.IniciarEnfrentamiento();
        menu.UsarMovimientos(1);
        
        Pokemon rival = menu.GetPokemonActual();
        
        //Verifica que el pokemon rival esta envenenado
        Assert.That(rival.GetEfecto().GetType(),Is.EqualTo(typeof(Envenenar)));
    }
    [Test]
    public void Inmune() //En este test se puede ver que cuando un Pokemon que es inmune a otro es atacado, su vida no se ve afectada
    {
        Menu menu = new Menu();
        menu.UnirJugadores("ash");
        menu.UnirJugadores("red");
        menu.AgregarPokemonesA("Pikachu");
        //Usamos a pikachu porque electrico es inmune a electrico y el ataque Rayo es de tipo electrico
        menu.AgregarPokemonesD("Pikachu");
        menu.IniciarEnfrentamiento();
        menu.UsarMovimientos(1);//Jugador 1 usa Rayo(electrico)
        menu.UsarMovimientos(1);//Jugador2 usa Rayo(electrico)
        int vidaesperadadefensor = 80;
        double vidaObtenidaDefensor = menu.GetHpDefensor();
        
        Assert.That(vidaesperadadefensor,Is.EqualTo(vidaObtenidaDefensor));
    }
    [Test]
    public void DanioCritico() //En este test se puede ver que un ataque crítico hace el daño que le corresponde
    {
        Menu menu = new Menu();
        menu.UnirJugadores("ash");
        menu.UnirJugadores("red");
        menu.AgregarPokemonesA("Gengar");
        menu.AgregarPokemonesD("Charmander"); // 85 vida, 60 def 
        menu.IniciarEnfrentamiento();
        Pokemon charmander = menu.GetPokemonRival();
        charmander.SetStrategy(new AtaqueCritico());
        string retorno = menu.UsarMovimientos(3); //daño 80 * 1.2 = 96
        int vidaesperadadefensor = 85 - 36 ;
        double vidaObtenidaDefensor = menu.GetHpAtacante();
        Assert.That(retorno, Does.Contain("Además ha sido un ataque crítico"));
        Assert.That(vidaesperadadefensor,Is.EqualTo(vidaObtenidaDefensor));
    }
    [Test]
    public void DanioNoCritico() //En este test se puede ver que el ataque no resulta ser critico
    {
        Menu menu = new Menu();
        menu.UnirJugadores("ash");
        menu.UnirJugadores("red");
        menu.AgregarPokemonesA("Gengar");
        menu.AgregarPokemonesD("Charmander"); // 85 vida, 60 def 
        menu.IniciarEnfrentamiento();
        Pokemon charmander = menu.GetPokemonRival();
        charmander.SetStrategy(new AtaqueNoCritico());
        menu.UsarMovimientos(3); //daño 80 * 1.2 = 96
        int vidaesperadadefensor = 85 - 20 ;
        double vidaObtenidaDefensor = menu.GetHpAtacante();
        Assert.That(vidaesperadadefensor,Is.EqualTo(vidaObtenidaDefensor));
    }

    [Test]
    public void ProbarMovimientoDefensa()
    {
        string nombreesperado = "Escudo Epico";
        int defensaesperada = 50;
        Tipo tipoesperado = new Tipo("Hielo");
        
        MovimientoDeDefensa movimiento = new MovimientoDeDefensa(nombreesperado, defensaesperada, tipoesperado);

        // Assert
        Assert.That(nombreesperado, Is.EqualTo(movimiento.GetName()));
        Assert.That(defensaesperada,Is.EqualTo(movimiento.GetDefensa()));
        Assert.That(tipoesperado, Is.EqualTo(movimiento.GetTipo()));
    }
}

