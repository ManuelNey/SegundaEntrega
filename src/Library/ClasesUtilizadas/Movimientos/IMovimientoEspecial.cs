using Library.Tipos;
using Library.Tipos.Paralisis_Strategy;

namespace Ucu.Poo.Pokemon;
//IMovimientoEspecial:
//Define las propiedades relacionadas con movimientos especiales, lo que mantiene la cohesión de la interfaz.
//LSP: Puede ser sustituida por cualquier subclase que implemente esta interfaz. Tambien cumple con polimorfismo.
//Cumple con ISP: Debido a que esta separada de IMovimimentoAtaque e IMovimientoDefensa, cada una tiene sus metodos 
//correspondientes para cada tipo de movimiento, evitando que alguna tenga metodos irrelevantes.

public interface IMovimientoEspecial : IMovimientoAtaque
{
    /// <summary>
    /// Obtiene un valor que indica si el movimiento especial ha sido usado anteriormente.
    /// </summary>
    /// <returns><c>true</c> si el movimiento especial ha sido usado anteriormente, <c>false</c> en caso contrario.</returns>
    bool GetUsadoAnteriormente();

    /// <summary>
    /// Establece si el movimiento especial ha sido usado anteriormente.
    /// </summary>
    /// <param name="valor"><c>true</c> si el movimiento ha sido usado anteriormente, <c>false</c> en caso contrario.</param>
    void UsadoAnteriormente(bool valor);

    /// <summary>
    /// Obtiene el efecto asociado al movimiento especial.
    /// </summary>
    /// <returns>El efecto que se aplica cuando el movimiento especial es utilizado.</returns>
    Efecto GetEfecto();

}