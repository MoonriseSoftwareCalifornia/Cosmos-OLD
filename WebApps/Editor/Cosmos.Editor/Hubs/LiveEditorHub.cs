using Amazon.Runtime.Internal.Util;
using Cosmos.Cms.Controllers;
using Cosmos.Cms.Data.Logic;
using Cosmos.Cms.Models;
using Cosmos.Common.Data;
using Cosmos.Common.Data.Logic;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Drawing;
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
        /// Joins the editing room
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Notification(string data)
        {
            var model = JsonConvert.DeserializeObject<LiveEditorSignal>(data);

            switch (model.Command)
            {
                case "join":
                    await Groups.AddToGroupAsync(Context.ConnectionId, model.EditorId);
                    break;
                case "save":
                    await SaveEditorContent(model);
                    model.Command = "saved"; // Let caller know item is saved.
                    await Clients.Caller.SendCoreAsync("broadcastMessage", new[] { JsonConvert.SerializeObject(model) });
                    await Clients.OthersInGroup(model.EditorId).SendCoreAsync("broadcastMessage", new[] { data });
                    break;
                default:
                    await Clients.OthersInGroup(model.EditorId).SendCoreAsync("broadcastMessage", new[] { data });
                    break;
            }

        }

        private async Task SaveEditorContent(LiveEditorSignal model)
        {
            if (model == null)
            {
                _logger.LogError("SIGNALR: SaveEditorContent method, model was null.");
                return;
            }

            // Next pull the original. This is a view model, not tracked by DbContext.
            var article = await _articleLogic.Get(model.ArticleId, EnumControllerName.Edit, model.UserId);
            if (article == null)
            {
                _logger.LogError($"SIGNALR: SaveEditorContent method, could not find artile with ID: {model.ArticleId}.");
                return;
            }

            // Get the editable regions from the original document.
            var originalHtmlDoc = new HtmlDocument();
            originalHtmlDoc.LoadHtml(article.Content);
            var originalEditableDivs = originalHtmlDoc.DocumentNode.SelectNodes("//*[@data-ccms-ceid]");

            // Find the region we are updating
            var target = originalEditableDivs.FirstOrDefault(w => w.Attributes["data-ccms-ceid"].Value == model.EditorId);
            if (target != null)
            {
                // Update the region now
                target.InnerHtml = model.Data;
            }

            // Now carry over what's being updated to the original.
            article.Content = originalHtmlDoc.DocumentNode.OuterHtml;

            // Make sure we are setting to the orignal updated date/time
            // This is validated to make sure that someone else hasn't already edited this
            // entity
            article.Updated = DateTimeOffset.UtcNow;

            // Save changes back to the database
            var result = await _articleLogic.Save(article, model.UserId);


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
