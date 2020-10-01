// Mandelbrood figure
// Co authors. Maarten Gerritse and Martijn Totté
// Studentnumbers: 8845874 and 1235002
// C# imperatief programmeren

using System.Windows.Forms;
using System.Drawing;
using System;
using amandelbrood;

class MandelForm : Form
{
    // Set member form variables
    private PictureBox pictureBox1;
    private Label labelYValue;
    private TextBox textBoxXValue;
    private TextBox textBoxYValue;
    private TextBox textBoxScale;
    private TextBox textBoxMax;
    private Label labelScale;
    private Label labelMax;
    private Button buttonCalculate;
    private Button buttonReset;
    private Label labelXValue;
    private ComboBox comboBoxColors;
    private Label labelPreset;
    private Label labelZoomOnClick;
    private Button buttonZoomOut;
    private Button buttonZoomIn;
    private Label labelColorPicker;
    private Button buttonZoomCenter;

    // Preset buttons
    private Button buttonPreset1;
    private Button buttonPreset2;
    private Button buttonPreset3;
    private Button buttonPreset4;
    private Button buttonPreset5;

    // Set global necessities
    private Bitmap mandelBrotImage;
    private Boolean zoomIn = true;
    private Color colorError = Color.Red;
    private Color colorBtSelected = Color.LightGray;
    private Color colorNormal = default(Color);
    private int pictureboxWidth = 400;
    private int pictureboxHeight = 400;
    private int screenWidth = 555;
    private int screenHight = 515;

    // Defining presets
    private static Preset defaultPreset = new Preset(0, 0, 0.01, 1000, "Basic");
    private static Preset preset1 = new Preset(0.105390625, -0.92462890625, 3.90625E-05, 1000, "Rainbow");
    private static Preset preset2 = new Preset(0.5622216796875, -0.6428271484375, 1E-05, 1000, "Rainbow");
    private static Preset preset3 = new Preset(0.75248046875, 0.038642578125, 2E-06, 400, "Basic");
    private static Preset preset4 = new Preset(0.3672, -0.60185, 4E-05, 1000, "Rainbow");
    private static Preset preset5 = new Preset(0.36489375, -0.60032578125, 2.5E-06, 1000, "Blauw");
    private Preset currentPreset = new Preset(defaultPreset.getXMiddle(), defaultPreset.getYMiddle(), defaultPreset.getScale(), defaultPreset.getMax(), defaultPreset.getMandelColor());

    //Set colors for mandel
    Color black = Color.FromArgb(0, 0, 0);
    Color white = Color.FromArgb(255, 255, 255);
    Color yellow = Color.FromArgb(255, 185, 0);
    Color orange = Color.FromArgb(247, 130, 0);
    Color red = Color.FromArgb(226, 56, 56);
    Color purple = Color.FromArgb(151, 57, 153);
    Color blue = Color.FromArgb(0, 156, 223);
    Color green = Color.FromArgb(94, 189, 62);

    // Creating the constructor for a class
    public MandelForm()
    {
        this.Text = "Mandelbrot Designerotronic v1🚀";
        this.Paint += this.drawMandel;
        InitializeComponent();
        this.pictureBox1.MouseClick += this.pictureBoxClicked;

        // Give extra preset parameter to onclick
        // This way we only need one function. (Reset is also a preset)
        this.buttonReset.MouseClick += (sender, EventArgs) => { setPreset(sender, EventArgs, defaultPreset); }; ;
        this.buttonPreset1.MouseClick += (sender, EventArgs) => { setPreset(sender, EventArgs, preset1); }; ;
        this.buttonPreset2.MouseClick += (sender, EventArgs) => { setPreset(sender, EventArgs, preset2); }; ;
        this.buttonPreset3.MouseClick += (sender, EventArgs) => { setPreset(sender, EventArgs, preset3); }; ;
        this.buttonPreset4.MouseClick += (sender, EventArgs) => { setPreset(sender, EventArgs, preset4); }; ;
        this.buttonPreset5.MouseClick += (sender, EventArgs) => { setPreset(sender, EventArgs, preset5); }; ;
        this.buttonZoomOut.MouseClick += setZoomOut;
        this.buttonZoomIn.MouseClick += setZoomIn;
        this.buttonZoomCenter.MouseClick += zoomCenter;

        // Set label text for buttons, labels and the combobox
        this.labelYValue.Text = "Center Y";
        this.labelXValue.Text = "Center X";
        this.labelScale.Text = "Scale";
        this.labelMax.Text = "Max";
        this.buttonCalculate.Text = "Calculate";
        this.buttonReset.Text = "Reset";
        this.buttonPreset1.Text = "Minimandel";
        this.buttonPreset2.Text = "Neverland";
        this.buttonPreset3.Text = "Croissant";
        this.buttonPreset4.Text = "Chain";
        this.buttonPreset5.Text = "Void";
        this.labelPreset.Text = "Available presets:";
        this.buttonZoomOut.Text = "-";
        this.buttonZoomIn.Text = "+";
        this.labelZoomOnClick.Text = "Zooming in";
        this.labelColorPicker.Text = "Color picker";
        this.buttonZoomIn.BackColor = colorBtSelected;
        this.buttonZoomOut.BackColor = colorNormal;
        this.buttonZoomCenter.Text = "Zoom in center";

        // The array with the options to choose from
        this.comboBoxColors.Items.AddRange(new object[] {
                        "Basic",
                        "Groen",
                        "Rood",
                        "Blauw",
                        "Rainbow"
        });
        this.setTextBoxes();
    }

