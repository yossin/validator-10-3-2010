using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidatorCoreLib
{

    // Type Definition
    public class Validate
    {
        Type type;
        public TypeDefinition(Type type)
        {
            this.type = type;
        }
        public int CompareTo(TypeDefinition other)
        {
            return type.Name.CompareTo(other.type.Name);
        }
        public int CompareTo(Type other)
        {
            return type.Name.CompareTo(other.Name);
        }
    }

    public interface OperatorDefinition
    {
        bool Evaluate(IComparable comparable);
    }

    public abstract class BaseOperation : OperatorDefinition
    {
        TypeDefinition typeDefinition;
        public BaseOperation(TypeDefinition typeDefinition)
        {
            this.typeDefinition = typeDefinition;
        }
        public TypeDefinition TypeDefinition
        {
            get { return typeDefinition; }
        }
        internal abstract bool Eval(IComparable comparable);
        public bool Evaluate(IComparable comparable)
        {
            if (typeDefinition.CompareTo(comparable.GetType()) != 0)
            {
                throw new Exception("wrong validation types");
            }
            return Eval(comparable);
        }

    }

    public class EqualsOperatorDefinition : BaseOperation
    {
        IComparable value;
        public EqualsOperatorDefinition(TypeDefinition typeDefinition, IComparable value)
            : base(typeDefinition)
        {
            this.value = value;
        }
        public IComparable Value
        {
            get { return value; }
        }
        override internal bool Eval(IComparable comparable)
        {
            return (comparable.CompareTo(value)==0);
        }
    }

    public class RangeOperatorDefinition : BaseOperation
    {
        IComparable lower;
        IComparable upper;
        public RangeOperatorDefinition(TypeDefinition typeDefinition, IComparable lower, IComparable upper)
            : base(typeDefinition)
        {
            this.lower = lower;
            this.upper = upper;
        }
        public IComparable Lower
        {
            get { return lower; }
        }
        public IComparable Upper
        {
            get { return upper; }
        }
        override internal bool Eval(IComparable comparable)
        {
            if (comparable.CompareTo(lower) >= 0)
            {
                if (comparable.CompareTo(upper) <= 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
