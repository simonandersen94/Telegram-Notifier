using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramNofitier.Telegram.Interfaces {
    public interface ITelegramSender {
        Task SendMessage(string message);
    }
}
