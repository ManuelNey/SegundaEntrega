using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

public class SurrenderCommand:ModuleBase<SocketCommandContext>
{
    ///<summary>
    ///
    ///</summary>
    [Command("surrender")]
    [Summary("Hace perder automaticamente al equipo")]
    public async Task ExecuteAsync()
    {
        var result = Facade.Instance.Rendirse();
        await ReplyAsync(result);
    }
}