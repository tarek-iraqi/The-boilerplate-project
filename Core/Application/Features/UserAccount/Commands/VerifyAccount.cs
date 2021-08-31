﻿using Application.Interfaces;
using FluentValidation;
using Helpers.Constants;
using Helpers.Exceptions;
using Helpers.Resources;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserAccount.Commands
{
    public class VerifyAccount
    {
        public class Command : IRequest
        {
            public string email { get; set; }
            public string token { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IApplicationLocalization localizer)
            {
                RuleFor(p => p.email)
                    .NotEmpty().EmailAddress()
                    .WithName(p => localizer.Get(ResourceKeys.Email));

                RuleFor(p => p.token)
                   .NotEmpty()
                   .WithName(p => localizer.Get(ResourceKeys.Token));
            }
        }

        private class Handler : IRequestHandler<Command>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _identityService.FindByEmail(request.email);

                if (user == null)
                    throw new AppCustomException(ErrorStatusCodes.InvalidAttribute,
                            new List<Tuple<string, string>> { new Tuple<string, string>(nameof(request.email),
                                    ResourceKeys.UserNotFound) });

                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.token));

                var result = await _identityService.VerifyUserAccount(user, code);

                if (!result.success)
                    throw new AppCustomException(ErrorStatusCodes.InvalidAttribute, result.errors);

                return Unit.Value;
            }
        }
    }
}
