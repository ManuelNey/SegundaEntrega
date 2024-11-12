using System;
using System.Collections.Generic;
using DefaultNamespace;
using Library.Combate;
using Library.Tipos;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Program.Tests.Combate;

[TestFixture]
// [TestSubject(typeof(Menu))]
public class MenuTest
{
    /// <summary>
    /// Prueba de la clase <see cref="Menu"/>.
    /// Estos test nos permiten verificar que las historias de usuarios están bien implementadas
    /// </summary>
    [Test]
    /// <summary>
    /// Este test verifica la tercer historia de usuario
    /// </summary>
    public void PikachuDañaAPidgey()
    {
        // 95 de daño del ataque rayo * efectividad (2) 
        int vidaesperada = 0; // tiene 60 de vida y 40 de defensa así que aguantría un golpe de 99 de daño como mucho 
        Menu menuPP = new Menu();
        menuPP.UnirJugadores("Ash");
        menuPP.UnirJugadores("Red");
        menuPP.AgregarPokemonesA("Pikachu");
        menuPP.AgregarPokemonesD("Pidgey");
        menuPP.IniciarEnfrentamiento();
        menuPP.GetHpDefensor();
        menuPP.UsarMovimientos(1); //Pikachu usa royo
        Assert.That(vidaesperada,Is.EqualTo(menuPP.GetHpAtacante())); // Verificar que la vida de Pidgey es 0
        Assert.That(80,Is.EqualTo(menuPP.GetHpDefensor())); // Verificar que Pikachu mantiene su HP
    }
    [Test]
    public void PidgeyPorCharmanderParaAguantarAPikachu()
    {
        // Arrange
        double dañoPorAtaque = 65; // Daño de rayo
        double defensaCharmander = 60; // Defensa de Charmander
        double hpCharmander = 85; // Charmander arranca con 85 de vida
        double vidaCharmanderRestanteEsperada = hpCharmander - (dañoPorAtaque - defensaCharmander); // Calculos de supuesta vida charmander
        double vidaPidgeyEsperada = 60; // Pidgey debería iniciar con 60 de vida

        Menu menuPP = new Menu();
        menuPP.UnirJugadores("Ash");
        menuPP.UnirJugadores("Red");
        menuPP.AgregarPokemonesA("Pidgey"); 
        menuPP.AgregarPokemonesD("Pikachu");
        menuPP.AgregarPokemonesA("Charmander");
        menuPP.IniciarEnfrentamiento();
        menuPP.CambiarPokemon(1); // Cambia a Charmander
        menuPP.UsarMovimientos(2);//Pikachu usa rayo, danio de rayo: 65, defensa de Charmander: vida 85, defensa: 60
        
        Assert.That(vidaCharmanderRestanteEsperada,Is.EqualTo(menuPP.GetHpAtacante())); // Verificar que la vida de Charmander es la esperada
        menuPP.CambiarPokemon(1);//Cambio a Pidgey, pasa a ser defensor al usar su turno
        menuPP.UsarMovimientos(4);//Pikachu usa proteccion
        Assert.That( menuPP.GetHpAtacante(),Is.EqualTo(vidaPidgeyEsperada)); //Verifica que pidgey sigue intecto
    }
    [Test]
    /// <summary>
    /// Este test verifica la cuarta historia de usuario ya que un ataque electrico a un tipo planta le hace la mitad del daño del ataque
    /// Además tambien cumple con la septima historia de usuario que dice puedo de pokemon cuando es mi turno pasando de Squirtle a Bulbasaur
    /// Además que el que comienza es squirtle y bulbasaur , para luego jugar pikachu repetando el orden del enfrentamiento
    /// </summary>
    public void BulbasaurPorSquirtleParaAguantarAPikachu()
    {
        int dañoPorAtaque = 95 / 2 ;
        int defensaBulbasaur = 70; 
        int hpBulbasaurEsperado = 90;
        int vidabulbasaurRestanteEsperada = 90; // Sabemos que no alzcanza para romper su defensa
        int vidatortugaEsperada = 80; 

        Menu menuPP = new Menu();
        menuPP.UnirJugadores("Ash");
        menuPP.UnirJugadores("Red");
        menuPP.AgregarPokemonesA("Squirtle"); 
        menuPP.AgregarPokemonesD("Pikachu"); 
        menuPP.AgregarPokemonesA("Bulbasaur"); 
        menuPP.IniciarEnfrentamiento();
        menuPP.CambiarPokemon(1); // Cambia a bulbasaur
        menuPP.UsarMovimientos(2); // Pikachu usa rayo, daño de 65/2 = 32,5 
            
            
        Assert.That(vidabulbasaurRestanteEsperada,Is.EqualTo(menuPP.GetHpAtacante())); // Verificar que la vida de bulbasour es la esperada
        menuPP.CambiarPokemon(1); // Cambiar a Squirtle
        Assert.That(vidatortugaEsperada,Is.EqualTo(menuPP.GetHpDefensor())); //Verifica que bulbasaur sigue intecto
    }
    [Test]
    /// <summary>
    /// Este test verifica la segunda historia de usuario
    /// </summary>
    public void Especial() //En este test se puede ver que cuando el jugador 1 intenta usar el ataque especial de nuevo no puede hacerlo. 
    {
        Menu juego1 = new Menu();
        juego1.UnirJugadores("Ash");
        juego1.UnirJugadores("Red");
        juego1.AgregarPokemonesA("Pikachu");
        
        juego1.AgregarPokemonesD("Caterpie");
        
        juego1.IniciarEnfrentamiento();
        juego1.MostrarAtaquesDisponibles();
        juego1.UsarMovimientos(1);//Jugador 1 usa Rayo(especial), vida del contrincante en 45
        juego1.UsarMovimientos(1);//Jugador2 usa picotazo cola
        juego1.UsarMovimientos(1);//Jugador 1 intenta usar el Rayo nuevamente pero no puede, vida del contrincante se mantiene
        int vidaesperadadefensor = 5;
        double vidaObtenidaDefensor = juego1.GetHpDefensor();
        Assert.That(vidaesperadadefensor,Is.EqualTo(vidaObtenidaDefensor));
    }
    
