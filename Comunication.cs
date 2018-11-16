using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class Form1 : Form
{
    private TabControl tabControl1;

    // Declares tabPage1 as a TabPage type.
    private System.Windows.Forms.TabPage terminal;
	private System.Windows.Forms.TabPage connection;

    private void MyTabs()
    {
        this.tabControl1 = new TabControl();

        // Invokes the TabPage() constructor to create the tabPage1.
        this.terminal = new System.Windows.Forms.TabPage();
		this.connection = new System.Windows.Forms.TabPage();
		this.terminal.Text="Terminal";
		this.connection.Text="Connection";
        this.tabControl1.Controls.AddRange(new Control[] { this.terminal});
		this.tabControl1.Controls.AddRange(new Control[] { this.connection});	
        this.tabControl1.Location = new Point(25, 25);
        this.tabControl1.Size = new Size(750, 550);

        this.ClientSize = new Size(800, 600);
        this.Controls.AddRange(new Control[] {
	        this.tabControl1});
    }

    public Form1()
    {
        MyTabs();
    }

    static void Main() 
    {
        Application.Run(new Form1());
    }
}