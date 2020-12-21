using System.Linq;
using FluentValidation;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Common.Validation;

namespace Whyvra.Tunnel.Presentation.ViewModels
{
    public class ServerViewModelValidator : AbstractValidator<ServerViewModel>
    {
        public ServerViewModelValidator(IValidator<CreateUpdateServerDto> childValidator)
        {
            RuleFor(x => x.Server).SetValidator(childValidator);
            RuleFor(x => x.DefaultAllowedRange)
                .Must(x => x.All(x => x.IsIPAddressWithCidr()))
                .WithMessage("All addresses must be valid IPv4 or IPv6 address in CIDR notation.")
                .Must(x => x.All(x => x.HasNoBitsRightOfNetmask()))
                .WithMessage("One or more addresses has bits set to the right of the netmask.");
        }
    }
}