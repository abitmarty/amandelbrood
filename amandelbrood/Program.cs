// Mandelbrood figure
// Co authors. Maarten Gerritse and Martijn Totté
// Studentnumbers: 8845874 and 1235002
// C# imperatief programmeren

using System.Windows.Forms;
using System.Drawing;
using System;

class MandelForm : Form
{
    // Set global form variables
    private PictureBox pictureBox1;
    private Label labelYValue;
    private TextBox textBoxXValue;
    private TextBox textBoxYValue;
    private TextBox textBoxScale;
    private TextBox textBoxMax;
    private Label labelScale;
    private Label labelMax;
    private Button buttonCalculate;
    private Label labelXValue;
    private ComboBox comboBoxColors;

    // Set defaults
    private static double defaultxMiddle = 0;
    private static double defaultyMiddle = 0;
    private static double defaultScale = 100;
    private static double defaultMax = 1000;
    private static String defaultSetMandelColor = "Basic";

    // Set global necessities
    private double xMiddle = defaultxMiddle;
    private double yMiddle = defaultyMiddle;
    private double scale = defaultScale;
    private double max = defaultMax;
    private Bitmap mandelBrotImage;
    private String setMandelColor = defaultSetMandelColor;

    //Set colors
    Color black = Color.FromArgb(0, 0, 0);
    Color white = Color.FromArgb(255, 255, 255);
    Color yellow = Color.FromArgb(255, 185, 0);
    Color orange = Color.FromArgb(247, 130, 0);
    Color red = Color.FromArgb(226, 56, 56);
    Color purple = Color.FromArgb(151, 57, 153);
    Color blue = Color.FromArgb(0, 156, 223);
    private Button buttonReset;
    Color green = Color.FromArgb(94, 189, 62);

    public MandelForm()
    {
        this.Text = "Mandelbrot Designerotronic v1🚀";
        this.Size = new Size(500, 500);
        this.Paint += this.drawMandel;
        InitializeComponent();
        this.pictureBox1.MouseClick += this.pictureBoxClicked;
        this.buttonReset.MouseClick += this.resetMandel;

        // Set label text
        this.labelYValue.Text = "Center Y";
        this.labelXValue.Text = "Center X";
        this.labelScale.Text = "Scale";
        this.labelMax.Text = "Max";
        this.buttonCalculate.Text = "Calculate";

        this.comboBoxColors.Items.AddRange(new object[] {
                        "Basic",
                        "Sails",
                        "Fire",
                        "Sig sag",
                        "Rainbow"
        });
    }

    void resetMandel(object obj, MouseEventArgs ea)
    {
        xMiddle = defaultxMiddle;
        yMiddle = defaultyMiddle;
        scale = defaultScale;
        max = defaultMax;
        setMandelColor = defaultSetMandelColor;
        this.Invalidate();
    }

    void pictureBoxClicked(object obj, MouseEventArgs ea)
    {
        // Get x and y coords of the image
        double posX = ea.X;
        double posY = ea.Y;

        // Set the x and y coords to the x and y values of the clicked position
        this.xMiddle = (((this.mandelBrotImage.Width / 2) - posX) / this.scale) + this.xMiddle;
        this.yMiddle = - (((this.mandelBrotImage.Height / 2) - posY) / this.scale) + this.yMiddle;

        // Zoom in by updating scale
        this.scale += 500;

        this.Invalidate();
    }

    void drawMandel(object obj, PaintEventArgs pea)
    {
        this.mandelBrotImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);

