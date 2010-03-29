using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidatorCoreLib
{
    // the operator will compare two objects with operator as calls definition, 
    // the result is true if ( ( object1 operator object2 ) == true )
    public abstract class IOperator
    {
        public abstract bool Evaluate(IComparable obj1, IComparable obj2);
    }

    public abstract class OperatorHelper
    {
        public static ValidatorCoreLib.IOperator GetOperator(string sName) 
        {
            if (sName != null)
            {
                if (sName.Equals(typeof(GreaterOperator).ToString())) return new GreaterOperator();
                if (sName.Equals(typeof(LowerOperator).ToString())) return new LowerOperator();
                if (sName.Equals(typeof(NotEqualOperator).ToString())) return new NotEqualOperator();
                if (sName.Equals(typeof(GreaterOrEqualOperator).ToString())) return new GreaterOrEqualOperator();
                if (sName.Equals(typeof(LowerOrEqualOperator).ToString())) return new LowerOrEqualOperator();
            }
            // default
            return new EqualOperator() ;
        }
    }

    public abstract class Operator : IOperator
    {
        protected bool CheckTypeMatching(IComparable obj1, IComparable obj2)
        {
            return obj1.GetType().Equals(obj2.GetType());
        }
        public override bool Evaluate(IComparable obj1, IComparable obj2)
        {
            return CheckTypeMatching(obj1, obj2) && OperatorEvaluation(obj1, obj2);
        }
        protected abstract bool OperatorEvaluation(IComparable obj1, IComparable obj2);
    }

    public class EqualOperator : Operator
    {     
        protected override bool OperatorEvaluation(IComparable obj1, IComparable obj2)
        {
            return obj1.Equals(obj2);
        }
    }

    

    public class GreaterOperator : Operator
    {
        protected override bool OperatorEvaluation(IComparable obj1, IComparable obj2)
        {
            return obj1.CompareTo(obj2) > 0;
        }
    }

    public class LowerOperator : Operator
    {
        protected override bool OperatorEvaluation(IComparable obj1, IComparable obj2)
        {
            return obj1.CompareTo(obj2) < 0;
        }
    }

    public class NotEqualOperator : IOperator
    {
        EqualOperator equal = new EqualOperator();
        public override bool Evaluate(IComparable obj1, IComparable obj2)
        {
            return !equal.Evaluate(obj1, obj2);
        }
    }

    public class GreaterOrEqualOperator : IOperator
    {
        GreaterOperator greater = new GreaterOperator();
        EqualOperator equal = new EqualOperator();
        public override bool Evaluate(IComparable obj1, IComparable obj2)
        {
            return (greater.Evaluate(obj1, obj2) || equal.Evaluate(obj1, obj2));
        }
    }

    public class LowerOrEqualOperator : IOperator
    {
        LowerOperator lower = new LowerOperator();
        EqualOperator equal = new EqualOperator();
        public override bool Evaluate(IComparable obj1, IComparable obj2)
        {
            return (lower.Evaluate(obj1, obj2) || equal.Evaluate(obj1, obj2));
        }
    }

}
