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

    // Set global necessities
    private double xMiddle;
    private double yMiddle;
    private double scale;
    private double max;
    private Bitmap mandelBrotImage;

    //Set colors
    Color black = Color.FromArgb(0, 0, 0);
    Color white = Color.FromArgb(255, 255, 255);

    public MandelForm()
    {
        this.Text = "Hallo";
        this.Size = new Size(500, 500);
        this.Paint += this.drawMandel;
        InitializeComponent();

        Console.WriteLine(this.calculateMandel(0.5, 0.8));

        // Set label text
        this.labelYValue.Text = "Center Y";
        this.labelXValue.Text = "Center X";
        this.labelScale.Text = "Scale";
        this.labelMax.Text = "Max";
        this.buttonCalculate.Text = "Calculate";
    }

    void drawMandel(object obj, PaintEventArgs pea)
    {
        //int tempMandel;
        // Color tempColor;

        this.mandelBrotImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);

        for (int x = 0; x < mandelBrotImage.Width; x++)
        {
            for (int y = 0; y < mandelBrotImage.Height; y++)
            {
                double tempX = Convert.ToDouble(x) / 100 - 1;
                double tempY = Convert.ToDouble(y) / 100 - 1;

                int tempMandel = this.calculateMandel(tempX, tempY);
                Color tempColor = this.colorMandel(tempMandel);
                this.mandelBrotImage.SetPixel(x, y, tempColor);
            }
        }
        this.pictureBox1.Image = this.mandelBrotImage;
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
        } while (distance < 2 && mandel < 7);
        return mandel;
    }

    private Color colorMandel(int mandel)
    {
        if (mandel % 2 == 0)
            return this.white; 
        if (mandel == 7)
            return Color.FromArgb(255, 0, 0); // Red
        return this.black;
    }

    private void InitializeComponent()
    {
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
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
        this.SuspendLayout();
        // 
        // pictureBox1
        // 
        this.pictureBox1.Location = new System.Drawing.Point(12, 59);
        this.pictureBox1.Name = "pictureBox1";
        this.pictureBox1.Size = new System.Drawing.Size(443, 304);
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
        // MandelForm
        // 
        this.ClientSize = new System.Drawing.Size(467, 375);
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
        this.Name = "MandelForm";
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    private void buttonCalculate_MouseClick(object sender, MouseEventArgs e)
    {
        // Read textboxes
        // TODO: Change to tryParse!
        xMiddle = double.Parse(textBoxXValue.Text, System.Globalization.CultureInfo.InvariantCulture);
        yMiddle = double.Parse(textBoxYValue.Text, System.Globalization.CultureInfo.InvariantCulture);
        scale = double.Parse(textBoxScale.Text, System.Globalization.CultureInfo.InvariantCulture);
        max = double.Parse(textBoxMax.Text, System.Globalization.CultureInfo.InvariantCulture);
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