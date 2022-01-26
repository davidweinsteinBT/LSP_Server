using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System.Threading;
using System.Threading.Tasks;

namespace LSP_Server.Handlers
{
    public class HoverHandler : IHoverHandler
    {
        public HoverRegistrationOptions GetRegistrationOptions(HoverCapability capability, ClientCapabilities clientCapabilities)
        {
            return new HoverRegistrationOptions()
            {
                DocumentSelector = new DocumentSelector(
                    new DocumentFilter()
                    {
                        Pattern = "**/*.txt"
                    },
                    new DocumentFilter()
                    {
                        Language = "plaintext"
                    }
                )
            };
        }

        public Task<Hover> Handle(HoverParams request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Hover()
            {
                Contents = new MarkedStringsOrMarkupContent("This is hover text")
            });
        }
    }
}
