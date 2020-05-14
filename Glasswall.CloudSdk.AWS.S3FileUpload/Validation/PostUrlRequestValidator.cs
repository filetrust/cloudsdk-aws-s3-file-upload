﻿using Amazon.Lambda.APIGatewayEvents;
using FluentValidation;
using System.IO;

namespace AwsDotnetCsharp
{
    public class PostUrlRequestValidator : AbstractValidator<APIGatewayProxyRequest>
    {
        public PostUrlRequestValidator()
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(filename => filename.PathParameters["Filename"]).NotNull().WithMessage("You must provide a file name")
                .NotEmpty().WithMessage("You must provide a file name")
                .DependentRules(() => {
                    RuleFor(filename => filename.PathParameters["Filename"]).Must(HasExtension)
                    .WithMessage("You must provide a filename with extension. e.g. 'cat.jpg'")
                    .Must(DoesNotContainAForwardSlash).WithMessage("You must not include any slashes in your filename. e.g. 'cat.jpg'");
                });
        }

        private bool DoesNotContainAForwardSlash(string filename)
        {
            if (filename.Contains("/"))
            {
                return false;
            }
            return true;
        }

        private bool HasExtension(string filename)
        {
            if (string.IsNullOrEmpty(Path.GetExtension(filename)))
            {
                return false;
            }
            return true;                       
        }
    }
}

