using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ValidatorSDK;

namespace Validator
{
    public partial class Form1 : Form
    {
        Person p1 = new Person("name1",22);     // property definition + value !!
        MockValidatorFactory factory = new MockValidatorFactory();
        ValidatorContext context;
        public Form1()
        {
            InitializeComponent();
            context = new ValidatorContext();
            context.Add("person1", p1);
            textBox1.Text = "" + p1.Age;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ValidatorSDK.Validator validator = factory.CreateValidator(context);
            p1.Age = Convert.ToInt32(textBox1.Text);
            ValidationResult result = validator.Validate("flow1");
            if (result.ContainErrors())
            {
                label3.Text = "validation errors!";
            }
            else
            {
                label3.Text = "no validation errors";
            }
        }
    }

    public class Person
    {
        string name;
        int age;
        public Person(string name, int age)
        {
            this.name = name;
            this.age = age;
        }
        public int Age
        {
            get { return age; }
            set { age = value; }
        }
        public string Name
        {
            get { return name; }
        }
    }
}
