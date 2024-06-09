using UnityEngine;
using UnityEngine.SocialPlatforms;
using System;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.GameCenter;

namespace Services.GameCenter
{
    public class GameCenterManager : MonoBehaviour
    {
       [SerializeField] private string _leaderboardID;
      
        public void AuthenticateToGameCenter()
        {
            Social.localUser.Authenticate(success => {
                if (success)
                {
                    Debug.Log ("Authentication successful");
                    String userInfo = "Username: " + Social.localUser.userName;
                    Debug.Log (userInfo);
                }
                else
                {
                    Debug.LogError("Authentication failed");
                }
            });
        }

        public void ReportScore(int score)
        {
            if (Social.localUser.authenticated)
            {
                Social.ReportScore(score, _leaderboardID, success => {
                    if (success)
                    {
                        Debug.Log("Score reported successfully");
                    }
                    else
                    {
                        Debug.LogError("Score reporting failed");
                    }
                });
            }
            else
            {
                Debug.LogError("User not authenticated");
            }
        }
    }
}