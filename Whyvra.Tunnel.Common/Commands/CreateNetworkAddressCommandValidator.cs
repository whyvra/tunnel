using FluentValidation;
using Whyvra.Tunnel.Common.Validation;

namespace Whyvra.Tunnel.Common.Commands
{
    public class CreateNetworkAddressCommandValidator : AbstractValidator<CreateNetworkAddressCommand>
    {
        public CreateNetworkAddressCommandValidator()
        {
            RuleFor(x => x.Address)
                .NotEmpty()
                .NotNull()
                .Must(x => x.IsIPAddressWithCidr())
                .WithMessage("{PropertyName} must be a valid IPv4 or IPv6 address in CIDR notation.")
                .Must(x => x.HasNoBitsRightOfNetmask())
                .WithMessage("{PropertyName} has bits set to the right of the netmask.");
        }
    }
}