    [Test]
    public void Inmune() //En este test se puede ver que cuando un Pokemon que es inmune a otro es atacado, su vida no se ve afectada
    {
        Menu juego1 = new Menu();
        juego1.UnirJugadores("Ash");
        juego1.UnirJugadores("Red");
        juego1.AgregarPokemonesA("Pikachu");
        //Usamos a pikachu porque electrico es inmune a electrico y el ataque Rayo es de tipo electrico
        juego1.AgregarPokemonesD("Pikachu");
        
        juego1.IniciarEnfrentamiento();
        juego1.UsarMovimientos(1);//Jugador 1 usa Rayo(electrico)
        juego1.UsarMovimientos(1);//Jugador2 usa Rayo(electrico)
        int vidaesperadadefensor = 80;
        double vidaObtenidaDefensor = juego1.GetHpDefensor();
        Assert.That(vidaesperadadefensor,Is.EqualTo(vidaObtenidaDefensor));
    }

    [Test]
    /// <summary>
    /// Este test verifica la quinta historia de usuario ya que pikachu siempre va a ser el pokemon atacante, es decir el que empiece
    /// Junto a esto tenemos un Console.WriteLine que indica de quien es el turno (Tenemos pensado hacer un string builder que forme el mensaje para el usuario en cada turno)
    /// </summary>
    public void Defensa()//Demuestra que defensa no hace daño
    {
        Menu juego2 = new Menu();
        juego2.UnirJugadores("Ash");
        juego2.UnirJugadores("Red");
        juego2.AgregarPokemonesA("Pikachu");
        juego2.AgregarPokemonesD("Charmander");
        juego2.IniciarEnfrentamiento();
        juego2.UsarMovimientos(4);//El movimiento 4 siempre es de defensa, por lo que no provoca daño al contrincante
        double vidaesperadadefensor = 80;
        double vidaObtenidaDefensor = juego2.GetHpDefensor();
        Assert.That(vidaesperadadefensor,Is.EqualTo(vidaObtenidaDefensor));
    }

