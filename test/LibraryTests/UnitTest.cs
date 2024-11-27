using System;
using System.Collections.Generic;
using DefaultNamespace;
using Library.Combate;
using Library.Tipos;
using Library.Tipos.Paralisis_Strategy;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Ucu.Poo.DiscordBot.ClasesUtilizadas.Characters.Strategy_Ataque;
using Ucu.Poo.DiscordBot.Domain;
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
        Menu menu = new Menu();
        menu.UnirJugadores("player1");
        menu.UnirJugadores("player2");
        menu.AgregarPokemonesA("Pikachu");
        menu.AgregarPokemonesD("Charmander");
        menu.IniciarEnfrentamiento();
        Pokemon charmander = menu.GetPokemonRival();
        Pokemon pikachu = menu.GetPokemonActual();
        pikachu.SetStrategy(new AtaqueNoCritico()); // seteo el ataque para que no haga crítico
        menu.UsarMovimientos(1); // Pikachu Paraliza a Charmander
        Assert.That(charmander.GetEfecto().GetType(), Is.EqualTo(typeof(Paralizar)));
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
        
        // Stufful usa un movimiento que duerme al rival (squirtle).
        juego.UsarMovimientos(1);

        // Obtiene el Pokémon rival y su efecto después del movimiento.
        Pokemon rival = juego.GetPokemonRival();
        
        // Verifica que el tipo de efecto aplicado es del mismo tipo que 'Dormir'.
        Assert.That(rival.GetEfecto().GetType(), Is.EqualTo(typeof(Dormir)));
    }

    [Test]
    public void UsoCuraTotal()
    {
        Menu juego = new Menu();
        juego.UnirJugadores("Ash");
        juego.UnirJugadores("Red");
        juego.AgregarPokemonesA("Arbok");
        juego.AgregarPokemonesD("Squirtle");
        juego.IniciarEnfrentamiento();
        
        // Arbok usa un movimiento que envenena al rival (Squirtle).
        juego.UsarMovimientos(1);
        juego.UsarItem("curatotal", 0); //aplica curatotal en el pokemon squirtle
        Pokemon actual = juego.GetPokemonActual();
        Pokemon rival = juego.GetPokemonRival();

        Assert.That(actual.GetEfecto(), Is.EqualTo(rival.GetEfecto()));
    }

    [Test]
    public void MuestroItems()
    {
        Menu juego = new Menu();
        juego.UnirJugadores("Ash");
        juego.UnirJugadores("Red");
        juego.AgregarPokemonesA("Stufful");
        juego.AgregarPokemonesD("Squirtle");
        juego.IniciarEnfrentamiento();

        string items = juego.MostrarItemsDisponibles();
        Assert.That(items,Is.EqualTo("superpocion: 4 disponibles\nrevivir: 1 disponibles\ncuratotal: 2 disponibles\n"));
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
    /// <summary>
    /// Este test verifica el efecto Envenenar
    /// </summary>
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
    /// <summary>
    /// Este test verifica que se respeta el danio segun el tipo de ataque y el tipo de pokemon atacado,
    /// en este caso al combatir 2 electricos, la vida del que es atacado no es afectada.
    /// </summary>
    public void Inmune() 
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
    /// <summary>
    /// Este test verifica que Un ataque pueda ser Critico, y que aumenta en danio un 20%
    /// </summary>
    public void DanioCritico() 
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
    /// <summary>
    /// Este test verifica que un ataque tambien puede ser No Critico y noaumentar el danio del ataque
    /// </summary>
    public void DanioNoCritico() 
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
    /// <summary>
    /// Este test verifica que Un ataque puede ser preciso a la hora de usarlo y critico al mismo tiempo
    /// </summary>
    public void PresicionAciertaYAtaqueCritico()
    {
        Menu menu = new Menu();
        menu.UnirJugadores("ash");
        menu.UnirJugadores("red");
        menu.AgregarPokemonesA("Pidgey");
        menu.AgregarPokemonesD("Charmander"); 
        menu.IniciarEnfrentamiento();
        menu.SetStrategyPresicion(new StrategyPreciso());
        Pokemon charmander = menu.GetPokemonRival();
        charmander.SetStrategy(new AtaqueCritico());
        string mensajeObtenido = menu.UsarMovimientos(1);
        Assert.That(mensajeObtenido, Does.Contain("Y ha acertado."));
        double numeroesperado = 85 - (60 * 1.2 - 60);// Calculo de danio con critico
        double numeroObtenido = menu.GetHpAtacante();//Vida de charmander ya que pasa a ser el atacante
        Assert.That(numeroesperado,Is.EqualTo(numeroObtenido));
    }
    
    [Test]
    /// <summary>
    /// Este test verifica que Un ataque puede ser preciso a la hora de usarlo y tambien puede ser no critico
    /// </summary>
    public void PresicionAciertaYAtaqueNoCritico()
    {
        Menu menu = new Menu();
        menu.UnirJugadores("ash");
        menu.UnirJugadores("red");
        menu.AgregarPokemonesA("Pidgey");
        menu.AgregarPokemonesD("Charmander"); 
        menu.IniciarEnfrentamiento();
        menu.SetStrategyPresicion(new StrategyPreciso());
        string mensajeObtenido = menu.UsarMovimientos(1);
        Assert.That(mensajeObtenido, Does.Contain("Y ha acertado."));
    }
    
    [Test]
    /// <summary>
    /// Este test verifica que Un ataque puede ser no preciso a la hora de usarlo
    /// </summary>
    public void PresicionNoAcierta()
    {
        Menu menu = new Menu();
        menu.UnirJugadores("ash");
        menu.UnirJugadores("red");
        menu.AgregarPokemonesA("Pidgey");
        menu.AgregarPokemonesD("Charmander"); 
        menu.IniciarEnfrentamiento();
        menu.SetStrategyPresicion(new StrategyNoPreciso());
        string mensajeObtenido = menu.UsarMovimientos(1);
        Assert.That(mensajeObtenido, Does.Contain("Y ha fallado."));
        double numeroesperado = 85;// Vita totalya que el ataque ha fallado y no le ha hecho danio
        double numeroObtenido = menu.GetHpAtacante();//Vida de charmander ya que pasa a ser el atacante
        Assert.That(numeroesperado,Is.EqualTo(numeroObtenido));
    }

    [Test]
    /// <summary>
    /// Este test verifica que Un ataque puede ser no preciso a la hora de usarlo y que aunque sea critico
    /// esto no afecta al pokemon atacado y se respeta que fallo el ataque
    /// </summary>
    public void PresicionNoAciertaYAtaqueCritico()
    {
        Menu menu = new Menu();
        menu.UnirJugadores("ash");
        menu.UnirJugadores("red");
        menu.AgregarPokemonesA("Pidgey");
        menu.AgregarPokemonesD("Charmander");
        menu.IniciarEnfrentamiento();
        menu.SetStrategyPresicion(new StrategyNoPreciso());
        Pokemon charmander = menu.GetPokemonRival();
        charmander.SetStrategy(new AtaqueCritico());
        string mensajeObtenido = menu.UsarMovimientos(1);
        Assert.That(mensajeObtenido, Does.Contain("Y ha fallado."));
        double numeroesperado = 85; // Vita totalya que el ataque ha fallado y no le ha hecho danio
        double numeroObtenido = menu.GetHpAtacante(); //Vida de charmander ya que pasa a ser el atacante
        Assert.That(numeroesperado, Is.EqualTo(numeroObtenido));
    }

    [Test]
    public void ProbarMovimientoDefensa()
    {
        string nombreesperado = "Escudo Epico";
        int defensaesperada = 50;
        Tipo tipoesperado = new Tipo("Hielo");
        
        MovimientoDeDefensa movimiento = new MovimientoDeDefensa(nombreesperado, defensaesperada, tipoesperado);

        Assert.That(nombreesperado, Is.EqualTo(movimiento.GetName()));
        Assert.That(defensaesperada,Is.EqualTo(movimiento.GetDefensa()));
        Assert.That(tipoesperado, Is.EqualTo(movimiento.GetTipo()));
    }
    
    [Test]
    public void GettersStrategyCriticos()
    {
        IAtaqueDanioStrategy critico = new AtaqueCritico();
        int esperadoCritico = critico.GetNumero();
        Assert.That(esperadoCritico,Is.EqualTo(0));
        
        IAtaqueDanioStrategy noCritico = new AtaqueNoCritico();
        int esperadoNoCritico = noCritico.GetNumero();
        Assert.That(esperadoNoCritico,Is.EqualTo(1));

        IAtaqueDanioStrategy random = new AtaqueRandom();
        int esperadoRandom = random.GetNumero();
        Assert.That(esperadoRandom, Is.InRange(0, 9));
    }
 [Test]
    /// <summary>
    /// Este test verifica que los Getters de Strategy de la precision de los ataques den el valor requerido
    /// </summary>
    public void GettersStrategyPrecision()
    {
        IStrategyPresicion preciso = new StrategyPreciso();
        int numpreciso = preciso.GetNumber();
        Assert.That(numpreciso,Is.EqualTo(1));

        IStrategyPresicion noPreciso = new StrategyNoPreciso();
        int numNoPreciso = noPreciso.GetNumber();
        Assert.That(numNoPreciso,Is.EqualTo(100));

        IStrategyPresicion random = new StrategyPrecisoRandom();
        int numRandom = random.GetNumber();
        Assert.That(numRandom, Is.InRange(1, 100));
    }

    [Test]
    /// <summary>
    /// Este test verifica que los Getters de Strategy del efecto Paralisis den el valor requerido
    /// </summary>
    public void GettersStrategyParalisis()
    {
        IEfectoParalisisStrategy paralisisTrue = new EfectoParalisisTrue();
        bool valorTrue = paralisisTrue.GetValor();
        Assert.That(valorTrue,Is.EqualTo(true));
        
        IEfectoParalisisStrategy paralisisFalse = new EfectoParalisisFalse();
        bool valorFalse = paralisisFalse.GetValor();
        Assert.That(valorFalse,Is.EqualTo(false));
    }
    [Test]
    public void AtaqueConParalisis()
    {
        Menu menu = new Menu();
        menu.UnirJugadores("player1");
        menu.UnirJugadores("player2");
        menu.AgregarPokemonesA("Pikachu");
        menu.AgregarPokemonesD("Charmander");
        menu.IniciarEnfrentamiento();
        Pokemon charmander = menu.GetPokemonRival();
        Paralizar paralizar = new Paralizar();
        paralizar.SetStrategyParalisis(new EfectoParalisisFalse());
        charmander.AgregarEfecto(paralizar);
        string primerpokemon = menu.GetPokemonActual().GetName();
        menu.UsarMovimientos(3);
        string segundopokemon = menu.GetPokemonActual().GetName();
        Assert.That(primerpokemon, Is.EqualTo(segundopokemon));
    }

    [Test]
    public void AtaqueConParalisisPeroPermitoJugar()
    {
        Menu menu = new Menu();
        menu.UnirJugadores("player1");
        menu.UnirJugadores("player2");
        menu.AgregarPokemonesA("Pikachu");
        menu.AgregarPokemonesD("Charmander");
        menu.IniciarEnfrentamiento();
        Pokemon charmander = menu.GetPokemonRival();
        Paralizar paralizar = new Paralizar();
        paralizar.SetStrategyParalisis(new EfectoParalisisTrue());
        charmander.AgregarEfecto(paralizar);
        string primerpokemon = menu.GetPokemonActual().GetName(); //Pikachu
        menu.UsarMovimientos(3);
        string segundopokemon = menu.GetPokemonActual().GetName(); //Charmander
        string pokemon1esperado = "Pikachu";
        string pokemon2esperado = "Charmander";
        Assert.That(primerpokemon, Is.EqualTo(pokemon1esperado));
        Assert.That(segundopokemon, Is.EqualTo(pokemon2esperado));
    }
    
    [Test]
    /// <summary>
    /// Este test verifica que el MetodoMostrarNum funcione correctamente, diciendole al jugador quien
    /// es el pokemon en turno y los numeros de los otrospokemones del equipo, para que pueda usar por ejemplo
    // el comando de change pokemon, el cual funciona solo co el numero del pokemon
    /// </summary>
    public void MostratNumPokemon()
    {
        Menu menu = new Menu();
        menu.UnirJugadores("player1");
        menu.UnirJugadores("player2");
        menu.AgregarPokemonesA("Pikachu");
        menu.AgregarPokemonesA("Bulbasaur");
        menu.AgregarPokemonesA("Arbok");
        menu.AgregarPokemonesD("Charmander");
        menu.IniciarEnfrentamiento();
        string mensaje=menu.MostrarNumPokemon();
        Assert.That(mensaje, Does.Contain("Pikachu está en turno\n el número 1 es Bulbasaur \nel número 2 es Arbok "));
    }

    [Test]
    /// <summary>
    /// Este test verifica que Mostrar el estadodel equipo lo haga correctamente, asi el jugador puede ver la vida de los pokemones que tiene
    /// </summary>
    public void MostrarEstadoEquipo()
    {
        Menu menu = new Menu();
        menu.UnirJugadores("player1");
        menu.UnirJugadores("player2");
        menu.AgregarPokemonesA("Pikachu");
        menu.AgregarPokemonesD("Stufful");
        menu.AgregarPokemonesA("Charmander");
        menu.AgregarPokemonesD("Arbok");
        Pokemon pikachu = menu.GetPokemonActual();
        pikachu.SetStrategy(new AtaqueNoCritico());
        Pokemon stufful = menu.GetPokemonRival();
        stufful.SetStrategy(new AtaqueNoCritico());
        menu.IniciarEnfrentamiento();
        menu.IniciarEnfrentamiento();
        menu.UsarMovimientos(2);
        menu.UsarMovimientos(2);
        menu.UsarMovimientos(1);//Uso defensa para no matar a Pikachu
        menu.UsarMovimientos(2);
        string mensaje = "";
        if (menu.JugadorA().GetName() == "player1")
        {
            mensaje=menu.MostrarEstadoEquipo();
        }

        if (menu.JugadorA().GetName() == "player2")
        {
             mensaje=menu.MostrarEstadoEquipo();
        }

        Assert.That(mensaje, Does.Contain("Pikachu 15/80\nCharmander 85/85"));

    }
    
    [Test]
    /// <summary>
    /// Este test verifica que Mostrar el estado del equipo contrincante lo haga correctamente,
    /// asi el jugador puede ver la vida de los pokemones que tiene el rival
    /// </summary>
    public void MostrarEstadoRival()
    {
        Menu menu = new Menu();
        menu.UnirJugadores("player1");
        menu.UnirJugadores("player2");
        menu.AgregarPokemonesA("Pikachu");
        menu.AgregarPokemonesD("Stufful");
        menu.AgregarPokemonesA("Charmander");
        menu.AgregarPokemonesD("Arbok");
        Pokemon pikachu = menu.GetPokemonActual();
        pikachu.SetStrategy(new AtaqueNoCritico());
        Pokemon stufful = menu.GetPokemonRival();
        stufful.SetStrategy(new AtaqueNoCritico());
        menu.IniciarEnfrentamiento();
        Console.WriteLine(menu.UsarMovimientos(2));
        Console.WriteLine(menu.UsarMovimientos(2));
        Console.WriteLine(menu.UsarMovimientos(2));
        Console.WriteLine(menu.UsarMovimientos(2));
        string mensaje = "";
        if (menu.JugadorD().GetName() == "player1")
        {
            mensaje=menu.MostrarEstadoRival();
        }

        if (menu.JugadorD().GetName() == "player2")
        {
             mensaje=menu.MostrarEstadoRival();
        }

        Assert.That(mensaje, Does.Contain("Arbok 60/60\nStufful ha muerto"));

    }
}

