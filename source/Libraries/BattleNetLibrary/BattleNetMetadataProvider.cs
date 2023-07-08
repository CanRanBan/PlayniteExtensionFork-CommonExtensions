﻿using Playnite.SDK;
using Playnite.SDK.Models;
using System.Collections.Generic;

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
                Name = product.Name,
                Links = product.Links == null ? new List<Link>() : product.Links
            };

            gameInfo.Links.Add(new Link("PCGamingWiki", @"http://pcgamingwiki.com/w/index.php?search=" + product.Name));
            if (!string.IsNullOrEmpty(product.IconUrl))
            {
                gameInfo.Icon = new MetadataFile(product.IconUrl);
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
