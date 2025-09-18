using System;
using MongoDB.Bson;

namespace Premix.Services;

public class SessionService
{
    public static ObjectId? LoggedUserId { get; private set; }
    public static string Username { get; private set; }

    public static void SetUser(ObjectId userId, string username)
    {
        LoggedUserId = userId;
        Username = username;
    }

    public static void Clear()
    {
        LoggedUserId = null;
        Username = null;
    }
    
}