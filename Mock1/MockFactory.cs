using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidatorCoreLib;
using ValidatorSDK;
using ValidatorCoreLib;

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
            table.Add("person1", person1);
            table.Add("person2", person2);
            table.Add("person3", person3);
            return table;
        }

        private ValidationFlow CreateValidationFlow()
        {
            ValidationFlow flow1 = new ValidationFlow("main_flow", true);
            
            ValidationFlow flow11 = new ValidationFlow("person_validations", true);
            flow1.Add(flow11);

            ValidationRule role111 = new ValidationRule(1, "PersonName", "person1", "Person", new EqualOperator(), true, "resolve string for ui");
            flow11.Add(role111);

            ValidationRule role112 = new ValidationRule(2, "person", "person1", new ObjectSelection("person2","person"), new EqualOperator(), true, "resolve string for ui");
            flow11.Add(role112);

            ValidationFlow flow12 = new ValidationFlow("person_age_validations", true);
            flow1.Add(flow12);

            ValidationRule role121 = new ValidationRule(1, "person.age", "person3", 30, new EqualOperator(), true, "age must be 30");
            flow12.Add(role121);
            
            return flow1;
        }

        private ValidationConvertionItem CreateValidationConvertionItem()
        {
            ValidationConvertionItem item = new ValidationConvertionItem("person", "Mock1Test.MyPerson");
            item.Add(new ValidationConvertionItem("name","Name"));
            item.Add(new ValidationConvertionItem("age", "Age"));
            

            return item;
        }

        public ValidationData CreateValidationData()
        {

            
            ContextTable table = CreateContextTable();
            ValidationFlow flow = CreateValidationFlow();
            ValidationConvertionItem item=CreateValidationConvertionItem();

            ValidationData data = new ValidationData(table, flow, item);

            return data;

        }
       
    }

        

        
        

        
    
}
