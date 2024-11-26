using Library.Tipos;

namespace Ucu.Poo.Pokemon;
//MovimientoDeDefensa:
//Cumple con SRP: La clase MovimientoDeDefensa tiene una única responsabilidad: encapsular 
//la información relacionada con un movimiento de defensa, es decir, su nombre, su valor de 
//defensa y su tipo. No está sobrecargada con otras responsabilidades.
//Cumple con OCP: La clase está abierta para la extensión, ya que puedes crear nuevos tipos 
//de movimientos de defensa con diferentes valores para name, defensa y tipo. Está cerrada 
//para la modificación, ya que la lógica de la clase no requiere cambios para añadir nuevos 
//movimientos de defensa. Solo es necesario extender la clase si fuera necesario añadir 
//funcionalidades adicionales.
//Cumple con LSP: La clase MovimientoDeDefensa implementa correctamente la interfaz 
//IMovimientoDefensa. Puedes sustituir cualquier instancia de IMovimientoDefensa por una instancia 
//de MovimientoDeDefensa sin que se rompa el comportamiento del sistema, respetando el contrato de 
//la interfaz.
//Tiene alta cohesion, ya que todas sus propiedades y métodos están relacionados con un movimiento 
//defensivo.



public class MovimientoDeDefensa : IMovimientoDefensa
{
    private string Name { get; set; }
    private int Defensa { get; set; }
    private Tipo Tipo { get; set; }
    
    /// <summary>  
    /// Inicializa una nueva instancia de la clase <see cref="MovimientoDeDefensa"/>.  
    /// </summary>  
    /// <param name="name">El nombre del movimiento de defensa.</param>  
    /// <param name="defensa">El valor numérico de defensa del movimiento.</param>  
    /// <param name="tipo">El tipo asociado con el movimiento de defensa.</param>  
    /// <param name="es_especial">Indica si el movimiento es especial (no se utiliza en este contexto).</param>  
    public MovimientoDeDefensa(string name, int defensa, Tipo tipo)
    {
        this.Name = name;
        this.Defensa = defensa;
        this.Tipo = tipo;
    }
    /// <summary>  
    /// Obtiene el valor de defensa del movimiento.  
    /// </summary>  
    /// <returns>El valor de defensa del movimiento de defensa.</returns>  
    public int GetDefensa()
    {
        return Defensa;
    }
    
    /// <summary>  
    /// Obtiene el nombre del movimiento de defensa.  
    /// </summary>  
    /// <returns>El nombre del movimiento de defensa.</returns>  
    public string GetName()
    {
        return Name;
    }

    /// <summary>  
    /// Obtiene el tipo del movimiento de defensa.  
    /// </summary>  
    /// <returns>El tipo asociado con el movimiento de defensa.</returns>  
    public Tipo GetTipo()
    {
        return Tipo;
    }
   
}