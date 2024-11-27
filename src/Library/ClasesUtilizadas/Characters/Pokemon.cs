using Library.Combate;
using Library.Tipos;
using Library.Tipos.Paralisis_Strategy;
using Ucu.Poo.DiscordBot.ClasesUtilizadas.Characters.Strategy_Ataque;
using Ucu.Poo.Pokemon;

namespace DefaultNamespace;

//Clase Pokemon:
//Sigue el principio SRP al encargarse solo de gestionar atributos como vida, defensa, 
//movimientos, estado, acciones de ataque y defensa, etc.
//También aplica el principio Expert, ya que maneja su lógica propia al ejecutar los
//movimientos de los Pokemones, como defenderse, recibir ataques o danios de los efectos.
//El polimorfismo se muestra en el uso de las interfaces IMovimientoAtaque, IMovimientoDefensa e IMovimientoEspecial, 
//haciendo que diferentes tipos de movimientos interactúen con el mismo Pokémon. 
//Respeta el principio LSP porque los objetos que implementan estas interfaces pueden usarse de manera 
//intercambiable sin romper el funcionamiento.
public class Pokemon
{
    private string name;
    private List<IMovimiento> listaMovimientos;
    private List<Tipo> listaTipos;
    private double vidaActual;
    private double vidaTotal;
    private double defensa;
    private bool isAlive;
    private Efecto estado;
    private bool puedeAtacar;
    private IAtaqueDanioStrategy ataquedanio;
    public Paralizar paralisis;
    
    /// <summary>
    /// Constructor para crear un Pokémon con nombre, movimientos, tipos, vida y defensa.
    /// </summary>
    /// <param name="nombre">Nombre del Pokémon.</param>
    /// <param name="movimientos">Lista de movimientos del Pokémon.</param>
    /// <param name="tipos">Lista de tipos del Pokémon.</param>
    /// <param name="vida">Vida total del Pokémon.</param>
    /// <param name="defensa">Defensa del Pokémon.</param>
    public Pokemon(string nombre, List<IMovimiento> movimientos, List<Tipo> tipos, double vida, double defensa)
    {
        name = nombre;
        listaMovimientos = movimientos;
        listaTipos = tipos;
        vidaActual = vida;
        vidaTotal = vida;
        isAlive = true;
        this.defensa = defensa;
        puedeAtacar = true;
        this.ataquedanio = new AtaqueRandom();
    }

    public void SetStrategy(IAtaqueDanioStrategy ataque)
    {
        this.ataquedanio = ataque;
    }
    
    /// <summary>
    /// Aplica el efecto de un Pokémon a otro Pokémon.
    /// </summary>
    /// <param name="pokemon">El Pokémon objetivo al que se le aplicará el efecto.</param>
    public string HacerEfectoPokemon(Pokemon pokemon)
    {
        return estado.HacerEfecto(pokemon);
    }

    /// <summary>
    /// Obtiene la lista de tipos del Pokémon.
    /// </summary>
    /// <returns>Lista de tipos del Pokémon.</returns>
    public List<Tipo> GetTipos()
    {
        return listaTipos;
    }
    
    /// <summary>
    /// Obtiene el estado de vida del Pokémon (si está vivo o no).
    /// </summary>
    /// <returns>Retorna <c>true</c> si el Pokémon está vivo, <c>false</c> si está muerto.</returns>
    public bool GetIsAlive()
    {
        return isAlive;
    }

    /// <summary>
    /// Obtiene el nombre del Pokémon.
    /// </summary>
    /// <returns>Nombre del Pokémon.</returns>
    public string GetName()
    {
        return name;
    }
    
    /// <summary>
    /// Obtiene la lista de movimientos del Pokémon.
    /// </summary>
    /// <returns>Lista de movimientos del Pokémon.</returns>
    public List<IMovimiento> GetListaMovimientos()
    {
        return listaMovimientos;
    }

    /// <summary>
    /// Obtiene la vida total del Pokémon.
    /// </summary>
    /// <returns>Vida total del Pokémon.</returns>
    public double GetVidaTotal()
    {
        return vidaTotal;
    }

    /// <summary>
    /// Obtiene la vida actual del Pokémon.
    /// </summary>
    /// <returns>Vida actual del Pokémon.</returns>
    public double GetVidaActual()
    {
        return vidaActual;
    }

    //// <summary>
    /// Usa un movimiento del Pokémon (puede ser un movimiento de ataque o defensa).
    /// </summary>
    /// <param name="movimiento">Movimiento que se va a utilizar.</param>
    public string UsarMovimiento(IMovimiento movimiento)
    {
        string texto = "";
        if (isAlive)
        {
            foreach (IMovimiento accion in listaMovimientos)
            {

                if (accion.GetName() == movimiento.GetName() && movimiento is IMovimientoAtaque ataque)
                {
                    if (ataque is MovimientoDeAtaque) //Verifica si el movimiento usado en este momento Es de Ataque y no especial
                    {
                        foreach (IMovimiento mov in
                                 listaMovimientos) //Recorre todoslos movimientos de lalista hasta dar con el especial
                        {
                            if (mov is IMovimientoEspecial movesp)
                            {
                                movesp.UsadoAnteriormente(
                                    false); //Pone su estado de Uado anteriormente en false, ya que en este ataque se uso un movimiento comun y no uno especial
                            }
                        }
                    }
                }

                if (movimiento is IMovimientoDefensa defensamovimiento)
                {
                    defensa += defensamovimiento.GetDefensa();
                    return  $"{GetName()} ha usado su {movimiento.GetName()} para subir su defensa {defensamovimiento.GetDefensa()} puntos";
                }
            }
        }
        return texto;
    }

