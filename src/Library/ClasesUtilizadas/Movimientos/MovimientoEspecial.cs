using Library.Tipos;
using Library.Tipos.Paralisis_Strategy;

namespace Ucu.Poo.Pokemon;
//MovimientoEspecial:
//SRP:  La clase MovimientoEspecial tiene una sola responsabilidad: encapsular la información sobre 
//un movimiento especial, que incluye el nombre, ataque, tipo, precisión, efecto y si ha sido usado 
//previamente. No tiene otras responsabilidades.
//Cumple con OCP: La clase está abierta a la extensión. Se pueden agregar más movimientos especiales 
//creando nuevas instancias de MovimientoEspecial con diferentes parámetros (nombre, ataque, tipo, 
//precisión, efecto). Está cerrada a la modificación, lo que significa que no es necesario cambiar 
//la clase para agregar nuevos tipos de movimientos especiales.
//LSP: La clase MovimientoEspecial implementa correctamente la interfaz IMovimientoEspecial.Cualquier 
//instancia de IMovimientoEspecial se puede sustituir por una instancia de MovimientoEspecial sin 
//romper el comportamiento del sistema.
// La clase tiene alta cohesión, ya que todos sus atributos y métodos están estrechamente relacionados 
//con el concepto de un movimiento especial.
//Al implementar la interfaz IMovimientoEspecial, la clase puede ser tratada de manera polimórfica. 
//Esto significa que se pueden gestionar diferentes tipos de movimientos especiales sin saber qué tipo 
//específico se está utilizando.



public class MovimientoEspecial : IMovimientoEspecial
{
    private string Name { get; set; }
    private int Ataque { get; set; }
    private Efecto Efecto { get; set; }
    private Tipo Tipo { get; set; }
    private int Precision { get; set; }
    private bool UsadoAnteriormentee { get; set; }
    
    /// <summary>  
    /// Inicializa una nueva instancia de la clase <see cref="MovimientoEspecial"/>.  
    /// </summary>  
    /// <param name="name">El nombre del movimiento especial.</param>  
    /// <param name="ataque">El valor numérico de ataque del movimiento.</param>  
    /// <param name="tipo">El tipo asociado con el movimiento especial.</param>  
    /// <param name="precision">La precisión del movimiento especial.</param>  
    /// <param name="efecto">El efecto que produce el movimiento especial.</param>  
    public MovimientoEspecial(string name, int ataque, Tipo tipo, int precision, Efecto efecto)
    {
        this.Name = name;
        this.Ataque = ataque;
        this.Tipo = tipo;
        this.Precision = precision;
        this.Efecto = efecto;
    }
    
    /// <summary>  
    /// Establece si el movimiento especial ha sido usado anteriormente.  
    /// </summary>  
    /// <param name="valor">Valor a establecer: <c>true</c> si ha sido usado, <c>false</c> en caso contrario.</param>  
    public void UsadoAnteriormente(bool valor) //Setea el valor de los ataques especiales para saber si se pueden usar
    { 
        UsadoAnteriormentee = valor; 
    }
    
    /// <summary>  
    /// Obtiene el valor de ataque del movimiento especial.  
    /// </summary>  
    /// <returns>El valor de ataque del movimiento especial.</returns>  
    public int GetAtaque()
    {
        return Ataque;
    }
    
    /// <summary>  
    /// Obtiene el estado de si el movimiento especial ha sido utilizado anteriormente.  
    /// </summary>  
    /// <returns><c>true</c> si ha sido utilizado, <c>false</c> en caso contrario.</returns>  
    public bool GetUsadoAnteriormente()
    {
        return UsadoAnteriormentee;
    }
    
    /// <summary>  
    /// Obtiene el nombre del movimiento especial.  
    /// </summary>  
    /// <returns>El nombre del movimiento especial.</returns>  
    public string GetName()
    {
        return Name;
    }

    /// <summary>  
    /// Obtiene el tipo del movimiento especial.  
    /// </summary>  
    /// <returns>El tipo asociado con el movimiento especial.</returns>  
    public Tipo GetTipo()
    {
        return Tipo;
    }

    /// <summary>  
    /// Obtiene la precisión del movimiento especial.  
    /// </summary>  
    /// <returns>La precisión del movimiento especial.</returns>  
    public int GetPrecision()
    {
        return Precision;
    }
    
    /// <summary>  
    /// Obtiene el efecto del movimiento especial.  
    /// </summary>  
    /// <returns>El efecto asociado con el movimiento especial.</returns>  
    public Efecto GetEfecto()
    {
        return Efecto;
    }
    
}