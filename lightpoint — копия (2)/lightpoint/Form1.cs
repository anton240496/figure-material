using System;

using System.Drawing;

using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace lightpoint
{
    public partial class Form1 : Form
    {
        Device dr;
        PresentParameters ppr = new PresentParameters();
        Vector3 cp = new Vector3(5, 0, 0), cp0, ct = new Vector3(0, 0, 0), cup = new Vector3(0, 0, 1), cup0, lp;
        Point cur0;
        Mesh myp, myp1, myp3;
        Matrix rot;
        Material mat,mat1,mat2; //Материал
        float pi = (float)Math.PI;
   

        public Form1()
        {
            InitializeComponent();
        
        }

        //Инициализация 3D устройства Low
        public bool InitializeGraphicsL()
        {
            try
            {
                ppr.Windowed = true;
                ppr.SwapEffect = SwapEffect.Discard;
                ppr.EnableAutoDepthStencil = true;
                ppr.AutoDepthStencilFormat = DepthFormat.D16;
                ppr.MultiSample = MultiSampleType.None;
                dr = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, ppr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Инициализация 3D устройства Hi
        public bool InitializeGraphicsH()
        {
            try
            {
                ppr.Windowed = true;
                ppr.SwapEffect = SwapEffect.Discard;
                ppr.PresentationInterval = PresentInterval.One; //Default
                ppr.EnableAutoDepthStencil = true;
                ppr.AutoDepthStencilFormat = DepthFormat.D24S8;
                ppr.MultiSample = MultiSampleType.FourSamples;
                dr = new Device(0, DeviceType.Hardware, this, CreateFlags.HardwareVertexProcessing, ppr);
                return true;
            }
            catch
            {
                return false;
            }
        }

       

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            cur0 = e.Location;
            cp0 = cp;
            cup0 = cup;
        }


        //Загрузка формы
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!this.InitializeGraphicsH())
            {
                if (!this.InitializeGraphicsL())
                {
                    MessageBox.Show("Невозможно инициализировать Direct3D устройство. Программа будет закрыта.");
                    this.Close();
                }
            }
            //myp = Mesh.Teapot(dr);
            myp = Mesh.Sphere(dr, 0.9f, 20, 10);
            rot = Matrix.RotationX(pi / 2f);
            lp = cp;
            myp1 = Mesh.Torus(dr, 0.3f, 0.5f, 20, 20); //Тор
            myp3 = Mesh.Cylinder(dr, 0.6f, 0.6f, 2, 20, 1); //цилиндр

            timer1.Enabled = true;
        }

        //Вывод по таймеру
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                dr.BeginScene();
                dr.RenderState.CullMode = Cull.None;
                dr.RenderState.ZBufferEnable = true;

                float ar = (float)this.ClientSize.Width / (float)this.ClientSize.Height;
                var view = Matrix.LookAtRH(cp, ct, cup);
                var projection = Matrix.PerspectiveFovRH(pi / 4f, ar, 1f, 100f);

                //Настройка источника света
                dr.RenderState.Lighting = true;
                dr.Lights[0].Type = LightType.Point;
              

                if (светИзКамерыToolStripMenuItem.ForeColor == Color.FromName("Control")) lp = cp;
                dr.Lights[0].Position = lp;
                dr.Lights[0].Range = hScrollBar1.Value/2f;
                dr.Lights[0].Attenuation0 = hScrollBar2.Value / 100f;
                dr.Lights[0].Enabled = true;

                dr.Clear(ClearFlags.ZBuffer | ClearFlags.Target, 5, 1f, 0);

                dr.Transform.View = Matrix.LookAtRH(cp, ct, cup);
                dr.Transform.Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4f, ar, 0.1f, 200f);

                dr.Transform.World = Matrix.Translation(0.0f, -2.0f, 0.0f);
                dr.Material = mat;
                mat.Diffuse = Color.Red;
                mat.Emissive = Color.Black;
               
                myp.DrawSubset(0);
                
                dr.Transform.World = Matrix.Translation(0.0f, 0.0f, 0.0f)*Matrix.RotationY(1.6f);
                dr.Material = mat1;
                mat1.Diffuse = Color.Transparent;
                mat1.Emissive = Color.Black;

                myp1.DrawSubset(0);
                dr.Transform.World = Matrix.Translation(0.0f, 0.0f, -2.0f) * Matrix.RotationX(1.6f);
                dr.Material = mat2;
                mat2.Diffuse = Color.Bisque;
                mat2.Emissive = Color.Black;
                myp3.DrawSubset(0);




                dr.EndScene();
                dr.Present();
            }
            catch
            {
                this.Close();
            }
        }

       

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      

    
    
        
      

    }
}
