using System.Text;

namespace ClassLibrary.Textos_para_Bot;

public interface ITexto
{
    string EnviarTexto(StringBuilder texto);
}