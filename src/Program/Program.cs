//--------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//--------------------------------------------------------------------------------

using System;
using ClassLibrary;
using DefaultNamespace;
using Library.Combate;

namespace ConsoleApplication
{
    /// <summary>
    /// Programa de consola de demostración.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Punto de entrada al programa principal.
        /// </summary>
        public static void Main()
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
        }
    }
}