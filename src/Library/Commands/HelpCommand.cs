using Discord;
using Discord.Commands;

namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'help' del bot.
/// Devuelve información sobre qué hace cada comando disponible.
/// </summary>
// ReSharper disable once UnusedType.Global
public class HelpCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'help'.
    /// </summary>
    [Command("help")]
    [Summary("Muestra una lista de los comandos disponibles y su descripción.")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync()
    {
        try
        {
            string helpMessage = """
            **Lista de Comandos:**
            - `!addpokemon`: Agrega un Pokémon al equipo del usuario
            - `!battle <user>`: Une al jugador que manda el comando con el user ingresado para una batalla
            - `!changepokemon <num pokemon>`: Cambia el pokemon actual del jugador por el seleccionado en la lista. Solo se puede usar si es el turno del jugador
            - `!help`: Muestra las funciones de cada uno de los comandos
            - `!isbattleongoing`: Muestra si hay una batalla activa
            - `!join`: Une el usuario que envía el mensaje a la lista de espera
            - `!leave`: Remueve al usuario que envía el mensaje de la lista de espera
            - `!opponentstatus`: Muestra el estado actual del equipo del oponente de quien mandó el mensaje. Solo se puede usar si es el turno del jugador
            - `!playerstatus`: Muestra el estado actual del equipo del jugador que mandó el mensaje. Solo se puede usar si es el turno del jugador
            - `!ShowPokemonDisponibles`: Muestra el equipo del usuario que realizó el usuario y qué número tiene cada pokemon
            - `!showpokedex`: Muestra la lista de Pokémon disponibles
            - `!showitems`: Muestra los items disponibles del usuario que usó el comando. Solo se puede usar si es el turno del jugador
            - `!ShowMov`: Muestra los movimientos disponibles del pokemon combatiente
            - `!usemov`: Usa un movimiento del pokemon en turno. Solo se puede usar si es el turno del jugador
            - `!useitem <nombre item> <num pokemon>`: Usa el item dado en el pokemon seleccionado. Solo se puede usar si es el turno del jugador
            - `!waitinglist`: Muestra la lista de espera actual
            - `!who <user>`: Muestra el estado del usuario dado, si está en una batalla o no. Si no se ingresa uno, devolverá la información de quien mandó el mensaje.
            """;
            // Construcción del embed para mostrar el gif de pikachu 
            var embed = new EmbedBuilder()
                .WithTitle("Ayuda del Bot")
                .WithDescription(helpMessage)
                .WithImageUrl("https://media1.tenor.com/m/q63GikDmOygAAAAd/pokemon-pikachu.gif")
                .WithColor(Color.Gold) //Pone el borde de color dorado
                .Build();

            // Enviar el embed como respuesta
            await ReplyAsync(embed: embed);
        }
        catch (Exception ex)
        {
            // Manejo de errores
            await ReplyAsync($"Ocurrió un error al generar la ayuda: {ex.Message}");
        }
    }
}
