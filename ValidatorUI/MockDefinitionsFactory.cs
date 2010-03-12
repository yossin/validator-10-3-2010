using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidatorCoreLib;

namespace ValidatorSDK
{
    public class MockDefinitionsFactory : IValidationDataFactory
    {
        private HashSet<EntityDefinition> CreateEntityDefinitions()
        {
            HashSet<EntityDefinition> entities = new HashSet<EntityDefinition>();
            EntityDefinition person = new EntityDefinition("Person");
            person.Set("Name", new TypeDefinition(typeof(string)));
            person.Set("Age", new TypeDefinition(typeof(int)));
            entities.Add(person);
            return entities;
        }

        private HashSet<RoleDefinition> CreateRoleDefinitions()
        {
            HashSet<RoleDefinition> roles = new HashSet<RoleDefinition>();
            RoleDefinition role1 = new RoleDefinition("role1");
            role1.AddPropertySelection(new PropertySelection("person1", "Age"));
            role1.Operator = new RangeOperatorDefinition(new TypeDefinition(typeof(int)),
                20, 40);
            roles.Add(role1);
            return roles;
        }

        private HashSet<FlowDefinition> CreateFlowDefinitions(Dictionary<string, RoleDefinition> rolesIndex, Dictionary<string, ContextDefinition> contextsIndex)
        {
            HashSet<FlowDefinition> flows = new HashSet<FlowDefinition>();
            ContextDefinition context1 = contextsIndex["context1"];
            FlowDefinition flow1 = new FlowDefinition("flow1", context1);
            RoleDefinition role1 = rolesIndex["role1"];
            flow1.AddRoleDefinition(role1);
            flows.Add(flow1);
            return flows;
        }

        private HashSet<ContextDefinition> CreateContextDefinitions(Dictionary<string, EntityDefinition> entitiesIndex)
        {
            HashSet<ContextDefinition> contexts = new HashSet<ContextDefinition>();
            ContextDefinition context1 = new ContextDefinition("context1");
            EntityDefinition definition = entitiesIndex["Person"];
            context1.Put("person1", definition);
            contexts.Add(context1);
            return contexts;
        }
        public ValidationData Create()
        {
            HashSet<EntityDefinition> entities = CreateEntityDefinitions();
            Dictionary<string, EntityDefinition> entitiesIndex = new Dictionary<string, EntityDefinition>();
            foreach (EntityDefinition entity in entities)
            {
                entitiesIndex.Add(entity.Name, entity);
            }
            HashSet<RoleDefinition> roles = CreateRoleDefinitions();
            Dictionary<string, RoleDefinition> rolesIndex = new Dictionary<string, RoleDefinition>();
            foreach (RoleDefinition role in roles)
            {
                rolesIndex.Add(role.Name, role);
            }
            HashSet<ContextDefinition> contexts = CreateContextDefinitions(entitiesIndex);
            Dictionary<string, ContextDefinition> contextsIndex = new Dictionary<string, ContextDefinition>();
            foreach (ContextDefinition context in contexts)
            {
                contextsIndex.Add(context.Name, context);
            }
            HashSet<FlowDefinition> flows = CreateFlowDefinitions(rolesIndex, contextsIndex);

            ValidationData definitions = new ValidationData(entities, roles, contexts, flows);
            return definitions;
        }
    }
}
