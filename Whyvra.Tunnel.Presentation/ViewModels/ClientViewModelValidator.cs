using System.Linq;
using FluentValidation;
using Whyvra.Tunnel.Common.Validation;

namespace Whyvra.Tunnel.Presentation.ViewModels
{
    public class ClientViewModelValidator<TModel, TBase> : AbstractValidator<TModel> where TModel : IClientViewModel<TBase>
    {
        public ClientViewModelValidator(IValidator<TBase> childValidator)
        {
            RuleFor(x => x.Client).SetValidator(childValidator);

            RuleFor(x => x.AllowedIps)
                .Must(x => x.Any())
                .WithMessage("At least one allowed IP address is required.");

            RuleFor(x => x.AllowedIps)
                .Must(x => x.All(x => x.IsIPAddressWithCidr()))
                .WithMessage("All addresses must be valid IPv4 or IPv6 address in CIDR notation.")
                .Must(x => x.All(x => x.HasNoBitsRightOfNetmask()))
                .WithMessage("One or more addresses has bits set to the right of the netmask.");
        }
    }
}