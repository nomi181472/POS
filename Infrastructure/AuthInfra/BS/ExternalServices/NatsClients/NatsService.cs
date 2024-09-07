using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using NATS.Client.JetStream;

namespace BS.ExternalServices.NatsClients
{
    public class NatsService //: IAsyncDisposable
    {
        //    private readonly IConnection _connection;
        //    private readonly IJetStream _jetStream;

        //    public NatsService(string url)
        //    {
        //        var options = ConnectionFactory.GetDefaultOptions();
        //        options.Url = url;
        //        _connection = new ConnectionFactory().CreateConnection(options);
        //        _jetStream = _connection.CreateJetStreamContext();
        //    }

        //    public IJetStream JetStream => _jetStream;

        //    public async ValueTask DisposeAsync()
        //    {
        //        _connection.Dispose();
        //        await ValueTask.CompletedTask;
        //    }
    }
}