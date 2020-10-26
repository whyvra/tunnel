using FluentValidation;
using Whyvra.Tunnel.Common.Validation;

namespace Whyvra.Tunnel.Common.Commands
{
    public class CreateServerCommandValidator : AbstractValidator<CreateServerCommand>
    {
        public CreateServerCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(64);

            RuleFor(x => x.Description)
                .MaximumLength(128);

            RuleFor(x => x.AssignedRange)
                .Must(x => x.IsIPAddressWithCidr())
                .WithMessage("{PropertyName} must be a valid IPv4 or IPv6 address in CIDR notation.");

            RuleFor(x => x.Dns)
                .Must(x => x.IsIPv4Address() || x.IsIPv6Address())
                .WithMessage("{PropertyName} must be a valid IP Address.");

            RuleFor(x => x.Endpoint)
                .Must(x => {
                    var chunks = x.Split(':');
                    return chunks.Length == 2
                        && chunks[0].IsIPv4Address()
                        && chunks[1].IsAllDigits()
                        && int.TryParse(chunks[1], out var port)
                        && 0 <= port && port <= 65535;
                })
                .WithMessage("{PropertyName} is not a valid address, please specify <IPAddress>:<port>.");

            RuleFor(x => x.PublicKey)
                .Length(44)
                .NotEmpty()
                .NotNull()
                .Must(x => x.IsBase64())
                .WithMessage("{PropertyName} must be a valid base64 string.");
        }
    }
}