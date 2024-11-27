using System;
using System.Collections.Generic;
using DefaultNamespace;
using Library.Combate;
using Library.Tipos;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Ucu.Poo.DiscordBot.ClasesUtilizadas.Characters.Strategy_Ataque;
using Ucu.Poo.DiscordBot.Domain;

namespace Program.Tests.Combate;

[TestFixture]
public class UserStoriesTests
{
    /// <summary>
    /// Este test verifica la primer historia de usuario y verifica la historia de usuario que no suma a una lista a los usuarios que no pueden combatir
    /// </summary>
    [Test]
    public void Agrego6Pokemons()  
    {
        Facade.Reset();
        Facade.Instance.StartBattle("Ash", "Red");
        Facade.Instance.AddPokemosA("Charmander");
        Facade.Instance.AddPokemosA("Squirtle");
        Facade.Instance.AddPokemosA("Bulbasaur");
        Facade.Instance.AddPokemosA("Dratini");
        Facade.Instance.AddPokemosA("Arbok");
        Facade.Instance.AddPokemosA("Charmander");
        List<string> listadadaAstring = new List<string>();
        foreach (var pokemon in Facade.Instance.Menu.GetEquipoA())
        {
            listadadaAstring.Add(pokemon.GetName());
        }
        List<string> listaesperada = new List<string>
        {
            "Charmander",
            "Squirtle",
            "Bulbasaur",
            "Dratini",
            "Arbok",
            "Charmander"
        };

        foreach (string nombre in listaesperada)
        {
            Assert.That(listadadaAstring, Does.Contain(nombre));
        }
        
    }
    /// <summary>
    /// Este test verifica la primera historia de usuario mostrando los pokemones del catálogo
    /// </summary>
    [Test]
    public void MostarCatalogo()  
    {
        Facade.Reset();
        Facade.Instance.StartBattle("Ash", "Red");
        string catalogo = Facade.Instance.ShowCatolog();
        string catalogoesperado =
            "Los pokemones disponibles son:\nPikachu \nPidgey \nLarvitar \nBulbasaur \nCharmander \nSquirtle \nCaterpie " +
            "\nDratini \nGengar \nRegice \nStufful \nGardevoir \nArbok \n";
        Assert.That(catalogo, Is.EqualTo(catalogoesperado));
    }