    [Test]
    public void CambioPokemon()//Verificacion Cambio de Pokemon de Turno
    {
        Menu juego3 = new Menu();
        juego3.UnirJugadores("Ash");
        juego3.UnirJugadores("Red");
        juego3.AgregarPokemonesA("Squirtle");//Squirtle era el Pokemon en Turno al inicio porque fue agregado primero
        juego3.AgregarPokemonesD("Charmander");
        juego3.AgregarPokemonesA("Bulbasaur");//Bulbasaur era el segundo pokemon del equipo
        juego3.IniciarEnfrentamiento();
        juego3.CambiarPokemon(1);//Bulbasaur para a ser el Pokemon en Turno
        juego3.UsarMovimientos(1);
        string pokemonesperado = "Bulbasaur";
        string pokemonobtenido = juego3.GetPokemonActual().GetName();
        Assert.That(pokemonesperado,Is.EqualTo(pokemonobtenido));
    }

    [Test]
    /// <summary>
    /// Este test verifica la primer historia de usuario y verifica la historia de usuario que no suma a una lista a los usuarios que no pueden combatir
    /// </summary>
    public void Agrego6Pokemons()  
    {
        Menu juego4 = new Menu();
        juego4.UnirJugadores("Don Dimadon");
        juego4.UnirJugadores("Timmy Turner");
        juego4.UnirJugadores("Jorge");
        juego4.AgregarPokemonesA("Charmander");
        juego4.AgregarPokemonesA("Squirtle");
        juego4.AgregarPokemonesA("Bulbasaur");
        juego4.AgregarPokemonesA("Dratini");
        juego4.AgregarPokemonesA("Arbok");
        juego4.AgregarPokemonesA("Charmander");
        //List<Pokemon> listadada = juego4.(); falta getear el equipo del atacantes
        List<string> listadadaAstring = new List<string>();
        foreach (Pokemon pokemon in juego4.GetEquipoA())
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
        
        // CollectionAssert.AreEqual(listaesperada,listadadaAstring);
    }

