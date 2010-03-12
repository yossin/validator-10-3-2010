using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ValidatorCoreLib;

namespace ValidatorSDK
{
    public class Validate
    {
        public bool bValidationResult{get;set;}
        public Validate(ValidationData validationData)
        {
            bValidationResult = true;
            foreach (Rule rule in validationData.ruleList.GetRuleList())
            {
                if (rule.nRuleType > (int)RuleType.RuleType_MonoRule__First__ && rule.nRuleType < (int)RuleType.RuleType_MonoRule__Last__)
                {
                    Context context = validationData.contexts.Get(rule.getEntity(0).PropertyID);
                    bValidationResult &= rule.validate(context);
                }            
            }

        }
    }
}
