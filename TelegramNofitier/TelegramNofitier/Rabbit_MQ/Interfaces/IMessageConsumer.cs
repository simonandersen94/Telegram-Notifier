using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramNofitier.RabbitMQ.Interfaces {
    public interface IMessageConsumer {
        void StartConsuming();
    }
}
