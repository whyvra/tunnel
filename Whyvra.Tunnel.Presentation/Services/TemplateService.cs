using Bunit;
using Whyvra.Tunnel.Presentation.Templates;
using Whyvra.Tunnel.Presentation.ViewModels;

namespace Whyvra.Tunnel.Presentation.Services
{
    public class TemplateService
    {
        private readonly TestContext _context;

        public TemplateService(TestContext context)
        {
            _context = context;
        }

        public string RenderClientConfiguration(ClientConfigurationViewModel viewModel)
        {
            var parameter = ComponentParameterFactory.Parameter("Model", viewModel);
            var component = _context.RenderComponent<ClientConfiguration>(parameter);

            return component.Markup;
        }
    }
}