    public void zoomCenter(object obj, MouseEventArgs mou)
    {
        Console.WriteLine("Zoom center");
        this.currentPreset.setScale(this.currentPreset.getScale() * 0.5);
        this.Invalidate();
    }

    public void setPreset(object obj, MouseEventArgs ea, Preset tempPreset)
    {
        // Set all values to tempPreset values given
        currentPreset = new Preset(tempPreset.getXMiddle(), tempPreset.getYMiddle(), tempPreset.getScale(), tempPreset.getMax(), tempPreset.getMandelColor());
        this.Invalidate();
    }

    public void pictureBoxClicked(object obj, MouseEventArgs ea)
    {
        // Get x and y coordinates of the image
        double posX = ea.X;
        double posY = ea.Y;

        // Set the x and y coordinates to the x and y values of the clicked position
        this.currentPreset.setXMiddle((((this.mandelBrotImage.Width / 2) - posX) * this.currentPreset.getScale()) + this.currentPreset.getXMiddle());
        this.currentPreset.setYMiddle( - (((this.mandelBrotImage.Height / 2) - posY) * this.currentPreset.getScale()) + this.currentPreset.getYMiddle());

        // Zoom in by updating scale
        this.zoomInZoomOut();

        this.Invalidate();
    }

    private void zoomInZoomOut()
    {
        if (this.zoomIn)
            this.currentPreset.setScale(this.currentPreset.getScale() * 0.5);
        else
        {
            this.currentPreset.setScale(this.currentPreset.getScale() * 2);
        }
    }

    private void setZoomOut(object obj, MouseEventArgs ea)
    {
        this.zoomIn = false;
        this.labelZoomOnClick.Text = "Zooming out";
        this.buttonZoomIn.BackColor = this.colorNormal;
        this.buttonZoomOut.BackColor = this.colorBtSelected;
    }
    
    private void setZoomIn(object obj, MouseEventArgs ea)
    {
        this.zoomIn = true;
        this.labelZoomOnClick.Text = "Zooming in";
        this.buttonZoomIn.BackColor = this.colorNormal;
        this.buttonZoomOut.BackColor = this.colorBtSelected;
    }

    // Creating the Mandelbrot image and adding it to the bitmap
    void drawMandel(object obj, PaintEventArgs pea)
    {
        // Gets the width of the bitmap image
        this.setMandelImageWidth();

        this.mandelBrotImage = new Bitmap(this.pictureboxWidth, this.pictureboxHeight);

        // Iterate through all the mandel image pixels
        // Assign each pixel a value. Thus a color
        for (int x = 0; x < mandelBrotImage.Width; x++)
        {
            for (int y = 0; y < mandelBrotImage.Height; y++)
            {
                // Devide the x by the scale. And get the position of the figure (not the screen)
                double tempX = Convert.ToDouble(x) * this.currentPreset.getScale() - (this.mandelBrotImage.Width * this.currentPreset.getScale() / 2 + this.currentPreset.getXMiddle()) ;
                double tempY = Convert.ToDouble(y) * this.currentPreset.getScale() - (this.mandelBrotImage.Height * this.currentPreset.getScale() / 2 - this.currentPreset.getYMiddle()) ;

                // Use these temp numbers to calculate the mandel
                int tempMandel = this.calculateMandel(tempX, tempY);

                // Use the mandel to configure a color
                Color tempColor = this.colorMandel(tempMandel);
                this.mandelBrotImage.SetPixel(x, y, tempColor);
            }
        }
        this.pictureBox1.Image = this.mandelBrotImage;
    }

