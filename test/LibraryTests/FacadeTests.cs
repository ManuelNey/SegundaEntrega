using NUnit.Framework;

namespace Ucu.Poo.DiscordBot.Domain.Tests;

public class FacadeTests
{
    [TearDown]
    public void TearDown()
    {
        Facade.Reset();
    }
    
    [Test]
    public void AddTrainerToWaitingList_WhenTrainerWasNotAdded_ReturnsAdded()
    {
        string result = Facade.Instance.AddTrainerToWaitingList("user");
        
        Assert.That(result, Is.EqualTo("user agregado a la lista de espera"));
    }
    
    [Test]
    public void AddTrainerToWaitingList_WhenTrainerWasAdded_ReturnsExisting()
    {
        Facade.Instance.AddTrainerToWaitingList("user");
        
        string result = Facade.Instance.AddTrainerToWaitingList("user");
        
        Assert.That(result, Is.EqualTo("user ya está en la lista de espera"));
    }
    
    [Test]
    public void RemoveTrainerFromWaitingList_WhenTrainerWasAdded_ReturnsRemoved()
    {
        Facade.Instance.AddTrainerToWaitingList("user");
        
        string result = Facade.Instance.RemoveTrainerFromWaitingList("user");
        
        Assert.That(result, Is.EqualTo("user removido de la lista de espera"));
    }
    
    [Test]
    public void RemoveTrainerFromWaitingList_WhenTrainerWasNotAdded_ReturnsNotAdded()
    {
        string result = Facade.Instance.RemoveTrainerFromWaitingList("user");
        
        Assert.That(result, Is.EqualTo("user no está en la lista de espera"));
    }

    [Test]
    public void TrainerIsWaiting_WhenNotAdded_ReturnsNotWaiting()
    {
        string result = Facade.Instance.TrainerIsWaiting("user");
        
        Assert.That(result, Is.EqualTo("user no está esperando"));
    }
    
    [Test]
    public void TrainerIsWaiting_WhenAdded_ReturnsWaiting()
    {
        Facade.Instance.AddTrainerToWaitingList("user");
        
        string result = Facade.Instance.TrainerIsWaiting("user");
        
        Assert.That(result, Is.EqualTo("user está esperando"));
    }

    [Test]
    public void GetAllTrainersWaiting_WhenNobodyIsWaiting_ReturnsNobodyIsWaiting()
    {
        string result = Facade.Instance.GetAllTrainersWaiting();
        
        Assert.That(result, Is.EqualTo("No hay nadie esperando"));
    }
    
    [Test]
    public void GetAllTrainersWaiting_WhenSomebodyIsWaiting_ReturnsSomebody()
    {
        Facade.Instance.AddTrainerToWaitingList("user");
        
        string result = Facade.Instance.GetAllTrainersWaiting();
        
        Assert.That(result, Does.Contain("user"));
    }

    [Test]
    public void StartBattle_WhenNoOpponentAndNobodyIsWaiting_ReturnsNobodyIsWaiting()
    {
        string result = Facade.Instance.StartBattle("user", null);
        
        Assert.That(result, Is.EqualTo("No hay nadie esperando"));
    }

    [Test]
    public void StartBattle_WhenOpponentButIsNotWaiting_ReturnsNotWaiting()
    {
        string result = Facade.Instance.StartBattle("user", "opponent");
        
        Assert.That(result, Is.EqualTo("opponent no está esperando"));
    }

    [Test]
    public void StartBattle_WhenNoOpponentButOneWaiting_ReturnsBattleWithWaiting()
    {
        Facade.Instance.AddTrainerToWaitingList("opponent");
        
        string result = Facade.Instance.StartBattle("user", null);
        
        Assert.That(result, Is.EqualTo("Comienza user vs opponent"));
    }

    [Test]
    public void StartBattle_WhenOpponentWaiting_ReturnsBattleWithOpponent()
    {
        Facade.Instance.AddTrainerToWaitingList("opponent");
        
        string result = Facade.Instance.StartBattle("user", "opponent");
        
        Assert.That(result, Is.EqualTo("Comienza user vs opponent\n"+"user tu empezaras el combate\n"+"opponent te va tocar esperar, empieza tu oponente"));
    }
    
    [Test]
    public void RendirseEnBatalla()
    {
        Facade.Instance.StartBattle("ASH", "RED");
        Facade.Instance.AddPokemosA("Squirtle");
        Facade.Instance.AddPokemosD("Charmander");
        Facade.Instance.InitializeBattle();

        Assert.That(Facade.Instance.Rendirse(), Is.EqualTo($"ASH se ha rendido por voluntad propia\n"+"¡Ha ganado el jugador RED! \n"+"La batalla ha terminado"));
    }

    [Test]
    public void GetHistorial()
    {
        Facade.Instance.StartBattle("ash", "red");
        Facade.Instance.AddPokemosA("Squirtle");
        Facade.Instance.AddPokemosD("Charmander");
        Facade.Instance.InitializeBattle();
        Facade.Instance.UsePokemonMove(1);
        
        Assert.That(Facade.Instance.RecibirLista(),Is.EqualTo($"1.red ha usado Squirtle que ha usado Hidropulso.Y ha hacertado. El pokemon Charmander se ha debilitado, por que no podrá combatir más\n"));
    }
}