using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidatorCoreLib;
using System.Reflection;


namespace ValidatorSDK
{
    public class Validator
    {


        class PropertyBinder
        {
            string propertyChain;
            PropertyInfo[] cachedInfo;
            internal PropertyBinder(string propertyChain)
            {
                this.propertyChain = propertyChain;
            }

            IComparable initializeCache(object obj){
                string[] properties = propertyChain.Split('.');

                cachedInfo = new PropertyInfo[properties.Length];
                PropertyInfo propertyInfo = null;
                for (int i = 0; i < cachedInfo.Length; i++)
                {
                    propertyInfo = obj.GetType().GetProperty(properties[i], (BindingFlags.Public | BindingFlags.Instance));
                    cachedInfo[i] = propertyInfo;
                    obj = propertyInfo.GetValue(obj, null);
                }
                return (IComparable)obj;

            }

            internal IComparable Bind(object obj)
            {
                if (propertyChain == null || propertyChain.Equals(""))
                {
                    return (IComparable)obj;
                }

                if (cachedInfo == null)
                {
                    return (IComparable)initializeCache(obj);
                }
                else
                {
                    foreach (PropertyInfo info in cachedInfo)
                    {
                        obj = info.GetValue(obj, null);
                    }
                    return (IComparable)obj;
                }
            }

        }


        class QuickAndDirtyBinder:ObjectBinder
        {
            ValidationData data;
            Dictionary<string, Dictionary<Type, PropertyBinder>> propertyBindingTable;


            public QuickAndDirtyBinder(ValidationData data)
            {
                this.data = data;
                propertyBindingTable = new Dictionary<string, Dictionary<Type, PropertyBinder>>();
            }
 
            public IComparable Bind(string tableKey, string propertyPath)
            {
                return Bind(new PropertySelection(tableKey, propertyPath));
            }

            private PropertyBinder getPropertyBinder(string propertyChain, string contextKey, object obj)
            {
                Dictionary<Type, PropertyBinder> typeBibings =null;
                if (propertyBindingTable.ContainsKey(propertyChain))
                {
                    typeBibings = propertyBindingTable[propertyChain];
                } else 
                {
                    typeBibings =new Dictionary<Type, PropertyBinder>();
                    propertyBindingTable[propertyChain] = typeBibings;
                }

                PropertyBinder binder = null;
                if (typeBibings.ContainsKey(obj.GetType()))
                {
                    binder = typeBibings[obj.GetType()];
                } else 
                {
                    binder = new PropertyBinder(propertyChain);
                    typeBibings[obj.GetType()] = binder;
                }
                return binder;
            }

            public IComparable Bind(PropertySelection selection)
            {

                string runtimeBinding = null;
                string propertyChain = null;

                try
                {
                    runtimeBinding = data.bindingContainer.ContextBinding[selection.ContextKey.ToLower()];
                }
                catch (KeyNotFoundException e)
                {
                    throw new Exception("no runtime binding mapping for: " + selection.ContextKey,e);
                }

                try
                {
                    propertyChain = data.bindingContainer.ReferenceBinding[selection.ReferenceMeaning.ToLower()];
                }
                catch (KeyNotFoundException e)
                {
                    throw new Exception("no property chain binding mapping for: " + selection.ReferenceMeaning,e);
                }
                

                object obj = data.Contexts.Get(runtimeBinding);
                PropertyBinder binder = getPropertyBinder(propertyChain, runtimeBinding, obj);


                return binder.Bind(obj);
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
            FlowErrorTrace errorTrace = data.flow.Validate(request);
            ValidationResult result = new ValidationResult(errorTrace);
            return result;
        }
    }

}
