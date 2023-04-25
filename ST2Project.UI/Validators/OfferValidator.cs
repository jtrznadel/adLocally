using FluentValidation;
using ST2Project.UI.Models;

namespace ST2Project.UI.Validators
{
    public class OfferValidator : AbstractValidator<OfferModel>
    {
        public OfferValidator()
        {
            RuleFor(x => x.Title).Length(1, 100);
            RuleFor(x => x.Description).Length(1, 250);
            RuleFor(x => x.Price).InclusiveBetween(0, 999999);
        }
    }
}
