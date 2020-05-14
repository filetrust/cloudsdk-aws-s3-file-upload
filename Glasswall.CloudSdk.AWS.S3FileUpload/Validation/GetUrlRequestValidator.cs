using Amazon.Lambda.APIGatewayEvents;
using FluentValidation;
using System.IO;

namespace AwsDotnetCsharp
{
    public class GetUrlRequestValidator : AbstractValidator<APIGatewayProxyRequest>
    {
        public GetUrlRequestValidator()
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(filename => filename.QueryStringParameters["fileName"]).NotNull().WithMessage("You must provide a file name")
                .NotEmpty().WithMessage("You must provide a file name")
                .DependentRules(() => {
                    RuleFor(filename => filename.QueryStringParameters["fileName"]).Must(HasExtension)
                    .WithMessage("You must provide a filename with extension. e.g. 'cat.jpg'")
                    .Must(DoesNotContainAForwardSlash).WithMessage("You must not include any slashes in your filename. e.g. 'cat.jpg'");
                     });
            RuleFor(filename => filename.QueryStringParameters["region"]).NotNull().WithMessage("You must provide a region")
               .NotEmpty().WithMessage("You must provide a region");

            RuleFor(filename => filename.QueryStringParameters["bucketName"]).NotNull().WithMessage("You must provide a bucket name")
              .NotEmpty().WithMessage("You must provide a bucket name");

            RuleFor(filename => filename.QueryStringParameters["objectPath"]).NotNull().WithMessage("You must provide an object key")
             .NotEmpty().WithMessage("You must provide an object key");
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