    /// <summary>
    /// Este test verifica la segunda historia de usuario que comprueba que el usuario puede ver el catalogo de movmientos
    /// </summary>
    [Test]
    public void VerAtaquesDisponiblesDeMisPokemons() //En este test se puede ver que cuando el jugador 1 intenta usar el ataque especial de nuevo no puede hacerlo. 
    {
        Facade.Reset();
        Facade.Instance.StartBattle("Ash", "Red");
        Facade.Instance.AddPokemosA("Pikachu");
        Facade.Instance.AddPokemosD("Caterpie");
        Facade.Instance.InitializeBattle();
        
        Assert.That(Facade.Instance.ShowAvailableMoves(),Is.EqualTo("El Pokémon Pikachu tiene los siguientes movimientos y sus numeros para ser utilizados son los siguientes:\n" +
                                                                    "1.Rayo (especial)\n"+
                                                                    "2.Electro Bola\n"+
                                                                    "3.Ataque Rápido\n"+
                                                                    "4.Protección\n"+
                                                                    "Para usarlos, ingresa el número correspondiente a cada movimiento.\n"));
    }
    /// <summary>
    /// Este test verifica la segunda historia de usuario que comprueba que el pokemon no pueda usar su mismo ataque especial 2 veces seguidas
    /// </summary>
    [Test]
    public void RepticionEspecial()
    {
        Facade.Reset();
        //Este test muestra como un pokemon envenena a otro
        Facade.Instance.StartBattle("qcy", "manu¿");
        Facade.Instance.AddPokemosA("Charmander");
        Facade.Instance.AddPokemosD("Squirtle");
        Facade.Instance.InitializeBattle();
        Facade.Instance.UsePokemonMove(2); //usa un ataque especial que hace 70/2 de daño
        Facade.Instance.UsePokemonMove(4);
        Facade.Instance.UsePokemonMove(2);
        string nombrepokemonactual = Facade.Instance.ShowAtualPokemonA();
        double vidaesperadasquirtle = 72 ; //no toca su vida porque tiene 60 de defensa pero el efecto de quemar lo daña
        double vidadada = Facade.Instance.Menu.GetHpDefensor();
        Assert.That(vidaesperadasquirtle,Is.EqualTo(vidadada));
        Assert.That(nombrepokemonactual, Is.EqualTo("Charmander"));
    }
    /// <summary>
    /// Este test verifica la tercer historia de usuario que comprueba que las vidas se actualizan despues de un ataque
    /// </summary>
    [Test]
    public void PikachuDañaAPidgey()
    {
        Facade.Reset();
        // 95 de daño del ataque rayo * efectividad (2) 
        int vidaesperada = 0; // tiene 60 de vida y 40 de defensa así que aguantría un golpe de 99 de daño como mucho 
        Facade.Instance.StartBattle("Ash","joshua");
        Facade.Instance.AddPokemosA("Pikachu");
        Facade.Instance.AddPokemosD("Pidgey");
        Facade.Instance.InitializeBattle();
        Facade.Instance.UsePokemonMove(1); //Pikachu usa royo
        
        Assert.That(vidaesperada,Is.EqualTo(Facade.Instance.Menu.GetHpAtacante())); // Verificar que la vida de Pidgey es 0
        Assert.That(80,Is.EqualTo(Facade.Instance.Menu.GetHpDefensor())); // Verificar que Pikachu mantiene su HP
    }
    /// <summary>
    /// Este test verifica la cuarta historia de usuario ya que un ataque electrico a un tipo planta le hace la mitad del daño del ataque
    /// Además tambien cumple con la septima historia de usuario que dice puedo de pokemon cuando es mi turno pasando de Squirtle a Bulbasaur
    /// Además que el que comienza es squirtle y bulbasaur , para luego jugar pikachu repetando el orden del enfrentamiento
    /// </summary>
    [Test]
    public void BulbasaurPorSquirtleParaAguantarAPikachu()
    {
        Facade.Reset();
        Facade.Instance.StartBattle("Ash", "Red");
        Facade.Instance.AddPokemosA("Squirtle");
        Facade.Instance.AddPokemosA("Bulbasaur");
        Facade.Instance.AddPokemosD("Pikachu");
        Facade.Instance.InitializeBattle();
        Facade.Instance.ChangePokemon(1); // Cambia a bulbasaur
        Facade.Instance.UsePokemonMove(2); // Pikachu usa rayo, daño de 65/2 = 32,5 
        
        int dañoPorAtaque = 95 / 2 ;
        int defensaBulbasaur = 70; 
        int hpBulbasaurEsperado = 90;
        int vidabulbasaurRestanteEsperada = 90; // Sabemos que no alzcanza para romper su defensa
        int vidatortugaEsperada = 80;
            
        Assert.That(vidabulbasaurRestanteEsperada,Is.EqualTo(Facade.Instance.Menu.GetHpAtacante())); // Verificar que la vida de bulbasour es la esperada
        Facade.Instance.ChangePokemon(1); // Cambiar a Squirtle
        Assert.That(vidatortugaEsperada,Is.EqualTo(Facade.Instance.Menu.GetHpDefensor())); //Verifica que bulbasaur sigue intacto
    }
    /// <summary>
    /// Este test verifica la quinta historia de usuario ya que pikachu siempre va a ser el pokemon atacante, es decir el que empiece
    /// Junto a esto tenemos un Console.WriteLine que indica de quien es el turno (Tenemos pensado hacer un string builder que forme el mensaje para el usuario en cada turno)
    /// </summary>
    [Test]
    public void Defensa()//Demuestra que defensa no hace daño
    {
        Facade.Reset();
        Facade.Instance.StartBattle("Ash", "Red");
        Facade.Instance.AddPokemosA("Pikachu");
        Facade.Instance.AddPokemosD("Charmander");
        Facade.Instance.InitializeBattle();
        Facade.Instance.UsePokemonMove(4);//El movimiento 4 siempre es de defensa, por lo que no provoca daño al contrincante
        double vidaesperadadefensor = 80;
        double vidaObtenidaDefensor = Facade.Instance.Menu.GetHpDefensor();
        
        Assert.That(vidaesperadadefensor,Is.EqualTo(vidaObtenidaDefensor));
    }
    /// <summary>
    /// Este test verifica la sexta historia de usuario ya que la batalla inicia y termina
    /// El mensaje impreso en programa pasará a ser un string pasado al bot de discord
    /// </summary>
    [Test]
    public void GanoBatalla()
    {
        Facade.Reset();
        Facade.Instance.StartBattle("Ash", "Red");
        Facade.Instance.AddPokemosA("Pikachu");
        Facade.Instance.AddPokemosD("Pidgey");
        Facade.Instance.InitializeBattle();
        Facade.Instance.UsePokemonMove(1); //Pikachu usa royo
        Facade.Instance.ShowOpponentStatus();
        Facade.Instance.UsePokemonMove(3);
        Facade.Instance.UsePokemonMove(2);
        Facade.Instance.UsePokemonMove(2);
        bool batallasigue = Facade.Instance.IsBattleOngoing();
        bool estadobattallasupuesto = false; //Is going da false porque ya terminó la batalla al haber ganado alguien
        
        Assert.That(batallasigue, Is.EqualTo(estadobattallasupuesto));
    }
    /// <summary>
    /// Este test verifica la septima historia de usuario ya que podes cambiar correctamente
    /// de Pokemon durante la batalla
    /// </summary>
    [Test]
    public void CambioPokemon()//Verificacion Cambio de Pokemon de Turno
    {
        Facade.Reset();
        Facade.Instance.StartBattle("Ash", "Red");
        Facade.Instance.AddPokemosA("Squirtle");//Squirtle era el Pokemon en Turno al inicio porque fue agregado primero
        Facade.Instance.AddPokemosD("Charmander");
        Facade.Instance.AddPokemosA("Bulbasaur");//Bulbasaur era el segundo pokemon del equipo
        Facade.Instance.InitializeBattle();
        Facade.Instance.ChangePokemon(1);//Bulbasaur para a ser el Pokemon en Turno
        Facade.Instance.UsePokemonMove(1);
        string pokemonesperado = "Bulbasaur";
        string pokemonobtenido = Facade.Instance.Menu.GetPokemonActual().GetName();
        Assert.That(pokemonesperado,Is.EqualTo(pokemonobtenido));
    }
    /// <summary>
    /// Este test verifica la octava historia de usuario que nos permite usar varios items
    /// </summary>
    [Test]
    public void UsoItemEnBatalla()
    {
        Facade.Reset();
        //Este test muestra el uso de un revivir en la batalla
        Facade.Instance.StartBattle("Ash", "Red");
        Facade.Instance.AddPokemosA("Squirtle");//Squirtle era el Pokemon en Turno al inicio porque fue agregado primero
        Facade.Instance.AddPokemosD("Charmander");
        var pokemon = Facade.Instance.Menu.GetPokemonActual();
        Facade.Instance.InitializeBattle();
        pokemon.ChangeIsAlive();
        // Usar Revivir para restaurar 50% del HP total
        Facade.Instance.UseItem("revivir", 0); //Revive a Squirtle
        double vidaEsperada = 40;
        double vidaObtenida = pokemon.GetVidaActual();
        Assert.That(vidaObtenida, Is.EqualTo(vidaEsperada)); // Verifica que Squirtle tiene 40 HP no anda aún
    }
    /// <summary>
    /// Este test verifica la novena historia de usuario que nos permite unirnos a la lista
    /// de jugadores esperando por un oponente
    /// </summary>
    [Test]
    public void AgregarJugadorAListaDeEspera()
    {
        Facade.Reset();
        Facade.Instance.StartBattle("Ash","red");
        Facade.Instance.AddPokemosA("Squirtle");
        Facade.Instance.AddPokemosD("Charmander");

        Assert.That(Facade.Instance.AddTrainerToWaitingList("Ash"),Is.EqualTo("Ash agregado a la lista de espera"));
    }
    /// <summary>
    /// Este test verifica la decima historia de usuario que nos permite ver qué jugadores
    /// están en la lista esperando por un oponente cuando hay oponentes y cuando no hay
    /// </summary>
    [Test]
    public void VerListaDeJugadoresEsperandoUnOponente()
    {
        Facade.Instance.StartBattle("Ash","red");
        Facade.Instance.AddPokemosA("Squirtle");
        Facade.Instance.AddPokemosD("Charmander");
        Facade.Instance.AddTrainerToWaitingList("Ash");
        
        Assert.That(Facade.Instance.GetAllTrainersWaiting(),Is.EqualTo("Esperan: Ash; "));

        Facade.Instance.RemoveTrainerFromWaitingList("Ash");
        string texto = Facade.Instance.GetAllTrainersWaiting();
        
        Assert.That(texto,Is.EqualTo("No hay nadie esperando"));
    }

