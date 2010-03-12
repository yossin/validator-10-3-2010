using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidatorSDK
{
    public class MockValidatorFactory
    {

        ValidatorDefinitionsHelper definitions;
        public MockValidatorFactory()
        {
            IValidationDataFactory definitionFactory = new MockDefinitionsFactory();
            ValidationData definitions = definitionFactory.Create();
            this.definitions= new DefinitionsHelper(definitions);
        }

        public Validator CreateValidator(ValidatorContext context)
        {

            return new Validator(context, definitions);
        }
    }

    public class ValidatorError : Exception
    {
        public ValidatorError(string message)
            : base(message)
        {

        }
    }
}
