using FluentValidation;
using Whyvra.Tunnel.Common.Validation;

namespace Whyvra.Tunnel.Common.Models.Validation
{
    public class UpdateClientDtoValidator : AbstractValidator<UpdateClientDto>
    {
        public UpdateClientDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(64);

            RuleFor(x => x.Description)
                .MaximumLength(128);

            RuleFor(x => x.AssignedIp)
                .NotNull()
                .NotEmpty()
                .Must(x => x.IsIPAddressWithCidr())
                .WithMessage("{PropertyName} must be a valid IPv4 or IPv6 address in CIDR notation.")
                .Must(x => x.HasNoBitsRightOfNetmask())
                .WithMessage("{PropertyName} has bits set to the right of the netmask.");

            RuleFor(x => x.PublicKey)
                .Length(44)
                .NotEmpty()
                .NotNull()
                .Must(x => x.IsBase64())
                .WithMessage("{PropertyName} must be a valid base64 string.");
        }
    }
}