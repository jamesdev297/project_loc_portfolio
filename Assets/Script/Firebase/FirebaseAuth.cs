using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class FirebaseLoginResult
{
    public bool Success { get; set; }
}

public class FirebaseAuth
{
    private static FirebaseAuth instance;
    public static FirebaseAuth Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FirebaseAuth();
            }
            return instance;
        }
    }

    public Firebase.Auth.FirebaseUser User { get; set; }
    public string AuthToken { get; set; }

    Firebase.Auth.FirebaseAuth auth;
    DateTime lastRefreshTime;

    public ConcurrentQueue<FirebaseLoginResult> LoginResultQueue { get; set; }
        = new ConcurrentQueue<FirebaseLoginResult>();

    private FirebaseAuth()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public void LoginAnynim()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(async task => {
            if(task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                LoginResultQueue.Enqueue(new FirebaseLoginResult() { Success = false });
                return;
            }
            if(task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                LoginResultQueue.Enqueue(new FirebaseLoginResult() { Success = false });
                return;
            }

            await OnLogin(task.Result.User);
        });
    }

    public void LoginGoogle(string googleToken)
    {
        Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(googleToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(async task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Google authTask.IsCanceled");
                LoginResultQueue.Enqueue(new FirebaseLoginResult() { Success = false });
                return;
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Google authTask.IsFaulted");
                LoginResultQueue.Enqueue(new FirebaseLoginResult() { Success = false });
                return;
            }

            await OnLogin(task.Result);
        });
    }

    public async Task OnLogin(FirebaseUser user)
    {
        User = user;

        Debug.Log($"User signed in successfully: {User.DisplayName} ({User.UserId})");

        
        
        AuthToken = await User.TokenAsync(true);
        lastRefreshTime = DateTime.Now;
        Debug.Log($"AuthToken:{AuthToken}");

        var autoRefreshTokenTask = Task.Run(AutoRefreshToken);

        LoginResultQueue.Enqueue(new FirebaseLoginResult() { Success = true });
    }

    public async Task AutoRefreshToken()
    {
        while(true)
        {
            var nextRefreshTime = lastRefreshTime + TimeSpan.FromMinutes(50); // expire time 1h
            if (nextRefreshTime <= DateTime.Now)
            {
                await RefreshToken();
            }
            await Task.Delay(10 * 1000); // 10 sec
        }
    }

    public async Task RefreshToken()
    {
        try
        {
            AuthToken = await User.TokenAsync(true);
            lastRefreshTime = DateTime.Now;

            Debug.Log($"Refresh AuthToken:{AuthToken}, LastRefreshTime:{lastRefreshTime}");
        }
        catch (Exception e)
        {
            Debug.Log($"Error. {e.Message}");
        }
    }
}
