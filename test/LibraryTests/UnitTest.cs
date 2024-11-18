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
public class UnitTest
{
    /// <summary>
    /// Prueba de la clase <see cref="Menu"/>.
    /// Estos test nos permiten verificar que las historias de usuarios están bien implementadas
    /// </summary>
    [Test]
    public void TratoDeAtacarSinIniciarBatalla() //Verificacion Cambio de Pokemon de Turno
    {
        Menu juego3 = new Menu();
        juego3.UnirJugadores("Ash");
        juego3.UnirJugadores("Red");
        juego3.AgregarPokemonesA("Squirtle"); //Squirtle era el Pokemon en Turno al inicio porque fue agregado primero
        juego3.AgregarPokemonesD("Charmander");
        juego3.UsarMovimientos(1);
        Assert.That(juego3.GetHpDefensor(),Is.EqualTo(85));
    }

    [Test]
    /// <summary>
    /// Este test verifica la primer historia de usuario y verifica la historia de usuario que no suma a una lista a los usuarios que no pueden combatir
    /// </summary>
    public void NoAgregoPokemons()
    {
        Menu juego4 = new Menu();
        juego4.UnirJugadores("Don Dimadon");
        juego4.UnirJugadores("Timmy Turner");
        juego4.AgregarPokemonesD("Squirtle");
        juego4.IniciarEnfrentamiento();
        juego4.UsarMovimientos(1);
        double vidaDefensor = juego4.GetHpDefensor();
        Assert.That(vidaDefensor,Is.EqualTo(80));
    }

    [Test]
    

    public void JugadorUsaPocionParaPokemonConVidaCompleta()
    {
        Menu juego4 = new Menu();
        juego4.UnirJugadores("Don Dimadon");
        juego4.UnirJugadores("Bellota");
        juego4.AgregarPokemonesA("Charmander"); //85
        juego4.UsarItem("Superpocion", 1);
        int vidatotalCharmander = 85;
        Assert.That(vidatotalCharmander, Is.EqualTo(juego4.GetHpAtacante()));
    }

    [Test]
    /// <summary>
    /// Este test verifica la sexta historia de usuario ya que la batalla inicia y termina
    /// El mensaje impreso en programa pasará a ser un string pasado al bot de discord
    /// </summary>
    public void PierdoBatalla()
    {
        Menu juego6 = new Menu();
        juego6.UnirJugadores("Ash");
        juego6.UnirJugadores("Red");
        juego6.AgregarPokemonesA("Pikachu");
        juego6.AgregarPokemonesD("Pidgey");
        juego6.IniciarEnfrentamiento();
        juego6.UsarMovimientos(1); //Pikachu usa royo
        juego6.MostrarEstadoEquipo();
        bool batallaperdida = juego6.GetBatallaI() && juego6.GetBatallaT();
        bool batallaperdidasupuesta = true;
        Assert.That(batallaperdida, Is.EqualTo(batallaperdidasupuesta));
    }

    [Test]
    /// <summary>
    /// Este test verifica la  historia de usuario que nos permite usar varios items
    /// </summary>
    public void JugadorAgrega7Pokemons()
    {
        StringWriter consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        Menu juego1 = new Menu();
        juego1.UnirJugadores("Ash");
        juego1.UnirJugadores("Red");

        juego1.AgregarPokemonesA("Squirtle");
        juego1.AgregarPokemonesA("Pidgey");
        juego1.AgregarPokemonesA("Larvitar");
        juego1.AgregarPokemonesA("Caterpie");
        juego1.AgregarPokemonesA("Charmander");
        juego1.AgregarPokemonesA("Dratini");
        juego1.AgregarPokemonesA("Gengar");

        List<Pokemon> listapokemonsatacante = juego1.GetEquipoA();
        int numero = listapokemonsatacante.Count;

        Assert.That(6, Is.EqualTo(numero));

    }

    [Test]
    public void JugadorTrataDeUsarPokemonQueNoTiene()
    {
        Menu Menu1 = new Menu();
        Menu1.UnirJugadores("Satoshi");
        Menu1.UnirJugadores("Kasumi");
        Menu1.AgregarPokemonesA("Arbok");
        Menu1.AgregarPokemonesD("Squirtle");
        Menu1.IniciarEnfrentamiento();
        Menu1.CambiarPokemon(2);
        Assert.That(Menu1.GetPokemonActual().GetName(), Is.EqualTo("Arbok"));
    }
}
