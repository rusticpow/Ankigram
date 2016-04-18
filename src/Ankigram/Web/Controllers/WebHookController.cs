using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Telegram.Bot.Types;


namespace Web.Controllers
{
    public class WebHookController : ApiController
    {
        public async Task<IHttpActionResult> Post(Update update)
        {
            var message = update.Message;

            Debug.WriteLine("Received Message from {0}", message.Chat.Id);

            if (message.Type == MessageType.TextMessage)
            {
                // Echo each Message
                await Bot.Api.SendTextMessage(message.Chat.Id, message.Text);
            }
            else if (message.Type == MessageType.PhotoMessage)
            {
                // Download Photo
                var file = await Bot.Api.GetFile(message.Photo.LastOrDefault().FileId);

                var filename = file.FileId + "." + file.FilePath.Split('.').Last();

                using (var saveImageStream = System.IO.File.Open(filename, FileMode.Create))
                {
                    await file.FileStream.CopyToAsync(saveImageStream);
                }

                await Bot.Api.SendTextMessage(message.Chat.Id, "Thx for the Pics");
            }

          return Ok();
        }
    }
}