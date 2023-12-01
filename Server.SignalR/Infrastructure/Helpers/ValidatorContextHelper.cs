using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Server.SignalR.Infrastructure.Helpers
{
    public static class ValidatorContextHelper
    {
        public static bool TryValidate(object obj, out Collection<ValidationResult> result, ValidationContext validationContext = null)
        {
            var context = validationContext ?? new ValidationContext(obj, serviceProvider:  null, items: null);
            result = new Collection<ValidationResult>();
            return Validator.TryValidateObject(obj, context, result, validateAllProperties: true);
        }
    }
}
