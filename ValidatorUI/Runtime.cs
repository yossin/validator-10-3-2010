using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ValidatorCoreLib;

namespace ValidatorSDK
{
    public class DefinitionsHelper
    {
        ValidationData validationData;
        Dictionary<string, FlowDefinition> flowIndex;
        Dictionary<string, EntityDefinition> entityIndex;
        Dictionary<string, ContextDefinition> contextIndex;

        internal DefinitionsHelper(ValidationData validationData)
        {
            this.validationData = validationData;
            this.flowIndex = new Dictionary<string, FlowDefinition>();
            foreach (FlowDefinition flow in validationData.Flows)
            {
                flowIndex[flow.Name] = flow;
            }

            this.entityIndex = new Dictionary<string, EntityDefinition>();
            foreach (EntityDefinition entity in validationData.Entities)
            {
                entityIndex[entity.Name] = entity;
            }

            this.contextIndex = new Dictionary<string, ContextDefinition>();
            foreach (ContextDefinition context in validationData.Contexts)
            {
                contextIndex[context.Name] = context;
            }
        }
        public ValidationData Definitions
        {
            get { return validationData; }
        }
        public Dictionary<string, FlowDefinition> FlowByName
        {
            get { return flowIndex; }
        }
        public Dictionary<string, EntityDefinition> EntityByName
        {
            get { return entityIndex; }
        }
        public Dictionary<string, ContextDefinition> ContextByName
        {
            get { return contextIndex; }
        }

        public IComparable ExtractValue(ValidatorContext vcontext, ContextDefinition context, PropertySelection selection)
        {
            object instance = vcontext.Get(selection.InstanceName);
            object value = instance.GetType().GetProperty(selection.PropertyName).GetValue(instance, null);

            EntityDefinition entity = context.Get(selection.InstanceName);
            TypeDefinition typeDefinition = entity.Get(selection.PropertyName);
            if (typeDefinition.CompareTo(value.GetType()) !=0)
            {
                throw new ValidatorError("unble to validate - wrong type ");
            }
            
            return (IComparable)value;
        }
    }
    

    

    public class Validator
    {
        ValidatorContext context;
        ValidatorDefinitionsHelper definitionsHelper;
        internal Validator(ValidatorContext context, ValidatorDefinitionsHelper definitionsHelper)
        {
            this.context = context;
            this.definitionsHelper = definitionsHelper;
        }

        public ValidationResult Validate(string flowName)
        {
            FlowDefinition flow = definitionsHelper.FlowByName[flowName];
            ValidationResult result = new ValidationResult();
            foreach (RoleDefinition role in flow.Roles)
            {
                OperatorDefinition op = role.Operator;
                foreach (PropertySelection selection in role.PropertySelections)
                {
                    IComparable value = definitionsHelper.ExtractValue(context, flow.Context, selection);
                    if (op.Evaluate(value) == false){
                        result.Add("error in " + selection.PropertyName + " at istance" + selection.InstanceName);                    
                    }
                }
            }
            return result;
        }
            
    }

    public class ValidationResult
    {
        LinkedList<string> errors;
        public ValidationResult()
        {
            errors = new LinkedList<string>();
        }
        public void Add(string error)
        {
            errors.AddLast(error);
        }
        public bool ContainErrors()
        {
            return (errors.Count > 0);
        }
    }
}
