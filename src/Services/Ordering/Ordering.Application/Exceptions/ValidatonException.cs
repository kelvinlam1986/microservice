using FluentValidation.Results;

namespace Ordering.Application.Exceptions
{
    public class ValidatonException : ApplicationException
    {
        public ValidatonException() : base("One or more validation failures have occurred")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidatonException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                .GroupBy(x => x.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public IDictionary<string, string[]> Errors { get; set; }
    }
}
