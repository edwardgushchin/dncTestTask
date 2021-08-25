using System;
using System.Collections.Generic;
using System.Linq;
using iMessengerCoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dncTestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetDialogIdsController : ControllerBase
    {
        private readonly ILogger<GetDialogIdsController> _logger;

        public GetDialogIdsController(ILogger<GetDialogIdsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public string GetDialogId([FromBody]Guid[] clientIds)
        {
            //Получаем список клиентов
            var clients = new RGDialogsClients().Init();

            //Получаем список диалогов из списка клиентов
            var dialogs = clients.Select(l => l.IDRGDialog).Distinct().ToList();
            
            foreach (var dialog in dialogs)
            {
                //Получаем список клиентов в диалоге
                var cls = clients.Where(client => client.IDRGDialog == dialog).ToList();

                var insertion = true;
                
                // Проверяем нахождение клиентов из входных данных в диалоге
                foreach (var c in cls.Where(c => !clientIds.Contains(c.IDClient)))
                {
                    insertion = false;
                }

                // Если все клиенты из входных данных входят в диалог, возвращаем его
                if (insertion) return dialog.ToString();
            }

            //Иначе возвращаем пустую строку
            return string.Empty;
        }
    }
}