        // Iterate through all the mandel image pixels
        // Assign each pixel a value. Thus a color
        for (int x = 0; x < mandelBrotImage.Width; x++)
        {
            for (int y = 0; y < mandelBrotImage.Height; y++)
            {
                double tempX = Convert.ToDouble(x) / this.scale - (this.mandelBrotImage.Width / this.scale / 2 + this.xMiddle) ;
                double tempY = Convert.ToDouble(y) / this.scale - (this.mandelBrotImage.Height / this.scale / 2 - this.yMiddle) ;

                int tempMandel = this.calculateMandel(tempX, tempY);
                Color tempColor = this.colorMandel(tempMandel);
                this.mandelBrotImage.SetPixel(x, y, tempColor);
            }
        }
        this.pictureBox1.Image = this.mandelBrotImage;
        this.setTextBoxes();
    }

    public void setTextBoxes()
    {
        this.textBoxXValue.Text = this.xMiddle.ToString();
        this.textBoxYValue.Text = this.yMiddle.ToString();
        this.textBoxScale.Text = this.scale.ToString();
        this.textBoxMax.Text = this.max.ToString();
        this.comboBoxColors.Text = this.setMandelColor;
    }

    public int calculateMandel(double xCoord, double yCoord)
    {
        // set variables for the method
        int mandel = 0;
        double a = 0;
        double b = 0;
        double aNew;
        double bNew;
        double distanceSquere;
        double distance;

        this.checkMax();

        do
        {
            // Calculate using given formula
            aNew = ((a * a) - (b * b) + xCoord);
            bNew = (2 * a * b + yCoord);

            // Set old values to the new values
            a = aNew;
            b = bNew;

            // Calculate distance using pytagoras
            distanceSquere = (a * a) + (b * b);
            distance = Math.Sqrt(distanceSquere);

            // Add one to mandel
            mandel++;
        } while (distance < 2 && mandel < this.max);
        return mandel;
    }

    private void checkMax()
    {
        // Check the max to avoid extreme recursion
        if (this.max > 1000)
            this.max = 1000;
    }

    private Color colorMandel(int mandel)
    { 
        if (setMandelColor == "Sails")
            return this.sailsColor(mandel);
        if (setMandelColor == "Fire") 
            return this.firesColor(mandel);
        if (setMandelColor == "Sig sag") 
            return this.sigsagsColor(mandel);
        if (setMandelColor == "Rainbow")
            return this.rainbowColor(mandel);

        return this.basicColor(mandel);
    }

    private Color basicColor(int mandel)
    {
        if (mandel == this.max)
            return this.black;
        if (mandel % 2 == 0)
            return this.white;
        return this.black;
    }

    private Color sailsColor(int mandel)
    {
        int r = 255 / mandel;
        int g = 255 / (Convert.ToInt32(this.max) - mandel + 1);
        int b = 0;
        Color sailColor = Color.FromArgb(r, g , b);
        return sailColor;
    }


    private Color firesColor(int mandel)
    {
        int r = 255 / mandel;
        int g = 0 ;
        int b = 255 / (Convert.ToInt32(this.max) - mandel + 1);
        Color fireColor = Color.FromArgb(r, g, b);
        return fireColor;
    }

    private Color sigsagsColor(int mandel)
    {
        int r = 0;
        int g = 255 / mandel;
        int b = 255 / (Convert.ToInt32(this.max) - mandel + 1);
        Color sigsagColor = Color.FromArgb(r, g, b);
        return sigsagColor;
    }

    private Color rainbowColor(int mandel)
    {
        if (mandel % 6 == 1)
            return this.yellow;
        if (mandel % 6 == 2)
            return this.orange;
        if (mandel % 6 == 3)
            return this.red;
        if (mandel % 6 == 4)
            return this.purple;
        if (mandel % 6 == 5)
            return this.blue;
        if (mandel % 6 == 0)
            return this.green;

        int r = 255 / mandel;
        int g = 255 / (Convert.ToInt32(this.max) - mandel + 1);
        int b = 0;
        Color sailColor = Color.FromArgb(r, g, b);
        return sailColor;
    }

    private void buttonCalculate_MouseClick(object sender, MouseEventArgs e)
    {
        // If textboxes are filled in read the input
        if (!string.IsNullOrEmpty(textBoxXValue.Text) && Double.TryParse(textBoxXValue.Text, out this.xMiddle))
            this.xMiddle = double.Parse(textBoxXValue.Text, System.Globalization.CultureInfo.InvariantCulture);
        if (!string.IsNullOrEmpty(textBoxYValue.Text) && Double.TryParse(textBoxYValue.Text, out this.yMiddle))
            this.yMiddle = double.Parse(textBoxYValue.Text, System.Globalization.CultureInfo.InvariantCulture);
        if (!string.IsNullOrEmpty(textBoxScale.Text) && Double.TryParse(textBoxScale.Text, out this.scale))
            this.scale = double.Parse(textBoxScale.Text, System.Globalization.CultureInfo.InvariantCulture);
        if (!string.IsNullOrEmpty(textBoxMax.Text) && Double.TryParse(textBoxMax.Text, out this.max))
            this.max = double.Parse(textBoxMax.Text, System.Globalization.CultureInfo.InvariantCulture);

        // Read dropdown
        Object selectedItem = comboBoxColors.SelectedItem;
        this.setMandelColor = selectedItem.ToString();

        // Invalidate drawing
        this.Invalidate();
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MandelForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelXValue = new System.Windows.Forms.Label();
            this.labelYValue = new System.Windows.Forms.Label();
            this.textBoxXValue = new System.Windows.Forms.TextBox();
            this.textBoxYValue = new System.Windows.Forms.TextBox();
            this.textBoxScale = new System.Windows.Forms.TextBox();
            this.textBoxMax = new System.Windows.Forms.TextBox();
            this.labelScale = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.comboBoxColors = new System.Windows.Forms.ComboBox();
            this.buttonReset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 59);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 400);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // labelXValue
            // 
            this.labelXValue.AutoSize = true;
            this.labelXValue.Location = new System.Drawing.Point(12, 9);
            this.labelXValue.Name = "labelXValue";
            this.labelXValue.Size = new System.Drawing.Size(63, 13);
            this.labelXValue.TabIndex = 1;
            this.labelXValue.Text = "labelXValue";
            // 
            // labelYValue
            // 
            this.labelYValue.AutoSize = true;
            this.labelYValue.Location = new System.Drawing.Point(12, 32);
            this.labelYValue.Name = "labelYValue";
            this.labelYValue.Size = new System.Drawing.Size(63, 13);
            this.labelYValue.TabIndex = 2;
            this.labelYValue.Text = "labelYValue";
            // 
            // textBoxXValue
            // 
            this.textBoxXValue.Location = new System.Drawing.Point(73, 6);
            this.textBoxXValue.Name = "textBoxXValue";
            this.textBoxXValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxXValue.TabIndex = 3;
            // 
            // textBoxYValue
            // 
            this.textBoxYValue.Location = new System.Drawing.Point(73, 29);
            this.textBoxYValue.Name = "textBoxYValue";
            this.textBoxYValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxYValue.TabIndex = 4;
            // 
            // textBoxScale
            // 
            this.textBoxScale.Location = new System.Drawing.Point(216, 29);
            this.textBoxScale.Name = "textBoxScale";
            this.textBoxScale.Size = new System.Drawing.Size(100, 20);
            this.textBoxScale.TabIndex = 8;
            // 
            // textBoxMax
            // 
            this.textBoxMax.Location = new System.Drawing.Point(216, 6);
            this.textBoxMax.Name = "textBoxMax";
            this.textBoxMax.Size = new System.Drawing.Size(100, 20);
            this.textBoxMax.TabIndex = 7;
            // 
            // labelScale
            // 
            this.labelScale.AutoSize = true;
            this.labelScale.Location = new System.Drawing.Point(175, 32);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(56, 13);
            this.labelScale.TabIndex = 6;
            this.labelScale.Text = "labelScale";
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.labelMax.Location = new System.Drawing.Point(175, 9);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(49, 13);
            this.labelMax.TabIndex = 5;
            this.labelMax.Text = "labelMax";
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Location = new System.Drawing.Point(334, 6);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(75, 43);
            this.buttonCalculate.TabIndex = 9;
            this.buttonCalculate.Text = "buttonCalculate";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonCalculate_MouseClick);
            // 
            // comboBoxColors
            // 
            this.comboBoxColors.FormattingEnabled = true;
            this.comboBoxColors.Location = new System.Drawing.Point(415, 7);
            this.comboBoxColors.Name = "comboBoxColors";
            this.comboBoxColors.Size = new System.Drawing.Size(121, 21);
            this.comboBoxColors.TabIndex = 10;
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(415, 59);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(121, 23);
            this.buttonReset.TabIndex = 11;
            this.buttonReset.Text = "buttonReset";
            this.buttonReset.UseVisualStyleBackColor = true;
            // 
            // MandelForm
            // 
            this.ClientSize = new System.Drawing.Size(539, 476);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.comboBoxColors);
            this.Controls.Add(this.buttonCalculate);
            this.Controls.Add(this.textBoxScale);
            this.Controls.Add(this.textBoxMax);
            this.Controls.Add(this.labelScale);
            this.Controls.Add(this.labelMax);
            this.Controls.Add(this.textBoxYValue);
            this.Controls.Add(this.textBoxXValue);
            this.Controls.Add(this.labelYValue);
            this.Controls.Add(this.labelXValue);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MandelForm";
            this.Load += new System.EventHandler(this.MandelForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    private void MandelForm_Load(object sender, EventArgs e)
    {

    }
}

class HalloWin3
{
    static void Main()
    {
        MandelForm scherm;
        scherm = new MandelForm();
        Application.Run(scherm);
    }
}