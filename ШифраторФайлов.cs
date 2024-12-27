using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using static System.Security.Cryptography.AesCng;
using static System.Security.Cryptography.AesCryptoServiceProvider;
using static System.Security.Cryptography.AesManaged;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace CIPHER_WF
{
    public partial class ШифраторФайлов : Form
    {
        public ШифраторФайлов()
        {
            InitializeComponent();
        }

        private void ШифраторФайлов_Load(object sender, EventArgs e)
        {

        }



        // Определение CspParameters и RsaCryptoServiceProvider
        readonly CspParameters _cspp = new CspParameters();
        RSACryptoServiceProvider _rsa;

        // Вызов источников (проводника)
        const string EncrFolder = @"c:\Encrypt\";
        const string DecrFolder = @"c:\Decrypt\";
        const string SrcFolder = @"c:\docs\";

        // файл для Public key
        const string PubKeyFile = @"c:\encrypt\rsaPublicKey.txt";

        // Имя хранилища ключа для значения
        // private/public key
        const string KeyName = "Key01";





        private void buttonCreateAsmKeys_Click(object sender, EventArgs e)
        {
            // Переносим обе части ключа в хранилище
            _cspp.KeyContainerName = KeyName;
            _rsa = new RSACryptoServiceProvider(_cspp)
            {
                PersistKeyInCsp = true
            };

            label1.Text = _rsa.PublicOnly
                ? $"Key: {_cspp.KeyContainerName} - Public Only"
                : $"Key: {_cspp.KeyContainerName} - Full Key Pair";
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void DATAGRIDED(string sqly)
        {

            {

            }
        }


        private void buttonEncryptFile_Click(object sender, EventArgs e)
        {
            if (_rsa is null)
            {
                MessageBox.Show("Key not set.");
            }
            else
            {
                // Окно проводника для выбора шифруемого файла
                openFileDialog1.InitialDirectory = SrcFolder;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fName = openFileDialog1.FileName;
                    if (fName != null)
                    {
                        // Имя файла без пути
                        EncryptFile(new FileInfo(fName));
                    }
                }
            }
        }


        private void EncryptFile(FileInfo file)
        {
            // Экземпляр Aes для симметричного шифр данных
            Aes aes = Aes.Create();
            ICryptoTransform transform = aes.CreateEncryptor();

            byte[] keyEncrypted = _rsa.Encrypt(aes.Key, false);

            // Create byte arrays to contain
            // the length values of the key and IV.
            int lKey = keyEncrypted.Length;
            byte[] LenK = BitConverter.GetBytes(lKey);
            int lIV = aes.IV.Length;
            byte[] LenIV = BitConverter.GetBytes(lIV);

            // Ввести след. в FileStream
            // Для шифруемого файла:
            // - length of the key
            // - length of the IV
            // - encrypted key
            // - the IV
            // - the encrypted cipher content

            // Сменить расширение файла ".enc"
            string outFile =
                Path.Combine(EncrFolder, Path.ChangeExtension(file.Name, ".enc"));

            using (var outFs = new FileStream(outFile, FileMode.Create))
            {
                outFs.Write(LenK, 0, 4);
                outFs.Write(LenIV, 0, 4);
                outFs.Write(keyEncrypted, 0, lKey);
                outFs.Write(aes.IV, 0, lIV);

                // Писание шифра, испотльзуя CryptoStream for encrypting.
                using (var outStreamEncrypted =
                    new CryptoStream(outFs, transform, CryptoStreamMode.Write))
                {
                    // Для оптимизации шифруется большой кусок данных
                    int count = 0;
                    int offset = 0;

                    // blockSizeBytes
                    int blockSizeBytes = aes.BlockSize / 8;
                    byte[] data = new byte[blockSizeBytes];
                    int bytesRead = 0;

                    using (var inFs = new FileStream(file.FullName, FileMode.Open))
                    {
                        do
                        {
                            count = inFs.Read(data, 0, blockSizeBytes);
                            offset += count;
                            outStreamEncrypted.Write(data, 0, count);
                            bytesRead += blockSizeBytes;
                        } while (count > 0);
                    }
                    outStreamEncrypted.FlushFinalBlock();
                }
            }
        }

        private void buttonDecryptFile_Click(object sender, EventArgs e)
        {
            if (_rsa is null)
            {
                MessageBox.Show("Key not set.");
            }
            else
            {
                // Открываем окно проводника для выбора шифрованного файла
                openFileDialog2.InitialDirectory = EncrFolder;
                if (openFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    string fName = openFileDialog2.FileName;
                    if (fName != null)
                    {
                        DecryptFile(new FileInfo(fName));
                    }
                }
            }
        }





        private void DecryptFile(FileInfo file)
        {
            // Создать симметричный метод для шифра данный (AES)
            Aes aes = Aes.Create();

            // Массив байтов для выяснения резмера the encrypted key и IV.
            // Расположить по 4 байта каждые с начала
            byte[] LenK = new byte[4];
            byte[] LenIV = new byte[4];

            // Имя для расшифрованного файла
            string outFile =
                Path.ChangeExtension(file.FullName.Replace("Encrypt", "Decrypt"), ".txt");

            // Использовать объекты из FileStream, чтобы читать и сохранять файлы
            using (var inFs = new FileStream(file.FullName, FileMode.Open))
            {
                inFs.Seek(0, SeekOrigin.Begin);
                inFs.Read(LenK, 0, 3);
                inFs.Seek(4, SeekOrigin.Begin);
                inFs.Read(LenIV, 0, 3);

                // Манипуляция с размером и значениями
                int lenK = BitConverter.ToInt32(LenK, 0);
                int lenIV = BitConverter.ToInt32(LenIV, 0);

                // Определение позиции шифра и его размера
                int startC = lenK + lenIV + 8;
                int lenC = (int)inFs.Length - startC;

                // Еще одна цепочка битов
                byte[] KeyEncrypted = new byte[lenK];
                byte[] IV = new byte[lenIV];

                // key и IV
                // Начало - с индекса 8-го
                inFs.Seek(8, SeekOrigin.Begin);
                inFs.Read(KeyEncrypted, 0, lenK);
                inFs.Seek(8 + lenK, SeekOrigin.Begin);
                inFs.Read(IV, 0, lenIV);

                Directory.CreateDirectory(DecrFolder);
                // Используем RSACryptoServiceProvider для расшифровки ключа
                byte[] KeyDecrypted = _rsa.Decrypt(KeyEncrypted, false);

                // Дешифр ключа
                ICryptoTransform transform = aes.CreateDecryptor(KeyDecrypted, IV);

                // расшифровка и перемещение FileSteam зашифрованного файла в FileStream для дешифрованных
                using (var outFs = new FileStream(outFile, FileMode.Create))
                {
                    int count = 0;
                    int offset = 0;

                    // blockSizeBytes еще раз
                    int blockSizeBytes = aes.BlockSize / 8;
                    byte[] data = new byte[blockSizeBytes];

                    // Еще оптимизации, ибо этот код страшный

                    // Начало в зашифрованном тексте
                    inFs.Seek(startC, SeekOrigin.Begin);
                    using (var outStreamDecrypted =
                        new CryptoStream(outFs, transform, CryptoStreamMode.Write))
                    {
                        do
                        {
                            count = inFs.Read(data, 0, blockSizeBytes);
                            offset += count;
                            outStreamDecrypted.Write(data, 0, count);
                        } while (count > 0);

                        outStreamDecrypted.FlushFinalBlock();
                    }
                }
            }
        }



        void buttonExportPublicKey_Click(object sender, EventArgs e)
        {
            // сохранение public key, созданного RSA для этого файла (не надо, если не доверяем)
            Directory.CreateDirectory(EncrFolder);
            using (var sw = new StreamWriter(PubKeyFile, false))
            {
                sw.Write(_rsa.ToXmlString(false));
            }
        }



        void buttonImportPublicKey_Click(object sender, EventArgs e)
        {
            using (var sr = new StreamReader(PubKeyFile))
            {
                _cspp.KeyContainerName = KeyName;
                _rsa = new RSACryptoServiceProvider(_cspp);

                string keytxt = sr.ReadToEnd();
                _rsa.FromXmlString(keytxt);
                _rsa.PersistKeyInCsp = true;

                label1.Text = _rsa.PublicOnly
                    ? $"Key: {_cspp.KeyContainerName} - Public Only"
                    : $"Key: {_cspp.KeyContainerName} - Full Key Pair";
            }
        }


        private void buttonGetPrivateKey_Click(object sender, EventArgs e)
        {
            _cspp.KeyContainerName = KeyName;
            _rsa = new RSACryptoServiceProvider(_cspp)
            {
                PersistKeyInCsp = true
            };

            label1.Text = _rsa.PublicOnly
                ? $"Key: {_cspp.KeyContainerName} - Public Only"
                : $"Key: {_cspp.KeyContainerName} - Full Key Pair";
        }

        private void _encryptOpenFileDialog(object sender, CancelEventArgs e)
        {

        }

        private void _decryptOpenFileDialog(object sender, CancelEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            {
                Description ins = new Description();
                ins.Show();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Главное_меню ГМ = new Главное_меню();
            ГМ.Show();
            this.Close();
        }
    }
}