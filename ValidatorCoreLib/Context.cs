using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidatorCoreLib
{
    // holds the HostSystem object that will be validate, with a uniqe key(as string)
    public class ContextTable
    {
        Dictionary<string, Object> objectByKey = new Dictionary<string, Object>();      // key + value

        public void Add(string key, Object obj)
        {
            objectByKey[key] = obj;
        }

        public Object Get(string key)
        {
            if (objectByKey.ContainsKey(key))
                return objectByKey[key];
            return null;
        }

        public void Clear()
        {
            objectByKey.Clear();
        }

        // ***///***/// ***///***/// ***///***/// 
        //more TBD...
        // ***///***/// ***///***/// ***///***/// 
        // static, get a compared object from HostSystem object
        public static Object ExtractObject(string contextContain, Object obj)
        {
            String cc = contextContain;
            Object resObj = obj;
            while ( cc.Contains('\\') )
            {
                int Index = cc.IndexOf("\\");
                string FirstObject = cc.Substring(0,Index-1);
                cc = cc.Substring(Index+1);
                
                if ( ! cc.Contains('\\') ) 
                    return obj;

                Index = cc.IndexOf("\\");
                string ContainedObject = cc.Substring(0,Index-1);

                Type t_FirstObject = System.Type.ReflectionOnlyGetType(FirstObject, true, true);
                Type t_ContainedObject = System.Type.ReflectionOnlyGetType(ContainedObject, true, true);

                if (!obj.GetType().IsInstanceOfType(t_FirstObject))
                    return obj;

//                object value = t_FirstObject.GetType().GetProperty(t_ContainedObject ???).GetValue(t_FirstObject, null);
            }
            return obj; 
        }
    }

    

   
}