    [Test]
    public void VeoHPDeMisPokemonsYPokemonsOponentes()
    {
        Menu juego4 = new Menu();
        juego4.UnirJugadores("Don Dimadon");
        juego4.UnirJugadores("Bellota");
        juego4.AgregarPokemonesA("Charmander");//85
        juego4.AgregarPokemonesD("Squirtle");//80
        string vidasdadas= $"{juego4.GetHpAtacante()}/{juego4.GetHpDefensor()}";
        string vidasesperadas = "85/80";
        Assert.That(vidasdadas, Is.EqualTo(vidasesperadas));
    }
    [Test]
    /// <summary>
    /// Este test verifica la sexta historia de usuario ya que la batalla inicia y termina
    /// El mensaje impreso en programa pasará a ser un string pasado al bot de discord
    /// </summary>
    public void GanoBatalla()
    {
        Menu juego6 = new Menu();
        juego6.UnirJugadores("Ash");
        juego6.UnirJugadores("Red");
        juego6.AgregarPokemonesA("Pikachu");
        juego6.AgregarPokemonesD("Pidgey");
        juego6.IniciarEnfrentamiento();
        juego6.UsarMovimientos(1); //Pikachu usa royo
        juego6.MostrarEstadoRival();
        juego6.UsarMovimientos(3);
        juego6.UsarMovimientos(2);
        juego6.UsarMovimientos(2);
        bool batallaganada = juego6.GetBatallaI() && juego6.GetBatallaT();
        bool batallaganadasupuesta = true;
        Assert.That(batallaganada, Is.EqualTo(batallaganadasupuesta));
    }
    [Test]
    /// <summary>
    /// Este test verifica la  historia de usuario que nos permite usar varios items
    /// </summary>
    public void UsoItemEnBatalla()
    {
        //Este test muestra el uso de un revivir en la batalla
        Menu juego1 = new Menu();
        juego1.UnirJugadores("Ash");
        juego1.UnirJugadores("Red");
        juego1.AgregarPokemonesA("Squirtle");//Squirtle era el Pokemon en Turno al inicio porque fue agregado primero
        juego1.AgregarPokemonesD("Charmander");
        Pokemon pokemon = juego1.GetPokemonActual();
        juego1.IniciarEnfrentamiento();
        pokemon.ChangeIsAlive();
        // Usar Revivir para restaurar 50% del HP total
        juego1.UsarItem("Revivir", 0); //Revive a Squirtle
        double vidaEsperada = 40;
        double vidaObtenida = pokemon.GetVidaActual();
        Assert.That(vidaObtenida, Is.EqualTo(vidaEsperada)); // Verifica que Squirtle tiene 40 HP no anda aún
        
        //Este test muestra el uso de la CuraTotal en batalla
        Menu juego2 = new Menu();
        Dormir dormido = new Dormir();
        juego2.UnirJugadores("Ash");
        juego2.UnirJugadores("Red");
        juego2.AgregarPokemonesA("Squirtle");//Squirtle era el Pokemon en Turno al inicio porque fue agregado primero
        juego2.AgregarPokemonesD("Charmander");
        Pokemon pokemon2 = juego1.GetPokemonActual();
        juego2.IniciarEnfrentamiento();
        Pokemon rival = juego1.GetPokemonRival();
        dormido.HacerEfecto(pokemon2);
        // Usa CuraTotal para quitarle el efecto de dormido
        juego2.UsarItem("Curatotal", 0); //Cura el efecto de squirtle
        Efecto efectohecho = pokemon2.GetEfecto();
        Efecto efectoesperado = rival.GetEfecto();
        Assert.That(efectohecho, Is.EqualTo(efectoesperado)); // Compara el efecto de squirtle con el del rival, los dos son nulos
        
        //Este test muestra el uso de la superpocion en batalla
        Menu juego3 = new Menu();        
        juego3.UnirJugadores("Ash");
        juego3.UnirJugadores("Red");
        juego3.AgregarPokemonesA("Pikachu");
        
        juego3.AgregarPokemonesD("Arbok");
        
        juego3.IniciarEnfrentamiento();
        juego3.UsarMovimientos(2);//Jugador 1 usa Electrobola
        juego3.UsarMovimientos(1);//Jugador2 usa LanzaMugre
        juego3.UsarItem("Superpocion", 0); //Cura a Pikachu
        double vidaEsperada2 = 80;
        Pokemon pikachu = juego3.GetPokemonRival();
        double vidaObtenida2 = pikachu.GetVidaActual();
        // Usar Superpoción para restaurar 70 HP 
        Assert.That(vidaEsperada2, Is.EqualTo(vidaObtenida2));
        
        //Este test demuestra que no se pueden usar items una vez se acabaron ya que el pokemon va a seguir dormido
        Menu juego4 = new Menu();
        Dormir dormido2 = new Dormir();

        juego4.UnirJugadores("Ash");
        juego4.UnirJugadores("Red");
        juego4.AgregarPokemonesA("Charmander");
        juego4.AgregarPokemonesD("Pidgey");
        Pokemon pokemon4 = juego4.GetPokemonActual();
        Pokemon rival2 = juego4.GetPokemonRival();
        dormido2.HacerEfecto(pokemon4);
        juego4.UsarItem("Curatotal",0);
        dormido2.HacerEfecto(pokemon4);
        juego4.UsarItem("Curatotal",0); // para que avance el turno
        juego4.UsarItem("Curatotal",0);
        juego4.UsarItem("Curatotal",0); // para que avance el turno
        dormido2.HacerEfecto(pokemon4);
        juego4.UsarItem("Curatotal",0); // no va a dejar
        Efecto estado = pokemon4.GetEfecto();
        dormido2.HacerEfecto(rival2);
        Efecto estadormido = rival2.GetEfecto();
        
        Assert.That(estado,Is.EqualTo(estadormido));
    }
    [Test]
    public void UsoDeEnvenenamiento()
    {
        Menu Menu1 = new Menu();
        Menu1.UnirJugadores("Ansu");
        Menu1.UnirJugadores("Cima");
        Menu1.AgregarPokemonesA("Arbok");
        Menu1.AgregarPokemonesD("Squirtle");
        Menu1.IniciarEnfrentamiento();
        Menu1.UsarMovimientos(1);
        double vidaesperadasquirtle = 60;
        double vidadada = Menu1.GetHpDefensor();
        Assert.That(vidaesperadasquirtle,Is.EqualTo(vidadada));
    }
   
}
