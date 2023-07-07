
using Cosmos.Cms.Common.Services.Configurations;
using Cosmos.Cms.Data.Logic;
using Cosmos.Common.Data;
using Cosmos.Common.Models;
using Cosmos.EmailServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Cosmos.Editor.Controllers
{
    /// <summary>
    /// Contact Us Controller
    /// </summary>
    public class Ccms__ContactUsController : Controller
    {
        private readonly ArticleEditLogic _articleLogic;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AzureCommunicationEmailSender _emailSender;
        private readonly ILogger<Ccms__ContactUsController> _logger;
        private readonly IOptions<CosmosConfig> _cosmosOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cosmosOptions"></param>
        /// <param name="logger"></param>
        /// <param name="emailSender"></param>
        /// <param name="articleLogic"></param>
        /// <param name="userManager"></param>
        public Ccms__ContactUsController(
            IOptions<CosmosConfig> cosmosOptions,
            ILogger<Ccms__ContactUsController> logger,
            IEmailSender emailSender,
            ArticleEditLogic articleLogic,
            UserManager<IdentityUser> userManager)
        {
            _cosmosOptions = cosmosOptions;
            _logger = logger;
            _emailSender = (AzureCommunicationEmailSender)emailSender;
            _articleLogic = articleLogic;
            _userManager = userManager;
        }

        /// <summary>
        /// Contact us email form
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(new EmailMessageViewModel()
            {
                FromEmail = user.Email
            });
        }


        /// <summary>
        /// Send Email Message
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EmailMessageViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    await _emailSender.SendEmailAsync(user.Email,
                        model.Subject, $"<h5>{model.FromEmail} sent the following message:</h5><br />{model.Content}" );

                    model.SendSuccess = true;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, e);
                }
            }

            return View(model);
        }

    }
}
