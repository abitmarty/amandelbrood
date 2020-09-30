using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amandelbrood
{
    class Preset
    {
        private double xMiddle;
        private double yMiddle;
        private double scale;
        private double max;
        private String mandelColor;

        public Preset(double xMiddle, double yMiddle, double scale, double max, String mandelColor)
        {
            this.setXMiddle(xMiddle);
            this.setYMiddle(yMiddle);
            this.setScale(scale);
            this.setMax(max);
            this.setMandelColor(mandelColor);
        }

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

        public void setMax(double yMiddle)
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
