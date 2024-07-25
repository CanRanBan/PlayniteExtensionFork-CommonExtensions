﻿using System.Collections.Generic;
using System.IO;
using Playnite.SDK;
using Playnite.SDK.Models;

namespace BattleNetLibrary
{
    public class BattleNetMetadataProvider : LibraryMetadataProvider
    {
        public override GameMetadata GetMetadata(Game game)
        {
            var product = BattleNetGames.GetAppDefinition(game.GameId);
            if (product == null)
            {
                return null;
            }

            var gameInfo = new GameMetadata
            {
                Name = product.Name, Links = product.Links == null ? new List<Link>() : product.Links
            };

            gameInfo.Links.Add(new Link("PCGamingWiki", @"http://pcgamingwiki.com/w/index.php?search=" + product.Name));
            if (!string.IsNullOrEmpty(product.IconUrl))
            {
                gameInfo.Icon = new MetadataFile(product.IconUrl);
            }
            else
            {
                if (product.Type == Models.BNetAppType.Classic && game.IsInstalled)
                {
                    var exe = Path.Combine(game.InstallDirectory, product.ClassicExecutable);
                    if (File.Exists(exe))
                    {
                        gameInfo.Icon = new MetadataFile(exe);
                    }
                }
            }

            if (!string.IsNullOrEmpty(product.CoverUrl))
            {
                gameInfo.CoverImage = new MetadataFile(product.CoverUrl);
            }

            if (!string.IsNullOrEmpty(product.BackgroundUrl))
            {
                gameInfo.BackgroundImage = new MetadataFile(product.BackgroundUrl);
            }

            return gameInfo;
        }
    }
}
