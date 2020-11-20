using FluentValidation;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Common.Validation
{
    public class CreateUpdateServerDtoValidator : AbstractValidator<CreateUpdateServerDto>
    {
        public CreateUpdateServerDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(64);

            RuleFor(x => x.Description)
                .MaximumLength(128);

            RuleFor(x => x.AssignedRange)
                .NotNull()
                .NotEmpty()
                .Must(x => x.IsIPAddressWithCidr())
                .WithMessage("{PropertyName} must be a valid IPv4 or IPv6 address in CIDR notation.");

            RuleFor(x => x.Dns)
                .NotNull()
                .NotEmpty()
                .Must(x => x.IsIPv4Address() || x.IsIPv6Address())
                .WithMessage("{PropertyName} must be a valid IP Address.");

            RuleFor(x => x.Endpoint)
                .NotNull()
                .NotEmpty()
                .Must(x => {
                    if (string.IsNullOrWhiteSpace(x)) return false;

                    var chunks = x.Split(':');
                    return chunks.Length == 2
                        && (chunks[0].IsIPv4Address() || chunks[0].IsValidDomain())
                        && chunks[1].IsAllDigits()
                        && int.TryParse(chunks[1], out var port)
                        && 0 <= port && port <= 65535;
                })
                .WithMessage("{PropertyName} is not a valid address, please specify <IPAddress or domain>:<port>.");

            RuleFor(x => x.PublicKey)
                .Length(44)
                .NotEmpty()
                .NotNull()
                .Must(x => x.IsBase64())
                .WithMessage("{PropertyName} must be a valid base64 string.");
        }
    }
}