    /// <summary>
    /// Este test verifica la decimo uno historia de usuarioque nos permite
    /// iniciar una batalla con un jugador que esta esperando por un oponente
    /// </summary>
    [Test]
    public void IniciarBatallaConJugadorQueEstaEsperandoPorOponente()
    {
        Facade.Instance.StartBattle("Ash","red");
        Facade.Instance.AddPokemosA("Squirtle");
        Facade.Instance.AddPokemosD("Charmander");
        Facade.Instance.AddTrainerToWaitingList("red");
        
        Assert.That(Facade.Instance.StartBattle("Ash", ""),Is.EqualTo("Comienza Ash vs red"));
    }
    [Test]
    public void PidgeyPorCharmanderParaAguantarAPikachu()
    {
        Facade.Reset();
        double dañoPorAtaque = 65; // Daño de rayo
        double defensaCharmander = 60; // Defensa de Charmander
        double hpCharmander = 85; // Charmander arranca con 85 de vida
        double vidaCharmanderRestanteEsperada = hpCharmander - (dañoPorAtaque - defensaCharmander); // Calculos de supuesta vida charmander
        double vidaPidgeyEsperada = 60; // Pidgey debería iniciar con 60 de vida

        Facade.Instance.StartBattle("Ash", "Red");
        Facade.Instance.AddPokemosA("Pidgey"); 
        Facade.Instance.AddPokemosD("Pikachu");
        Facade.Instance.AddPokemosA("Charmander");
        Facade.Instance.InitializeBattle();
        Facade.Instance.ChangePokemon(1); // Cambia a Charmander
        Facade.Instance.Menu.SetStrategyPresicion(new StrategyPreciso());
        Pokemon pokemon = Facade.Instance.Menu.GetPokemonRival();
        pokemon.SetStrategy(new AtaqueNoCritico());
        Facade.Instance.UsePokemonMove(2);//Pikachu usa rayo, danio de rayo: 65, defensa de Charmander: vida 85, defensa: 60
        
        Assert.That(vidaCharmanderRestanteEsperada,Is.EqualTo(Facade.Instance.Menu.GetHpAtacante())); // Verificar que la vida de Charmander es la esperada
        Facade.Instance.ChangePokemon(1);//Cambio a Pidgey, pasa a ser defensor al usar su turno
        Facade.Instance.UsePokemonMove(4);//Pikachu usa proteccion
        Assert.That( Facade.Instance.Menu.GetHpAtacante(),Is.EqualTo(vidaPidgeyEsperada)); //Verifica que pidgey sigue intecto
    }
    [Test]
    public void UsoItemEnBatalla2()
    {
        Facade.Reset();
        //Este test muestra el uso de la CuraTotal en batalla
        Quemar quemar = new Quemar();
        Facade.Instance.StartBattle("Ash", "Red");
        Facade.Instance.AddPokemosA("Squirtle");//Squirtle era el Pokemon en Turno al inicio porque fue agregado primero
        Facade.Instance.AddPokemosD("Charmander");
        Pokemon pokemon2 = Facade.Instance.Menu.GetPokemonActual();
        Facade.Instance.InitializeBattle();
        Pokemon rival = Facade.Instance.Menu.GetPokemonRival();
        quemar.HacerEfecto(pokemon2);
        // Usa CuraTotal para quitarle el efecto de dormido
        Facade.Instance.UseItem("curatotal", 0); //Cura el efecto de squirtle
        Efecto efectohecho = pokemon2.GetEfecto();
        Efecto efectoesperado = rival.GetEfecto();
        Assert.That(efectohecho, Is.EqualTo(efectoesperado)); // Compara el efecto de squirtle con el del rival, los dos son nulos
    }

