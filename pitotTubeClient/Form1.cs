using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace pitotTubeClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {    
            // すべてのシリアル・ポート名を取得する
            string[] ports = SerialPort.GetPortNames();

            // シリアルポートを毎回取得して表示するために表示の度にリストをクリアする
            comboBox1.Items.Clear();

            foreach (string port in ports)
            {
                // 取得したシリアル・ポート名を出力する
                comboBox1.Items.Add(port);
            }

        }

        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            // ボーレートを毎回取得して表示するために表示の度にリストをクリアする
            comboBox2.Items.Clear();

            // ボーレートを出力する
            comboBox2.Items.Add("115200"); //デフォルトなのでこれを最初にもってくる

            comboBox2.Items.Add("9600");
            comboBox2.Items.Add("14400");
            comboBox2.Items.Add("19200");
            comboBox2.Items.Add("28800");
            comboBox2.Items.Add("38400");
            comboBox2.Items.Add("57600");
            comboBox2.Items.Add("76800");
            comboBox2.Items.Add("153600");
            comboBox2.Items.Add("230400");
            comboBox2.Items.Add("460800");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //comboBox1とcomboBox2に設定があればシリアルポートを開く
            if (comboBox1.Text != "" && comboBox2.Text != "")
            {
                serialPort1.PortName = comboBox1.Text;

                serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);

                if (serialPort1.IsOpen == true)
                {
                    serialPort1.Close();
                }

                if (serialPort1.IsOpen == false)
                {
                    serialPort1.Open();

                    if (serialPort1.IsOpen == true)
                    {
                        MessageBox.Show("Open Success\n" + serialPort1.PortName.ToString() + " " + serialPort1.BaudRate.ToString());
                    }
                    else
                    {
                        MessageBox.Show("COM Port error");
                    }
                }
            }
            else
            {
                MessageBox.Show("COM Port error");
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = serialPort1.ReadLine();
                if (!string.IsNullOrEmpty(data))
                {
                    //delegateを呼び出す
                    Invoke(new AppendTextDelegate(packet_restortion), data);

                    Console.WriteLine("{0}", data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    //delegate（処理を委譲）
    delegate void AppendTextDelegate(string text);

    //委譲先の関数
    private void packet_restortion(string data)
    {
        if (data.StartsWith("#"))
        {
            listBox1.Items.Add(data);
        }
        else
        {
            String[] element=data.Split(',');
            
            double p = Double.Parse(element[0]);
            double y = Double.Parse(element[1]);
            double t = Double.Parse(element[2]);
            double pa = Double.Parse(element[3]);
            double ya = Double.Parse(element[4]);

            DataPoint point = new DataPoint(p, y);
            point.MarkerSize = 5;

            DataPoint point2 = new DataPoint(pa, ya);
            point2.MarkerSize = 10;

            chart1.Series[0].Points.Add(point);
            
            chart1.Series[1].Points.Add(point2);

            if (chart1.Series[0].Points.Count > 50)
            {
                chart1.Series[0].Points.RemoveAt(0);
            }
                if (chart1.Series[0].Points.Count > 2)
                {
                    chart1.Series[1].Points.RemoveAt(0);
                }
        }
        
    }

    private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
