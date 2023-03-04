﻿using Cosmos.Cms.Common.Data;
using Cosmos.Cms.Common.Models;
using Cosmos.Cms.Common.Services.Configurations;
using Cosmos.Cms.Data.Logic;
using Cosmos.Cms.Models;
using Cosmos.Cms.Services;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Cosmos.Cms.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    public abstract class BaseController : Controller
    {
        private readonly ArticleEditLogic _articleEditLogic;
        private readonly UserManager<IdentityUser> _baseUserManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IOptions<CosmosConfig> _options;

        /// <summary>
        /// Gets the user ID of the currently logged in user
        /// </summary>
        /// <returns></returns>
        protected async Task<string> GetUserId()
        {
            // Get the user's ID for logging.
            var user = await _baseUserManager.GetUserAsync(User);
            return user.Id;
        }

        /// <summary>
        /// Gets the user Email address of the currently logged in user
        /// </summary>
        /// <returns></returns>
        protected async Task<string> GetUserEmail()
        {
            // Get the user's ID for logging.
            var user = await _baseUserManager.GetUserAsync(User);
            return user.Email;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="userManager"></param>
        /// <param name="articleLogic"></param>
        /// <param name="options"></param>
        internal BaseController(ApplicationDbContext dbContext,
            UserManager<IdentityUser> userManager,
            ArticleEditLogic articleLogic,
            IOptions<CosmosConfig> options)
        {
            _dbContext = dbContext;
            _articleEditLogic = articleLogic;
            _baseUserManager = userManager;
            _options = options;
        }

        /// <summary>
        ///     Server-side validation of HTML.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="inputHtml"></param>
        /// <returns>HTML content</returns>
        /// <remarks>
        ///     <para>
        ///         The purpose of this method is to validate HTML prior to be saved to the database.
        ///         It uses an instance of <see cref="HtmlAgilityPack.HtmlDocument" /> to check HTML formatting.
        ///     </para>
        /// </remarks>
        internal string BaseValidateHtml(string fieldName, string inputHtml)
        {
            if (!string.IsNullOrEmpty(inputHtml))
            {
                var contentHtmlDocument = new HtmlDocument();
                contentHtmlDocument.LoadHtml(HttpUtility.HtmlDecode(inputHtml));
                //if (contentHtmlDocument.ParseErrors.Any())
                //    foreach (var error in contentHtmlDocument.ParseErrors)
                //        modelState.AddModelError(fieldName, error.Reason);

                return contentHtmlDocument.ParsedText.Trim();
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get Layout List Items
        /// </summary>
        /// <returns></returns>
        internal async Task<List<SelectListItem>> BaseGetLayoutListItems()
        {
            var layouts = await _dbContext.Layouts.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.LayoutName
            }).ToListAsync();
            if (layouts != null) return layouts;

            var layoutViewModel = new LayoutViewModel();

            _dbContext.Layouts.Add(layoutViewModel.GetLayout());
            await _dbContext.SaveChangesAsync();

            return await _dbContext.Layouts.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.LayoutName
            }).ToListAsync();
        }

        /// <summary>
        /// Minifies HTML
        /// </summary>
        /// <param name="input"></param>
        /// <remarks>If the content can't be minified because of errors, it will echo back the input.</remarks>
        /// <returns></returns>
        internal string MinifyHtml(string input)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)) return input;

            var result = NUglify.Uglify.Html(input);

            if (result.HasErrors) return input;

            return result.Code;
        }

        /// <summary>
        ///     Generates a random string of 32 numbers and charachers.
        /// </summary>
        /// <returns></returns>
        internal string RandomSalt()
        {
            var random = new RNGCryptoRandomGenerator();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, 32)
                .Select(s => s[random.Next(0, s.Length)]).ToArray());
        }

        ///// <summary>
        ///// Updates date time stamps of all published articles
        ///// </summary>
        ///// <returns></returns>
        //public virtual async Task<JsonResult> UpdateTimeStamps()
        //{
        //    var result = await _articleEditLogic.UpdateDateTimeStamps();
        //    return Json(result);
        //}

        /// <summary>
        /// Strips Byte Order Marks
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal string StripBOM(string data)
        {
            // See: https://danielwertheim.se/utf-8-bom-adventures-in-c/

            if (string.IsNullOrEmpty(data) || string.IsNullOrWhiteSpace(data)) return data;

            // Get rid of Zero Length strings
            var rows = data.Split("\r\n");
            var builder = new StringBuilder();
            foreach (var row in rows)
            {
                if (row.Trim().Equals("") == false)
                {
                    builder.AppendLine(row);
                }
            }

            data = builder.ToString();

            // Search for and eliminate BOM
            var filtered = new string(data.ToArray().Where(c => c != '\uFEFF' && c != '\u00a0').ToArray());

            using var memStream = new MemoryStream();
            using var writer = new StreamWriter(memStream, new UTF8Encoding(false));
            writer.Write(filtered);
            writer.Flush();

            var clean = Encoding.UTF8.GetString(memStream.ToArray());

            return clean;
        }

        /// <summary>
        /// Removes BOM by searching for them and deleting.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string RemoveBom(string data)
        {
            string BOMMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (data.StartsWith(BOMMarkUtf8, StringComparison.OrdinalIgnoreCase))
                data = data.Remove(0, BOMMarkUtf8.Length);
            return data.Replace("\0", "");
        }

        #region SIGNAL METHODS

        #region SIGNAL LOGIC IS SPLIT UP TO FACILITY UNIT TESTING

        /// <summary>
        /// Used to send an encrypted signal to another Editor.
        /// </summary>
        /// <param name="editorUrl">Root Url of the editor</param>
        /// <param name="data">unencrypted signal arguments</param>
        /// <returns></returns>
        /// <remarks>
        /// This method appends a "seed" then encrypts data before sending signal. The pipe ("|") delineates arguments.
        /// </remarks>
        internal async Task<T> SendSignal<T>(string editorUrl, string data)
        {
            // Prepare the message to be sent
            var message = Signal_PrepareMessage(data);

            #region SEND SIGNAL AND RECIEVE RESULTS VIA HTTPCLIENT

            // Send message here
            // Create client
            var endpoint = $"{editorUrl.TrimEnd('/')}/Editor/Signal";

            using var httpClient = new HttpClient();
            var parameters = new Dictionary<string, string>
            {
                ["data"] = message
            };
            var response = await httpClient.PostAsync(endpoint, new FormUrlEncodedContent(parameters));

            var encrypted = await response.Content.ReadAsStringAsync();

            #endregion

            // Post process the result
            var result = Signal_PostProcess<T>(encrypted);

            return result;
        }


        /// <summary>
        /// Returns model state errors as serialization
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        internal string SerializeErrors(ModelStateDictionary modelState)
        {
            var errors = modelState.Values
                .Where(w => w.ValidationState == ModelValidationState.Invalid).Select(s => s.Errors)
                .ToList();

            return Newtonsoft.Json.JsonConvert.SerializeObject(errors);
        }

        /// <summary>
        /// Seeds and encyrpts message prior to sending signal
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Encrypted message ready to send via signal</returns>
        /// <remarks>Returns an encrypted, seeded message ready to send via HttpClient.</remarks>
        public string Signal_PrepareMessage(string data)
        {
            data = data.TrimEnd('|');
            var seededData = $"{data}|{Guid.NewGuid().ToString() + Guid.NewGuid().ToString()}";
            return EncryptString(seededData);
        }

        /// <summary>
        /// Process the results comming back
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <returns></returns>
        public T Signal_PostProcess<T>(string encryptedData)
        {
            var decrypted = DecryptString(encryptedData);

            var signalResult = JsonConvert.DeserializeObject<SignalResult>(decrypted);

            if (signalResult.HasErrors)
            {
                if (signalResult.Exceptions.Count > 1)
                {
                    throw new Exception(signalResult.Exceptions[0].Message, signalResult.Exceptions[1]);
                }
                throw signalResult.Exceptions[0];
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(signalResult.JsonValue);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        /// <summary>
        /// Encrypts a string for use with communications
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string EncryptString(string plainText)
        {
            var key = _options.Value.SecretKey;
            var iv = new byte[16];
            byte[] array;

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using var memoryStream = new MemoryStream();
                using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                using (var streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                array = memoryStream.ToArray();
            }
            return Convert.ToBase64String(array);
        }

        /// <summary>
        /// Decrypts a string for communications
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public string DecryptString(string cipherText)
        {
            var key = _options.Value.SecretKey;
            var iv = new byte[16];
            var buffer = Convert.FromBase64String(cipherText);

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        #endregion
    }
}