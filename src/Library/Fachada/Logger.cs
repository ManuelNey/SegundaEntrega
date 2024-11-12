using System;
using System.Text;

public class ManejadorMensajes
{
    private static ManejadorMensajes _instance;
    private StringBuilder mensaje;

    // Constructor privado para impedir que se cree una instancia desde fuera
    private ManejadorMensajes()
    {
        mensaje = new StringBuilder();
    }

    // Propiedad para acceder a la instancia única del Logger (singleton)
    public static ManejadorMensajes Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ManejadorMensajes();
            }
            return _instance;
        }
    }
    
    public void LogMessage(string message)
    {
        mensaje.AppendLine(message);
        Console.WriteLine(message); // Mostrar en consola si es necesario
    }

    // Método para obtener y limpiar el log, se lo envía al bot y se vacía
    public string GetAndClearLog()
    {
        string logContent = mensaje.ToString();
        mensaje.Clear();
        return logContent;
    }
}