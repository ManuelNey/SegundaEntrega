using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

public class GetHistorialCommand: ModuleBase<SocketCommandContext>
{
    ///<summary>
    ///
    ///</summary>
    [Command("historial")]
    [Summary("Devuelve el historial de movimientos")]
    public async Task ExecuteAsync()
    {
        string result = Facade.Instance.RecibirLista();
        await ReplyAsync(result);
    }
}