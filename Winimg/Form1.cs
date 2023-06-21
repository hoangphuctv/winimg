using System.Linq;

namespace Winimg
{
    public partial class Form1 : Form
    {
        String[] files;
        String imgPath;
        int imgIndex;
        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG" };
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.imgIndex = 0;
            this.textBox1.Visible = false;
            this.textBox1.ReadOnly = true;
            string[] args = Environment.GetCommandLineArgs();

            imgPath = "";
            foreach (String f in args)
            {
                this.log("args[] = " + f);
                if (this.isImg(f))
                {
                    imgPath = f;
                    break;
                }
            }
            this.log("isDir(imgPath) = " + imgPath);
            this.log("isDir(imgPath) = " + isDir(imgPath));

            switch (isDir(imgPath))
            {
                case 0:
                    if (this.isImg(imgPath))
                    {
                        this.setImgePath(imgPath);
                        this.scanFiles(Path.GetDirectoryName(imgPath));
                    }
                    else
                    {
                        imgPath = "";
                    }
                    break;
                case 1:
                    this.scanFiles(imgPath);
                    if (this.files != null && this.files[0] != null)
                    {
                        this.setImgePath(this.files[0]);
                    }
                    break;
            }
        }
        private void showCurImg()
        {
            if (this.files != null && this.files[this.imgIndex] != null)
            {
                this.setImgePath(this.files[this.imgIndex]);
            }
        }
        private void setImgePath(String apath)
        {
            if (!File.Exists(apath)) { return; }
            this.log("setImgePath " + apath);
            if (this.files != null && this.files.Length > 0)
            {
                this.Text = (this.imgIndex + 1) + "/" + this.files.Length + " | " + apath;
            }
            else
            {
                this.Text = apath;
            }
            if (imgPath != apath)
            {
                imgPath = apath;
            }
            pictureBox.ImageLocation = apath;
            pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox.TabIndex = 0;
            pictureBox.Left = 0;
            pictureBox.Top = 0;
            this.initSize();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            this.initSize();
        }

        private void initSize()
        {

            pictureBox.Size = new Size(this.Size.Width, this.Size.Height);
        }

        private void scanFiles(String dirpath)
        {
            this.log("scanFiles " + dirpath);
            if (dirpath == null) return;

            String[] _files = Directory.GetFiles(dirpath);
            String[] _images = { };
            foreach (String f in _files)
            {
                if (isImg(f))
                {
                    this.log(f + "\r\n");
                    _images = _images.Append(f).ToArray();
                }
            }
            this.log("scanFiles count = " + _images.Length);
            this.files = _images;
        }

        // return 1 is dir, 0 is file, -1 not exsti
        private int isDir(String pathname)
        {
            if (pathname == null) return -1;
            if (!Directory.Exists(pathname) && !File.Exists(pathname)) return -1;
            FileAttributes attr = File.GetAttributes(pathname);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                return 1;
            return 0;
        }

        private bool isImg(String f)
        {
            return ImageExtensions.Contains(Path.GetExtension(f).ToUpperInvariant());
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Q:
                    Application.Exit();
                    break;
                case Keys.Right:
                    this.imgIndex++;
                    if (this.imgIndex >= this.files.Length)
                    {
                        this.imgIndex = 0;
                    }
                    this.showCurImg();
                    break;
                case Keys.Left:
                    this.imgIndex--;
                    if (this.imgIndex < 0)
                    {
                        this.imgIndex = this.files.Length - 1;
                    }
                    this.showCurImg();
                    break;
            }
        }

        private void log(String line)
        {
            if (!this.textBox1.Visible)
            {
                return;
            }
            this.textBox1.Text += line + "\r\n";
        }
    }
}