    /// <summary>
    /// Recibe un ataque de otro Pokémon.
    /// </summary>
    /// <param name="movimiento">Movimiento de ataque que inflige daño al Pokémon.</param>
    public string RecibirAtaque(IMovimientoAtaque movimiento)
    {
        double efectividadTipo = 1.0;
        Tipo tipoAtaque = movimiento.GetTipo();

        foreach (Tipo tipoDefensor in listaTipos)
        {
            efectividadTipo *= tipoAtaque.DarEfectividad(tipoDefensor);
        }

        string texto = "";
        double danio = (movimiento.GetAtaque() * efectividadTipo);
        int numero = ataquedanio.GetNumero();
        if (numero == 0)
        {
            danio *= 1.2;
            texto += "Además ha sido un ataque crítico.\n";
        }
        // Aplicar el daño a la defensa o vida o un poco y un poco
        if (defensa > danio)
        {
            defensa -= danio;
            texto += $"{GetName()} ha perdido {danio} de defensa, quedandose a {defensa} de defensa y {vidaActual} de vida";
        }
        else
        {
            danio -= defensa; // Resta el daño restante a la vida
            defensa = 0;
            vidaActual -= danio;
            if (vidaActual <= 0)
            {
                isAlive = false;
                vidaActual = 0;
                texto += ($"El pokemon {name} se ha debilitado, por que no podrá combatir más");
                return texto;
            }
            texto += ($"{GetName()} ha perdido toda su defensa y se ha quedado con {vidaActual} de vida restante");
        }
        if (movimiento is IMovimientoEspecial movimientoEspecial)
        {
            texto += AgregarEfecto(movimientoEspecial.GetEfecto());
        }
        return texto;
    }

    /// <summary>
    /// Recibe daño de un efecto externo.
    /// </summary>
    /// <param name="numero">Valor de daño recibido.</param>
    public string RecibirDanioDeEfecto(double numero)
    {
        double porcentaje = (numero *this.vidaTotal) / 100;
        this.vidaActual -= porcentaje;
        return $"{GetName()} ha recibido {porcentaje} de daño adicional. \n";
    }

    /// <summary>
    /// Establece si el Pokémon puede atacar en su turno.
    /// </summary>
    /// <param name="valor">Valor que indica si el Pokémon puede atacar.</param>
    public void SetPuedeAtacar(bool valor) //Funciona para cambiar el valor dentro de los efectos de paralisis y de dormir
    {
        puedeAtacar = valor; 
    }

    /// <summary>
    /// Agrega un efecto al Pokémon.
    /// </summary>
    /// <param name="efecto">El efecto a agregar al Pokémon.</param>
    public string AgregarEfecto(Efecto efecto)
    {
        if (estado == null)
        {
            estado = Efecto.CrearCopia(efecto); // Usa el tipo del efecto para crear una nueva instancia
            return $"\n{GetName()} caído bajo el efecto {efecto.GetType().Name}\n";
        }

        return "";
    }
    
    /// <summary>
    /// Elimina el efecto actual del Pokémon.
    /// </summary>
    public void EliminarEfectoActual()
    {
        estado = null;
        // Acá iría el método para eliminar el cambio de estado del pokemon
    }

    /// <summary>
    /// Obtiene el efecto actual que está aplicándose al Pokémon.
    /// </summary>
    /// <returns>El efecto actual del Pokémon.</returns>
    public Efecto GetEfecto()
    {
        return estado;
    }
    /// <summary>
    /// Cura al Pokémon incrementando su vida actual.
    /// </summary>
    /// <param name="vidacurada">Cantidad de vida a curar.</param>
    public void Curar(int vidacurada)
    {
        this.vidaActual += vidacurada;
        if (vidaActual > vidaTotal)
        {
            vidaActual = vidaTotal;
        }
    }
    /// <summary>
    /// Cambia el estado de vida del Pokémon. Si el Pokémon está muerto, lo revive; si está vivo, lo debilita.
    /// </summary>
    public void ChangeIsAlive()
    {
        if (this.isAlive == false)
        {
            this.isAlive = true;
        }
        else
        {
            this.isAlive = false;
            this.vidaActual = 0;
        }
    }

    /// <summary>
    /// Revive al Pokémon, asignándole la mitad de su vida total.
    /// </summary>
    public void Revivir()
    {
        this.vidaActual = vidaTotal/2;
    }

    /// <summary>
    /// Obtiene si el Pokémon puede atacar en su turno.
    /// </summary>
    /// <returns><c>true</c> si el Pokémon puede atacar, <c>false</c> si no puede.</returns>
    public bool GetPuedeAtacar()
    {
        return puedeAtacar;
    }

    /// <summary>
    /// Obtiene el valor de defensa del Pokémon.
    /// </summary>
    /// <returns>Defensa del Pokémon.</returns>
    public double GetDefensa()
    {
        return this.defensa;
    }
    
}