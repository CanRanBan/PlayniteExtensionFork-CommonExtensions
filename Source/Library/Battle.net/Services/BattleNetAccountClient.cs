﻿using BattleNetLibrary.Models;
using Playnite.SDK;
using Playnite.SDK.Data;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace BattleNetLibrary.Services
{
    public class BattleNetAccountClient
    {
        private const string apiStatusUrl = @"https://account.battle.net/api/";
        private const string gamesUrl = @"https://account.battle.net/api/games-and-subs";
        private const string classicGamesUrl = @"https://account.battle.net/api/classic-games";
        private const string defaultLogoutUri = @"https://account.battle.net:443/api/logout";
        private static ILogger logger = LogManager.GetLogger();
        private IWebView webView;

        public BattleNetAccountClient(IWebView webView)
        {
            this.webView = webView;
        }

        public List<GamesAndSubs.GameAccount> GetOwnedGames()
        {
            webView.NavigateAndWait(gamesUrl);
            var textGames = webView.GetPageText();
            var games = Serialization.FromJson<GamesAndSubs>(textGames);
            return games.gameAccounts;
        }

        public List<ClassicGames.ClassicGame> GetOwnedClassicGames()
        {
            webView.NavigateAndWait(classicGamesUrl);
            var textGames = webView.GetPageText();
            var games = Serialization.FromJson<ClassicGames>(textGames);
            return games.classicGames;
        }

        public void Login()
        {
            var apiUrls = GetDefaultApiStatus();
            webView.LoadingChanged += (s, e) =>
            {
                var address = webView.GetCurrentAddress();
                if (address == "https://account.battle.net/overview" || address == "https://account.battle.net/")
                {
                    webView.Close();
                }
            };

            webView.Navigate(apiUrls.logoutUri ?? defaultLogoutUri);
            webView.OpenDialog();
        }

        public bool GetIsUserLoggedIn()
        {
            var status = GetApiStatus();
            if (status == null) // This sometimes fails due to some timeout reasons...
            {
                Task.Delay(5000).ConfigureAwait(false).GetAwaiter().GetResult();
                status = GetApiStatus();
            }

            return status?.authenticated == true;
        }

        public static BattleNetApiStatus GetDefaultApiStatus()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadData(apiStatusUrl);
                }
                catch (WebException exception) // Response is always 401
                {
                    string responseText = string.Empty;
                    var responseStream = exception.Response?.GetResponseStream();
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseText = reader.ReadToEnd();
                        }
                    }

                    if (Serialization.TryFromJson<BattleNetApiStatus>(responseText, out var status))
                    {
                        return status;
                    }
                    else
                    {
                        return new BattleNetApiStatus();
                    }
                }

                return null;
            }
        }

        public BattleNetApiStatus GetApiStatus()
        {
            // This refreshes authentication cookie
            webView.NavigateAndWait("https://account.battle.net:443/oauth2/authorization/account-settings");
            webView.NavigateAndWait(apiStatusUrl);
            var textStatus = webView.GetPageText();
            if (Serialization.TryFromJson<BattleNetApiStatus>(textStatus, out var status))
            {
                return status;
            }
            else
            {
                return new BattleNetApiStatus();
            }
        }
    }
}