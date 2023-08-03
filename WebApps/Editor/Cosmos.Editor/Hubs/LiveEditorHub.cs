using Cosmos.Cms.Data.Logic;
using Cosmos.Cms.Models;
using Cosmos.Common.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmos.Cms.Hubs
{
    /// <summary>
    /// Live editor collaboration hub
    /// </summary>
    /// [Authorize(Roles = "Reviewers, Administrators, Editors, Authors")]
    public class LiveEditorHub : Hub
    {
        private readonly ArticleEditLogic _articleLogic;
        private readonly ILogger<LiveEditorHub> _logger;

        private string GetArticleGroupName(int articleNumber)
        {
            return $"Article:{articleNumber}";
        }
        private string GetArticleGroupName(string articleNumber)
        {
            return $"Article:{articleNumber}";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="articleLogic"></param>
        /// <param name="logger"></param>
        public LiveEditorHub(ArticleEditLogic articleLogic, ILogger<LiveEditorHub> logger)
        {
            _articleLogic = articleLogic;
            _logger = logger;
        }

        /// <summary>
        /// Adds an editor to the page group.
        /// </summary>
        /// <param name="articleNumber"></param>
        /// <returns></returns>
        public async Task JoinArticleGroup(string articleNumber)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetArticleGroupName(articleNumber));
        }

        /// <summary>
        /// Joins the editing room
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Notification(string data)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<LiveEditorSignal>(data);

                switch (model.Command)
                {
                    case "join":
                        await Groups.AddToGroupAsync(Context.ConnectionId, GetArticleGroupName(model.ArticleNumber));
                        break;
                    case "save":
                    case "SavePageProperties":
                        // Alert others
                        await Clients.OthersInGroup(GetArticleGroupName(model.ArticleNumber)).SendCoreAsync("broadcastMessage", new[] { JsonConvert.SerializeObject(model) });
                        break;
                    default:
                        await Clients.OthersInGroup(GetArticleGroupName(model.ArticleNumber)).SendCoreAsync("broadcastMessage", new[] { data });
                        break;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}", e);
            }

        }

        /// <summary>
        /// Sends a signal to update editors in the group
        /// </summary>
        /// <param name="editorId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateEditors(string editorId, string data)
        {
            await Clients.OthersInGroup(editorId).SendCoreAsync("updateEditors", new[] { data });
        }

    }
}
