namespace ScalperUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Strike1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Strike2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bid1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Offer1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bid2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Offer2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BidSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OfferSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoofLots = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(50, 63);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(170, 24);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(113, 278);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(395, 278);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DateTime,
            this.Strike1,
            this.Strike2,
            this.Bid1,
            this.Offer1,
            this.Bid2,
            this.Offer2,
            this.BidSum,
            this.OfferSum,
            this.NoofLots});
            this.dataGridView1.Location = new System.Drawing.Point(409, 23);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(607, 166);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(60, 165);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(214, 22);
            this.textBox1.TabIndex = 4;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Strategy";
            // 
            // DateTime
            // 
            this.DateTime.HeaderText = "DateTime";
            this.DateTime.Name = "DateTime";
            // 
            // Strike1
            // 
            this.Strike1.HeaderText = "Strike1";
            this.Strike1.Name = "Strike1";
            // 
            // Strike2
            // 
            this.Strike2.HeaderText = "Strike2";
            this.Strike2.Name = "Strike2";
            // 
            // Bid1
            // 
            this.Bid1.HeaderText = "Bid1";
            this.Bid1.Name = "Bid1";
            // 
            // Offer1
            // 
            this.Offer1.HeaderText = "Offer1";
            this.Offer1.Name = "Offer1";
            // 
            // Bid2
            // 
            this.Bid2.HeaderText = "Bid2";
            this.Bid2.Name = "Bid2";
            // 
            // Offer2
            // 
            this.Offer2.HeaderText = "Offer2";
            this.Offer2.Name = "Offer2";
            // 
            // BidSum
            // 
            this.BidSum.HeaderText = "BidSum";
            this.BidSum.Name = "BidSum";
            // 
            // OfferSum
            // 
            this.OfferSum.HeaderText = "OfferSum";
            this.OfferSum.Name = "OfferSum";
            // 
            // NoofLots
            // 
            this.NoofLots.HeaderText = "NoofLots";
            this.NoofLots.Name = "NoofLots";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1112, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.comboBox1);
            this.Name = "Form1";
            this.Text = "Scalper Dashboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Strike1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Strike2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bid1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Offer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bid2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Offer2;
        private System.Windows.Forms.DataGridViewTextBoxColumn BidSum;
        private System.Windows.Forms.DataGridViewTextBoxColumn OfferSum;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoofLots;
    }
}

