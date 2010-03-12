using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ValidatorSDK;
using ValidatorCoreLib;


namespace HostSysTest
{
    public partial class HostSystemTest : Form
    {
        static int nNumOfProps = 3;
        ValidationData validationData = new ValidationData();
            
        public HostSystemTest()
        {
            InitializeComponent();
            SuspendLayout();

            TB_Buttons = new System.Windows.Forms.TextBox[4][];

            TB_Buttons[0] = TB_Names    = new System.Windows.Forms.TextBox[nNumOfProps];
            TB_Buttons[1] = TB_IDs      = new System.Windows.Forms.TextBox[nNumOfProps];
            TB_Buttons[2] = TB_Types    = new System.Windows.Forms.TextBox[nNumOfProps];
            TB_Buttons[3] = TB_Values   = new System.Windows.Forms.TextBox[nNumOfProps];

            for (int ii = 0; ii < nNumOfProps; ii++)
            {
                TB_Names[ii] = ii == 0 ? TB_Name1 : ii == 1 ? TB_Name2 : TB_Name3;
                TB_IDs[ii] = ii == 0 ? TB_ID1 : ii == 1 ? TB_ID2 : TB_ID3;
                TB_Types[ii] = ii == 0 ? TB_Type1 : ii == 1 ? TB_Type2 : TB_Type3;
                TB_Values[ii] = ii == 0 ? TB_Value1 : ii == 1 ? TB_Value2 : TB_Value3;
             }
        }

        protected bool GetPropertiesData()
        {
            for (int ii = 0; ii < nNumOfProps; ii++)
            {
                Object oValue;
                if (TB_Types[ii].Text.ToLower().Equals("system.string"))
                    oValue = TB_Values[ii].Text;
                else if (TB_Types[ii].Text.ToLower().Equals("system.int32"))
                    oValue = System.Convert.ToInt32(TB_Values[ii].Text, 10);
                else
                {
                    string sMsg = "Wrong type(prop=" + TB_Names[ii].Text + ",type=" + TB_Types[ii].Text + ")";
                    MessageBox.Show(sMsg);
                    return false;
                }
                validationData.Add(TB_IDs[ii].Text, oValue);
            }
            return true;
        }

        private IValidatorFactory CreateValidationFactory()
        {
            string flow11 = TB_Rule1_PropID.Text; 
            int obj11 = System.Convert.ToInt32(TB_Rule1_IntMinValue.Text);
            string flow12 = TB_Rule1_PropID.Text; 
            int obj12 = System.Convert.ToInt32(TB_Rule1_IntMaxValue.Text);
            string flow21 = TB_Rule2_PropID.Text; 
            string obj21 = TB_Rule2_StringValue.Text;

            return new MockValidatorFactory(flow11,obj11,flow12,obj12,flow21,obj21);
        }


        private ValidationResult Validate(IValidatorFactory factory, ValidationData validationData, string flowName)
        {
            Validator validator = factory.CreateValidator(flowName);
            ValidationResult result = validator.Validate(validationData);
            return result;
        }

        private void ValidateInt_Click(object sender, EventArgs e)
        {
            validationData.Clear();
            GetPropertiesData();

            IValidatorFactory factory = CreateValidationFactory();
            ValidationResult result = Validate(factory, validationData,"flow1");

            L_Rule1_Conflict.Text = result.Message;
        }

        private void ValidateString_Click(object sender, EventArgs e)
        {
            validationData.Clear();
            GetPropertiesData();


            IValidatorFactory factory = CreateValidationFactory();
            ValidationResult result = Validate(factory, validationData, "flow2");

           L_Rule2_Conflict.Text = result.Message;
        }
    }


    
    

}

   