    [Test]
    public void UsoItemEnBatalla3()
    {
        Facade.Reset();
        //Este test muestra el uso de la superpocion en batalla
        Facade.Instance.StartBattle("Ash", "Red");
        Facade.Instance.AddPokemosA("Pikachu");
        Facade.Instance.AddPokemosD("Arbok");
        Facade.Instance.InitializeBattle();
        Facade.Instance.UsePokemonMove(2);//Jugador 1 usa Electrobola
        Facade.Instance.UsePokemonMove(1);//Jugador2 usa LanzaMugre
        Facade.Instance.UseItem("superpocion", 0); //Cura a Pikachu
        double vidaEsperada2 = 60;
        Pokemon pikachu = Facade.Instance.Menu.GetPokemonActual();
        double vidaObtenida2 = pikachu.GetVidaActual();
        // Usar Superpoción para restaurar 70 HP 
        Assert.That(vidaEsperada2, Is.EqualTo(vidaObtenida2));
    }

    [Test]
    public void UsoItemEnBatalla4()
    {
        Facade.Reset();
        //Este test demuestra que no se pueden usar items una vez se acabaron ya que el pokemon va a seguir dormido
        Dormir dormido2 = new Dormir();
        Facade.Instance.StartBattle("Ash", "Red");
        Facade.Instance.AddPokemosA("Charmander");
        Facade.Instance.AddPokemosD("Pidgey");
        Pokemon pokemon4 = Facade.Instance.Menu.GetPokemonActual();
        Pokemon rival2 = Facade.Instance.Menu.GetPokemonRival();
        dormido2.HacerEfecto(pokemon4);
        Facade.Instance.UseItem("curatotal",0);
        dormido2.HacerEfecto(pokemon4);
        Facade.Instance.UsePokemonMove(4); // para que avance el turno
        Facade.Instance.UseItem("curatotal",0);
        Facade.Instance.UsePokemonMove(4); // para que avance el turno
        dormido2.HacerEfecto(pokemon4);
        Facade.Instance.UseItem("curatotal",0); // no va a dejar
        Efecto estado = pokemon4.GetEfecto();
        dormido2.HacerEfecto(rival2);
        Efecto estadormido = rival2.GetEfecto();
        
        Assert.That(estado,Is.EqualTo(estadormido));
    }
    
    [Test]
    public void UsoDeEnvenenamiento()
    {
        Facade.Reset();
        //Este test muestra como un pokemon envenena a otro
        Facade.Instance.StartBattle("qcy", "manu¿");
        Facade.Instance.AddPokemosA("Arbok");
        Facade.Instance.AddPokemosD("Squirtle");
        Facade.Instance.InitializeBattle();
        Facade.Instance.Menu.SetStrategyPresicion(new StrategyPreciso());
        Pokemon pokemon = Facade.Instance.Menu.GetPokemonRival();
        pokemon.SetStrategy(new AtaqueNoCritico());
        Facade.Instance.UsePokemonMove(1);
        double vidaesperadasquirtle = 66;
        double vidadada = Facade.Instance.Menu.GetHpAtacante();
        Assert.That(vidaesperadasquirtle,Is.EqualTo(vidadada));
    }
}
