using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This class has been made to reduce the use of variables in the form 'Class'
// By creating an object of this class in the amandelbrood class, optimises the use of presets 😎

namespace amandelbrood
{
    class Preset
    {
        private double xMiddle;
        private double yMiddle;
        private double scale;
        private double max;
        private String mandelColor;

        // Call this constructor with the assigned member variables to create an object 'preset' in the amandelbrood class
        public Preset(double xMiddle, double yMiddle, double scale, double max, String mandelColor)
        {
            this.setXMiddle(xMiddle);
            this.setYMiddle(yMiddle);
            this.setScale(scale);
            this.setMax(max);
            this.setMandelColor(mandelColor);
        }

        // Create getters and setters
        // We can use these methods in the amandelbrood class to get the values of the current preset
        public double getXMiddle()
        {
            return this.xMiddle;
        }

        public void setXMiddle(double xMiddle)
        {
            this.xMiddle = xMiddle;
        }

        public double getYMiddle()
        {
            return this.yMiddle;
        }

        public void setYMiddle(double yMiddle)
        {
            this.yMiddle = yMiddle;
        }

        public double getScale()
        {
            return this.scale;
        }

        public void setScale(double scale)
        {
            this.scale = scale;
        }

        public double getMax()
        {
            return this.max;
        }

        public void setMax(double max)
        {
            this.max = max;
        }

        public String getMandelColor()
        {
            return this.mandelColor;
        }

        public void setMandelColor(String mandelColor)
        {
            this.mandelColor = mandelColor;
        }

        
    }
}
