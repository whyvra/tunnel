using System.Linq;
using FluentValidation;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Common.Validation;

namespace Whyvra.Tunnel.Presentation.ViewModels
{
    public class UpdateClientViewModelValidator : AbstractValidator<UpdateClientViewModel>
    {
        public UpdateClientViewModelValidator(IValidator<UpdateClientDto> childValidator)
        {
            RuleFor(x => x.Client).SetValidator(childValidator);

            RuleFor(x => x.AllowedIps)
                .Must(x => x.Any())
                .WithMessage("At least one allowed IP address is required.");

            RuleFor(x => x.AllowedIps)
                .Must(x => x.All(x => x.IsIPAddressWithCidr()))
                .WithMessage("All addresses must be valid IPv4 or IPv6 address in CIDR notation.");
        }
    }
}