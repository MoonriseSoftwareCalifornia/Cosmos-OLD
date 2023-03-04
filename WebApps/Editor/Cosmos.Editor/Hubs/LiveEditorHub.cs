using Cosmos.BlobService;
using Cosmos.Cms.Common.Data;
using Cosmos.Cms.Data.Logic;
using Cosmos.Cms.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Cms.Hubs
{
    /// <summary>
    /// Live editor collaboration hub
    /// </summary>
    public class LiveEditorHub : Hub
    {
        /// <summary>
        /// Joins the editing room
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Notification(string data)
        {
            var model = JsonConvert.DeserializeObject<LiveEditorSignal>(data);

            if (model.Command == "join")
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, model.EditorId);
            }
            else
            {
                await Clients.OthersInGroup(model.EditorId).SendCoreAsync("broadcastMessage", new[] { data });
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
