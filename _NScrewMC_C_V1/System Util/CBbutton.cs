using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _NScrewMC_C_V1
{
    public class CBbutton
    {
        public static Color NormalColor = Color.FromArgb(153, 180, 209); //normal - blue
        public static Color SelectColor = Color.Orange;
        public static Color StatusOK = Color.FromArgb(0, 192, 0);//origin OK - green
        public static Color ErrorColor = Color.Red;

        public bool m_iSel = false;
        public SUserControls.ColorButton Button;
        public CBbutton(SUserControls.ColorButton _bt, bool _isel)
        {
            Button = _bt;
            m_iSel = _isel;
        }
        public void SetValue(Color color) {
            try
            {
                Button.GradientBottom = color;
                Button.GradientTop = color;
                if (color == SelectColor) m_iSel = true;
                else m_iSel = false;
            }
            catch (Exception) 
            {
            
            }
            
        }
        public void SetValeUnSelect()
        {
            Button.GradientBottom = Color.Gainsboro;
            Button.GradientTop = Color.WhiteSmoke;
            m_iSel = false;
        }
        public Color GetColor
        {
            get {
                return Button.GradientBottom;
            }
        }
    }
    public class CBbutton2
    {
        public static Color NormalColor = new Color();
        public static Color SelectColor = new Color();
        public static Color StatusOK = new Color();
        public static Color ErrorColor = new Color();

        public bool m_iSel = false;
        //public AxBTNENHLib4.AxBtnEnh Button;
        public MyButton.ButtonPress Button;
        public CBbutton2() 
        {
            m_iSel = false;
        }
        public CBbutton2(MyButton.ButtonPress _bt)
        {
            NormalColor = _bt.GradientTop;
            SelectColor = _bt.ColorPress;
            Button = _bt;
        }
        public void SetValue()
        {
            Button.GradientTop = SelectColor;
            m_iSel = true;
        }
        public void Release()
        {
            Button.GradientTop = NormalColor;
            m_iSel = false;
        }
        public Color GetColor
        {
            get
            {
                return Button.GradientTop;
            }
        }
    }
}
