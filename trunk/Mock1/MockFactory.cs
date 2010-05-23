using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidatorCoreLib;
using ValidatorSDK;


namespace Mock1Test
{

    

    public class MockFactory 
    {
        private ContextTable CreateContextTable()
        {
            MyPerson person1 = new MyPerson();
            person1.Age=30;
            person1.Name="Person";
            MyPerson person2 = new MyPerson();
            person2.Age = 30;
            person2.Name = "Person";
            MyPerson person3 = new MyPerson();
            person3.Age = 40;
            person3.Name = "Person3";
            ContextTable table = new ContextTable();
            table.Add("Person1", person1);
            table.Add("Person2", person2);
            table.Add("Person3", person3);
            return table;
        }

        private ValidationFlow CreateValidationFlow()
        {
            ValidationFlow flow1 = new ValidationFlow("main_flow", true);
            
            ValidationFlow flow11 = new ValidationFlow("person_validations", true);
            flow1.Add(flow11);

            ValidationRule role111 = new ValidationRule("rule-111", new PropertySelection("person1","Person's Name"), "Person", new EqualOperator(), true, "resolve string for ui");
            flow11.Add(role111);

            ValidationRule role112 = new ValidationRule("rule-112", new PropertySelection("person1", "Person"), new PropertySelection("person2", "Person"), new EqualOperator(), true, "resolve string for ui");
            flow11.Add(role112);

            ValidationFlow flow12 = new ValidationFlow("person_age_validations", true);
            flow1.Add(flow12);

            ValidationRule role121 = new ValidationRule("rule-121", new PropertySelection("person3", "Person's Age"), 30, new EqualOperator(), true, "age must be 30");
            flow12.Add(role121);
            
            return flow1;
        }

        private BindingContainer CreateBindingContainer()
        {

            //definitionKey, string runtimeKey
            Dictionary<string, string> contextBinding = new Dictionary<string, string>();

            contextBinding["person1"] = "Person1";
            contextBinding["person2"] = "Person2";
            contextBinding["person3"] = "Person3";


            //referenceKey, string propertyChain
            Dictionary<string, string> referenceBinding = new Dictionary<string, string>();
            referenceBinding["Person's Name"] = "Name";
            referenceBinding["Person's Age"] = "Age";
            referenceBinding["Person"] = "";

            BindingContainer container = new BindingContainer(contextBinding, referenceBinding);
            return container;
        }

        public ValidationData CreateValidationData()
        {

            
            ContextTable table = CreateContextTable();
            ValidationFlow flow = CreateValidationFlow();
            BindingContainer binding=CreateBindingContainer();

            ValidationData data = new ValidationData(table, flow, binding);

            return data;

        }
       
    }

        

        
        

        
    
}