    // Setting the values to the appropriate textboxes
    public void setTextBoxes()
    {
        this.textBoxXValue.Text = this.currentPreset.getXMiddle().ToString();
        this.textBoxYValue.Text = this.currentPreset.getYMiddle().ToString();
        this.textBoxScale.Text = this.currentPreset.getScale().ToString();
        this.textBoxMax.Text = this.currentPreset.getMax().ToString();
        this.comboBoxColors.Text = this.currentPreset.getMandelColor();
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

        // Checking if the max > 1000, if so, set max to 1000
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
        } while (distance < 2 && mandel < this.currentPreset.getMax());
        return mandel;
    }

    private void checkMax()
    {
        // Check the max to avoid extreme recursion
        if (this.currentPreset.getMax() > 1000)
            this.currentPreset.setMax(1000);
    }

    private Color colorMandel(int mandel)
    { 
        // Check what color is selected
        if (this.currentPreset.getMandelColor() == "Groen")
            return this.GroenColor(mandel);
        if (this.currentPreset.getMandelColor() == "Rood") 
            return this.RoodsColor(mandel);
        if (this.currentPreset.getMandelColor() == "Blauw") 
            return this.BlauwsColor(mandel);
        if (this.currentPreset.getMandelColor() == "Rainbow")
            return this.rainbowColor(mandel);

        return this.basicColor(mandel);

    }

    private Color basicColor(int mandel)
    {
        // All even numbers return white
        if (mandel == this.currentPreset.getMax())
            return this.black;
        if (mandel % 2 == 0)
            return this.white;
        return this.black;
    }

    // Creating a gradient for the 3 color palettes Groen, Rood and Blauw 
    // Using doubles's and converting to int's for the RGB format
    private Color GroenColor(int mandel)
    {
        int g = 255;
        double bDou = (1 - (mandel / this.currentPreset.getMax())) * 255;
        int b = Convert.ToInt32(bDou);
        double rDou = mandel / this.currentPreset.getMax() * 255;
        int r = Convert.ToInt32(rDou);
        Color RoodColor = Color.FromArgb(r, g, b);
        return RoodColor;
    }

    private Color RoodsColor(double mandel)
    {
        int r = 255 ;
        double gDou = (1 - (mandel / this.currentPreset.getMax())) * 255;
        int g = Convert.ToInt32(gDou);
        double bDou = mandel / this.currentPreset.getMax() * 255;
        int b = Convert.ToInt32(bDou);
        Color RoodColor = Color.FromArgb(r, g, b);
        return RoodColor;
    }

    private Color BlauwsColor(int mandel)
    {
        int b = 255;
        double rDou = (1 - (mandel / this.currentPreset.getMax())) * 255;
        int r = Convert.ToInt32(rDou);
        double gDou = mandel / this.currentPreset.getMax() * 255;
        int g = Convert.ToInt32(gDou);
        Color RoodColor = Color.FromArgb(r, g, b);
        return RoodColor;
    }

    // Creating our favourite colour scale. 
    // These rainbow colours are mapped in order by using modulo and using the pre defined colours
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
        int g = 255 / (Convert.ToInt32(this.currentPreset.getMax()) - mandel + 1);
        int b = 0;
        Color sailColor = Color.FromArgb(r, g, b);
        return sailColor;
    }

    private void buttonCalculate_MouseClick(object sender, MouseEventArgs e)
    {
        // If textboxes have a appropriate input, read the input
        try
        {
            this.currentPreset.setXMiddle(double.Parse(textBoxXValue.Text));
            this.textBoxXValue.BackColor = this.colorNormal;
        }
        catch (Exception ea) { this.textBoxXValue.BackColor = this.colorError; }

        try
        {
            this.currentPreset.setYMiddle(double.Parse(textBoxYValue.Text));
            this.textBoxYValue.BackColor = this.colorNormal;
        }
        catch (Exception ea) { this.textBoxYValue.BackColor = this.colorError; }

        try
        {
            this.currentPreset.setScale(double.Parse(textBoxScale.Text));
            this.textBoxScale.BackColor = this.colorNormal;
        }
        catch (Exception ea) { this.textBoxScale.BackColor = this.colorError; }

        try
        {
            this.currentPreset.setMax(double.Parse(textBoxMax.Text));
            this.textBoxMax.BackColor = this.colorNormal;
        }
        catch (Exception ea) { this.textBoxMax.BackColor = this.colorError; }

        // Invalidate drawing
        this.Invalidate();
    }

    private void comboBoxColors_SelectionChangeCommitted(object sender, EventArgs e)
    {
        // Read combobox input on click :)
        try
        {
            Object selectedItem = comboBoxColors.SelectedItem;
            this.currentPreset.setMandelColor(selectedItem.ToString());
        }
        catch (Exception ea)
        { }

        this.Invalidate();
    }

    // Calculate the difference between the previous and current screen width
    // Subtract the change from the picture box
    private void setMandelImageWidth()
    {
        int xChange = this.Width - this.screenWidth;
        int yChange = this.Height - this.screenHight;

        this.screenWidth = this.Width;
        this.screenHight = this.Height;

        this.pictureboxWidth += xChange;
        this.pictureboxHeight += yChange;

        this.pictureBox1.Size = new System.Drawing.Size(this.pictureboxWidth, this.pictureboxHeight);
    }

    // Give values to the elements created in the designer
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
            this.buttonPreset1 = new System.Windows.Forms.Button();
            this.buttonPreset2 = new System.Windows.Forms.Button();
            this.buttonPreset3 = new System.Windows.Forms.Button();
            this.labelPreset = new System.Windows.Forms.Label();
            this.labelZoomOnClick = new System.Windows.Forms.Label();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.buttonPreset4 = new System.Windows.Forms.Button();
            this.buttonPreset5 = new System.Windows.Forms.Button();
            this.labelColorPicker = new System.Windows.Forms.Label();
            this.buttonZoomCenter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(136, 60);
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
            this.comboBoxColors.Location = new System.Drawing.Point(415, 28);
            this.comboBoxColors.Name = "comboBoxColors";
            this.comboBoxColors.Size = new System.Drawing.Size(121, 21);
            this.comboBoxColors.TabIndex = 10;
            this.comboBoxColors.SelectionChangeCommitted += new System.EventHandler(this.comboBoxColors_SelectionChangeCommitted);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(13, 60);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(121, 23);
            this.buttonReset.TabIndex = 11;
            this.buttonReset.Text = "buttonReset";
            this.buttonReset.UseVisualStyleBackColor = true;
            // 
            // buttonPreset1
            // 
            this.buttonPreset1.Location = new System.Drawing.Point(13, 117);
            this.buttonPreset1.Name = "buttonPreset1";
            this.buttonPreset1.Size = new System.Drawing.Size(121, 23);
            this.buttonPreset1.TabIndex = 12;
            this.buttonPreset1.Text = "buttonPreset1";
            this.buttonPreset1.UseVisualStyleBackColor = true;
            // 
            // buttonPreset2
            // 
            this.buttonPreset2.Location = new System.Drawing.Point(13, 146);
            this.buttonPreset2.Name = "buttonPreset2";
            this.buttonPreset2.Size = new System.Drawing.Size(121, 23);
            this.buttonPreset2.TabIndex = 12;
            this.buttonPreset2.Text = "buttonPreset1";
            this.buttonPreset2.UseVisualStyleBackColor = true;
            // 
            // buttonPreset3
            // 
            this.buttonPreset3.Location = new System.Drawing.Point(13, 175);
            this.buttonPreset3.Name = "buttonPreset3";
            this.buttonPreset3.Size = new System.Drawing.Size(121, 23);
            this.buttonPreset3.TabIndex = 12;
            this.buttonPreset3.Text = "buttonPreset1";
            this.buttonPreset3.UseVisualStyleBackColor = true;
            // 
            // labelPreset
            // 
            this.labelPreset.AutoSize = true;
            this.labelPreset.Location = new System.Drawing.Point(16, 101);
            this.labelPreset.Name = "labelPreset";
            this.labelPreset.Size = new System.Drawing.Size(59, 13);
            this.labelPreset.TabIndex = 13;
            this.labelPreset.Text = "labelPreset";
            // 
            // labelZoomOnClick
            // 
            this.labelZoomOnClick.AutoSize = true;
            this.labelZoomOnClick.Location = new System.Drawing.Point(19, 420);
            this.labelZoomOnClick.Name = "labelZoomOnClick";
            this.labelZoomOnClick.Size = new System.Drawing.Size(93, 13);
            this.labelZoomOnClick.TabIndex = 14;
            this.labelZoomOnClick.Text = "labelZoomOnClick";
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.Location = new System.Drawing.Point(19, 437);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(35, 23);
            this.buttonZoomOut.TabIndex = 15;
            this.buttonZoomOut.Text = "buttonZoomOut";
            this.buttonZoomOut.UseVisualStyleBackColor = true;
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.Location = new System.Drawing.Point(73, 437);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(35, 23);
            this.buttonZoomIn.TabIndex = 16;
            this.buttonZoomIn.Text = "buttonZoomIn";
            this.buttonZoomIn.UseVisualStyleBackColor = true;
            // 
            // buttonPreset4
            // 
            this.buttonPreset4.Location = new System.Drawing.Point(13, 204);
            this.buttonPreset4.Name = "buttonPreset4";
            this.buttonPreset4.Size = new System.Drawing.Size(121, 23);
            this.buttonPreset4.TabIndex = 17;
            this.buttonPreset4.Text = "buttonPreset1";
            this.buttonPreset4.UseVisualStyleBackColor = true;
            // 
            // buttonPreset5
            // 
            this.buttonPreset5.Location = new System.Drawing.Point(13, 233);
            this.buttonPreset5.Name = "buttonPreset5";
            this.buttonPreset5.Size = new System.Drawing.Size(121, 23);
            this.buttonPreset5.TabIndex = 18;
            this.buttonPreset5.Text = "buttonPreset1";
            this.buttonPreset5.UseVisualStyleBackColor = true;
            // 
            // labelColorPicker
            // 
            this.labelColorPicker.AutoSize = true;
            this.labelColorPicker.Location = new System.Drawing.Point(415, 9);
            this.labelColorPicker.Name = "labelColorPicker";
            this.labelColorPicker.Size = new System.Drawing.Size(83, 13);
            this.labelColorPicker.TabIndex = 19;
            this.labelColorPicker.Text = "labelColorPicker";
            // 
            // buttonZoomCenter
            // 
            this.buttonZoomCenter.Location = new System.Drawing.Point(15, 367);
            this.buttonZoomCenter.Name = "buttonZoomCenter";
            this.buttonZoomCenter.Size = new System.Drawing.Size(115, 36);
            this.buttonZoomCenter.TabIndex = 20;
            this.buttonZoomCenter.Text = "buttonZoomCenter";
            this.buttonZoomCenter.UseVisualStyleBackColor = true;
            // 
            // MandelForm
            // 
            this.ClientSize = new System.Drawing.Size(539, 476);
            this.Controls.Add(this.buttonZoomCenter);
            this.Controls.Add(this.labelColorPicker);
            this.Controls.Add(this.buttonPreset5);
            this.Controls.Add(this.buttonPreset4);
            this.Controls.Add(this.buttonZoomIn);
            this.Controls.Add(this.buttonZoomOut);
            this.Controls.Add(this.labelZoomOnClick);
            this.Controls.Add(this.labelPreset);
            this.Controls.Add(this.buttonPreset3);
            this.Controls.Add(this.buttonPreset2);
            this.Controls.Add(this.buttonPreset1);
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
            this.ResizeEnd += new System.EventHandler(this.MandelForm_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    // Set image width if application is resized
    private void MandelForm_ResizeEnd(object sender, EventArgs e)
    {
        this.setMandelImageWidth();
    }
}

class MandelClass
{
    static void Main()
    {
        MandelForm scherm;
        scherm = new MandelForm();
        Application.Run(scherm);
    }
}