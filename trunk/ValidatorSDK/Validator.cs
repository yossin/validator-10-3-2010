using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidatorCoreLib;
using ValidatorCoreLib;
using System.Reflection;


namespace ValidatorSDK
{
    public class Validator
    {

        public class Entity2ClassMapping
        {
            public Entity2ClassMapping(string entityName, string className)
            {
                this.EntityName = entityName;
                this.ClassName = className;
                propertyMapping = new Dictionary<string, string>();
            }
            public string EntityName { get; set; }
            public string ClassName { get; set; }
            Dictionary<string, string> propertyMapping;
            public Dictionary<string, string> PropertyMapping 
            {
                get { return propertyMapping; }
            }
            public void AddProperty(string entityProperty, string classProperty)
            {
                propertyMapping.Add(entityProperty, classProperty);
            }
        }

        class QuickAndDirtyBinder:ObjectBinder
        {
            ValidationData data;
            Dictionary<string, Entity2ClassMapping> classMapping;

            private static void IndexValidationConvertionItems(Dictionary<string, Entity2ClassMapping> classMapping, ValidationConvertionItem item)
            {
                string entityName = item.convertionItemName;
                string className = item.convertTo;
                Entity2ClassMapping mapping = new Entity2ClassMapping(entityName, className);
                foreach (ValidationConvertionItem property in item.convertionItems)
                {
                    mapping.AddProperty(property.convertionItemName, property.convertTo);
                }
                classMapping.Add(entityName, mapping);

            }


            public QuickAndDirtyBinder(ValidationData data)
            {
                this.data = data;
                classMapping = new Dictionary<string, Entity2ClassMapping>();
                IndexValidationConvertionItems(classMapping, data.convertionItem);
                

            }
 
            public IComparable Bind(string tableKey, string propertyPath)
            {
                return Bind(new ObjectSelection(tableKey, propertyPath));
            }

            public IComparable Bind(ObjectSelection selection)
            {
                Entity2ClassMapping mapping =  classMapping[selection.EntityName];
                string className = mapping.ClassName;
                string classProperty =null;
                if (selection.PropertyName != null)
                {
                    classProperty = mapping.PropertyMapping[selection.PropertyName];
                }
                object obj = data.Contexts.Get(selection.ContextKey);
                if (obj.GetType().ToString().Equals(className))
                {
                    if (classProperty == null)
                        return (IComparable)obj;
                    else
                        return (IComparable)ExtractObject(classProperty, obj);
                }
                else
                {
                    throw new Exception("unable to bind object - object type does not match mapping specification");
                }

            }

            private static Object ExtractObject(string property, Object obj)
            {
                PropertyInfo propertyInfo = obj.GetType().GetProperty(property, (BindingFlags.Public | BindingFlags.Instance));
                return propertyInfo.GetValue(obj, null);
            }
            
            private static Object ExtractObject1(string contextContain, Object obj)
            {

                String cc = contextContain;
                Object resObj = obj;
                while (cc.Contains('\\'))
                {
                    int Index = cc.IndexOf("\\");
                    string FirstObject = cc.Substring(0, Index - 1);
                    cc = cc.Substring(Index + 1);

                    if (!cc.Contains('\\'))
                        return obj;

                    Index = cc.IndexOf("\\");
                    string ContainedObject = cc.Substring(0, Index - 1);

                    Type t_FirstObject = System.Type.ReflectionOnlyGetType(FirstObject, true, true);
                    Type t_ContainedObject = System.Type.ReflectionOnlyGetType(ContainedObject, true, true);

                    if (!obj.GetType().IsInstanceOfType(t_FirstObject))
                        return obj;

                    //                object value = t_FirstObject.GetType().GetProperty(t_ContainedObject ???).GetValue(t_FirstObject, null);
                }
                return obj;
            }

            
        }

        private static ObjectBinder CreateObjectBinder(ValidationData data)
        {
            return new QuickAndDirtyBinder(data);
        }

        public static ValidationResult Validate(ValidationData data)
        {
            ObjectBinder binder= CreateObjectBinder(data);
            ValidationRequest request = new ValidationRequest(binder);
            ValidationResult result = new ValidationResult();
            data.flow.Validate(request, result);
            return result;
        }
    }

}
