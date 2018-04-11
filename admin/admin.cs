using GTANetworkAPI;

public class AdminScript : Script
{
    [ServerEvent(Event.PlayerConnected)]
    public void OnPlayerConnected(Client player)
    {
        var log = NAPI.ACL.LoginPlayer(player, "");
        switch (log)
        {
            case LoginResult.LoginSuccessful:
            case LoginResult.LoginSuccessfulNoPassword:
                NAPI.Chat.SendChatMessageToPlayer(player, "Logado como ~b~" + NAPI.ACL.GetPlayerAclGroup(player) + "~w~.");
                break;

            case LoginResult.WrongPassword:
                NAPI.Chat.SendChatMessageToPlayer(player, "Por favor faça o login com ~b~/login [senha]");
                break;
        }
    }

    [ServerEvent(Event.PlayerDisconnected)]
    public void OnPlayerDisconnected(Client player, DisconnectionType type, string reason)
    {
        NAPI.ACL.LogoutPlayer(player);
    }

    [Command(SensitiveInfo = true, ACLRequired = true)]
    public void Login(Client sender, string password)
    {
        var logResult = NAPI.ACL.LoginPlayer(sender, password);
        switch (logResult)
        {
            case LoginResult.NoAccountFound:
                NAPI.Chat.SendChatMessageToPlayer(sender, "~r~ERROR:~w~ Nenhuma conta encontrada com seu nome.");
                break;

            case LoginResult.LoginSuccessfulNoPassword:
            case LoginResult.LoginSuccessful:
                NAPI.Chat.SendChatMessageToPlayer(sender, "~g~Login bem sucedido!~w~ Logado como ~b~" + NAPI.ACL.GetPlayerAclGroup(sender) + "~w~.");
                break;
            case LoginResult.WrongPassword:
                NAPI.Chat.SendChatMessageToPlayer(sender, "~r~ERROR:~w~ Senha incorreta!");
                break;
            case LoginResult.AlreadyLoggedIn:
                NAPI.Chat.SendChatMessageToPlayer(sender, "~r~ERROR:~w~ Você já está logado!");
                break;
            case LoginResult.ACLDisabled:
                NAPI.Chat.SendChatMessageToPlayer(sender, "~r~ERROR:~w~ A ACL foi desativada neste servidor.");
                break;
        }
    }

    [Command(ACLRequired = true)]
    public void SetTime(Client sender, int hours, int minutes)
    {
        NAPI.World.SetTime(hours, minutes, 0);
    }

    [Command(ACLRequired = true)]
    public void SetWeather(Client sender, int newWeather)
    {
        NAPI.World.SetWeather((Weather)newWeather);
    }

    [Command(ACLRequired = true)]
    public void Logout(Client sender)
    {
        NAPI.ACL.LogoutPlayer(sender);
    }

    [Command(ACLRequired = true)]
    public void Start(Client sender, string resource)
    {
        if (!NAPI.Resource.DoesResourceExist(resource))
        {
            NAPI.Chat.SendChatMessageToPlayer(sender, "~r~Nenhum resource desse tipo encontrado: \"" + resource + "\"");
        }
        else if (NAPI.Resource.IsResourceRunning(resource))
        {
            NAPI.Chat.SendChatMessageToPlayer(sender, "~r~Resource \"" + resource + "\" já está a rodando!");
        }
        else
        {
            NAPI.Resource.StartResource(resource);
            NAPI.Chat.SendChatMessageToPlayer(sender, "~g~Iniciado resource \"" + resource + "\"");
        }
    }

    [Command(ACLRequired = true)]
    public void Stop(Client sender, string resource)
    {
        if (!NAPI.Resource.DoesResourceExist(resource))
        {
            NAPI.Chat.SendChatMessageToPlayer(sender, "~r~Nenhum resource desse tipo encontrado: \"" + resource + "\"");
        }
        else if (!NAPI.Resource.IsResourceRunning(resource))
        {
            NAPI.Chat.SendChatMessageToPlayer(sender, "~r~Resource \"" + resource + "\" já está a rodando!");
        }
        else
        {
            NAPI.Resource.StopResource(resource);
            NAPI.Chat.SendChatMessageToPlayer(sender, "~g~Parado resource \"" + resource + "\"");
        }
    }

    [Command(ACLRequired = true)]
    public void Restart(Client sender, string resource)
    {
        if (NAPI.Resource.DoesResourceExist(resource))
        {
            NAPI.Resource.StopResource(resource);
            NAPI.Resource.StartResource(resource);
            NAPI.Chat.SendChatMessageToPlayer(sender, "~g~Reiniciado resource \"" + resource + "\"");
        }
        else
        {
            NAPI.Chat.SendChatMessageToPlayer(sender, "~r~Nenhum resource desse tipo encontrado: \"" + resource + "\"");
        }
    }

    [Command(GreedyArg = true, ACLRequired = true)]
    public void Kick(Client sender, Client target, string reason)
    {
        NAPI.Player.KickPlayer(target, reason);
    }

    [Command(ACLRequired = true)]
    public void Kill(Client sender, Client target)
    {
        NAPI.Player.SetPlayerHealth(target, -1);
    }
}
