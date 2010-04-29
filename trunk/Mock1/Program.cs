using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidatorSDK;
using ValidatorCoreLib;

namespace Mock1Test
{
    class Program
    {
        static void Main(string[] args)
        {

            MockFactory factory = new MockFactory();
            ValidationData data = factory.CreateValidationData();
            ValidationResult result = Validator.Validate(data);
            Console.Write(result);
            
        }

        
    }

    public class MyPerson : IComparable<MyPerson>, IComparable
    {
        public MyPerson()
        {
            name = null;
            age = 0;
            generateKey();
        }
        private string name = null;
        private int age;
        private string key;

        public string Name 
        {
            get { return name; }
            set { name = value; generateKey(); }
        }
        public int Age 
        { 
            get {return age;}
            set {age = value; generateKey(); } 
        }

        private void generateKey()
        {
            key =(Age) + "__" + (Name == null ? "" : Name);
        }
        public int CompareTo(MyPerson other)
        {     
            return key.CompareTo(other.key);
        }
        

        public override int GetHashCode()
        {
            return key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.CompareTo(obj)==0;
        }


        public int CompareTo(object obj)
        {
            try
            {
                return CompareTo((MyPerson)obj);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }

    
}
