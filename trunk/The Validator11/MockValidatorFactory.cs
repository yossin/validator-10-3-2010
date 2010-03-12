using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidatorSDK;
using ValidatorCoreLib;

namespace The_Validator11
{
    public class MockValidatorFactory : IValidatorFactory
    {
        private ValidationFlow flow1;
        private ValidationFlow flow2;
        private static int ii = 1;
        
        private static ValidationFlow CreateFlow1(string minContext, int minValue, string maxContext, int maxValue)
        {
            ValidationRule role1MinBoundary = new ValidationRule(1, "System.Int32", minContext, minValue, new GreaterOrEqualOperator());
            ValidationRule role2MaxBoundary = new ValidationRule(2, "System.Int32", maxContext, maxValue, new LowerOrEqualOperator());
            ValidationFlow flow = new ValidationFlow((ii++).ToString(), true);
            flow.Add(role1MinBoundary);
            flow.Add(role2MaxBoundary);
            return flow;
        }
        private static ValidationFlow CreateFlow2(string strContext, string strValue)
        {
            ValidationRule rrVal = new ValidationRule(1, "System.String", strContext, strValue, new EqualOperator());
            ValidationFlow flow = new ValidationFlow((ii++).ToString(), true);
            flow.Add(rrVal);
            return flow;
        }
        public MockValidatorFactory(string minContext, int minValue, string maxContext, int maxValue, string strContext, string strValue)
        {
            flow1 = CreateFlow1(minContext, minValue, maxContext, maxValue);
            flow2 = CreateFlow2(strContext, strValue);
        }

        public Validator CreateValidator(string flowName)
        {
            if (flowName.Equals("flow1"))
            {
                return new Validator(flow1);
            }
            else if (flowName.Equals("flow2"))
            {
                return new Validator(flow2);
            }
            else
            {
                throw new Exception("undefined flow");
            }
        }
    }
}
