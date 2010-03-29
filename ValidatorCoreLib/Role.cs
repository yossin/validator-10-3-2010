﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
    
namespace ValidatorCoreLib
{
    // input:
    //      nRuleID
    //      contextContain  - the path to the compared object in the contextTable[key]
    //      key             - uniqe key to get the object from contextTable
    //      comparedObject  - object to compare with
    //      op              - operator to compare with
    // output: 
    //      by using the validate(contextTable) the rule detarmain if the object data has conflict     
    //
    // the rule is a set of definitions to check object data
    [Serializable(), XmlRoot("ValidationRule", Namespace = "ValidatorCoreLib", IsNullable = false)]
    public class ValidationRule
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public IOperator Operator { get; set; }
        // Used only for xml Serialization
        //public string OperatorName { get { return Operator.GetType().ToString(); } set; }
        public string OperatorName { get; set; }

        public string contextContain { get; set; }
        public string key { get; set; }
        public Object ComparedObject { get; set; }
        public int id { get; set; }

        // Used only for xml Serialization
        public ValidationRule() 
        {
            this.Operator = OperatorHelper.GetOperator(this.OperatorName);
        }

        public ValidationRule(int nRuleID, string contextContain, string key, Object comparedObject, IOperator op)
        {
            this.id = nRuleID;
            this.Operator = op;
            this.OperatorName = op.GetType().ToString();
            this.contextContain = contextContain;
            this.key = key;            
            this.ComparedObject = comparedObject;
        }

        public bool Validate(ContextTable contextTable)
        {
            Object obj_context = contextTable.Get(key);
            Object obj_to_check = ContextTable.ExtractObject(contextContain, obj_context);
            IComparable comparableToCheck = (IComparable)obj_to_check;
            IComparable comparableComaredObject = (IComparable)ComparedObject;
            return Operator.Evaluate(comparableToCheck, comparableComaredObject);
        }
    }

}
