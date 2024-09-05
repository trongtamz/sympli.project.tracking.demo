﻿using FluentValidation;
using Sympli.Project.Tracking.Applications.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Sympli.Project.Tracking.Applications.Validators
{
    [ExcludeFromCodeCoverage]
    public class RequestValidator<TRequest> : IRequestValidator<TRequest>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidator(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public IEnumerable<string> ValidateRequest(TRequest request)
        {
            var context = new ValidationContext<TRequest>(request);

            return _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(vf => vf != null)
                .Select(vf => vf.ErrorMessage)
                .ToList();
        }
    }
}